using System.Collections.Generic;
using System.ComponentModel;
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

        [JsonIgnore]
        public string commandSubject { get; set; }

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

        #region Methods

        //---------------------//
        public Game(World world, Player player)
        //---------------------//
        {
            World = world;
            Player = player;

            Commands = new Dictionary<string, Command>()
            {
                { "QUIT", new Command("QUIT", new string[] { "QUIT", "Q", "CYA", "BYE", "ADIOS" }, Quit) },
                { "LOOK", new Command("LOOK", new string[] { "LOOK", "L" }, Look) },
                { "NORTH", new Command("NORTH", new string[] { "NORTH", "N" }, game => Move(game, Directions.NORTH)) },
                { "SOUTH", new Command("SOUTH", new string[] { "SOUTH", "S" }, game => Move(game, Directions.SOUTH)) },
                { "EAST", new Command("EAST", new string[] { "EAST", "E"}, game => Move(game, Directions.EAST)) },
                { "WEST", new Command("WEST", new string[] { "WEST", "W" }, game => Move(game, Directions.WEST)) },
                { "REWARD", new Command("REWARD", new string[] { "REWARD", "R"}, Reward) },
                { "SCORE", new Command("SCORE", new string[] { "SCORE"}, ScoreCheck) },
                { "INVENTORY", new Command("INVENTORY", new string[] { "INVENTORY", "I"}, ShowInventory) },
                { "GET", new Command("GET", new string[] { "GET", "G", "GRAB", "TAKE"}, game => Get(game, commandSubject)) },
                { "DROP", new Command("DROP", new string[] { "DROP", "D", "LEAVE", }, game => Drop(game, commandSubject)) },

            };

        }//END Game

        //---------------------//
        public void Start(IInputService input, IOutputService output)
        //---------------------//
        {
            Assert.IsNotNull(input);
            Input = input;
            Input.InputRecieved += InputRecievedHandler;

            Assert.IsNotNull(output);
            Output = output;

            IsRunning = true;

        }//END Start


        #region Game Functions


        //---------------------//
        private void InputRecievedHandler(object sender, string commandString)
        //---------------------//
        {

            Command foundCommand = null;
            string[] sortedString = commandString.Split(' ');

            foreach (Command command in Commands.Values)
            {

                if (command.Verbs.Contains(sortedString[0]))
                {
                    if (command.Verbs.Contains("GET"))
                    {
                        if(sortedString.Length > 1)
                        {
                            commandSubject = sortedString[1];
                        }
                        else
                        {
                            Output.WriteLine("What are you taking?");
                            Output.Write(" ");
                            return;
                        }
                    }
                    else if (command.Verbs.Contains("DROP"))
                    {
                        if (sortedString.Length > 1)
                        {
                            commandSubject = sortedString[1];
                        }
                        else
                        {
                            Output.WriteLine("What are you dropping?");
                            Output.Write(" ");
                            return;
                        }
                    }

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
                Output.Write(" ");
            }

        }//END InputRecievedHandler

        //---------------------//
        private static void Move(Game game, Directions direction)
        //---------------------//
        {
            if (game.Player.Move(direction) == false)
            {
                game.Output.WriteLine("The way is shut!");
            }
            else
            {
                game.Output.WriteLine(game.Player.Location);
            }

            if (game.previousLocation != game.Player.Location)
            {
                game.previousLocation = game.Player.Location;
                Look(game);
            }
            string value = " ";
            game.Output.Write(value);

        }//END Move

        //---------------------//
        public static void Look(Game game)
        //---------------------//
        {
            game.Output.WriteLine(game.Player.Location.Description);
            if (game.Player.Location.Items != null)
            {
                foreach(Item item in game.Player.Location.Items)
                {
                    game.Output.WriteLine(item.Description);
                }

            }
        }

        //---------------------//
        private static void Quit(Game game) => game.IsRunning = false;
        //---------------------//

        //---------------------//
        private static void Reward(Game game) => game.Player.Score += 1;
        //---------------------//

        //---------------------//
        private static void Get(Game game, string subject)
        //---------------------//
        {
            //game.Player.Inventory
            foreach (Item item in game.Player.Location.Items)
            {
                if (item.Name == subject)
                {
                    game.Player.Inventory.Add(item);
                    game.Player.Location.Items.Remove(item);

                    game.Output.WriteLine($"Took {subject}");
                    game.Output.Write(" ");
                    return;
                }
            }
            game.Output.WriteLine($"That isn't isn't here");
            game.Output.Write(" ");

        }

        //---------------------//
        private static void Drop(Game game, string subject)
        //---------------------//
        {
            foreach (Item item in game.Player.Inventory)
            {
                if (item.Name == subject)
                {
                    game.Player.Location.Items.Add(item);
                    game.Player.Inventory.Remove(item);

                    game.Output.WriteLine($"Dropped {subject}");
                    game.Output.Write(" ");
                    return;
                }
            }
            game.Output.WriteLine($"You don't have that");
            game.Output.Write(" ");

        }

        //---------------------//
        private static void ScoreCheck(Game game)
        //---------------------//
        {
            if (game.Player.Moves > 0)
            {
                game.Output.WriteLine($"Your score is: {game.Player.Score} and you have made {game.Player.Moves} move(s)");
                game.Output.Write(" ");
            }

        }//END ScoreCheck

        //---------------------//
        private static void ShowInventory(Game game)
        //---------------------//
        {
            if (game.Player.Inventory.Count > 0)
            {
                game.Output.WriteLine("Inventory:");

                foreach (Item item in game.Player.Inventory)
                {
                    game.Output.WriteLine(item.Name);
                }
                game.Output.Write(" ");
            }
            else
            {
                game.Output.WriteLine("You have no items.");
                game.Output.Write(" ");

            }

        }//END ScoreCheck


        #endregion Game Functions


        [OnDeserialized]
        private void OnDeserialized(StreamingContext context) => Player = new Player(World, StartingLocation);


        #endregion Methods


    }//END Game

}