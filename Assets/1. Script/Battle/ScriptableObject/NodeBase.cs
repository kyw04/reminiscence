using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum NodeType
{ 
    None,
    Fire,
    Wind,
    Soil,
    Water
}

[CreateAssetMenu(fileName = "NodeBase", menuName = "Tools/NodeBase")]
public class NodeBase : ScriptableObject
{
    public NodeType nodeType;
    public Mesh mesh;
    public Material material;
    public Color baseColor;

    public void CopyNodeBase(NodeBase target)
    {
        if (target == null) return;

        target.nodeType = this.nodeType;
        target.mesh = this.mesh;
        target.material = this.material;
        target.baseColor = this.baseColor;
    }
}
