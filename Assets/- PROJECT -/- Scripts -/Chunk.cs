using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Chunk : MonoBehaviour
{
    [SerializeField] private List<Chunk> neighbors;

    private MeshFilter _meshFilter;
    private MeshCollider _meshCollider;

    public MeshCollider MeshCollider => _meshCollider;
    public MeshFilter MeshFilter => _meshFilter;
    public List<Chunk> Neighbors => neighbors;

    private void Awake()
    {
        _meshCollider= GetComponent<MeshCollider>();
        _meshFilter= GetComponent<MeshFilter>();
    }

    private void OnTriggerEnter(Collider other)
    {
        Chunk chunk = other.GetComponent<Chunk>();
        if (chunk != null)
        {
           neighbors.Add(chunk);
        }
    }
}