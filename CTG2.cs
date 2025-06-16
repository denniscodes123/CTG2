using System;
using Terraria.ModLoader;
using System.IO;
using Terraria;
using CTG2.Content;
using CTG2.Content.ServerSide;
	

namespace CTG2
{   
    public enum MessageType : byte
    {
        RequestStartGame = 0,  // client → server
        RequestEndGame   = 1,  // client → server
        ServerGameStart  = 2,  // server → client
        ServerGameEnd    = 3,  // server → client
        ServerGamePause  = 4,  // server → client
    }
    
    public class CTG2 : Mod
    {
        public static GameManager GameManager { get; private set; }
        
        public override void Load()
        {
            if (Main.dedServ)
            {
                GameManager = new GameManager();
            }

        }
        
        // Custom Packet IDs: 0 == Start Game, 1 == Stop Game
        public override void HandlePacket(BinaryReader reader, int whoAmI)
        {   
            var ThisPlayer = Main.LocalPlayer.GetModPlayer<MyPlayer>();
            byte msgType = reader.ReadByte();
            switch (msgType) {
                // Client -> Server Packets
                case (byte)MessageType.RequestStartGame:
                    GameManager.StartGame();
                    Console.WriteLine("Server Received Game Start Request!");
                    break;
                
                case (byte)MessageType.RequestEndGame:
                    GameManager.EndGame();
                    break;
                
                // Server->Client Packets
                case (byte)MessageType.ServerGameStart:
                    ThisPlayer.EnterClassSelectionState();
                    Console.WriteLine("Client Received Game Start!");
                    break;
                case (byte)MessageType.ServerGameEnd:
                    ThisPlayer.EnterLobbyState();
                    break;
                case (byte)MessageType.ServerGamePause:
                    ThisPlayer.TogglePause();
                    break;
                
                // Gems Status Updates
                case 5:
                    break;
                case 6:
                    break;
                default:
                    Logger.WarnFormat("CTG2: Unknown Message type: {0}", msgType);
                    break;
            }
        }
    }
}