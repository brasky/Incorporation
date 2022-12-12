namespace Shared.Resources
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
