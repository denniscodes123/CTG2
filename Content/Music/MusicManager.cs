// using System.Collections.Generic;
// using Terraria;
// using Terraria.ID;
// using Terraria.ModLoader;

// namespace CTG2.Content.ServerSide;

// public enum MusicStage
// {
//     NoGame,
//     OngoingGame,
//     LastTwoMinutes,
//     Overtime
// }


// public class QueuedTrack
// {
//     public int MusicId { get; }
//     public MusicStage Stage { get; }

//     public QueuedTrack(int musicId, MusicStage stage)
//     {
//         MusicId = musicId;
//         Stage = stage;
//     }
// }

// public class MusicManager : ModSystem
// {

//     private static readonly List<QueuedTrack> MusicQueue = new();
    
//     public static int CurrentMusicId { get; internal set; } = 0;
//     private static int _lastPlayedMusicId = 0;

//     private static MusicStage _lastStage = MusicStage.NoGame;
    
//     private MusicStage GetCurrentStage()
//     {
//         var gameManager = ModContent.GetInstance<GameManager>();
//         if (gameManager == null || !gameManager.IsGameActive)
//         {
//             return MusicStage.NoGame;
//         }

//         if (gameManager.isOvertime) 
//         {
//             return MusicStage.Overtime;
//         }


//         int totalMatchDuration = (15 * 60 * 60); 
//         if (gameManager.MatchTime >= gameManager.matchStartTime + totalMatchDuration - (2 * 60 * 60))
//         {
//             return MusicStage.LastTwoMinutes;
//         }

//         return MusicStage.OngoingGame;
//     }




//     public override void PostUpdateWorld()
//     {
//         // This logic should only run on the server.
//         if (Main.netMode != NetmodeID.Server)
//         {
//             return;
//         }

//         MusicStage currentStage = GetCurrentStage();
//         int musicToPlay = 0;

//         // A change in stage forces a re-evaluation of the music.
//         if (currentStage != _lastStage)
//         {
//             // Find the highest priority song for the new stage.
//             musicToPlay = FindNextTrack(currentStage);
//         }



//         if (musicToPlay != 0 && musicToPlay != _lastPlayedMusicId)
//         {

//             var packet = Mod.GetPacket();
//             packet.Write((byte)MessageType.UpdateMusic);
//             packet.Write(musicToPlay);
//             packet.Send();


//             _lastPlayedMusicId = musicToPlay;
//             CurrentMusicId = musicToPlay;
//         }

//         _lastStage = currentStage;
//     }


//     private int FindNextTrack(MusicStage stage)
//     {

//         QueuedTrack trackToPlay = MusicQueue.Find(track => track.Stage == stage);

//         if (trackToPlay != null)
//         {
//             MusicQueue.Remove(trackToPlay); // Dequeue the song
//             return trackToPlay.MusicId;
//         }


//         switch (stage)
//         {
//             case MusicStage.NoGame:
//                 return MusicLoader.GetMusicSlot(Mod, "Assets/Music/clashroyaleOT");
//             case MusicStage.OngoingGame:
//                 return MusicLoader.GetMusicSlot(Mod, "Assets/Music/clashroyaleOT");
//             case MusicStage.LastTwoMinutes:
//                 return MusicLoader.GetMusicSlot(Mod, "Assets/Music/clashroyaleOT");
//             case MusicStage.Overtime:
//                 return MusicLoader.GetMusicSlot(Mod, "Assets/Music/clashroyaleOT");
//             default:
//                 return 0; 
//         }
//     }

// }