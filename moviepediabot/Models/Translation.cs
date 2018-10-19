using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SimpleEchoBot.Models
{
    public class TranslationModel
    {
        public Document[] documents { get; set; }
    }

    public class Document
    {
        public Result2[] result { get; set; }
    }

    public class Result2
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