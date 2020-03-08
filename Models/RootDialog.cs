using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Bot.Builder.AI.QnA;
using Microsoft.Bot.Builder.Dialogs;
using SimpleChatbot.Services;

namespace SimpleChatbot.Models
{
    public class RootDialog : ComponentDialog
    {
        private const string InitialDialog = "initial-dialog";
        public RootDialog(IBotServices services)
            : base("root")
        {
            AddDialog(new QnAMakerBaseDialog(services));

            AddDialog(new WaterfallDialog(InitialDialog)
               .AddStep(InitialStepAsync));
            InitialDialogId = InitialDialog;
        }
        private async Task<DialogTurnResult> InitialStepAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            var qnamakerOptions = new QnAMakerOptions
            {
                ScoreThreshold = QnAMakerBaseDialog.DefaultThreshold,
                Top = QnAMakerBaseDialog.DefaultTopN,
                Context = new QnARequestContext()
            };
            var qnaDialogResponseOptions = new QnADialogResponseOptions
            {
                NoAnswer = QnAMakerBaseDialog.DefaultNoAnswer,
                ActiveLearningCardTitle = QnAMakerBaseDialog.DefaultCardTitle,
                CardNoMatchText = QnAMakerBaseDialog.DefaultCardNoMatchText,
                CardNoMatchResponse = QnAMakerBaseDialog.DefaultCardNoMatchResponse
            };
            var dialogOptions = new Dictionary<string, object>
            {
                [QnAMakerBaseDialog.QnAOptions] = qnamakerOptions,
                [QnAMakerBaseDialog.QnADialogResponseOptions] = qnaDialogResponseOptions
            };
            return await stepContext.BeginDialogAsync(nameof(QnAMakerBaseDialog), dialogOptions, cancellationToken);
        }
    }
}