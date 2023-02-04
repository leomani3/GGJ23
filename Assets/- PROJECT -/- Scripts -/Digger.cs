using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Digger : MonoBehaviour
{
    [SerializeField] private GameObject planet;
    [SerializeField] private float shrinkSpeed = 0.1f;
    [SerializeField] private float shrinkRadius;

    private Vector3 _center;
    private Mesh _mesh;
    private Collider _collider;
    private RaycastHit _raycastHit;
    private Camera _mainCam;

    void Awake()
    {
        _mesh = planet.GetComponent<MeshFilter>().mesh;
        _center = _mesh.bounds.center;
        _collider = planet.GetComponent<Collider>();
        _mainCam = Camera.main;
    }

    void Update()
    {
        if (_collider.Raycast(_mainCam.ScreenPointToRay(Input.mousePosition), out _raycastHit, Mathf.Infinity))
        {
            Vector3[] vertices = _mesh.vertices;
            for (int i = 0; i < vertices.Length; i++)
            {
                if (Vector3.Distance(_raycastHit.point, planet.transform.TransformPoint(vertices[i])) < shrinkRadius)
                {
                    vertices[i] = Vector3.Lerp(vertices[i], _center, shrinkSpeed * Time.deltaTime);
                }
            }

            _mesh.vertices = vertices;
            _mesh.RecalculateBounds();
        }
    }
}