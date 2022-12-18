using Shared.Resources;
using System;
using System.Collections.Generic;

namespace Shared.Players
{
    [Serializable]
    public class PlayerData
    {
        public PlayerData() {}

        public PlayerData(string id)
        {
            Id = id;
            Money = StartingMoney;
        }

        public string Id { get; private set; } = string.Empty;

        public string name { get; set; } = string.Empty;

        public bool IsHost { get; set; } = false;

        public bool IsReady { get; set; } = false;

        public int Money { get; private set; } = 0;

        private Dictionary<Resource, int> _resources = new();

        
        //public ReadOnlyDictionary<Resource, int> Resources => new ReadOnlyDictionary<Resource, int>(_resources);

        public int StartingMoney => 10;

        public int Income => 0;

        //public int GetAvailableResourceQuantity(Resource resource) => Resources.ContainsKey(resource) ? Resources[resource] : 0;
    }
}
