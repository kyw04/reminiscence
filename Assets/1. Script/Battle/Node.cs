using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Search;
using UnityEngine;

[RequireComponent(typeof(MeshRenderer), typeof(MeshFilter))]
public class Node : MonoBehaviour
{
    private NodeBase nodeBaseTemp;
    private MeshRenderer meshRenderer;
    private MeshFilter meshFilter;
    public NodeBase nodeBase;
    public bool isDelete;
    public bool isDown;
    public float speed = 5.0f;
    public int x;
    public int y;

    private void Start()
    {
        meshRenderer = GetComponent<MeshRenderer>();
        meshFilter = GetComponent<MeshFilter>();
        
        isDelete = false;
        isDown = false; 
        
        DrawNode();
    }

    private void Update()
    {
        if (isDown)
        {
            if (transform.parent.position.y <= transform.position.y)
            {
                transform.position += Vector3.down * speed * GameManager.instance.gameTime * Time.deltaTime;
            }
            else
            {
                transform.position = transform.parent.position;
                GameManager.instance.downNodes.Remove(this);
                if (GameManager.instance.downNodes.Count <= 0)
                {
                    GameManager.instance.EndNodeDown();
                }
                
                isDown = false;
            }
        }
    }

    public void ChangeNodeBase(Node target)
    {
        if (target == null) return;

        nodeBaseTemp = target.nodeBase;
        target.nodeBase = this.nodeBase;
        this.nodeBase = nodeBaseTemp;

        target.DrawNode();
        this.DrawNode();
    }

    public void DrawNode()
    {
        try
        {
            meshRenderer.material = nodeBase.material;
            meshFilter.mesh = nodeBase.mesh;
        }
        catch (Exception ex)
        {
            Debug.Log(ex);
            throw;
        }
    }

    public void SetPosition(int x, int y)
    {
        GameManager.instance.puzzle[x, y] = this;
        
        this.x = x;
        this.y = y;
    }

    private void OnDrawGizmosSelected()
    {
        int[] move = { 0, 1, -1 };
        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                int newX = move[i] + x;
                int newY = move[j] + y;
                if (newX > 4 || newY > 4 || newX < 0 || newY < 0 || (newX == x && newY == y))
                    continue;
               
                if (GameManager.instance == null || GameManager.instance.puzzle[newX, newY] == null)
                    continue;

                Transform target = GameManager.instance.puzzle[newX, newY].transform;
                Gizmos.color = Color.red;
                Gizmos.DrawWireCube(target.position, target.localScale * 0.125f * 0.006f);
                
                Vector3 selectedNodeParentPos = this.transform.parent.position;
                Vector3 targetNodeParentPos = target.parent.position;
                Vector3 maxNodePos = Vector2.ClampMagnitude(targetNodeParentPos - selectedNodeParentPos, GameManager.instance.maxDistance);
                maxNodePos += selectedNodeParentPos;
                maxNodePos.z = targetNodeParentPos.z - 10f;
                Gizmos.color = Color.red;
                Gizmos.DrawCube(maxNodePos, Vector3.one);
            }
        }
    }
}
