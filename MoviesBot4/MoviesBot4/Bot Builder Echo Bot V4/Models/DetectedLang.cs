namespace Bot_Builder_Echo_Bot_V4.Models
{
    public class DetectedLang
    {
        public Result[] result { get; set; }
    }

    public class Result
    {
        public string language { get; set; }
        public float score { get; set; }
        public bool isTranslationSupported { get; set; }
        public bool isTransliterationSupported { get; set; }
        public Alternative[] alternatives { get; set; }
    }

    public class Alternative
    {
        public string language { get; set; }
        public float score { get; set; }
        public bool isTranslationSupported { get; set; }
        public bool isTransliterationSupported { get; set; }
    }
}
