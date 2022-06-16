using HarmonyLib;
using Sandbox.Game.Gui;
using Sandbox.Game.World;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VRageMath;

namespace AdminAbilitiesTracker
{
    [HarmonyPatch(typeof(MyGuiScreenAdminMenu), "AdminSettingsChangedClient")]
    internal static class AdminSettingsChangedClientPatch
    {
        
        [HarmonyPrefix]
        public static void OnSettingsUpdated(AdminSettingsEnum settings, ulong steamId)
        {
            StringBuilder sb = new StringBuilder();

            bool didResolve = MySession.Static.Players.TryGetPlayerBySteamId(steamId, out MyPlayer player);

            if (didResolve)
            {
                sb.AppendLine($"Admin settings changed for {player.DisplayName} ({steamId})");

            }
            else
            {
                sb.AppendLine($"Unable to convert steamid to player id {steamId}");
            }
            var oldSettings = MySession.Static.RemoteAdminSettings[steamId];
            bool didanyChanges = false;
            foreach (Enum setting in Enum.GetValues(typeof(AdminSettingsEnum)))
            {
                bool hasChanged = oldSettings.HasFlag(setting) != settings.HasFlag(setting);
                Console.WriteLine($"{oldSettings.HasFlag(setting)} - {settings.HasFlag(setting)}");
                if (hasChanged)
                {
                    sb.AppendLine($"{setting} - was: {oldSettings.HasFlag(setting)}, now: {settings.HasFlag(setting)}");
                    didanyChanges = true;
                }
            }

            if (didanyChanges)
            {
                Console.WriteLine(sb);
                MyHud.Chat.ShowMessage("Admin Abuse Detection", sb.ToString(), Color.White, "Green");
            }
        }
    }
}
