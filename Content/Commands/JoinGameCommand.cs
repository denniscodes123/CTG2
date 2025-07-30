using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System;
using CTG2.Content.ServerSide;

namespace CTG2.Content
{
    public class JoinGameCommand : ModCommand
    {
        public override CommandType Type => CommandType.Chat;
        public override string Command => "j";
        public override string Usage => "/j";
        public override string Description => "Join the current pubs match";

        public override void Action(CommandCaller caller, string input, string[] args)
        {
            if (args.Length != 0)
            {
                caller.Reply("Usage: /nm [mapname]");
                return;
            }

            ModPacket packet = ModContent.GetInstance<CTG2>().GetPacket();
            packet.Write((byte)MessageType.RequestTeamChange);
            packet.Write(Main.myPlayer);
            packet.Write(-1); 
            packet.Send();
              //swap order later  

        }
    }
}