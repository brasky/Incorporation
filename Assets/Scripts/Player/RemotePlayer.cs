using Newtonsoft.Json.Bson;
using Unity.VisualScripting;
using UnityEngine;

namespace Incorporation.Assets.Scripts.Player
{
    public class RemotePlayer : Player
    {
        public override bool IsRemote => true;

        void Start()
        {

        }

        void OnDestroy()
        {

        }
    }
}
