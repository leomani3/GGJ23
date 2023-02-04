using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Digger : MonoBehaviour
{
    [SerializeField] private Planet planet;
    [SerializeField] private float shrinkSpeed = 0.1f;
    [SerializeField] private float shrinkRadius;
    [SerializeField] private float invigorationRadius;
    [SerializeField] private float closestToPlanet;
    [SerializeField] private LayerMask chunkLayer;
    [SerializeField] private float timeBeforeDrag;

    public bool active;

    private Vector3 _center;
    private RaycastHit _raycastHit;
    private Camera _mainCam;
    private Vector3 _vertexWorldPos;
    private float _distanceToRaycastHitPos;
    private float _distanceToCenter;
    private List<Chunk> _detectedChunks = new List<Chunk>();
    private List<Collider> _colliders = new List<Collider>();
    private List<Collider> _colliders2 = new List<Collider>();

    void Awake()
    {
        _mainCam = Camera.main;
        _center = Vector3.zero;
    }

    void Update()
    {
        if (!active) return;


        if (Input.GetMouseButton(1))
        {
            _detectedChunks.Clear();
            if (Physics.Raycast(_mainCam.ScreenPointToRay(Input.mousePosition), out _raycastHit, Mathf.Infinity, chunkLayer))
            {
                _colliders = Physics.OverlapSphere(_raycastHit.point, shrinkRadius).ToList();
                foreach (Collider  collider in _colliders)
                {
                    Chunk chnk = collider.GetComponent<Chunk>();
                    if (chnk != null)
                    {
                        _detectedChunks.Add(chnk);
                    }

                    NatureObject obj = collider.GetComponent<NatureObject>();
                    if (obj != null)
                    {
                        obj.DestroyObject();
                    }
                }
            }

                foreach (Chunk chunk in _detectedChunks)
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

                        if (Vector3.Distance(chunk.transform.TransformPoint(vertices[i]), _center) < closestToPlanet)
                        {
                            _colliders2 = Physics.OverlapSphere(_raycastHit.point, invigorationRadius).ToList();
                            foreach (Collider collider in _colliders2)
                            {
                                NatureObject obj = collider.GetComponent<NatureObject>();
                                if (obj != null)
                                {
                                    obj.SetState(NatureState.Alive);
                                }
                            }
                        }
                    }
                }

                chunk.MeshFilter.mesh.vertices = vertices;
                chunk.MeshFilter.mesh.RecalculateBounds();
                chunk.MeshFilter.mesh.RecalculateNormals();
                chunk.MeshCollider.sharedMesh = chunk.MeshFilter.sharedMesh;
            }
        }
    }
}