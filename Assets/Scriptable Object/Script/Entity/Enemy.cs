using Map;
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

    public GameObject[] models = new GameObject[3];
    public string enemyName;
    public float damage;
    public int maxNodeCount = 2;
    public int maxDeleteCount = 3;
    private int nodeCount;
    public List<DeleteNode> deleteNodes = new List<DeleteNode>();

    protected override void Start()
    {
        if (GameStateManager.Instance.playerWinMode) health = 1;
        base.Start();

        CurrentBattleEnemyInfo battleEnemyInfo = GameStateManager.Instance.currentBattlleInfo;
        Debug.LogWarning(battleEnemyInfo.currentStageLevel);
        maxHealth = 100 * battleEnemyInfo.currentStageLevel;
        damage = 5 * battleEnemyInfo.currentStageLevel;
        string boss = "";
        if (battleEnemyInfo.isBoss)
        {
            boss = "°­·ÂÇÑ ";
            maxHealth += 100;
            damage += 10;
            if (battleEnemyInfo.currentStageLevel >= 3)
            {
                maxHealth += 50;
                damage += 5;
            }
        }
        health = maxHealth;
        HealthImageUpdate();

        string elementalTypeString;
        int elementalTypeIndex;
        switch (battleEnemyInfo.nodeElementalType)
        {
            case NodeElementalType.Fire:
                elementalTypeString = "È­¿°ÀÇ ";
                elementalTypeIndex = 0;
                break;
            case NodeElementalType.Wind:
                elementalTypeString = "ÆøÇ³ÀÇ ";
                elementalTypeIndex = 1;
                break;
            case NodeElementalType.Water:
                elementalTypeString = "ÆÄµµÀÇ ";
                elementalTypeIndex = 2;
                break;
            case NodeElementalType.Land:
                elementalTypeString = "´ëÁöÀÇ ";
                elementalTypeIndex = 3;
                break;
            default:
                Debug.LogWarning("error");
                elementalTypeString = "";
                elementalTypeIndex = -1;
                break;
        }

        int modelIndex = UnityEngine.Random.Range(0, models.Length);
        string modelName;
        switch (modelIndex)
        {
            case 0:
                modelName = "°ñ¸®¾Ñ";
                break;
            case 1:
                modelName = "È÷µå¶ó";
                break;
            case 2:
                modelName = "³ªÀÌÆ®";
                break;
            default:
                modelName = "„¢‹S¶Õ";
                break;
        }

        EnemyMaterial enemyMaterial = Instantiate(models[modelIndex], transform).GetComponent<EnemyMaterial>();
        enemyMaterial.SetMaterials(elementalTypeIndex);
        animator = enemyMaterial.animator;
        enemyName = boss + elementalTypeString + modelName;
        GameManager.instance.enemyNameText.text = enemyName;

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
            {
                StartCoroutine(PlayAttackSound(attackSpeed));
                GameManager.instance.player.GetDamage(nodeBase, damage);
                animator.SetTrigger("Attack");
                //StartCoroutine(MoveAndComeBack(pos, 10.0f, deleteNode.count));
            }
        }

        return result;
    }

    public override void Death()
    {
        GameManager.instance.EndBattle(true);

    }
}
