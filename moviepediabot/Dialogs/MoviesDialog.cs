using System;
using System.Collections.Generic;
using System.Configuration;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using Newtonsoft.Json;
using SimpleEchoBot.Classes;
using SimpleEchoBot.Models;

namespace SimpleEchoBot.Dialogs
{
    [Serializable]
    public class MoviesDialog : IDialog<object>
    {
        public Task StartAsync(IDialogContext context)
        {
            context.Wait(MessageReceivedAsync);

            return Task.CompletedTask;
        }

        private async Task MessageReceivedAsync(IDialogContext context, IAwaitable<object> result)
        {
            var activity = await result as Activity;
            var reply = activity.CreateReply();

            if (activity.Text == "nuevo idioma")
            {
                await context.PostAsync("¡Solo di la palabra!");
                ConfigurationManager.AppSettings["DesiredLanguage"] = String.Empty;
                context.Call(new TranslateDialog(), CallBack);
            }
            else
            {
                string newCharacter = await SearchMovie(activity, reply);
                await context.PostAsync(reply);
                context.Wait(MessageReceivedAsync);
            }
        }

        private async Task<string> SearchMovie(Activity activity, Activity reply)
        {
            string newMovie = activity.Text;


            string uriBase = @"http://www.omdbapi.com/?";
            string keyParameter = "apikey=209525c7";
            string tParameter = String.Format("&t={0}", newMovie);

            var basicUrl = uriBase + keyParameter + tParameter; ;

            HttpClient request = new HttpClient();
            var responseString = await request.GetStringAsync(basicUrl);

            var serializedEntity = JsonConvert.DeserializeObject<Movie>(responseString);
            string name = string.Format("{0} ({1})", serializedEntity.Title, serializedEntity.Year);
            string content = await Translator.TranslateSentenceAsync(serializedEntity.Plot);
            string thumbnail = serializedEntity.Poster;
            HeroCard myCard = await HeroCardCreation(name, content, thumbnail);

            reply.Attachments.Add(myCard.ToAttachment());
            return newMovie;
        }

        private async Task<HeroCard> HeroCardCreation(string name, string content, string imagePath)
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
            seriesButton.Title = await Translator.TranslateSentenceAsync("Awards");
            seriesButton.Type = ActionTypes.ImBack;
            seriesButton.Value = "series";
            buttonsList.Add(seriesButton);

            CardAction comicsButton = new CardAction();
            comicsButton.Title = await Translator.TranslateSentenceAsync("Rating");
            comicsButton.Type = ActionTypes.ImBack;
            comicsButton.Value = "comics";
            buttonsList.Add(comicsButton);
            myCard.Buttons = buttonsList;
            return myCard;
        }

        private async Task CallBack(IDialogContext context, IAwaitable<object> result)
        {
            context.Done("");
        }
    }
}