using Shared.Resources;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Shared.Players
{
    public struct PlayerColor
    {
        public PlayerColor(float r, float g, float b)
        {
            R = r;
            G = g;
            B = b;
        }

        public float R { get; set; }
        public float G { get; set; }
        public float B { get; set; }

        public static implicit operator Color(PlayerColor c)
        {
            return new Color(c.R, c.G, c.B);
        }
    }

    [Serializable]
    public class PlayerData
    {
        public PlayerData() {}

        public PlayerData(string id)
        {
            Id = id;
            Name = id;
            Money = StartingMoney;
            var rand = new System.Random();
            PlayerColor = new PlayerColor((float)rand.Next()/int.MaxValue, (float)rand.Next() / int.MaxValue, (float)rand.Next() / int.MaxValue);
        }

        public string Id { get; private set; } = string.Empty;

        public string Name { get; set; } = string.Empty;

        public bool IsHost { get; set; } = false;

        public bool IsReady { get; set; } = false;

        public int Money { get; private set; } = 0;

        private Dictionary<Resource, int> _resources = new();

        public PlayerColor PlayerColor { get; set; }
        
        //public ReadOnlyDictionary<Resource, int> Resources => new ReadOnlyDictionary<Resource, int>(_resources);

        public int StartingMoney => 10;

        public int Income => 0;

        //public int GetAvailableResourceQuantity(Resource resource) => Resources.ContainsKey(resource) ? Resources[resource] : 0;
    }
}
