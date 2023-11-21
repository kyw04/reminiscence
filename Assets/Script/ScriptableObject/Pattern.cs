using UnityEngine;

[System.Serializable]
public class NodePattern
{
    public NodeBase[] index = new NodeBase[3];
}

[CreateAssetMenu(fileName = "NodePattern", menuName = "Tools/NodePattern")]
public class Pattern : ScriptableObject
{
    [HideInInspector]
    public NodePattern[] nodePatternTemp = new NodePattern[3];
    public NodeType[,] nodePatternType = new NodeType[3, 3];

    public int damage;
}
