using System;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Dialogs;

namespace Bot_Builder_Echo_Bot_V4
{
    public class EchoBotAccessors
    {
        public EchoBotAccessors(ConversationState conversationState, UserState userState)
        {
            ConversationState = conversationState ?? throw new ArgumentNullException(nameof(conversationState));
            UserState = userState ?? throw new ArgumentNullException(nameof(userState));
        }

        public IStatePropertyAccessor<DialogState> ConversationDialogState { get; set; }

        public IStatePropertyAccessor<string> SelectedLanguagePreference { get; set; }

        public ConversationState ConversationState { get; }

        public UserState UserState { get; }
    }
}
