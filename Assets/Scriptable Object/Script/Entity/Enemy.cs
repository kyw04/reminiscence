using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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
        if (GameStateManager.Instance.playerWinMode) health = 1;
        base.Start();

        nodeCount = UnityEngine.Random.Range(1, maxNodeCount);
        int nodeBaseLength = GameManager.instance.nodeBases.Length;
        nodeBase = GameManager.instance.nodeBases[UnityEngine.Random.Range(0, nodeBaseLength)];
        for (int i = 0; i < nodeCount; i++)
        {
            NodeBase nodeBase = GameManager.instance.nodeBases[UnityEngine.Random.Range(0, nodeBaseLength)];
            int count = Random.Range(1, maxDeleteCount);
            DeleteNode node = new DeleteNode(nodeBase, count);
            deleteNodes.Add(node);
        }
    }

    public IEnumerator MoveAndComeBack(Vector3 endPos, float speed, int attackCount)
    {
        Vector3 startPos = transform.position;
        float time = 0;
        while (transform.position != endPos)
        {
            yield return new WaitForSeconds(Time.deltaTime);

            transform.position = Vector3.Lerp(startPos, endPos, time);
            time += Time.deltaTime * speed;
        }

        for (int i = 0; i < attackCount; i++)
        {
            // play attack animation
            
            yield return new WaitForSeconds(0.5f); // animation delay
            GameManager.instance.player.GetDamage(nodeBase, damage);
        }

        time = 0;
        while (transform.position != startPos)
        {
            yield return new WaitForSeconds(Time.deltaTime);

            transform.position = Vector3.Lerp(endPos, startPos, time);
            time += Time.deltaTime * speed;
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

            Vector3 pos = GameManager.instance.player.transform.position + transform.localScale;
            pos.y = GameManager.instance.player.transform.position.y;
            for (int i = 0; i < deleteNode.count; i++)
            {
                if (sameNode.Count == 0)
                    break;

                int index = Random.Range(0, sameNode.Count);
                result.Add(sameNode[index]);
                sameNode.RemoveAt(index);
            }

            if (deleteNode.count > 0)
                StartCoroutine(MoveAndComeBack(pos, 10.0f, deleteNode.count));
        }

        return result;
    }
    public override void Death()
    {
        GameManager.instance.EndBattle(true);
        
    }
}
