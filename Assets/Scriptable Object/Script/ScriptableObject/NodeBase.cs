using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public enum NodeType
{ 
    None,
    Light,
    Fire,
    Wind,
    Soil,
    Water,
}

[CreateAssetMenu(fileName = "NodeBase", menuName = "Tools/NodeBase")]
public class NodeBase : ScriptableObject
{
    public NodeType nodeType;
    public NodeType strong, weak;
    public Mesh mesh;
    public Material material;
    public Color baseColor;
    public GameObject deleteParticle;
    public GameObject AttackParticle;

    public float GetTotalDamage(NodeBase attackerNodeBase, float damage)
    {
        float totalDamage = damage;
        if (attackerNodeBase.strong == this.nodeType)
        {
            Debug.Log("strong attack");
            totalDamage *= 1.5f;
        }
        if (attackerNodeBase.weak == this.nodeType)
        {
            Debug.Log("weak attack");
            totalDamage *= 0.5f;
        }

        return totalDamage;
    }
}
