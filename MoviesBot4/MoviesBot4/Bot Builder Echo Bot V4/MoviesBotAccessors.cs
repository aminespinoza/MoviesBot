using System;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Dialogs;
using MoviesBot.Models;

namespace MoviesBot
{
    public class MoviesBotAccessors
    {
        public MoviesBotAccessors(ConversationState conversationState, UserState userState)
        {
            ConversationState = conversationState ?? throw new ArgumentNullException(nameof(conversationState));
            UserState = userState ?? throw new ArgumentNullException(nameof(userState));
        }

        public static string UserProfileName { get; } = "UserProfile";

        public IStatePropertyAccessor<DialogState> ConversationDialogState { get; set; }

        public IStatePropertyAccessor<UserProfile> UserProfileAccesor { get; set; }

        public ConversationState ConversationState { get; }

        public UserState UserState { get; }
    }
}
