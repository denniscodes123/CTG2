using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace YourModName.Players
{
    public class ChatPlayer : ModPlayer
    {
        public override void PreUpdate()
        {
            if (!string.IsNullOrEmpty(Main.chatText))
            {
                string teamTag = "";
                string colorTag = "";
                if (Player.team == 1) 
                {
                    teamTag = "[RED]";
                    colorTag = "[c/FF4040:";
                }
                else if (Player.team == 3) // Blue
                {
                    teamTag = "[BLUE]";
                    colorTag = "[c/40A0FF:";
                }
                else
                {
                    teamTag = "[NO TEAM]";
                    colorTag = "[c/AAAAAA:";
                }

               
                if (!Main.chatText.StartsWith(teamTag))
                {
                    Main.chatText = $"{colorTag}{teamTag} {Player.name}: {Main.chatText}]";
                }
            }
        }
    }
}