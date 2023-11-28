using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : EntityBase
{
    [System.Serializable]
    public struct DeleteNode
    {
        public NodeBase nodeBase;
        public int count;

        public DeleteNode(NodeBase nodeBase, int count)
        {
            this.nodeBase = nodeBase;
            this.count = count;
        }
    };

    public float damage;
    public int maxNodeCount = 2;
    public int maxDeleteCount = 3;
    private int nodeCount;
    public List<DeleteNode> deleteNodes = new List<DeleteNode>();

    protected override void Start()
    {
        base.Start();

        nodeCount = UnityEngine.Random.Range(1, maxNodeCount);
        int nodeBaseLength = GameManager.instance.nodeBases.Length;
        for (int i = 0; i < nodeCount; i++)
        {
            NodeBase nodeBase = GameManager.instance.nodeBases[UnityEngine.Random.Range(0, nodeBaseLength)];
            int count = Random.Range(1, maxDeleteCount);
            DeleteNode node = new DeleteNode(nodeBase, count);
            deleteNodes.Add(node);
        }
    }

    public HashSet<Node> Attack()
    {
        Debug.Log("enemy attacking");
        HashSet<Node> result = new HashSet<Node>();

        foreach (DeleteNode deleteNode in deleteNodes)
        {
            List<Node> sameNode = new List<Node>();

            foreach (Node node in GameManager.instance.puzzle)
            {
                if (node.nodeBase == deleteNode.nodeBase)
                {
                    sameNode.Add(node);
                }
            }

            for (int i = 0; i < deleteNode.count; i++)
            {
                if (sameNode.Count == 0)
                    break;

                int index = Random.Range(0, sameNode.Count);
                result.Add(sameNode[index]);
                sameNode.RemoveAt(index);
                GameManager.instance.player.GetDamage(nodeBase, damage);
            }
        }

        return result;
    }
}
