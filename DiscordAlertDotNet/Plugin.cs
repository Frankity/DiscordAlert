using System;

using SharedLibraryCore;
using SharedLibraryCore.Interfaces;
using SharedLibraryCore.Objects;
using SharedLibraryCore.Configuration;
using SharedLibraryCore.Services;
using SharedLibraryCore.Database.Models;
using System.Threading.Tasks;
using System.Reflection;
using System.IO;

namespace DiscordAlertDotNet
{
    public class Plugin : IPlugin
    {
        public string Name => "DiscordAlertDotnet";

        public float Version => 0.3f;

        public string Author => "Frankity";

        private BaseConfigurationHandler<DiscordAlertCfg> Config;

        public async Task OnLoadAsync(IManager manager)
        {
            Config = new BaseConfigurationHandler<DiscordAlertCfg>("DiscordAlertCfg");
            if (Config.Configuration() == null)
            {
                Config.Set((DiscordAlertCfg)new DiscordAlertCfg().Generate());
                await Config.Save();
            }
        }

        public async Task OnEventAsync(GameEvent E, Server S)
        {
            HookSender hookSender = new DiscordAlertDotNet.HookSender(Config.Configuration().HookId, Config.Configuration().HookToken);

            if (E.Type == GameEvent.EventType.Connect)
            {
                Player p = E.Origin;

                if (p.Level >= Player.Permission.Trusted)
                {
                    try
                    {
                        await hookSender.Send(Config.Configuration().OnJoinedMessage.Replace("player", p.Name).Replace("svname", S.Hostname) + " - **" + S.GameName.ToString() + "**", Config.Configuration().BotName, null, false, null);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.StackTrace);
                    }
                }
            }

            if(E.Type == GameEvent.EventType.Disconnect)
            {
                Player p = E.Origin;
                if (p.Level >= Player.Permission.Trusted)
                {
                    try
                    {
                        await hookSender.Send(Config.Configuration().OnLeftMessage.Replace("player", p.Name).Replace("svname", S.Hostname) + " - **" + S.GameName.ToString() + "**", Config.Configuration().BotName, null, false, null);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.StackTrace);
                    }
                }
            }

            if (E.Type == GameEvent.EventType.Report)
            {
                await hookSender.Send($"**{E.Origin.Name}** has reported **{E.Target.Name}** for: {E.Data.Trim()}",E.Target.Name, E.Origin.Name);
            }

        }

        public Task OnTickAsync(Server S) => Task.CompletedTask;

        public Task OnUnloadAsync() => Task.CompletedTask;

        private string Announce(string msg, Player joining)
        {
            msg = msg.Replace("{{ClientName}}", joining.Name);
            msg = msg.Replace("{{ClientLevel}}", Utilities.ConvertLevelToColor(joining.Level));
            try
            {

            }
            catch (Exception)
            {

            }
            return msg;
        }
    }
}
