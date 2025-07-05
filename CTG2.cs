using System;
using Terraria.ModLoader;
using System.IO;
using System.Text.Json;
using ClassesNamespace;
using Terraria;
using CTG2.Content;
using CTG2.Content.Classes;
using CTG2.Content.ClientSide;
using CTG2.Content.ServerSide;
using Microsoft.Xna.Framework;
using Terraria.ID;


namespace CTG2
{
    public enum MessageType : byte
    {
        RequestStartGame = 0,  // client → server
        RequestEndGame = 1,  // client → server
        ServerGameStart = 2,  // server → client
        ServerGameEnd = 3,  // server → client
        RequestPause = 4,  // client -> server
        ServerGameUpdate = 5,  // server → client
        ServerTeleport = 6, // server -> client
        ServerSetSpawn = 7, // server -> client
        RequestSpawnNpc = 8,
        RequestSpawnProjectile = 9,
        RequestAddBuff = 10,
        RequestNextMap = 11,
        ServerChangeMap = 12,
        RequestTeleport = 13,
        RequestMaxHealth = 14,
        SyncNpcIndex = 15,
        SetNpcTeam = 16,
        RequestTeamChange = 17,
        RequestEnterSpectator = 18, // client → server
        RequestExitSpectator = 19,  // client → server  
        ServerSpectatorUpdate = 20,  // server → client

        EnterClassSelection = 21,
        ExitClassSelection = 22,
    }
    
    public class CTG2 : Mod
    {   
        public static int requestedNpcIndex = 0;
        public static Random randomGenerator = new Random();
        public static ClientConfig config = new ClientConfig();
        
        public override void Load()
        {
            base.Load();
            // load client config in
            using (var stream = GetFileStream($"Content/Classes/clientconfig.json"))
            using (var fileReader = new StreamReader(stream))
            {
                var jsonData = fileReader.ReadToEnd();
                try
                {
                    config = JsonSerializer.Deserialize<ClientConfig>(jsonData);
                }
                catch
                {
                    Main.NewText("Failed to load or parse client config file.", Microsoft.Xna.Framework.Color.Red);
                    return;
                }
            }
        }

