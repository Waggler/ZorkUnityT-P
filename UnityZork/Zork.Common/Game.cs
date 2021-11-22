using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using Microsoft.CodeAnalysis.Scripting;
using Microsoft.CodeAnalysis.CSharp.Scripting;
using Newtonsoft.Json;
using System.Text;

namespace Zork.Common //Zork
{
    public class Game : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public World World { get; private set; }

        public string StartingLocation { get; set; }

        [JsonProperty]
        public string WelcomeMessage = null;

        public string ExitMessage { get; set; }

        [JsonIgnore]
        public Player Player { get; private set; }

        [JsonIgnore]
        public bool IsRunning { get; }
        //These ignores were not added in class
        //[JsonIgnore]
        public IInputService Input { get; set; }

        //[JsonIgnore]
        public IOutputService Output { get; set; }

        [JsonIgnore]
        public static Game Instance { get; private set; }

        [JsonIgnore]
        public CommandManager CommandManager { get; }


        [JsonIgnore]
        public Dictionary<string, Command> Commands { get; private set; }

        public Game(World world, Player player)
        {
            World = world;
            Player = player;

            /*Commands = new Dictionary<string, Command>()
            {
                { "QUIT", new Command("QUIT", new string[] { "QUIT", "Q", "BYE" }, Quit) },
                { "LOOK", new Command("LOOK", new string[] { "LOOK", "L" }, Look) },
                { "NORTH", new Command("NORTH", new string[] { "NORTH", "N" }, game => Move(game, Directions.North)) },
                { "SOUTH", new Command("SOUTH", new string[] { "SOUTH", "S" }, game => Move(game, Directions.South)) },
                { "EAST", new Command("EAST", new string[] { "EAST", "E"}, game => Move(game, Directions.East)) },
                { "WEST", new Command("WEST", new string[] { "WEST", "W" }, game => Move(game, Directions.West)) },
            };*/
        }

        public Game() => CommandManager = new CommandManager();

        private void LoadCommands()
        {
            Type[] types = Assembly.GetExecutingAssembly().GetTypes();

            foreach (Type type in types)
            {
                CommandClassAttribute commandClassAttribute = type.GetCustomAttribute<CommandClassAttribute>();
                if (commandClassAttribute != null)
                {
                    MethodInfo[] methods = type.GetMethods();
                    foreach (MethodInfo method in methods)
                    {
                        CommandAttribute commandAttribute = method.GetCustomAttribute<CommandAttribute>();
                        if (commandAttribute != null)
                        {
                            Command command = new Command(commandAttribute.CommandName, commandAttribute.Verbs,
                                (Action<Game, CommandContext>)Delegate.CreateDelegate(typeof(Action<Game, CommandContext>), method));
                            CommandManager.AddCommand(command);
                        }
                    }
                }
            }
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

        return game;
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

        public static Game Load(string jsonString)
        {
            Game game = JsonConvert.DeserializeObject<Game>(jsonString);
            game.Player = game.World.SpawnPlayer();

            return game;
        }

        public static void Start(string gameJsonString, IInputService input, IOutputService output) // should we have added Instance. like he does at 5:56?
        {
            if (!File.Exists(gameJsonString))
            {
                throw new FileNotFoundException("Excepted File.", gameJsonString);
            }

            while (Instance == null || Instance.mIsRestarting)
            {
                Instance = Load(gameJsonString);
                Instance.Input = input;
                Instance.Output = output;
                Instance.LoadCommands();
                Instance.DisplayWelcomeMessage();
            }

            /*
            Assert.IsNotNull(input);
            Instance = LoaderOptimization(gameJsonString);
            Input = input;
            Input.InputReceived += InputReceivedHandler;

            Assert.IsNotNull(output);
            Output = output;

            IsRunning = true;// Comment out? or replace with Instance.IsRunning = true;
            */


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

        private void Run()
        {
            mIsRunning = true;
            Room previousRoom = null;
            while (mIsRunning)
            {
                Console.WriteLine(Player.Location);
                if (previousRoom != Player.Location)
                {
                    CommandManager.PerformCommand(this, "LOOK");
                    previousRoom = Player.Location;
                }

                Console.Write("\n> ");
                if (CommandManager.PerformCommand(this, Console.ReadLine().Trim()))
                {
                    Player.Moves++;
                }
                else
                {
                    Console.WriteLine("That's not a verb I recognize.");
                }
            }
        }

        public void Restart()
        {
            mIsRunning = false;
            mIsRestarting = true;
            Console.Clear();
        }

        public static void Look(Game game) => game.Output.WriteLine(game.Player.Location.Description);

        //private static void Quit(Game game) => game.IsRunning = false;
        public void Quit() => mIsRunning = false;

        private void LoadScripts()
        {
            foreach (string file in Directory.EnumerateFiles(ScriptDirectory, ScriptFileExtension))
            {
                try
                {
                    var scriptOptions = ScriptOptions.Default.AddReferences(Assembly.GetExecutingAssembly());
#if DEBUG
                    scriptOptions = scriptOptions.WithEmitDebugInformation(true)
                        .WithFilePath(new FileInfo(file).FullName)
                        .WithFileEncoding(Encoding.UTF8);
#endif
                    string script = File.ReadAllText(file);
                    CSharpScript.RunAsync(script, scriptOptions).Wait();
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error compiling script: {file} Error: {ex.Message}");
                }
            }
        }


        public bool ConfirmAction(string prompt)
        {
            Console.Write(prompt);

            while (true)
            {
                string response = Console.ReadLine().Trim().ToUpper();
                if (response == "YES" || response == "Y")
                {
                    return true;
                }
                else if (response == "NO" || response == "N")
                {
                    return false;
                }
                else
                {
                    Console.Write("Please answer yes or no.> ");
                }
            }
        }


        private void DisplayWelcomeMessage() => Console.WriteLine(WelcomeMessage);

        public static readonly Random Random = new Random();
        public static readonly string ScriptDirectory = "Scripts";
        private static readonly string ScriptFileExtension = "*.csx";


        private bool mIsRunning;
        private bool mIsRestarting;

        [OnDeserialized]
        private void OnDeserialized(StreamingContext context) => Player = new Player(World, StartingLocation);
    }
}