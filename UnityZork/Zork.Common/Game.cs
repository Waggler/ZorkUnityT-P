using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO; //Remove?
using System.Linq;
using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace Zork.Common //Zork
{
    public class Game : INotifyPropertyChanged
    {

        public event EventHandler GameStarted;
        public event EventHandler GameStopped;

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

        /*
        public void Start(string gameJsonString, IInputService input, IOutputService output) // should we have added Instance. like he does at 5:56?
        {
            Assert.IsNotNull(input);
            Instance = LoaderOptimization(gameJsonString);
            Input = input;
            Input.InputReceived += InputReceivedHandler;

            Assert.IsNotNull(output);
            Output = output;

            IsRunning = true;// Comment out? or replace with Instance.IsRunning = true;
            /*
            Instance = Load(gameJsonString);
            Instance.Input = input;
            Instance.Output = output;
            Instance.LoadCommands();
            Instance.DisplayWelcomeMessage();
            Instance.IsRunning = true;
            */
        Instance.Input.InputReceived += Instance.InputReceivedHandler;

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
        GameStarted?.Invoke(this, EventArgs.Empty); //Swap to? game.GameStarted?.Invoke(game, EventArgs.Empty);

    }

    private void InputReceivedHandler(object sender, string inputString)
    {

        Command foundCommand = null;
        foreach (Command command in Commands.Values)
        {
            if (command.Verbs.Contains(inputString))
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

    private static void Quit(Game game)
    {
        game.IsRunning = false;
        game.GameStopped?.Invoke(game, EventArgs.Empty);
    }

    [OnDeserialized]
    private void OnDeserialized(StreamingContext context) => Player = new Player(World, StartingLocation);
}
}