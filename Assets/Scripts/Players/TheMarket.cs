namespace Incorporation.Assets.Scripts.Players
{
    public class TheMarket : Player
    {
        public override bool IsTheMarket => true;
        
        public override bool IsLocal => true;
        
        public override bool IsRemote => false;

        void Awake()
        {
        }

        void Start()
        {
        }
    }
}
