using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
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
    public NodeBase[] nodeBases;
    public GameState gameState = GameState.Idle;
    public Transform puzzleParent;
    public Node[,] puzzle = new Node[puzzleSize, puzzleSize];

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

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
            NodeSelect();
        if (Input.GetMouseButtonUp(0))
            PutNode();

        NodeMove();
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

    private void NodeMove()
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
                bool outOfRange = newX > 4 || newY > 4 || newX < 0 || newY < 0;

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
        if (targetNode)
            Debug.Log(targetNode.transform.parent.name);
        //if (moveDistance <= maxDistance * 0.5f)
        //{
        //    if (targetNode)
        //        targetNode.transform.position = targetNode.transform.parent.position;

        //    targetNode = null;
        //}

        //foreach (Transform target in moveNodes)
        //{
        //    Vector3 selectedNodeParentPos = selectedNode.transform.parent.position;
        //    Vector3 targetNodeParentPos = target.transform.parent.position;
        //    float currentDis = Vector3.Distance(targetNodeParentPos, selectedNode.transform.position);
        //    float maxDis = Vector3.Distance(targetNodeParentPos, selectedNodeParentPos);
        //    Vector3 newTargetNodePos = Vector3.Lerp(selectedNodeParentPos, targetNodeParentPos, currentDis / maxDis);
        //    target.transform.position = newTargetNodePos;
        //}
    }

    private void PutNode()
    {
        if (selectedNode == null)
            return;

        gameState = GameState.Idle;

        selectedNode.ChangeNodeBase(targetNode);
        foreach (Transform moveNode in moveNodes)
        {
            moveNode.position = moveNode.parent.position;
        }
        selectedNode.transform.position = selectedNode.transform.parent.position;
        selectedNode = null;
        targetNode = null;

        moveNodes.Clear();
    }
}
