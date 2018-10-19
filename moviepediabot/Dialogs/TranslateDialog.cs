using System;
using System.Configuration;
using System.Threading.Tasks;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using SimpleEchoBot.Classes;

namespace SimpleEchoBot.Dialogs
{
    [Serializable]
    public class TranslateDialog : IDialog<object>
    {
        public async Task StartAsync(IDialogContext context)
        {
            await StartConversation(context);
        }

        private async Task StartConversation(IDialogContext context)
        {
            context.Wait(MessageReceivedAsync);
        }

        private async Task MessageReceivedAsync(IDialogContext context, IAwaitable<object> result)
        {
            var activity = await result as Activity;
            string responseString = string.Empty;
            var query = activity.Text.ToString();
            string languageCode = ConfigurationManager.AppSettings["DesiredLanguage"].ToString();

            if (String.IsNullOrEmpty(languageCode))
            {
                await DefineLanguage(activity, context);
            }
        }

        private async Task DefineLanguage(Activity activity, IDialogContext reply)
        {
            ConfigurationManager.AppSettings["DesiredLanguage"] = await Translator.GetDesiredLanguageAsync(activity.Text);
            var finalReply = await Translator.TranslateSentenceAsync("Hi! About which movie you would like to know ?");
            await reply.PostAsync(finalReply);
            reply.Call(new MoviesDialog(), CallBack);
        }

        private async Task CallBack(IDialogContext context, IAwaitable<object> result)
        {
            context.Done("");
        }
    }
}