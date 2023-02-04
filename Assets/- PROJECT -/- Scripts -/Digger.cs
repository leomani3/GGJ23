using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Digger : MonoBehaviour
{
    [SerializeField] private GameObject planet;
    [SerializeField] private float shrinkSpeed = 0.1f;
    [SerializeField] private float shrinkRadius;
    [SerializeField] private float closestToPlanet;

    private Vector3 _center;
    private Mesh _mesh;
    private MeshFilter _meshFilter;
    private MeshCollider _meshCollider;
    private RaycastHit _raycastHit;
    private Camera _mainCam;

    void Awake()
    {
        _meshFilter = planet.GetComponentInChildren<MeshFilter>();
        _mesh = _meshFilter.mesh;
        _center = _mesh.bounds.center;
        _meshCollider = planet.GetComponentInChildren<MeshCollider>();
        _mainCam = Camera.main;
    }

    void Update()
    {
        if (_meshCollider.Raycast(_mainCam.ScreenPointToRay(Input.mousePosition), out _raycastHit, Mathf.Infinity))
        {
            Vector3[] vertices = _mesh.vertices;
            for (int i = 0; i < vertices.Length; i++)
            {
                if (Vector3.Distance(_raycastHit.point, planet.transform.TransformPoint(vertices[i])) < shrinkRadius && Vector3.Distance(planet.transform.TransformPoint(vertices[i]), _center) > closestToPlanet)
                {
                    vertices[i] = Vector3.Lerp(vertices[i], _center, shrinkSpeed * Time.deltaTime);
                }
            }

            _mesh.vertices = vertices;
            _mesh.RecalculateBounds();
            _meshCollider.sharedMesh = _meshFilter.sharedMesh;
        }
    }
}