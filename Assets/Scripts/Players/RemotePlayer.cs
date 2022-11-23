using UnityEngine;

namespace Incorporation.Assets.Scripts.Players
{
    public class RemotePlayer : Player
    {
        public override bool IsRemote => true;

        private Color _color;

        public override Color Color => _color;

        void Awake()
        {
            _color = Random.ColorHSV();
        }
    }
}
