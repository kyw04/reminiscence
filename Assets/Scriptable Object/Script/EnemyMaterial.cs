using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct InspectorMeterials
{
    public Material[] materials;
}

public class EnemyMaterial : MonoBehaviour
{
    public MeshRenderer meshRenderer;
    public InspectorMeterials[] inspectorMeterials;

    public void SetMaterials(int index)
    {
        meshRenderer.materials = inspectorMeterials[index].materials;
    }
}
