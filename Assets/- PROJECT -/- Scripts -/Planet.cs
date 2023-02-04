using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Planet : MonoBehaviour
{
    private List<Chunk> _chunks;

    public List<Chunk> Chunks => _chunks;

    private void Awake()
    {
        _chunks = GetComponentsInChildren<Chunk>().ToList();
    }
}