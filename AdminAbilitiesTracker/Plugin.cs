using HarmonyLib;
using NLog;
using Sandbox.Game.Gui;
using Sandbox.Game.Multiplayer;
using Sandbox.Game.World;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using VRage.Game.ModAPI;
using VRage.Input;
using VRage.ModAPI;
using VRage.Plugins;
using VRage.Utils;
using VRageMath;
using VRageRender;

namespace AdminAbilitiesTracker
{
    public class Plugin : IPlugin, IDisposable
    {
        public static Harmony Harmony
        {
            get
            {
                return new Harmony("AdminAbilitiesTracker");
            }
        }

        public static readonly Logger Log = LogManager.GetCurrentClassLogger();
        private static StringBuilder sb = new StringBuilder();

        [MethodImpl(MethodImplOptions.NoInlining)]
        public void Init(object gameInstance)
        {
            Log.Debug($"AdminAbilitiesTracker: Patching");
            Harmony.PatchAll(Assembly.GetExecutingAssembly());
            Log.Info($"AdminAbilitiesTracker: Patches applied");
        }
        public void Dispose()
        {

        }
        public void Update()
        {
            if (MySession.Static != null && MyRenderProxy.DrawRenderStats == MyRenderProxy.MyStatsState.SimpleTimingStats)
            {
                //Borrowed stuff from keen's debug menu
                IMyCamera camera = ((IMySession)MySession.Static).Camera;
                Vector2 vector2_1 = new Vector2(camera.ViewportSize.X * 0.01f, camera.ViewportSize.Y * 0.3f);
                float num1 = 0.65f * Math.Min(camera.ViewportSize.X / 1920f, camera.ViewportSize.Y / 1200f);
                foreach (var kvp in MySession.Static.RemoteAdminSettings)
                {
                    if ((kvp.Value & AdminSettingsEnum.AdminOnly) > AdminSettingsEnum.None)
                    {
                        string playerName;
                        if (MySession.Static.Players.TryGetPlayerBySteamId(kvp.Key, out MyPlayer player))
                        {
                            playerName = player.DisplayName;
                        }
                        else
                        {
                            playerName = kvp.Key.ToString();
                        }
                        sb.AppendLine($"{playerName} - {kvp.Value}");
                    }
                }
                MyRenderProxy.DebugDrawText2D(vector2_1, sb.ToString(), Color.Yellow ,num1);
                sb.Clear();
            }
        }
        
    }
}
