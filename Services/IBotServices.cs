using Microsoft.Bot.Builder.AI.QnA;

namespace SimpleChatbot.Services
{
    public interface IBotServices
    {
        QnAMaker QnAMakerService { get; }
    }
}