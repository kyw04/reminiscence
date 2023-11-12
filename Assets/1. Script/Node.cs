using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshRenderer), typeof(MeshFilter))]
public class Node : MonoBehaviour
{
    public GameManager GameManager;
    public NodeBase nodeBase;
    public int x;
    public int y;

    private void Start()
    {
        MeshRenderer meshRenderer = GetComponent<MeshRenderer>();
        if (meshRenderer != null) meshRenderer.material = nodeBase.material;

        MeshFilter meshFilter = GetComponent<MeshFilter>();
        if (meshFilter != null) meshFilter.mesh = nodeBase.mesh;
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
               
                if (GameManager.puzzle[newX, newY] == null)
                    continue;
                Transform target = GameManager.puzzle[newX, newY].transform;
                Gizmos.color = Color.green;
                Gizmos.DrawWireCube(target.position, target.localScale * 0.125f);
                
                Vector3 selectedNodeParentPos = this.transform.parent.position;
                Vector3 targetNodeParentPos = target.parent.position;
                Vector3 maxNodePos = Vector2.ClampMagnitude(targetNodeParentPos - selectedNodeParentPos, GameManager.maxDistance);
                maxNodePos += selectedNodeParentPos;
                maxNodePos.z = targetNodeParentPos.z - 10f;
                Gizmos.color = Color.red;
                Gizmos.DrawCube(maxNodePos, Vector3.one);
            }
        }
    }
}
