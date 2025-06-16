using System;
using Terraria;
using Terraria.ModLoader;
using Terraria.UI;
using Terraria.GameContent.UI;
using Terraria.GameContent;
using Terraria.ID;
using Microsoft.Xna.Framework;
using Terraria.Localization;
using System.Collections.Generic;
using Terraria.Chat;
using System.IO;
using Microsoft.Xna.Framework.Graphics;
using Newtonsoft.Json;
using CTG2.Content;
using System.Linq;


namespace CTG2.Content
{
    // This class holds your saved data. Static variables are accessible from anywhere in your mod.
    public class MapSave : ModSystem
    {
        public static Vector2 startPoint;
        public static Vector2 endPoint;
    }

    // This class defines the chat command.
    public class CheckPoints : ModCommand
    {
        public override CommandType Type => CommandType.Chat;


        public override string Command => "checkPoints";


        public override string Usage => "/checkPoints";

        public override string Description => "Displays the currently saved start and end points.";

        public override void Action(CommandCaller caller, string input, string[] args)
        {

            if (MapSave.startPoint == Vector2.Zero)
            {
                caller.Reply("Start point is not set.");
            }
            else
            {

                caller.Reply($"Start point is set to: {MapSave.startPoint.ToPoint()}");
            }

            // Check the endPoint
            if (MapSave.endPoint == Vector2.Zero)
            {
                caller.Reply("End point is not set.");
            }
            else
            {
                caller.Reply($"End point is set to: {MapSave.endPoint.ToPoint()}");
            }
        }
    }

    public class SaveWorld : ModCommand
    {
        public override CommandType Type => CommandType.Chat;
        public override string Command => "saveMap";
        public override string Usage => "/saveMap <name>";

        public override string Description => "saves world under <name>.json";

                public override void Action(CommandCaller caller, string input, string[] args)
        {
            if (args.Length == 0)
            {
                caller.Reply("Error: Please provide a name for the save file. Usage: " + Usage);
                return;
            }

            if (MapSave.startPoint == Vector2.Zero || MapSave.endPoint == Vector2.Zero)
            {
                caller.Reply("Error: Both a start point and an end point must be set first.");
                return;
            }

            try
            {
              
                TileSnapshot[,] savedTiles = WorldProperties.SaveRegion(
                    (int)(MapSave.startPoint.X / 16), (int)(MapSave.startPoint.Y / 16),
                    (int)(MapSave.endPoint.X / 16), (int)(MapSave.endPoint.Y / 16)
                );
                
    
                string json = JsonConvert.SerializeObject(savedTiles, Formatting.Indented);

  
                string saveDirectory = Path.Combine(Main.SavePath, "Mods", "CTG2", "MapSaves");
                Directory.CreateDirectory(saveDirectory); 
                string filePath = Path.Combine(saveDirectory, $"{args[0]}.json");
                
                File.WriteAllText(filePath, json);

                caller.Reply($"World region saved successfully to: {args[0]}.json");
            }
            catch (Exception e)
            {
                caller.Reply("An error occurred while saving the file.");
            
                Mod.Logger.Error("File save failed: " + e.Message, e);
            }
        }
    }
}