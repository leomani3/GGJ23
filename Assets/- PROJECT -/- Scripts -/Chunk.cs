using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Chunk : MonoBehaviour
{
    private MeshFilter _meshFilter;
    private MeshCollider _meshCollider;

    public MeshCollider MeshCollider => _meshCollider;
    public MeshFilter MeshFilter => _meshFilter;

    private void Awake()
    {
        _meshCollider= GetComponent<MeshCollider>();
        _meshFilter= GetComponent<MeshFilter>();
    }
}