using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeVisual : MonoBehaviour
{
    private Material mat;

    public float speed = 3f;

    // Start is called before the first frame update
    void Awake()
    {
        mat = GetComponent<MeshRenderer>().material;
    }

    private void Update()
    {
        Vector2 matOffset = mat.GetTextureOffset("_MainTex");
        matOffset.x -= speed * Time.deltaTime;
        mat.SetTextureOffset("_MainTex", matOffset);
    }
}
