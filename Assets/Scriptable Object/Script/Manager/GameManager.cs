using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using System.Linq;
using System.Net.NetworkInformation;
using TMPro;
using Unity.VisualScripting;
using UnityEditor.VersionControl;
using UnityEngine;
using UnityEngine.UI;

public enum GameState
{
    Idle,
    EndTurn,
    Change,
    Select,
    Attack,
    End
}

[RequireComponent(typeof(AudioSource))]
public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    #region Entity
    [Header("Entity")]
    public Player player;
    public Enemy enemy;

    #endregion

    #region Audio
    [Space(5)]
    [Header("Audio")]

    public AudioClip blockHoldAudio;
    private AudioSource audioSource;

    #endregion
    #region UI
    [Space(5)]
    [Header("UI")]

    public Image turnEndButtonImage;

    public int maxMovementCount = 3;
    private int currentMovementCount;
    public GameObject[] movementCountImages;
    public TextMeshProUGUI enemyNameText;

    #endregion
    #region GameInfo

    [Space(5)]
    [Header("Game Info")]

    [Range(0.0f, 1.0f)]
    public float gameTime = 1.0f;

    public GameState gameState = GameState.Idle;

    public int turn = 0;
    public int foundPatternCount = 0;
    public float totalDamage = 0.0f;

    #endregion
    #region Puzzle
    private const int puzzleSize = 5; // 2D(puzzleSize x puzzleSize)

    [Space(5)]
    [Header("Puzzle")]

    public Transform puzzleParent;

    public float maxDistance = 21.38653f; // maxDistance *= Puzzle Canvas/Puzzle/Pos.Rect Transfrom.Scale.x
    public float moveSensitivity;
    private float moveDistance;
    private Vector3 selectedNodeStartPos;
    private Vector3 targetMaxNodePos;
    private Node selectedNode;
    private Node targetNode;
    private HashSet<Transform> moveNodes = new HashSet<Transform>();
    public HashSet<Node> downNodes = new HashSet<Node>();
    private Dictionary<NodeBase, int> currentNodeBaseCount = new Dictionary<NodeBase, int>();
    public Node[,] puzzle = new Node[puzzleSize, puzzleSize];

    public Transform nodeSpawnPoints;
    private float spacing = 100.0f; // puzzle canvas size (default = 100.0f)

    public NodeBase[] nodeBases;

    #endregion

    private void Awake()
    {
        if (instance == null) { instance = this; }

        foreach (Pattern pattern in player.patterns)
        {
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    NodeBase nodeBase = pattern.nodePatternTemp[i].index[j];
                    pattern.nodePatternType[i, j] = nodeBase != null ? nodeBase.nodeType : NodeType.None;
                }
            }
        }

        foreach (NodeBase nodeBase in nodeBases)
        {
            currentNodeBaseCount.Add(nodeBase, 0);
        }

        GetPuzzle();
    }

    private void Start()
    {
        enemyNameText.text = enemy.enemyName;
        audioSource = GetComponent<AudioSource>();
        ResetCount();
        turn = 0;
    }
    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
            NodeSelect();
        if (Input.GetMouseButtonUp(0))
            PutNode();

        NodeDrag();

        turnEndButtonImage.color = gameState == GameState.Idle ? Color.white : Color.gray;
    }

    private NodeBase GetRandomNodeBase()
    {
        List<NodeBase> randomNodeBase = new List<NodeBase>();

        foreach (NodeBase nodeBase in nodeBases)
        {
            if (currentNodeBaseCount[nodeBase] < puzzleSize + 1)
            {
                randomNodeBase.Add(nodeBase);
            }
        }

        int nodeBaseIndex = UnityEngine.Random.Range(0, randomNodeBase.Count);
        foreach (NodeBase nodeBase in nodeBases)
        {
            if (nodeBase == randomNodeBase[nodeBaseIndex])
            {
                currentNodeBaseCount[nodeBase]++;
                return randomNodeBase[nodeBaseIndex];
            }
        }

        return null;
    }
    private void GetPuzzle()
    {
        for (int i = 0; i < puzzleSize; i++)
        {
            for (int j = 0; j < puzzleSize; j++)
            {
                puzzle[i, j] = puzzleParent.GetChild(i + (j * puzzleSize)).GetComponentInChildren<Node>();
                puzzle[i, j].x = i;
                puzzle[i, j].y = j;

                if (puzzle[i, j].nodeBase.nodeType == NodeType.None)
                {
                    puzzle[i, j].nodeBase = GetRandomNodeBase();
                }
            }
        }
    }
    private void NodeSelect()
    {
        if (gameState != GameState.Idle || currentMovementCount <= 0)
            return;

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, float.PositiveInfinity, LayerMask.GetMask("Node")))
        {
            gameState = GameState.Select;
            audioSource.PlayOneShot(blockHoldAudio);
            selectedNode = hit.transform.GetComponent<Node>();
            selectedNodeStartPos = Input.mousePosition;

            if (selectedNode.nodeBase.nodeType == NodeType.None)
                selectedNode = null;
        }
    }
    private void NodeDrag()
    {
        if (selectedNode == null)
            return;

        Vector3 moveDirection = Input.mousePosition - selectedNodeStartPos;
        //Debug.Log(moveDirection);
        moveDistance = Vector2.Distance(Input.mousePosition, selectedNodeStartPos) * moveSensitivity;
        moveDistance = maxDistance < moveDistance ? maxDistance : moveDistance;
        selectedNode.transform.position = moveDirection.normalized * moveDistance + selectedNode.transform.parent.position;

        int[] move = { 0, 1, -1 };
        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                int newX = move[i] + selectedNode.x;
                int newY = move[j] + selectedNode.y;
                bool outOfRange = newX > puzzleSize - 1 || newY > puzzleSize - 1 || newX < 0 || newY < 0;

                if (outOfRange || (newX == selectedNode.x && newY == selectedNode.y))
                    continue;

                moveNodes.Add(puzzle[newX, newY].transform);
                Transform target = puzzle[newX, newY].transform;
                Vector3 selectedNodeParentPos = selectedNode.transform.parent.position;
                Vector3 targetNodeParentPos = target.parent.position;
                Vector3 maxNodePos = Vector2.ClampMagnitude(targetNodeParentPos - selectedNodeParentPos, maxDistance);
                maxNodePos += selectedNodeParentPos;
                maxNodePos.z = targetNodeParentPos.z;

                float currentDis = Vector3.Distance(maxNodePos, selectedNode.transform.position);

                //Vector3 newTargetNodePos = Vector3.Lerp(selectedNodeParentPos, targetNodeParentPos, currentDis / maxDistance);
                //target.position = newTargetNodePos;

                if (targetNode)
                {
                    if (Vector3.Distance(targetMaxNodePos, selectedNode.transform.position) / maxDistance > 0.75f)
                    {
                        targetNode.transform.position = targetNode.transform.parent.position;
                        targetNode.SetMetarialAlpha(255);
                        targetNode = null;
                    }
                    else
                    {
                        targetNode.transform.position = selectedNode.transform.parent.position;
                        targetNode.SetMetarialAlpha(128);
                    }
                }

                Node lastTarget = targetNode;
                if (Mathf.Abs(move[i]) == Mathf.Abs(move[j]))
                {
                    if (currentDis / maxDistance <= 0.25f)
                    {
                        targetNode = puzzle[newX, newY];
                        targetMaxNodePos = maxNodePos;
                    }
                }
                else
                {
                    if (currentDis / maxDistance <= 0.5f)
                    {
                        targetNode = puzzle[newX, newY];
                        targetMaxNodePos = maxNodePos;
                    }
                }

                if (lastTarget && targetNode && lastTarget != targetNode)
                {
                    lastTarget.SetMetarialAlpha(255);
                    lastTarget.transform.position = lastTarget.transform.parent.position;
                }
            }
        }
        if (selectedNode.nodeBase.nodeType == NodeType.None)
            selectedNode = null;

        //if (targetNode)
        //    Debug.Log(targetNode.transform.parent.name);
    }
    private void PutNode()
    {
        if (selectedNode == null)
            return;

        gameState = GameState.Idle;

        if (targetNode && currentMovementCount > 0)
        {
            currentMovementCount--;
            movementCountImages[currentMovementCount].SetActive(false);
            selectedNode.ChangeNodeBase(targetNode);
            targetNode.SetMetarialAlpha(255);
        }

        foreach (Transform moveNode in moveNodes)
        {
            moveNode.position = moveNode.parent.position;
        }
        selectedNode.transform.position = selectedNode.transform.parent.position;
        selectedNode = null;
        targetNode = null;

        moveNodes.Clear();
    }
    public void TurnEnd()
    {
        if (gameState != GameState.Idle)
            return;

        gameState = GameState.EndTurn;
        turn++;
        HashSet<Node> deleteNodes = new HashSet<Node>();
        foundPatternCount = 0;
        totalDamage = 0.0f;

        foreach (Pattern pattern in player.patterns)
        {
            for (int i = 0; i < puzzleSize; i++)
            {
                for (int j = 0; j < puzzleSize; j++)
                {
                    HashSet<Node> temp = PatternCheck(pattern, i, j);
                    if (temp.Count() > 0)
                    {
                        foundPatternCount++;
                        totalDamage += pattern.damage;
                        deleteNodes.UnionWith(temp);
                    }
                }
            }
        }

        if (foundPatternCount == 0)
            StartCoroutine(EndNodeDown(0));
        else
            NodeDelete(deleteNodes);

        ResetCount();
    }
    private HashSet<Node> PatternCheck(Pattern pattern, int x, int y)
    {
        int[] dir = { -1, 0, 1 };
        HashSet<Node> deleteNode = new HashSet<Node>();
        List<NodeBase> foundNodeBaes = new List<NodeBase>();

        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                if (pattern.nodePatternType[i, j] == NodeType.None)
                    continue;

                int newX = x + dir[i];
                int newY = y + dir[j];

                bool outOfRange = newX > puzzleSize - 1 || newY > puzzleSize - 1 || newX < 0 || newY < 0;
                if (outOfRange || pattern.nodePatternType[i, j] != puzzle[newX, newY].nodeBase.nodeType)
                {
                    foundNodeBaes.Clear();
                    deleteNode.Clear();
                    return deleteNode;
                }
                else
                {
                    foundNodeBaes.Add(puzzle[newX, newY].nodeBase);
                    deleteNode.Add(puzzle[newX, newY]);
                }
            }
        }

        foreach (NodeBase nodeBase in foundNodeBaes)
        {
            // 장비 공격력 합산시킴
            Debug.Log(TempEquipData.PlayerEquipmentStat._atk);
            player.Attack(nodeBase, pattern.damage + TempEquipData.PlayerEquipmentStat._atk / (float)foundNodeBaes.Count);
        }

        return deleteNode;
    }

    private void NodeDelete(HashSet<Node> deleteNode)
    {
        HashSet<int> deleteNodeX = new HashSet<int>();

        foreach (Node node in deleteNode)
        {
            //Debug.Log($"({node.x}, {node.y}) {node.nodeBase.nodeType}");
            deleteNodeX.Add(node.x);
            node.isDelete = true;
            currentNodeBaseCount[node.nodeBase]--;
        }

        if (deleteNodeX.Count > 0)
        {
            foreach (int x in deleteNodeX)
            {
                StartCoroutine(NodeMove(x));
            }
        }
        else
            StartCoroutine(EndNodeDown(0));
    }

    public IEnumerator NodeMove(int x)
    {
        int[] counts = new int[puzzleSize];

        int maxY = 0;
        for (int i = puzzleSize - 1; i >= 0; i--)
        {
            if (puzzle[x, i].isDelete)
            {
                //Debug.Log($"push stack ({x}, {i})");
                maxY = i;
                break;
            }
        }
        //Debug.Log($"maxY: {maxY}");

        Stack<NodeParentPosition> parents = new Stack<NodeParentPosition>();
        for (int i = 0; i <= maxY; i++)
        {
            parents.Push(puzzle[x, i].GetComponentInParent<NodeParentPosition>());
        }
        //Debug.Log($"parents: {parents.Count}");

        Queue<Node> deleteNode = new Queue<Node>();
        for (int i = maxY; i >= 0; i--)
        {
            if (puzzle[x, i].isDelete)
            {
                if (puzzle[x, i].nodeBase.deleteParticle)
                {
                    GameObject particle = Instantiate(puzzle[x, i].nodeBase.deleteParticle, puzzle[x, i].transform.parent);
                    particle.transform.position += Vector3.back * 0.01f;
                    Destroy(particle, 1.0f);
                }

                yield return new WaitForSeconds(UnityEngine.Random.Range(0.0f, 0.25f));

                deleteNode.Enqueue(puzzle[x, i]);

                puzzle[x, i].transform.SetParent(nodeSpawnPoints.GetChild(puzzle[x, i].x));
                Vector3 pos = new Vector3(0, (puzzle[x, i].transform.localScale.x + spacing) * counts[puzzle[x, i].x]++, 0);
                puzzle[x, i].transform.localPosition = pos;
                puzzle[x, i].nodeBase = GetRandomNodeBase();
                puzzle[x, i].DrawNode();
                puzzle[x, i].isDelete = false;
            }
            else
            {
                NodeParentPosition parentPos = parents.Pop();
                puzzle[x, i].transform.SetParent(parentPos.transform);
                puzzle[x, i].SetPosition(parentPos.x, parentPos.y);
                puzzle[x, i].isDown = true;
                downNodes.Add(puzzle[x, i]);
            }
        }

        while (deleteNode.Count > 0)
        {
            if (parents.Count > 0)
            {
                Node node = deleteNode.Dequeue();
                NodeParentPosition parentPos = parents.Pop();
                node.transform.SetParent(parentPos.transform);
                node.SetPosition(parentPos.x, parentPos.y);
                node.isDown = true;
                downNodes.Add(node);
            }
            else
            {
                Debug.LogError("have few parents");
                break;
            }
        }
    }
    //������ ��������
    public void EndBattle(bool playerWon)
    {
        Debug.Log("게임결과 " + playerWon);
        GameStateManager.Instance.SetBattleResult(playerWon ? BattleResult.Win : BattleResult.Lose);
        MySceneManager.Instance.LoadPreviousScene();
    }

    public IEnumerator EndNodeDown(float seconds)
    {
        yield return new WaitForSeconds(seconds);

        if (gameState == GameState.EndTurn)
        {
            gameState = GameState.Change;
            NodeDelete(enemy.Attack());
        }
        else
        {
            gameState = GameState.Idle;
        }
    }

    private void ResetCount()
    {
        foundPatternCount = 0;
        currentMovementCount = maxMovementCount;
        foreach (GameObject image in movementCountImages)
        {
            image.SetActive(true);
        }
    }
}
