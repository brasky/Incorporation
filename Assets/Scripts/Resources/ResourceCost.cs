using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Incorporation.Assets.Scripts.Resources
{
    public class ResourceCost
    {
        public Resource Resource { get; private set; }
        public int Quantity { get; private set; }

        public ResourceCost(Resource resource, int quantity)
        {
            Resource = resource;
            Quantity = quantity;
        }
    }
}
