/*
 * User: Frankity
 */
using SharedLibraryCore;
using SharedLibraryCore.Interfaces;

namespace DiscordAlertDotNet
{
    class DiscordAlertCfg : IBaseConfiguration
    {
        public string HookId { get; set; }
        public string HookToken { get; set; }
        public string OnJoinedMessage { get; set; }
        public string OnLeftMessage { get; set; }
        public string BotName { get; set; }

        public IBaseConfiguration Generate()
        {
            HookId = "YOUR ID HERE";
            HookToken = "YOUR TOKEN HERE";
            OnJoinedMessage = "**player** joined in to the **svname** server!";
            OnLeftMessage = "**player** left the **svname** server!";
            BotName = "CHANGEME";
            return this;
        }

        public string Name() => "DiscordAlertCfg";
    }
}