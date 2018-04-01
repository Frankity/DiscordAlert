/*
 * User: Frankity
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SharedLibrary;
using SharedLibrary.Objects;
using SharedLibrary.Network;
using SharedLibrary.Helpers;
using SharedLibrary.Interfaces;
using System.Collections;
using System.Reflection;
using System.IO;

namespace DiscordAlert
{
    public class Plugin : IPlugin
    {
        public string Author => "Frankity";
        public float Version => 1.4f;
        public string Name => "Discord Alert Plugin";

        public string id;
        public string token;
        public string joined;
        public string left;

        public async Task OnLoadAsync(IManager manager)
        {
            string dir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            IniReader ini = new IniReader(Path.Combine(dir, "DiscordAlert.ini"));

            id = ini.IniReadValue("config", "id");
            token = ini.IniReadValue("config", "token");
            joined = ini.IniReadValue("config", "joined");
            left = ini.IniReadValue("config", "left");
        }

        public async Task OnUnloadAsync()
        {
        }

        public async Task OnTickAsync(Server S)
        {
        }

        public async Task OnEventAsync(Event E, Server s)
        {
            HookSender hookSender = new DiscordAlert.HookSender(id, token);

            if (E.Type == Event.GType.Connect)
            {
                Player p = E.Origin;

                if (p.Level >= Player.Permission.Trusted)
                    await E.Owner.Broadcast(Announce($"Player ^1{p.Name} ^7announced in ^1D^7iscord!", p));

                try
                {
                    hookSender.Send(joined.Replace("player", p.Name).Replace("svname", s.Hostname), "SilentBot", null, false, null);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.StackTrace);
                }
            }
            else if (E.Type == Event.GType.Disconnect)
            {
                Player p = E.Origin;
                try
                {
                    hookSender.Send(left.Replace("player", p.Name).Replace("svname", s.Hostname), "SilentBot", null, false, null);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.StackTrace);
                }

            }
        }

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