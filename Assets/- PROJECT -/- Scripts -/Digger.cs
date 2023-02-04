using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Digger : MonoBehaviour
{
    [SerializeField] private Planet planet;
    [SerializeField] private float shrinkSpeed = 0.1f;
    [SerializeField] private float shrinkRadius;
    [SerializeField] private float closestToPlanet;

    private Vector3 _center;
    private RaycastHit _raycastHit;
    private Camera _mainCam;
    private Vector3 _vertexWorldPos;
    private float _distanceToRaycastHitPos;
    private float _distanceToCenter;
    private List<Chunk> _chunks = new List<Chunk>();

    void Awake()
    {
        _mainCam = Camera.main;
        _center = Vector3.zero;
    }

    void Update()
    {
        if (Input.GetMouseButton(1))
        {
            foreach (Chunk raycastedChunk in planet.Chunks)
            {
                if (raycastedChunk.MeshCollider.Raycast(_mainCam.ScreenPointToRay(Input.mousePosition), out _raycastHit, Mathf.Infinity))
                {
                    _chunks.Add(raycastedChunk);
                    foreach (Chunk neighbor in raycastedChunk.Neighbors)
                    {
                        _chunks.Add(neighbor);
                    }

                    foreach (Chunk chunk in _chunks)
                    {
                        Vector3[] vertices = chunk.MeshFilter.mesh.vertices;
                        for (int i = 0; i < vertices.Length; i++)
                        {
                            _vertexWorldPos = chunk.transform.TransformPoint(vertices[i]);
                            _distanceToRaycastHitPos = Vector3.Distance(_raycastHit.point, _vertexWorldPos);
                            _distanceToCenter = Vector3.Distance(_vertexWorldPos, _center);
                            if (_distanceToRaycastHitPos < shrinkRadius && _distanceToCenter > closestToPlanet)
                            {
                                vertices[i] = Vector3.Lerp(vertices[i], (_center - vertices[i]) * (_distanceToRaycastHitPos / shrinkRadius), shrinkSpeed * Time.deltaTime);
                            }
                        }

                        chunk.MeshFilter.mesh.vertices = vertices;
                        chunk.MeshFilter.mesh.RecalculateBounds();
                        chunk.MeshCollider.sharedMesh = chunk.MeshFilter.sharedMesh;
                    }
                }
            }
        }
    }
}