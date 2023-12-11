using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Waterfall : MonoBehaviour
{
    public float speed;
    private MeshRenderer render;
    private float offset = 0.0f;
    void Start()
    {
        render = GetComponent<MeshRenderer>();
    }
    void Update()
    {
        offset += speed * Time.deltaTime;
        render.material.mainTextureOffset = new Vector2(0, offset);
    }
}
