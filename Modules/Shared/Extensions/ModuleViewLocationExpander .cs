using Microsoft.AspNetCore.Mvc.Razor;

namespace IndeedClone.Modules.Shared.Extensions
{
    public class ModuleViewLocationExpander : IViewLocationExpander
    {
        public void PopulateValues(ViewLocationExpanderContext context) { }

        public IEnumerable<string> ExpandViewLocations(ViewLocationExpanderContext context,IEnumerable<string> viewLocations)
        {
            var moduleLocations = new[]
            {
             // # Default MVC
                "/Views/Home/{0}.cshtml",
                "/Views/Shared/{0}.cshtml",

              // # Top-level modules
                "/Modules/{1}/Views/{0}.cshtml",
                "/Modules/{1}/Views/{1}/{0}.cshtml",
                "/Modules/{1}/Views/Layout/{0}.cshtml",

              // # SubModules
                "/Modules/SubModules/{1}/Views/{0}.cshtml",
                "/Modules/SubModules/{1}/Views/{1}/{0}.cshtml",
                "/Modules/SubModules/{1}/Views/Layout/{0}.cshtml",

             // # Explicit JobPost layout path
                "/Modules/SubModules/JobPost/Views/Layout/{0}.cshtml",
             // # Explicit JobDashboard path
                "/Modules/JobDashboard/Views/Layout/{0}.cshtml",
             // # Explicit Auth path
                "/Modules/SubModules/Auth/Views/Layout/{0}.cshtml",
            

             // # Explicit Main IndeedClone layout path
                "/Modules/IndeedClone/Views/Layout/{0}.cshtml"

            };

            return moduleLocations.Concat(viewLocations);
        }
    }
}
