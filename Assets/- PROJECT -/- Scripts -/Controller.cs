using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Controller : MonoBehaviour
{
    [SerializeField] private Drawer drawerPrefab;
    [SerializeField] private Transform planeSpace;
    [SerializeField] private Collider planetCollider;
    [SerializeField] private Collider[] colliders;
    [SerializeField] private MeshFilter[] meshFilter;
    [SerializeField] private MeshRenderer[] meshRenderer;
    [SerializeField] private float brushSize;
    [SerializeField] private Transform coordObject;
    [SerializeField] private Transform coordObject2;
    [SerializeField] private TrailRenderer trailRenderer;
    [SerializeField] private LayerMask chunkLayer;
    [SerializeField] private float detectionRadius;

    private Camera _mainCam;
    private Vector2[] uvs;
    private Vector3[] vertices;
    private List<Vector2> closeUVs = new List<Vector2>();
    private Vector2 _currentCoord;
    private Vector2 _smoothedCoord;
    private float dividerSum;
    private int _chunksDetectedCount;
    private Drawer[] drawers;


    private void Awake()
    {
        _mainCam = Camera.main;

        trailRenderer.emitting = false;
        coordObject.gameObject.SetActive(false);

        drawers = new Drawer[colliders.Length];
        for (int i = 0; i < drawers.Length; i++)
        {
            drawers[i] = Instantiate(drawerPrefab, planeSpace);
            drawers[i].Init(meshRenderer[i]);
            drawers[i].transform.position = new Vector3(100, 0, 100) * (i + 1);
            drawers[i].Deactivate();
        }
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            trailRenderer.Clear();
            trailRenderer.emitting = true;
            //for (int i = 0; i < drawers.Length; i++)
            //{
            //    drawers[i].Activate();
            //}
            //coordObject.gameObject.SetActive(true);
            //coordObject2.gameObject.SetActive(true);
        }

        if (Input.GetMouseButtonUp(0))
        {
            trailRenderer.Clear();
            trailRenderer.emitting = false;
            for (int i = 0; i < drawers.Length; i++)
            {
                drawers[i].Deactivate();
            }
            //coordObject.gameObject.SetActive(false);
            //coordObject2.gameObject.SetActive(false);
        }
        if (Input.GetMouseButton(0))
        {
            _chunksDetectedCount = 0;

            if (planetCollider.Raycast(_mainCam.ScreenPointToRay(Input.mousePosition), out RaycastHit hitInfo, Mathf.Infinity))
            {
                Collider[] collidersDetected = Physics.OverlapSphere(hitInfo.point, detectionRadius, chunkLayer);

                for (int i = 0; i < colliders.Length; i++)
                {
                    _currentCoord = Vector2.zero;
                    //_currentCoord += new Vector2(0, 0) * Vector3.Distance(hitInfo.point, new Vector2(0, 0));
                    //_currentCoord += new Vector2(1, 0) * Vector3.Distance(hitInfo.point, new Vector2(1, 0));
                    //_currentCoord += new Vector2(1, 1) * Vector3.Distance(hitInfo.point, new Vector2(1, 1));
                    //_currentCoord += new Vector2(0, 1) * Vector3.Distance(hitInfo.point, new Vector2(0, 1));
                    dividerSum = 0;
                    //dividerSum += Vector3.Distance(hitInfo.point, new Vector2(0, 0));
                    //dividerSum += Vector3.Distance(hitInfo.point, new Vector2(1, 0));
                    //dividerSum += Vector3.Distance(hitInfo.point, new Vector2(1, 1));
                    //dividerSum += Vector3.Distance(hitInfo.point, new Vector2(0, 1));
                    closeUVs.Clear();

                    if (collidersDetected.Contains(colliders[i]))
                    {
                        uvs = meshFilter[i].mesh.uv;
                        vertices = meshFilter[i].mesh.vertices;

                        for (int j = 0; j < vertices.Length; j++)
                        {
                            float dist = Vector3.Distance(hitInfo.point, meshFilter[i].transform.TransformPoint(vertices[j]));
                            if (dist < brushSize)
                            {
                                _currentCoord += uvs[j] * (1 - (dist / brushSize));

                                dividerSum += (1 - (dist / brushSize));

                                closeUVs.Add(uvs[j]);
                            }
                        }

                        //_currentCoord += new Vector2(0, 0) * (1 - (Vector3.Distance(hitInfo.point, new Vector2(0, 0)) / brushSize));
                        //_currentCoord += new Vector2(1, 0) * (1 - (Vector3.Distance(hitInfo.point, new Vector2(1, 0)) / brushSize));
                        //_currentCoord += new Vector2(1, 1) * (1 - (Vector3.Distance(hitInfo.point, new Vector2(1, 1)) / brushSize));
                        //_currentCoord += new Vector2(0, 1) * (1 - (Vector3.Distance(hitInfo.point, new Vector2(0, 1)) / brushSize));
                        //dividerSum += (1 - (Vector3.Distance(hitInfo.point, new Vector2(0, 0)) / brushSize));
                        //dividerSum += (1 - (Vector3.Distance(hitInfo.point, new Vector2(1, 0)) / brushSize));
                        //dividerSum += (1 - (Vector3.Distance(hitInfo.point, new Vector2(1, 1)) / brushSize));
                        //dividerSum += (1 - (Vector3.Distance(hitInfo.point, new Vector2(0, 1)) / brushSize));

                        _currentCoord /= dividerSum;
                        _smoothedCoord = Vector2.Lerp(_smoothedCoord, _currentCoord, Time.deltaTime * 20);
                        if (dividerSum != 0)
                        {
                            //coordObject.localPosition = new Vector3(_smoothedCoord.x * 50f, 0.02f, _smoothedCoord.y * 50f);
                            //coordObject2.localPosition = new Vector3(_smoothedCoord.x * 50f, 0.01f, _smoothedCoord.y * 50f);
                            //tracers[0].localPosition = new Vector3(_currentCoord.x * 50f, 0.01f, _currentCoord.y * 50f);

                            drawers[i].Activate();
                            drawers[i].SetTracerPos(new Vector3(_currentCoord.x * 50f, 0.01f, _currentCoord.y * 50f));
                        }
                    }
                    else
                    {
                        drawers[i].Deactivate();
                    }
                }
            }
        }
    }
}
