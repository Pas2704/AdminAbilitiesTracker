using HarmonyLib;
using NLog;
using Sandbox.Game.Gui;
using Sandbox.Game.Multiplayer;
using Sandbox.Game.World;
using Sandbox.Graphics.GUI;
using Sandbox.ModAPI;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using VRage.FileSystem;
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
        public static bool SendChatMessages = true;

        private static StringBuilder sb = new StringBuilder();
        private static string configLocation = Path.Combine(MyFileSystem.UserDataPath, "Storage", "AdminAbilitiesTracker.xml");

        [MethodImpl(MethodImplOptions.NoInlining)]
        public void Init(object gameInstance)
        {
            Log.Debug($"AdminAbilitiesTracker: Patching");
            Harmony.PatchAll(Assembly.GetExecutingAssembly());
            Log.Info($"AdminAbilitiesTracker: Patches applied");
            if (LoadConfig())
            {
                Log.Info("Config Loaded");
            }
        }
        public void Dispose()
        {

        }
        public void Update()
        {
            if (MySession.Static != null && MyRenderProxy.DrawRenderStats == MyRenderProxy.MyStatsState.SimpleTimingStats)
            {
                IMyCamera camera = ((IMySession)MySession.Static).Camera;
                Vector2 vector2_1 = new Vector2(camera.ViewportSize.X * (1 - 0.01f), camera.ViewportSize.Y * 0.01f);
                float num1 = 0.65f * Math.Min(camera.ViewportSize.X / 1920f, camera.ViewportSize.Y / 1200f);
                foreach (var kvp in MySession.Static.RemoteAdminSettings.ToList())
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
                MyRenderProxy.DebugDrawText2D(vector2_1, sb.ToString(), Color.Yellow, num1, MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_TOP);
                sb.Clear();
            }
        }
        public void OpenConfigDialog()
        {
            MyGuiSandbox.AddScreen(new MyGuiScreenConfig());
        }
        public static void SaveConfig()
        {
            ConfigObject config = new ConfigObject();
            config.SendChatMessages = SendChatMessages;
            try
            {
                XmlSerializer serializer = new XmlSerializer(typeof(ConfigObject));
                serializer.Serialize(File.CreateText(configLocation), config);
                Log.Info($"Config Saved, SendChatMessages: {SendChatMessages}");
            }
            catch (Exception ex)
            {
                Log.Error($"Error saving config: {ex.Message}");
            }
        }
        private static bool LoadConfig()
        {
            if (File.Exists(configLocation))
            {
                try
                {
                    XmlSerializer serializer = new XmlSerializer(typeof(ConfigObject));
                    string xml = File.ReadAllText(configLocation);
                    ConfigObject config = (ConfigObject)serializer.Deserialize(File.OpenRead(configLocation));
                    SendChatMessages = config.SendChatMessages;
                    return true;
                }
                catch (Exception ex)
                {
                    Log.Error($"Error loading config: {ex.Message}");
                    return false;
                }
            }
            return false;
        }
    }
}
