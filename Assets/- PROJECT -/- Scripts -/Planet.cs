using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Planet : MonoBehaviour
{
    [SerializeField] private float dragSpeed;
    [SerializeField] private Transform cameraPivot;

    private List<Chunk> _chunks;

    private Vector3 _previousPos;
    private Vector3 _drag;

    public List<Chunk> Chunks => _chunks;

    private void Awake()
    {
        _chunks = GetComponentsInChildren<Chunk>().ToList();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            _previousPos = Input.mousePosition;
        }

        if (Input.GetMouseButton(0))
        {
            _drag = Input.mousePosition - _previousPos;
            cameraPivot.Rotate(new Vector3(-_drag.y, _drag.x, 0) * dragSpeed, Space.Self);
            _previousPos = Input.mousePosition;
        }
    }
}