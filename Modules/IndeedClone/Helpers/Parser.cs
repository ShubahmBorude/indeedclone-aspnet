using System.Text.Json;

namespace IndeedClone.Modules.IndeedClone.Helpers
{
    public static class Parser
    {
        public static IEnumerable<string> ToList(string? param)
        {
            return ParseList(param);
        }
        public static IEnumerable<string> JsonArray(string? param)
        {
            return ParseJsonArray(param);
        }
        private static IEnumerable<string> ParseList(string? param)
        {
            if (string.IsNullOrWhiteSpace(param))
                return Enumerable.Empty<string>();

            param = param.Trim();

            // JSON format (future-safe)
            if (param.StartsWith("["))
            {
                return JsonSerializer.Deserialize<List<string>>(param)
                       ?? Enumerable.Empty<string>();
            }

            // CSV format (current DB)
            return param
                .Split(',', StringSplitOptions.RemoveEmptyEntries)
                .Select(v => v.Trim());
        }

        private static IEnumerable<string> ParseJsonArray(object param)
        {
            if (param == DBNull.Value)
                return Enumerable.Empty<string>();

            var raw = param.ToString();

            // Safety: handle old comma-separated data
            if (!string.IsNullOrWhiteSpace(raw) && !raw.Trim().StartsWith("["))
            {
                return raw
                    .Split(',', StringSplitOptions.RemoveEmptyEntries)
                    .Select(v => v.Trim());
            }

            return JsonSerializer.Deserialize<List<string>>(raw ?? "[]")
                   ?? Enumerable.Empty<string>();
        }
    }
}