        public override void HandlePacket(BinaryReader reader, int whoAmI)
        {   
            // GameManager
            var manager = ModContent.GetInstance<GameManager>();
            
            byte msgType = reader.ReadByte();
            switch (msgType) {
                // Client -> Server Packets (these cases will run on the Server)
                case (byte)MessageType.RequestStartGame:
                    manager.StartGame();
                    Console.WriteLine("Server Received Game Start Request!");
                    break;
                
                case (byte)MessageType.RequestEndGame:
                    manager.EndGame();
                    Console.WriteLine("Server Received Game End Request!");
                    break;
                case (byte)MessageType.RequestPause:
                    manager.PauseGame();
                    break;
                case (byte)MessageType.RequestSpawnNpc:
                    var npcX = reader.ReadInt32();
                    var npcY = reader.ReadInt32();
                    var npcType = reader.ReadInt32();
                    // use ai0 to store spawning player's team.
                    float ai0 = (float)reader.ReadInt32();
                    float ai1 = reader.ReadSingle();
                    int npcIndex = NPC.NewNPC(Main.LocalPlayer.GetSource_Misc("SpawnNPC"), npcX, npcY, npcType, 0, ai0, ai1);

                    break;
                case (byte)MessageType.SyncNpcIndex:
                    int syncedNpcIndex = reader.ReadInt32();
                    CTG2.requestedNpcIndex = syncedNpcIndex;
                    break;
                case (byte)MessageType.SetNpcTeam:
                    int NpcIndex = reader.ReadInt32();
                    int npcTeam = reader.ReadInt32();
                    Main.npc[NpcIndex].GetGlobalNPC<AllNpcs>().team = npcTeam;
                    break;
                case (byte)MessageType.RequestSpawnProjectile:
                    var spawnPos = new Vector2(reader.ReadSingle(), reader.ReadSingle());
                    var velocity = new Vector2(reader.ReadSingle(), reader.ReadSingle());
                    var projType = reader.ReadInt32();
                    var damage = reader.ReadInt32();
                    var knockback = reader.ReadSingle();

                    int projectileIndex = Projectile.NewProjectile(Main.LocalPlayer.GetSource_Misc("Class15Ability"), spawnPos, velocity, projType, damage, knockback);
                    break;
                case (byte)MessageType.RequestAddBuff:
                    var playerID = reader.ReadByte();
                    var buffType = reader.ReadInt32();
                    var time = reader.ReadInt32();

                    if (Main.player[playerID] is Player player)
                    {
                        Main.NewText("hitplayer", Color.Red);
                        player.AddBuff(buffType, time);
                    }

                    break;
                
                case (byte)MessageType.RequestNextMap:
                    string receivedMapName = reader.ReadString();


                    if (Enum.TryParse<MapTypes>(receivedMapName, true, out MapTypes mapType))
                    {

                        manager.queueMap(mapType);
                    }
                    else
                    {
                        // log?
                    }
                    break;
                
                // mainly used for clown ability
                case (byte)MessageType.RequestTeleport:
                    // teleport on server
                    var id2 = reader.ReadInt32();
                    int tpX2 = reader.ReadInt32();
                    int tpY2 = reader.ReadInt32();
                    var ply = Main.player[id2];
                    ply.Teleport(new Vector2(tpX2, tpY2));
                    Console.WriteLine("Server Received Teleport!");
                    // then tell all clients they were teleported
                    var mod = ModContent.GetInstance<CTG2>();
                    ModPacket packet2 = mod.GetPacket();
                    packet2.Write((byte)MessageType.ServerTeleport);
                    packet2.Write(ply.whoAmI);
                    packet2.Write(tpX2);
                    packet2.Write(tpY2);
                    packet2.Send();
                    break;
                
                case (byte)MessageType.RequestMaxHealth:
                    // teleport on server
                    int sender = reader.ReadInt32();
                    int newMax = reader.ReadInt32();
                    var playerToEdit = Main.player[sender];
                    playerToEdit.statLifeMax = newMax;
                    playerToEdit.statLifeMax2 = newMax;
                    break;
                case (byte)MessageType.RequestTeamChange:
                    int target = reader.ReadInt32();
                    int teamID = reader.ReadInt32();
                    Main.player[target].team = teamID;
                    NetMessage.SendData(MessageID.PlayerTeam, -1, -1, null, target, teamID);
                    break;
                
                case (byte)MessageType.RequestEnterSpectator:
                    int spectatorPlayerIndex = reader.ReadInt32();
                    manager.SetPlayerSpectator(spectatorPlayerIndex, true);
                    Console.WriteLine($"Server Received Enter Spectator Request from player {spectatorPlayerIndex}!");
                    break;
                
                case (byte)MessageType.RequestExitSpectator:
                    int exitSpectatorPlayerIndex = reader.ReadInt32();
                    manager.SetPlayerSpectator(exitSpectatorPlayerIndex, false);
                    Console.WriteLine($"Server Received Exit Spectator Request from player {exitSpectatorPlayerIndex}!");
                    break;
                
                // Server->Client Packets (these cases will run on the Client)
                case (byte)MessageType.ServerGameStart:
                    GameInfo.matchStage = 1;
                    Console.WriteLine("Client Received Game Start!");
                    break;
                case (byte)MessageType.ServerGameEnd:
                    GameInfo.matchStage = 0;
                    Console.WriteLine("Client Received Game End!");
                    break;
                case (byte)MessageType.ServerTeleport:
                    var id = reader.ReadInt32();
                    int tpX = reader.ReadInt32();
                    int tpY = reader.ReadInt32();
                    Main.player[id].Teleport(new Vector2(tpX, tpY));
                    Console.WriteLine("Client Received Teleport!");
                    break;
                case (byte)MessageType.ServerSetSpawn:
                    var localPlayer = Main.player[Main.myPlayer];
                    int spawnX = reader.ReadInt32();
                    int spawnY = reader.ReadInt32();
                    localPlayer.ChangeSpawn(spawnX, spawnY);
                    Console.WriteLine("Client Received Spawn Update!");
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
                
                case (byte)MessageType.ServerSpectatorUpdate:
                    int playerIndex = reader.ReadInt32();
                    bool isSpectator = reader.ReadBoolean();
                    if (playerIndex == Main.myPlayer)
                    {
                        Main.player[playerIndex].ghost = isSpectator;
                        if (isSpectator)
                        {
                            Main.player[playerIndex].respawnTimer = 9999;
                            Main.player[playerIndex].team = 0; 
                        }
                        else
                        {
                            Main.player[playerIndex].respawnTimer = 0;
                        }
                    }
                    Console.WriteLine($"Client Received Spectator Update: Player {playerIndex} is now {(isSpectator ? "spectator" : "active")}!");
                    break;
                
                case (byte)MessageType.EnterClassSelection:
                    int index = reader.ReadInt32();
                    int team = reader.ReadInt32();
                    bool gameStart = reader.ReadBoolean(); // different logic if game has started 
                    manager.startPlayerClassSelection(index, team, gameStart);
                    break;
                case (byte)MessageType.ExitClassSelection:
                    int pIndex = reader.ReadInt32();
                    int pTeam = reader.ReadInt32();
                    manager.endPlayerClassSelection(pIndex, pTeam);
                    break;              
                default:
                    Logger.WarnFormat("CTG2: Unknown Message type: {0}", msgType);
                    break;
            }
        }


        public void setRequestedNpcIndex(int index)
        {
            CTG2.requestedNpcIndex = index;
            var mod = ModContent.GetInstance<CTG2>();
            ModPacket packet = mod.GetPacket();
            packet.Write((byte)MessageType.SyncNpcIndex);
            packet.Write(index);
            packet.Send();
        }

        public static void SendEnterSpectatorRequest(int playerIndex)
        {
            var mod = ModContent.GetInstance<CTG2>();
            ModPacket packet = mod.GetPacket();
            packet.Write((byte)MessageType.RequestEnterSpectator);
            packet.Write(playerIndex);
            packet.Send();
        }
        
        public static void SendExitSpectatorRequest(int playerIndex)
        {
            var mod = ModContent.GetInstance<CTG2>();
            ModPacket packet = mod.GetPacket();
            packet.Write((byte)MessageType.RequestExitSpectator);
            packet.Write(playerIndex);
            packet.Send();
        }
    }
}
