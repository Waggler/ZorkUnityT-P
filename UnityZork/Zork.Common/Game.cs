using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using Zork.Common;

namespace Zork
{
    public class Game : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public World World { get; private set; }

        public string StartingLocation { get; set; }

        public Room previousLocation { get; set; }

        public string WelcomeMessage { get; set; }

        public string ExitMessage { get; set; }

        [JsonIgnore]
        public Player Player { get; private set; }
        [JsonIgnore]
        public bool IsRunning { get; set; }

        public IInputService Input { get; set; }

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
                { "NORTH", new Command("NORTH", new string[] { "NORTH", "N" }, game => Move(game, Directions.NORTH)) },
                { "SOUTH", new Command("SOUTH", new string[] { "SOUTH", "S" }, game => Move(game, Directions.SOUTH)) },
                { "EAST", new Command("EAST", new string[] { "EAST", "E"}, game => Move(game, Directions.EAST)) },
                { "WEST", new Command("WEST", new string[] { "WEST", "W" }, game => Move(game, Directions.WEST)) },
                { "REWARD", new Command("REWARD", new string[] { "REWARD", "R"}, Reward) },
                { "SCORE", new Command("SCORE", new string[] { "SCORE"}, ScoreCheck) },
            };
        }

        public void Start(IInputService input, IOutputService output)
        {
            Assert.IsNotNull(input);
            Input = input;
            Input.InputRecieved += InputRecievedHandler;

            Assert.IsNotNull(output);
            Output = output;

            IsRunning = true;
        }

        private void InputRecievedHandler(object sender, string commandString)
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
                Player.Moves++;

                
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
            else
            {
                game.Output.WriteLine($"You moved {direction}");
            }

            if (game.previousLocation != game.Player.Location)
            {
                game.previousLocation = game.Player.Location;
                Look(game);
            }
            string value = " ";
            game.Output.WriteLine(value);
        }

        public static void Look(Game game) => game.Output.WriteLine(game.Player.Location.Description);

        private static void Quit(Game game) => game.IsRunning = false;

        private static void Reward(Game game) => game.Player.Score += 1;

        private static void ScoreCheck(Game game)
        {
            if (game.Player.Moves > 0)
            {
                game.Output.WriteLine($"Your score is:{game.Player.Score} and you have made {game.Player.Moves} move(s)");
            }
        }

        [OnDeserialized]
        private void OnDeserialized(StreamingContext context) => Player = new Player(World, StartingLocation);
    }
}