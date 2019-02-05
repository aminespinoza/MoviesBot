namespace MoviesBot.Models
{
    public class TranslationModel
    {
        public TranslatedDocument[] documents { get; set; }
    }

    public class TranslatedDocument
    {
        public TranslatedResults[] result { get; set; }
    }

    public class TranslatedResults
    {
        public Detectedlanguage detectedLanguage { get; set; }
        public Translation[] translations { get; set; }
    }

    public class Detectedlanguage
    {
        public string language { get; set; }
        public float score { get; set; }
    }

    public class Translation
    {
        public string text { get; set; }
        public string to { get; set; }
    }

}
