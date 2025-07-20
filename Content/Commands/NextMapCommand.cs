using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System;
using CTG2.Content.ServerSide;

namespace CTG2.Content
{
    public class MapCommand : ModCommand
    {
        public override CommandType Type => CommandType.Chat;
        public override string Command => "nm";
        public override string Usage => "/nm [mapname]";
        public override string Description => "Set the current map for spawn points";

        public override void Action(CommandCaller caller, string input, string[] args)
        {
            if (args.Length == 0)
            {
                caller.Reply("Usage: /nm [mapname]");
                return;
            }

        string mapName = args[0];
        if (!Enum.TryParse<MapTypes>(mapName, true, out _))
        {
            caller.Reply($"Error: '{mapName}' is not a valid map name.");
            return;
        }

        ModPacket myPacket = Mod.GetPacket();
        myPacket.Write((byte)MessageType.RequestNextMap);

     
        myPacket.Write(mapName);

  
        myPacket.Send();
        }
    }
}