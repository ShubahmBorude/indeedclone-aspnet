using Microsoft.AspNetCore.Html;
using System.Net;

namespace IndeedClone.Modules.SubModules.JobPost.Helpers.Dropdowns
{
    public static class DropdownHelper
    {
        public static IHtmlContent RenderDropdown(string[] data, IEnumerable<string>? selectedValues = null)
        {
            if (data == null || data.Length == 0)
                return HtmlString.Empty;

            var selectedSet = selectedValues?.ToHashSet()?? new HashSet<string>();
            var builder = new HtmlContentBuilder();

            foreach (var item in data)
            {
                var safeItem = WebUtility.HtmlEncode(item);
                var isSelected = selectedSet.Contains(item)? " selected" : string.Empty;

                builder.AppendHtml($@"<div class=""dropdown-item{isSelected}"" data-value=""{safeItem}"">{safeItem} </div>");
            }

            return builder;
        }

        public static IHtmlContent RenderEducationDropdown(string[] data)
        {
            if (data == null || data.Length == 0)
                return HtmlString.Empty;

            var builder = new HtmlContentBuilder();

            foreach (var item in data)
            {
                var safeItem = WebUtility.HtmlEncode(item);
                builder.AppendHtml($"<div class=\"dropdown-item\" onclick=\"selectEducation('{safeItem}')\">{safeItem}</div>");
            }

            return builder;
        }

        public static IHtmlContent RenderCertificateDropdown(string[] data)
        {
            if (data == null || data.Length == 0)
                return HtmlString.Empty;

            var builder = new HtmlContentBuilder();

            foreach (var item in data)
            {
                var safeItem = WebUtility.HtmlEncode(item);
                builder.AppendHtml($"<div class=\"dropdown-item\" onclick=\"selectEducation('{safeItem}')\">{safeItem}</div>");
            }

            return builder;
        }


        public static IHtmlContent RenderLanguageDropdown(string[] data)
        {
            if (data == null || data.Length == 0)
                return HtmlString.Empty;

            var builder = new HtmlContentBuilder();

            foreach (var item in data)
            {
                var html = $"<div class=\"dropdown-item\" onclick=\"selectSkill('{System.Net.WebUtility.HtmlEncode(item)}')\">{System.Net.WebUtility.HtmlEncode(item)}</div>";
                builder.AppendHtml(html);
            }

            return builder;
        }

    }
}
