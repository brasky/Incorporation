using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Incorporation.Assets.Scripts.Players
{
    public class TheMarket : Player
    {
        public override bool IsTheMarket => true;
        
        public override bool IsLocal => true;
        
        public override bool IsRemote => false;

    }
}
