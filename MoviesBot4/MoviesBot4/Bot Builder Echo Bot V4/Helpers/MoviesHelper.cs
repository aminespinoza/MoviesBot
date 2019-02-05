using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Bot.Schema;
using MoviesBot.Models;
using Newtonsoft.Json;

namespace MoviesBot.Helpers
{
    public class MoviesHelper
    {
        public static async Task<HeroCard> SearchMovieAsync(string movieTitle, string selectedLanguage)
        {
            string uriBase = @"http://www.omdbapi.com/?";
            string keyParameter = "apikey=209525c7";
            string movieParameter = string.Format("&t={0}", movieTitle);

            var basicUrl = uriBase + keyParameter + movieParameter; ;

            HttpClient request = new HttpClient();
            var responseString = await request.GetStringAsync(basicUrl);

            var serializedEntity = JsonConvert.DeserializeObject<Movie>(responseString);
            string name = string.Format("{0} ({1})", serializedEntity.Title, serializedEntity.Year);
            string content = await TranslatorHelper.TranslateSentenceAsync(serializedEntity.Plot, selectedLanguage);
            string thumbnail = serializedEntity.Poster;
            HeroCard myCard = await HeroCardCreationAsync(name, content, thumbnail, selectedLanguage);

            return myCard;
        }

        public static async Task<HeroCard> HeroCardCreationAsync(string name, string content, string imagePath, string selectedLanguage)
        {
            HeroCard myCard = new HeroCard
            {
                Title = name,
                Subtitle = content
            };

            List<CardImage> imageList = new List<CardImage>();
            List<CardAction> buttonsList = new List<CardAction>();
            CardImage characterImage = new CardImage(imagePath);

            imageList.Add(characterImage);
            myCard.Images = imageList;

            CardAction seriesButton = new CardAction();
            seriesButton.Title = await TranslatorHelper.TranslateSentenceAsync("Awards", selectedLanguage);
            seriesButton.Type = ActionTypes.ImBack;
            seriesButton.Value = "series";
            buttonsList.Add(seriesButton);

            CardAction comicsButton = new CardAction();
            comicsButton.Title = await TranslatorHelper.TranslateSentenceAsync("Rating", selectedLanguage);
            comicsButton.Type = ActionTypes.ImBack;
            comicsButton.Value = "comics";
            buttonsList.Add(comicsButton);
            myCard.Buttons = buttonsList;
            return myCard;
        }
    }
}
