using Microsoft.Bot.Builder.Dialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Bot_Builder_Echo_Bot_V4.Dialogs
{
    public class MainDialog : ComponentDialog
    {
        private const string dialogId = "MainDialog";
        private EchoBotAccessors accessors = null;

        public MainDialog(EchoBotAccessors accessors) : base(dialogId)
        {
            this.accessors = accessors ?? throw new ArgumentNullException(nameof(accessors));

            AddDialog(new WaterfallDialog("MainDialog", new WaterfallStep[]
            {
                LaunchLanguageDialog,
                LaunchIdentifyDialog,
                EndMainDialog,
            }));

            //AddDialog(new LanguageDialog(accessors));
            //AddDialog(new IdentifyDialog(accessors));
        }

        private async Task<DialogTurnResult> LaunchLanguageDialog(WaterfallStepContext step, CancellationToken cancellationToken = default(CancellationToken))
        {
            string userLanguage = await accessors.SelectedLanguagePreference.GetAsync(step.Context, () => { return string.Empty; });

            if (string.IsNullOrEmpty(userLanguage))
            {
                return await step.BeginDialogAsync("LanguageDialog");
            }
            else
            {
                return await step.NextAsync();
            }
        }

        private async Task<DialogTurnResult> LaunchIdentifyDialog(WaterfallStepContext step, CancellationToken cancellationToken = default(CancellationToken))
        {
            return await step.BeginDialogAsync("IdentifyDialog");
        }

        private async Task<DialogTurnResult> EndMainDialog(WaterfallStepContext step, CancellationToken cancellationToken = default(CancellationToken))
        {
            string userLanguage = await accessors.SelectedLanguagePreference.GetAsync(step.Context, () => { return string.Empty; });

            if (string.IsNullOrEmpty(userLanguage))
            {
                throw new System.Exception("there was an issue reading the language preference");
            }
            else
            {
                var response = $"Thanks for using this bot preview, the project is still under development";
                //var message = await TranslatorHelper.TranslateSentenceAsync(response, userLanguage);
                await step.Context.SendActivityAsync($"{response}");
            }

            //ending dialog
            await step.EndDialogAsync(step.ActiveDialog.State);
            return Dialog.EndOfTurn;
        }
    }

}
