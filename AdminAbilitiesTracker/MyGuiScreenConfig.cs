using Sandbox;
using Sandbox.Graphics.GUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VRageMath;

namespace AdminAbilitiesTracker
{
    internal class MyGuiScreenConfig : MyGuiScreenBase
    {
        public override string GetFriendlyName()
        {
            return nameof(MyGuiScreenConfig);
        }
        public MyGuiScreenConfig() : base(new Vector2(0.5f, 0.5f), MyGuiConstants.SCREEN_BACKGROUND_COLOR, new Vector2(0.4f, 0.3f), false, null, MySandboxGame.Config.UIBkOpacity, MySandboxGame.Config.UIOpacity)
        {
        
        }
        public override void LoadContent()
        {
            base.LoadContent();
            RecreateControls(true);
        }
        public override void RecreateControls(bool constructor)
        {
            base.RecreateControls(constructor);
            Vector2 labelPosition = new Vector2(-.1f, -0.04f);
            Vector2 labelsize = new Vector2(0.15f, 0.05f);
            MyGuiControlLabel label = new MyGuiControlLabel(labelPosition, labelsize, "Send Chat Messages: ");
            MyGuiControlCheckbox checkbox = new MyGuiControlCheckbox(new Vector2(labelPosition.X + labelsize.X + 0.05f, labelPosition.Y))
            {
                IsChecked = Plugin.SendChatMessages,
                IsCheckedChanged = x => Plugin.SendChatMessages = !Plugin.SendChatMessages
            };
            Controls.Add(label);
            Controls.Add(checkbox);
        }
        public override void OnRemoved()
        {
            base.OnRemoved();
            Plugin.SaveConfig();
        }
    }
}
