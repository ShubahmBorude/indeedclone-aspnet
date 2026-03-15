namespace IndeedClone.Modules.IndeedClone.Helpers
{
    public static class DateTimeAgoHelper
    {
        public static string GetPostedAgo(DateTime created, DateTime edited)
        {
            var latest = edited > created ? edited : created;
            var diff = DateTime.UtcNow - latest;

            if (diff.TotalHours < 24)
                return $"{Math.Max(1, (int)diff.TotalHours)} hours ago";

            if (diff.TotalDays < 7)
                return $"{(int)diff.TotalDays} days ago";

            if (diff.TotalDays < 14)
                return "1 week ago";

            if (diff.TotalDays < 21)
                return "2 weeks ago";

            if (diff.TotalDays < 28)
                return "3 weeks ago";

            return "more than 4 weeks ago";
        }

    }
}
