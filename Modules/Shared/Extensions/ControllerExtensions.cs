using IndeedClone.Modules.Shared.Error;
using Microsoft.AspNetCore.Mvc;
using System.Runtime.CompilerServices;

namespace IndeedClone.Modules.Shared.Extensions
{
    public static class ControllerExtensions
    {
        // # Set error and return IActionResult
        public static IActionResult With(this IActionResult redirect, Action action, [CallerMemberName] string group = "")
        {
            if (!string.IsNullOrWhiteSpace(group))
                action?.Invoke();

            return redirect;
        }

        // Assign errors to TempData and return the same redirect
        public static IActionResult WithErrors(this IActionResult redirect, Controller controller, List<string> errors)
        {
            if (errors != null && errors.Any())
                if(errors.Count == 1)
                    controller.TempData["Error"] = errors.First();
                else
                    controller.TempData["Error"] = string.Join("<br/> ", errors.Select(e => "•&nbsp;&nbsp;" + e));
            

            return redirect;
        }

        public static IActionResult Views(this Controller controller, string viewName, string group = "")
        {
            controller.ViewBag.Errors = ErrorError.GetErrors(group);
            return controller.View(viewName);
        }

        public static string GetSafeReferer(this Controller controller)
        {
            var request = controller.Request;

         // # Try Referer first
            var referer = request.Headers["Referer"].ToString();
            if (!string.IsNullOrEmpty(referer))
                return referer;

         // # Fallback for Incognito / blocked referer
            var path = request.Path.Value?.ToLower() ?? "";

            return path.Contains("/register")
                ? controller.Url.Action("Register", "Register")!
                : controller.Url.Action("Login", "Auth")!;
        }
    }
}
