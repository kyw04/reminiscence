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

    public Transform selectedNodeBox;
    public float maxDistance;
    public float moveSensitivity;
    private float moveDistance;
    private Vector3 selectedNodeStartPos;
    private Node selectedNode;
    private Node targetNode;

    private void Awake()
    {
        GetPuzzle();
    }

    private void Start()
    {

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
            selectedNodeBox.SetParent(selectedNode.transform);
            selectedNodeBox.position = selectedNodeBox.parent.position;
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
        const float size = 75f;
        const float halfSize = size * 0.5f;

        HashSet<Transform> moveNodes = new HashSet<Transform>();

        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                int newX = move[i] + selectedNode.x;
                int newY = move[j] + selectedNode.y;
                if (newX > 4 || newY > 4 || newX < 0 || newY < 0 || (newX == selectedNode.x && newY == selectedNode.y))
                    continue;

                Collider[] colliders;
                //Debug.Log($"{newX}, {newY}");

                if (Mathf.Abs(move[i]) == Mathf.Abs(move[j]))
                {
                    colliders = Physics.OverlapBox(puzzle[newX, newY].transform.position, Vector3.one * size * 0.5f, Quaternion.identity, LayerMask.GetMask("SelectedNode"));

                }
                else
                {
                    float sizeX = halfSize * Mathf.Abs(move[i]) + halfSize;
                    float sizeY = halfSize * Mathf.Abs(move[j]) + halfSize;
                    colliders = Physics.OverlapBox(puzzle[newX, newY].transform.position, new Vector3(sizeX, sizeY) * 0.5f, Quaternion.identity, LayerMask.GetMask("SelectedNode"));
                }

                if (colliders.Length > 0)
                {
                    moveNodes.Add(puzzle[newX, newY].transform);
                    //if (targetNode)
                    //    targetNode.transform.position = targetNode.transform.parent.position;

                    //targetNode = puzzle[newX, newY];
                    //Debug.Log($"{targetNode.x}, {targetNode.y}");
                    //selectedNode.transform.position = selectedNode.transform.parent.position;

                    Vector3 selectedNodeParentPos = selectedNode.transform.parent.position;
                    Vector3 targetNodeParentPos = puzzle[newX, newY].transform.parent.position;
                    float currentDis = Vector3.Distance(targetNodeParentPos, selectedNode.transform.position);
                    float maxDis = Vector3.Distance(targetNodeParentPos, selectedNodeParentPos);

                    //currentDis *=  maxDistance / maxDis;

                    //if (Mathf.Abs(move[i]) == Mathf.Abs(move[j]))
                    //{
                    //    //maxDis = 10;
                    //    //maxDis = maxDis / Mathf.Sqrt(2);
                    //    //currentDis -= 10;
                    //}

                    //if (i == 2 && j == 2)
                    //{
                    //    Debug.Log($"{puzzle[newX, newY].x}, {puzzle[newX, newY].y} \ncur dis = {currentDis}  max dis = {maxDis}");
                    //    Debug.Log($"{maxDistance} {maxDis} {maxDistance / maxDis}");
                    //}

                    Vector3 newTargetNodePos = Vector3.Lerp(selectedNodeParentPos, targetNodeParentPos, currentDis / maxDis);
                    puzzle[newX, newY].transform.position = newTargetNodePos;
                }
            }
        }

        if (moveDistance <= maxDistance * 0.5f)
        {
            if (targetNode)
                targetNode.transform.position = targetNode.transform.parent.position;

            targetNode = null;
        }

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

        if (targetNode)
        {
            // change
        }
        selectedNodeBox.SetParent(puzzleParent);
        selectedNode.transform.position = selectedNode.transform.parent.position;
        selectedNode = null;
    }
}
