using UnityEngine;

namespace Incorporation.Assets.Scripts.Players
{
    public class TheMarket : Player
    {
        public override bool IsTheMarket => true;
        
        public override bool IsLocal => true;
        
        public override bool IsRemote => false;

        public override Color Color => new Color(0.86f, 0.86f, 0.86f);

        void Awake()
        {
        }

        void Start()
        {
        }
    }
}
