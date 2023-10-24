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
        const float size = 25f;
        const float halfSize = size * 0.5f;

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

                if (Mathf.Abs(move[i]) == Mathf.Abs(move[j]))
                {
                    Gizmos.DrawWireCube(GameManager.puzzle[newX, newY].transform.position, Vector3.one * size);
                }
                else
                {
                    float sizeX = halfSize * Mathf.Abs(move[i]) + halfSize;
                    float sizeY = halfSize * Mathf.Abs(move[j]) + halfSize;
                    Gizmos.DrawWireCube(GameManager.puzzle[newX, newY].transform.position, new Vector3(sizeX, sizeY, size));
                }
            }
        }
    }
}
