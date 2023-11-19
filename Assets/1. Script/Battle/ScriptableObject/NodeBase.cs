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
    public ParticleSystem deleteParticle;
}
