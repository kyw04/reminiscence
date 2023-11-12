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
    public NodeBase(NodeBase nodeBase)
    {
        this.nodeType = nodeBase.nodeType;
        this.mesh = nodeBase.mesh;
        this.material = nodeBase.material;
        this.baseColor = nodeBase.baseColor;
    }
}
