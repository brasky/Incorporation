using Shared.Resources;
using Shared.Players;
using System;

namespace Shared.Tiles
{
    public class TileData
    {
        public int X { get; set; }
     
        public int Y { get; set; }

        public int Price => 1;

        public PlayerData Owner { get; set; }

        public Resource[] Resources { get; set; }
        public ResourceCost[] ResourceCosts { get; set; }

        private int maxPossibleYield = 10;

        public bool IsImproved { get; set; } = false;

        public int Yield { get; set; }

        public TileData() {}

        public TileData(int x, int y)
        {
            X = x;
            Y = y;

            var random = new Random();
            var randomResource = (Resource)random.Next(0, Enum.GetNames(typeof(Resource)).Length);
            Resources = new Resource[1] { randomResource };
            Yield = random.Next(1, maxPossibleYield);

            //For now just setting the resource cost equal to the random resource and the yield.
            ResourceCosts = new ResourceCost[1];
            ResourceCosts[0] = new ResourceCost(randomResource, Yield);

        }
    }
}
