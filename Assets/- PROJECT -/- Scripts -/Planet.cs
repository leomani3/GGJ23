using C8;
using Cinemachine;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Planet : MonoBehaviour
{
    [SerializeField] private float dragSpeed;
    [SerializeField] private Transform cameraPivot;
    [SerializeField] private float zoomSpeed;
    [SerializeField] private float minZoom;
    [SerializeField] private float maxZoom;
    [SerializeField] private CinemachineVirtualCamera vCam;
    [SerializeField] private LayerMask layerMask;

    private List<Chunk> _chunks;

    private Vector3 _previousPos;
    private Vector3 _drag;
    private Camera _mainCam;
    private RaycastHit _raycastHit;
    private bool active;

    public bool Active
    {
        get { return active; }
        set 
        { 
            active = value;
            if (active)
            {
                transform.DOScale(Vector3.one, 1.5f).SetEase(Ease.OutElastic, 0.5f);
            }
        }
    }

    public List<Chunk> Chunks => _chunks;

    private void Awake()
    {
        _chunks = GetComponentsInChildren<Chunk>().ToList();
        _mainCam = Camera.main;
    }

    private void Update()
    {
        if (!active) return;

        vCam.GetCinemachineComponent<CinemachineTransposer>().m_FollowOffset.z += Input.mouseScrollDelta.y * zoomSpeed * Time.deltaTime;
        vCam.GetCinemachineComponent<CinemachineTransposer>().m_FollowOffset.z = Mathf.Clamp(vCam.GetCinemachineComponent<CinemachineTransposer>().m_FollowOffset.z, -150, -76);

        if (Input.GetMouseButtonDown(0))
        {
            _previousPos = Input.mousePosition;
            CheckForCollectibles();
        }

        if (Input.GetMouseButton(0))
        {
            _drag = Input.mousePosition - _previousPos;
            Vector3 rotationAmount = new Vector3(-_drag.y, _drag.x, 0) * dragSpeed;
            cameraPivot.Rotate(rotationAmount, Space.Self);
            //cameraPivot.rotation = Quaternion.Slerp(cameraPivot.rotation, Quaternion.LookRotation(cameraPivot.eulerAngles + rotationAmount, cameraPivot.transform.up), 20 * Time.deltaTime);
            _previousPos = Input.mousePosition;
        }
    }

    public void CheckForCollectibles()
    {
        if (Physics.Raycast(_mainCam.ScreenPointToRay(Input.mousePosition), out _raycastHit, layerMask))
        {
            Collectible collectible = _raycastHit.collider.GetComponent<Collectible>();
            if (collectible != null)
            {
                collectible.Collect();
            }
        }
    }

}