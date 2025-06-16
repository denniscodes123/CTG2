using Terraria.ModLoader;
using System.IO;
using Terraria;
using CTG2.Content;
using CTG2.Content.ServerSide;
	

namespace CTG2
{
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
            byte msgType = reader.ReadByte();
            switch (msgType) {
                case 0:
                    GameManager.StartGame();
                    break;
                case 1:
                    GameManager.EndGame();
                    break;
                case 2:
                    break;
                case 3:
                    break;
                case 4:
                    break;
                default:
                    Logger.WarnFormat("CTG2: Unknown Message type: {0}", msgType);
                    break;
            }
        }
    }
}