using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;
using CTG2.Content;
using Microsoft.Xna.Framework;
using Terraria.ID;

namespace CTG2.Content.ServerSide;

public class GameTeam
{   
    private List<Player> Players { get; set; }
    private Vector2 ClassLocation { get; set; }
    private Vector2 BaseLocation { get; set; }
    
    private int TeamColor { get; set; }
    
    public GameTeam(Vector2 classLocation, Vector2 baseLocation, int teamColor)
    {
        ClassLocation = classLocation;
        BaseLocation = baseLocation;
        TeamColor = teamColor;
        Players = new List<Player>();
    }

    public void UpdateTeam()
    {   
        Players.Clear();
        foreach (Player ply in Main.player)
        {   
            if ((ply.name == "carlos2" && TeamColor == 3) || (ply.name == "carlos3" && TeamColor == 1)) Players.Add(ply);
        }
    }
    
    public void StartMatch()
    {   
        var mod = ModContent.GetInstance<CTG2>();
        ModPacket packet = mod.GetPacket();
        packet.Write((byte)MessageType.ServerGameStart);
        packet.Send();
        
        foreach (Player ply in Players)
        {   
            ply.Teleport(ClassLocation);
            Console.WriteLine("teleported player!");
            NetMessage.SendData(
                MessageID.TeleportEntity,
                remoteClient: -1,           // send to everyone
                ignoreClient: -1,           // don’t ignore anyone
                text: null,
                number: ply.whoAmI,         // which player to teleport
                number2: ClassLocation.X,       // x (in pixels)
                number3: ClassLocation.Y,      // y (in pixels)
                number4: 1                  // style = 1
            );
        }
    }

    public void SendToBase()
    {   
        foreach (Player ply in Players)
        {
            ply.SpawnX = (int)BaseLocation.X;
            ply.SpawnY = (int)BaseLocation.Y;
            ply.Teleport(ClassLocation);
            NetMessage.SendData(
                MessageID.TeleportEntity,
                remoteClient: -1,           // send to everyone
                ignoreClient: -1,           // don’t ignore anyone
                text: null,
                number: ply.whoAmI,         // which player to teleport
                number2: ClassLocation.X,       // x (in pixels)
                number3: ClassLocation.Y,      // y (in pixels)
                number4: 1                  // style = 1
            );
        }
        
    }

    public void PauseTeam()
    {
        foreach (Player ply in Players)
        {
            int buffTicks = 15 * 60;
            ply.AddBuff(BuffID.Webbed, buffTicks);
            NetMessage.SendData(MessageID.AddPlayerBuff, -1, -1, null, ply.whoAmI, BuffID.Webbed, buffTicks);
        }
    }
    
}