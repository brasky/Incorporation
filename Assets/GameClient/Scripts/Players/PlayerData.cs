using Incorporation.Assets.Scripts.Resources;
using System.Collections.Generic;
using UnityEngine;

namespace Incorporation.Assets.Scripts.Players
{
    public class PlayerData : MonoBehaviour
    {
        public int Money = 0;
        public Dictionary<Resource, int> Resources = new();
    }
}
