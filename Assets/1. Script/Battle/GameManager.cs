using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Xml.Serialization;
using Unity.VisualScripting;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public enum GameState
{
    Idle,
    Change,
    Select
}

public class GameManager : MonoBehaviour
{
    private const int puzzleSize = 5;
    public GameState gameState = GameState.Idle;
    public Transform puzzleParent;
    public Pattern[] patterns;
    public NodeBase[] nodeBases;
    public Node[,] puzzle = new Node[puzzleSize, puzzleSize];

    public int maxMovementCount = 3;
    private int currentMovementCount;
    public GameObject[] movementCountImages;

    public float maxDistance = 21.38653f;
    public float moveSensitivity;
    private float moveDistance;
    private Vector3 selectedNodeStartPos;
    private Node selectedNode;
    private Node targetNode;
    private HashSet<Transform> moveNodes = new HashSet<Transform>();

    private void Awake()
    {
        GetPuzzle();
    }

    private void Start()
    {
        foreach (Pattern pattern in patterns)
        {
            foreach (NodeType nodeType in pattern.nodePattern)
            {
                Debug.Log(nodeType);
            }
            foreach (var node in pattern.nodePatternTemp)
            {
                for (int i = 0; i < 3; i++)
                {
                    Debug.Log(node.index[i]);
                }
            }
        }
        ResetCount();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
            NodeSelect();
        if (Input.GetMouseButtonUp(0))
            PutNode();

        NodeDrag();
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
                    puzzle[i, j].nodeBase = nodeBases[Random.Range(0, nodeBases.Length)];
                }
            }
        }
    }

    private void NodeSelect()
    {
        if (gameState != GameState.Idle)
            return;

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, float.PositiveInfinity, LayerMask.GetMask("Node")))
        {
            gameState = GameState.Select;
            selectedNode = hit.transform.GetComponent<Node>();
            selectedNodeStartPos = Input.mousePosition;
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

                Vector3 newTargetNodePos = Vector3.Lerp(selectedNodeParentPos, targetNodeParentPos, currentDis / maxDistance);
                target.position = newTargetNodePos;

                if (Mathf.Abs(move[i]) == Mathf.Abs(move[j]))
                {
                    if (currentDis / maxDistance <= 0.25f)
                    {
                        targetNode = puzzle[newX, newY];
                    }
                }
                else
                {
                    if (currentDis / maxDistance <= 0.5f)
                    {
                        targetNode = puzzle[newX, newY];
                    }
                }
            }
        }

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
        HashSet<Node> deleteNodes = new HashSet<Node>();

        foreach (Pattern pattern in patterns)
        {
            for (int i = 0; i < puzzleSize; i++)
            {
                for (int j = 0; j < puzzleSize; j++)
                {
                    HashSet<Node> temp = PatternCheck(pattern, i, j);
                    if (temp.Count() > 0)
                    {
                        deleteNodes.UnionWith(temp);
                    }
                }
            }
        }

        NodeDelete(deleteNodes);
        ResetCount();
    }
    private HashSet<Node> PatternCheck(Pattern pattern, int x, int y)
    {
        int[] dir = { -1, 0, 1 };
        HashSet<Node> deleteNode = new HashSet<Node>();

        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                if (pattern.nodePattern[i, j] == NodeType.None)
                    continue;

                int newX = x + dir[i];
                int newY = y + dir[j];

                bool outOfRange = newX > puzzleSize - 1 || newY > puzzleSize - 1 || newX < 0 || newY < 0;
                if (outOfRange || pattern.nodePattern[i, j] != puzzle[newX, newY].nodeBase.nodeType)
                {
                    Debug.Log("clear");
                    deleteNode.Clear();
                    return deleteNode;
                }
                else
                {
                    deleteNode.Add(puzzle[newX, newY]);
                }
            }
        }

        Debug.Log("delete");
        //enemy.GetDamage(pattern.damage);
        return deleteNode;
    }

    private void NodeDelete(HashSet<Node> deleteNode)
    {
        HashSet<int> index = new HashSet<int>();
        foreach (Node node in deleteNode)
        {
            index.Add(node.x);
            Debug.Log($"({node.x}, {node.y})");
            node.nodeBase = nodeBases[Random.Range(0, nodeBases.Length)];
            node.DrawNode();
        }

        //deleteCount = index.Count;
        //foreach (int i in index)
        //{
        //    StartCoroutine(NodeDown(i, 4));
        //}
    }

    private void ResetCount()
    {
        currentMovementCount = maxMovementCount;
        foreach (GameObject image in movementCountImages)
        {
            image.SetActive(true);
        }
    }
}
