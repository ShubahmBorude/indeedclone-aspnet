namespace IndeedClone.Modules.Shared.Error
{
    public class ErrorData
    {
        public string Message { get; set; }
        public string Group { get; set; }
        public int Line { get; set; }
        public string Method { get; set; }
        public DateTime Time { get; set; }
    }
}
