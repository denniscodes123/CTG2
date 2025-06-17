using System;
using Terraria.ModLoader;
using System.IO;
using Terraria;
using CTG2.Content;
using CTG2.Content.ClientSide;
using CTG2.Content.ServerSide;
using Microsoft.Xna.Framework;


namespace CTG2
{   
    public enum MessageType : byte
    {
        RequestStartGame = 0,  // client → server
        RequestEndGame   = 1,  // client → server
        ServerGameStart  = 2,  // server → client
        ServerGameEnd    = 3,  // server → client
        RequestPause  = 4,  // client -> server
        ServerGameUpdate  = 5,  // server → client
        RequestClass   = 6,  // client → server
        RequestAbility = 7, // client -> server
        ServerTeleport = 8, // server -> client
    }
    
    public class CTG2 : Mod
    {
        // Custom Packet IDs: 0 == Start Game, 1 == Stop Game
        public override void HandlePacket(BinaryReader reader, int whoAmI)
        {   
            // MyPlayer
            var thisPlayer = Main.LocalPlayer.GetModPlayer<MyPlayer>();
            // GameManager
            var manager = ModContent.GetInstance<GameManager>();
            
            byte msgType = reader.ReadByte();
            switch (msgType) {
                // Client -> Server Packets
                case (byte)MessageType.RequestStartGame:
                    manager.StartGame();
                    Console.WriteLine("Server Received Game Start Request!");
                    break;
                
                case (byte)MessageType.RequestEndGame:
                    manager.EndGame();
                    Console.WriteLine("Server Received Game End Request!");
                    break;
                
                case(byte)MessageType.RequestClass:
                    // TODO: Give inventory to player
                    break;
                case (byte)MessageType.RequestPause:
                    manager.PauseGame();
                    break;
                case (byte)MessageType.RequestAbility:
                    // TODO: run ability code here
                    break;
                // Server->Client Packets
                case (byte)MessageType.ServerGameStart:
                    thisPlayer.EnterClassSelectionState();
                    Console.WriteLine("Client Received Game Start!");
                    break;
                case (byte)MessageType.ServerGameEnd:
                    thisPlayer.EnterLobbyState();
                    Console.WriteLine("Client Received Game End!");
                    break;
                
                case (byte)MessageType.ServerTeleport:
                    var local = Main.LocalPlayer;
                    int tpX = reader.ReadInt32();
                    int tpY = reader.ReadInt32();
                    local.Teleport(new Vector2(tpX, tpY), 1);
                    Console.WriteLine("Client Received Teleport!");
                    break;
                
                // Gems Status Updates
                case (byte)MessageType.ServerGameUpdate:
                    // Populate GameInfo fields
                    GameInfo.matchStage = reader.ReadInt32();
                    GameInfo.matchTime = reader.ReadInt32();
                    GameInfo.blueGemX = reader.ReadInt32();
                    GameInfo.redGemX = reader.ReadInt32();
                    GameInfo.blueGemCarrier = reader.ReadString();
                    GameInfo.redGemCarrier = reader.ReadString();
                    break;
                default:
                    Logger.WarnFormat("CTG2: Unknown Message type: {0}", msgType);
                    break;
            }
        }
    }
}