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
    [SerializeField] private float pitchSpeed;
    [SerializeField] private float startHoleSize;
    [SerializeField] private Transform startHolePos;

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
    private Color[] colors;
    private Vector3[] drawPoints;
    private AudioSource _audioSource;

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
        _audioSource.pitch = 0.25f;
    }


    void Start()
    {
        _mainCam = Camera.main;
        _center = Vector3.zero;

        for (int i = 0; i < planet.Chunks.Count; i++)
        {
            colors = new Color[planet.Chunks[i].MeshFilter.mesh.vertices.Length];
            for (int j = 0; j < colors.Length; j++)
            {
                colors[j] = Color.clear;
            }
            planet.Chunks[i].MeshFilter.mesh.colors = colors;
        }

        _detectedChunks.Clear();
        _colliders = Physics.OverlapSphere(startHolePos.position, startHoleSize).ToList();
        foreach (Collider collider in _colliders)
        {
            Chunk chnk = collider.GetComponent<Chunk>();
            if (chnk != null)
            {
                _detectedChunks.Add(chnk);
            }
        }

        foreach (Chunk chunk in _detectedChunks)
        {
            Vector3[] vertices = chunk.MeshFilter.mesh.vertices;
            for (int i = 0; i < vertices.Length; i++)
            {
                _vertexWorldPos = chunk.transform.TransformPoint(vertices[i]);
                _distanceToRaycastHitPos = Vector3.Distance(startHolePos.position, _vertexWorldPos);
                _distanceToCenter = Vector3.Distance(_vertexWorldPos, _center);
                if (_distanceToRaycastHitPos < startHoleSize)
                {
                    vertices[i] += (_center - vertices[i]) * 0.1f * (_distanceToRaycastHitPos / startHoleSize);

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

            Color[] colors = chunk.MeshFilter.mesh.colors;
            for (int i = 0; i < vertices.Length; i++)
            {
                _vertexWorldPos = chunk.transform.TransformPoint(vertices[i]);
                _distanceToRaycastHitPos = Vector3.Distance(_raycastHit.point, _vertexWorldPos);
                _distanceToCenter = Vector3.Distance(_vertexWorldPos, _center);
                if (_distanceToRaycastHitPos < invigorationRadius * .8f)
                {
                    //colors[i] += Color.red * (1 - (_distanceToRaycastHitPos / invigorationRadius));
                    colors[i] = Color.red;
                }
                if (colors[i] != Color.red && _distanceToRaycastHitPos > invigorationRadius && _distanceToRaycastHitPos < invigorationRadius)
                {
                    colors[i] = Color.blue;
                }
            }

            chunk.MeshFilter.mesh.vertices = vertices;
            chunk.MeshFilter.mesh.colors = colors;
            chunk.MeshFilter.mesh.RecalculateBounds();
            chunk.MeshFilter.mesh.RecalculateNormals();
            chunk.MeshCollider.sharedMesh = chunk.MeshFilter.sharedMesh;
        }
    }

        void Update()
        {
        if (!active) return;

        if (Input.GetMouseButtonDown(1))
        {
            _audioSource.enabled = true;
            _audioSource.Play();
        }

        if (Input.GetMouseButtonUp(1))
        {
            _audioSource.Stop();
            _audioSource.enabled = false;
        }

        if (Input.GetMouseButton(1))
        {
            if (_audioSource.pitch < 1.3f)
                _audioSource.pitch += pitchSpeed * Time.deltaTime;


            _detectedChunks.Clear();
            if (Physics.Raycast(_mainCam.ScreenPointToRay(Input.mousePosition), out _raycastHit, Mathf.Infinity, chunkLayer))
            {
                _colliders = Physics.OverlapSphere(_raycastHit.point, invigorationRadius).ToList();
                foreach (Collider collider in _colliders)
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

                Color[] colors = chunk.MeshFilter.mesh.colors;
                for (int i = 0; i < vertices.Length; i++)
                {
                    _vertexWorldPos = chunk.transform.TransformPoint(vertices[i]);
                    _distanceToRaycastHitPos = Vector3.Distance(_raycastHit.point, _vertexWorldPos);
                    _distanceToCenter = Vector3.Distance(_vertexWorldPos, _center);
                    if (_distanceToRaycastHitPos < invigorationRadius * .8f)
                    {
                        //colors[i] += Color.red * (1 - (_distanceToRaycastHitPos / invigorationRadius));
                        colors[i] = Color.red;
                    }
                    if (colors[i] != Color.red && _distanceToRaycastHitPos > invigorationRadius * .95f && _distanceToRaycastHitPos < invigorationRadius)
                    {
                        colors[i] = Color.blue;
                    }
                }

                chunk.MeshFilter.mesh.vertices = vertices;
                chunk.MeshFilter.mesh.colors = colors;
                chunk.MeshFilter.mesh.RecalculateBounds();
                chunk.MeshFilter.mesh.RecalculateNormals();
                chunk.MeshCollider.sharedMesh = chunk.MeshFilter.sharedMesh;
            }
        }
    }

    //private IEnumerator ColorGrowth()
    //{

    //}

    private void OnDrawGizmos()
    {
        Gizmos.DrawSphere(_raycastHit.point, invigorationRadius);
    }
}