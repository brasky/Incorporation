using UnityEngine;

namespace Incorporation.Assets.ScriptableObjects
{
    [CreateAssetMenu(menuName = "DataContainers/TextData")]
    public class TextData : ScriptableObject
    {
        [TextArea]
        public string Text;
    }
}
