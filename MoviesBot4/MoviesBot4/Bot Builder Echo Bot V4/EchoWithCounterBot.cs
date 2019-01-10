using System;
using System.Threading;
using System.Threading.Tasks;
using Bot_Builder_Echo_Bot_V4.Dialogs;
using Bot_Builder_Echo_Bot_V4.Helpers;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Schema;

namespace Bot_Builder_Echo_Bot_V4
{
    public class EchoWithCounterBot : IBot
    {
        private DialogSet dialogs = null;
        private EchoBotAccessors accessors = null;

        public EchoWithCounterBot(EchoBotAccessors accessors)
        {
            this.accessors = accessors ?? throw new ArgumentNullException(nameof(accessors));
            this.dialogs = new DialogSet(accessors.ConversationDialogState);
            this.dialogs.Add(new MainDialog(accessors));
        }

        public EchoBotAccessors BotAccessors { get; }

        public async Task OnTurnAsync(ITurnContext turnContext, CancellationToken cancellationToken = default(CancellationToken))
        {
            if (turnContext.Activity.Type == ActivityTypes.Message)
            {
                //var responseMessage = $"Dijiste '{turnContext.Activity.Text}'\n";
                var selectedLanguage = TranslatorHelper.GetDesiredLanguageAsync(turnContext.Activity.Text);



                //await turnContext.SendActivityAsync(responseMessage);
            }
            else if (turnContext.Activity.Type == ActivityTypes.ConversationUpdate)
            {
                if (turnContext.Activity.MembersAdded != null)
                {
                    await SendWelcomeMessageAsync(turnContext, cancellationToken);
                }
            }
            else
            {
                await turnContext.SendActivityAsync($"{turnContext.Activity.Type} event detected", cancellationToken: cancellationToken);
            }
        }

        private static async Task SendWelcomeMessageAsync(ITurnContext turnContext, CancellationToken cancellationToken)
        {
            foreach (var member in turnContext.Activity.MembersAdded)
            {
                if (member.Id != turnContext.Activity.Recipient.Id)
                {
                    var reply = turnContext.Activity.CreateReply();
                    reply.Text = "Hola, para comenzar primero dime el idioma de tu elección";
                    await turnContext.SendActivityAsync(reply, cancellationToken);
                }
            }
        }
    }
}
