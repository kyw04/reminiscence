using Map;
using UnityEditor;
using UnityEngine;

namespace Map
{
    public enum NodeType
    {
        MinorEnemy,
        EliteEnemy,
        RestSite,
        Treasure,
        Store,
        Boss,
        Mystery
    }
}

namespace Map
{
    [CreateAssetMenu(fileName = "NewNodeBlueprint", menuName = "Map/Node Blueprint")]
    public class NodeBlueprint : ScriptableObject
    {
        public Sprite sprite;
        public Sprite[] sprites;
        public NodeType nodeType;
    }
}

