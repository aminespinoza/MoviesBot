using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Schema;
using MoviesBot.Helpers;
using MoviesBot.Models;

namespace MoviesBot
{
    public class MoviesBot : IBot
    {
        private MoviesBotAccessors accessors = null;

        public MoviesBot(MoviesBotAccessors accessors)
        {
            this.accessors = accessors ?? throw new ArgumentNullException(nameof(accessors));
        }

        public async Task OnTurnAsync(ITurnContext turnContext, CancellationToken cancellationToken = default(CancellationToken))
        {
            if (turnContext.Activity.Type == ActivityTypes.Message)
            {
                UserProfile userProfile = await accessors.UserProfileAccesor.GetAsync(turnContext, () => new UserProfile());

                if (string.IsNullOrEmpty(userProfile.Language))
                {
                    userProfile.Language = TranslatorHelper.GetDesiredLanguage(turnContext.Activity.Text);
                    await accessors.UserProfileAccesor.SetAsync(turnContext, userProfile);
                    await accessors.UserState.SaveChangesAsync(turnContext);
                    var confirmedLanguageResponse = await TranslatorHelper.TranslateSentenceAsync("Está hecho, tu lenguaje está seleccionado", userProfile.Language);
                    await turnContext.SendActivityAsync(confirmedLanguageResponse);
                }
                else
                {
                    if (turnContext.Activity.Text == "nuevo idioma")
                    {
                        await turnContext.SendActivityAsync("Solo dí la palabra mágica");
                        userProfile.Language = string.Empty;
                        await accessors.UserProfileAccesor.SetAsync(turnContext, userProfile);
                        await accessors.UserState.SaveChangesAsync(turnContext);
                    }
                    else
                    {
                        Attachment plAttachment = new Attachment()
                        {
                            ContentType = HeroCard.ContentType,
                            Content = await MoviesHelper.SearchMovieAsync(turnContext.Activity.Text, userProfile.Language),
                        };

                        var reply = turnContext.Activity.CreateReply();
                        reply.Attachments.Add(plAttachment);
                        await turnContext.SendActivityAsync(reply);
                    }
                }
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
