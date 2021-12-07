using System;
using System.Collections.Generic;
using System.ComponentModel;
using Newtonsoft.Json;

namespace Zork
{
    public class Item : IEquatable<Item>, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        [JsonProperty(Order = 1)]
        public string Name { get; set; }

        [JsonProperty(Order = 2)]
        public string Description { get; set; }

        //[JsonProperty(PropertyName = "Neighbors", Order = 3)]
        //private Dictionary<Directions, string> NeighborNames { get; set; } = new Dictionary<Directions, string>();

        //[JsonIgnore]
        //public IReadOnlyDictionary<Directions, Room> Neighbors => _neighbors;

        public Item(string name = null)
        {
            Name = name;
        }

        public static bool operator ==(Item lhs, Item rhs)
        {
            if (ReferenceEquals(lhs, rhs))
            {
                return true;
            }

            if (lhs is null || rhs is null)
            {
                return false;
            }

            return string.Compare(lhs.Name, rhs.Name, ignoreCase: true) == 0;
        }

        public static bool operator !=(Item lhs, Item rhs) => !(lhs == rhs);

        public override bool Equals(object obj) => obj is Item item && this == item;

        public bool Equals(Item other) => this == other;

        public override string ToString() => Name;

        public override int GetHashCode() => Name.GetHashCode();

        /*
        //---------------------//
        public void UpdateNeighbors(World world)
        //---------------------//
        {
            _neighbors.Clear();
            foreach (var entry in NeighborNames)
            {
                _neighbors.Add(entry.Key, world.RoomsByName[entry.Value]);
            }
        }//END UpdateNeighbors

        //---------------------//
        public void RemoveNeighbor(Directions direction)
        //---------------------//
        {
            _neighbors.Remove(direction);
            NeighborNames.Remove(direction);

        }//END RemoveNeighbor

        //---------------------//
        public void AssignNeighbor(Directions direction, Room neighbor)
        //---------------------//
        {
            _neighbors[direction] = neighbor;
            NeighborNames[direction] = neighbor.Name;

        }//END AssignNeighbor

        private Dictionary<Directions, Room> _neighbors = new Dictionary<Directions, Room>();
        */

    }//END Room
}