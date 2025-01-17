﻿using Microsoft.Bot.Builder.AI.QnA;
using Microsoft.Extensions.Configuration;

namespace SimpleChatbot.Services
{
    public class BotServices : IBotServices
    {
        public BotServices(IConfiguration configuration)
        {
            QnAMakerService = new QnAMaker(new QnAMakerEndpoint
            {
                KnowledgeBaseId = configuration["QnAKnowledgebaseId"],
                EndpointKey = configuration["QnAAuthKey"],
                Host = GetHostname(configuration["QnAEndpointHostName"])
            });
        }
        public QnAMaker QnAMakerService { get; private set; }
        private static string GetHostname(string hostname)
        {
            if (!hostname.StartsWith("https://"))
            {
                hostname = string.Concat("https://", hostname);
            }

            if (!hostname.EndsWith("/qnamaker"))
            {
                hostname = string.Concat(hostname, "/qnamaker");
            }
            return hostname;
        }
    }
}