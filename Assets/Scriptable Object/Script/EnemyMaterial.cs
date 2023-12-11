using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct Index
{
    public Material[] materials;
}

[System.Serializable]
public struct Meterials
{
    public Index[] index;
}

public class EnemyMaterial : MonoBehaviour
{
    public Animator animator;
    public SkinnedMeshRenderer[] meshRenderer;
    public Meterials[] meterial;
    public float attackSpeed;

    public void SetMaterials(int index)
    {
        GetComponentInParent<Enemy>().attackSpeed = attackSpeed;
        for (int i = 0; i < meshRenderer.Length; i++)
        {
            meshRenderer[i].materials = meterial[index].index[i].materials;
        }
    }
}
