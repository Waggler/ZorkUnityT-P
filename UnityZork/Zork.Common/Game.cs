using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace Zork.Common //Zork
{
    public class Game : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public World World { get; private set; }

        public string StartingLocation { get; set; }
        
        public string WelcomeMessage { get; set; }
        
        public string ExitMessage { get; set; }

        [JsonIgnore]
        public Player Player { get; private set; }

        [JsonIgnore]
        public bool IsRunning { get; set; }
        //These ignores were not added in class
        //[JsonIgnore]
        public IInputService Input { get; set; }
        
        //[JsonIgnore]
        public IOutputService Output { get; set; }


        [JsonIgnore]
        public Dictionary<string, Command> Commands { get; private set; }

        public Game(World world, Player player)
        {
            World = world;
            Player = player;

            Commands = new Dictionary<string, Command>()
            {
                { "QUIT", new Command("QUIT", new string[] { "QUIT", "Q", "BYE" }, Quit) },
                { "LOOK", new Command("LOOK", new string[] { "LOOK", "L" }, Look) },
                { "NORTH", new Command("NORTH", new string[] { "NORTH", "N" }, game => Move(game, Directions.North)) },
                { "SOUTH", new Command("SOUTH", new string[] { "SOUTH", "S" }, game => Move(game, Directions.South)) },
                { "EAST", new Command("EAST", new string[] { "EAST", "E"}, game => Move(game, Directions.East)) },
                { "WEST", new Command("WEST", new string[] { "WEST", "W" }, game => Move(game, Directions.West)) },
            };
        }

        //from the zork in unity vid, I think this is handled in Program actually
        /*
        public static void StartFromFile(string gameFileName, IInputService input, IOutputService output)
        {
            if (!File.Exists(gameFileName))
            {
                throw new FileNotFoundException("Expected file.", gameFileName);
            }

            Start(File.ReadAllText(gameFileName, input, output);

        }
        
        
        public static Game Load(string jsonString)
        {
            Game game = JsonConvert.DeserializeObject<Game>(jsonString);
            game.Player = game.World.SpawnPlayer();
        }

        private static void Input_InputReceivedHandler(object sender, string e)
        {
            Room previousRoom = null;'
            Output.WriteLine(Player.Location);
            if (previousRoom != Player.Location)
            {
                CommandManager.PerformCommand(this, "Look");
                previousRoom = Player.Location;
            }
        }

        */

        public void Start(IInputService input, IOutputService output)
        {
            Assert.IsNotNull(input);
            Input = input;
            Input.InputReceived += InputReceivedHandler;

            Assert.IsNotNull(output);
            Output = output;

            IsRunning = true;

        }

        private void InputReceivedHandler(object sender, string commandString)
        {

            Command foundCommand = null;
            foreach (Command command in Commands.Values)
            {
                if (command.Verbs.Contains(commandString))
                {
                    foundCommand = command;
                    break;
                }
            }

            if (foundCommand != null)
            {
                foundCommand.Action(this);
            }
            else
            {
                Output.WriteLine("Unknown command.");
            }
        }

        private static void Move(Game game, Directions direction)
        {
            if (game.Player.Move(direction) == false)
            {
                game.Output.WriteLine("The way is shut!");
            }
        }

        public static void Look(Game game) => game.Output.WriteLine(game.Player.Location.Description);

        private static void Quit(Game game) => game.IsRunning = false;

        [OnDeserialized]
        private void OnDeserialized(StreamingContext context) => Player = new Player(World, StartingLocation);
    }
}