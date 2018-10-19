using Autofac;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using SimpleEchoBot.Dialogs;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace Microsoft.Bot.Sample.SimpleEchoBot
{
    [BotAuthentication]
    public class MessagesController : ApiController
    {
        /// <summary>
        /// POST: api/Messages
        /// receive a message from a user and send replies
        /// </summary>
        /// <param name="activity"></param>
        public virtual async Task<HttpResponseMessage> Post([FromBody] Activity activity)
        {
            // check if activity is of type message
            if (activity != null && activity.GetActivityType() == ActivityTypes.Message)
            {
                await Conversation.SendAsync(activity, () => new TranslateDialog());
            }
            else
            {
                HandleSystemMessage(activity);
            }
            return new HttpResponseMessage(System.Net.HttpStatusCode.Accepted);
        }

        private async Task<Activity> HandleSystemMessage(Activity message)
        {
            if (message.Type == ActivityTypes.DeleteUserData)
            {
                // Implement user deletion here
                // If we handle user deletion, return a real message
            }
            else if (message.Type == ActivityTypes.ConversationUpdate)
            {
                IConversationUpdateActivity update = message;
                using (var scope = Microsoft.Bot.Builder.Dialogs.Internals.DialogModule.BeginLifetimeScope(Conversation.Container, message))
                {
                    var client = scope.Resolve<IConnectorClient>();
                    if (update.MembersAdded.Any())
                    {
                        var reply = message.CreateReply();
                        foreach (var newMember in update.MembersAdded)
                        {
                            if (newMember.Id != message.Recipient.Id)
                            {
                                string newMessage = "";
                                newMessage += $"¡Hola! Para iniciar solo escribe una palabra con tu idioma de elección";
                                reply.Text = newMessage;
                                await client.Conversations.ReplyToActivityAsync(reply);
                            }
                        }
                    }
                }
            }
            else if (message.Type == ActivityTypes.ContactRelationUpdate)
            {
                // Handle add/remove from contact lists
                // Activity.From + Activity.Action represent what happened
            }
            else if (message.Type == ActivityTypes.Typing)
            {
                // Handle knowing tha the user is typing
            }
            else if (message.Type == ActivityTypes.Ping)
            {
            }

            return null;
        }
    }
}