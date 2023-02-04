using C8;
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
    private Camera _mainCam;
    private RaycastHit _raycastHit;

    public List<Chunk> Chunks => _chunks;

    private void Awake()
    {
        _chunks = GetComponentsInChildren<Chunk>().ToList();
        _mainCam = Camera.main;
    }

    private void Update()
    {
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
        if (Physics.Raycast(_mainCam.ScreenPointToRay(Input.mousePosition), out _raycastHit))
        {
            Collectible collectible = _raycastHit.collider.GetComponent<Collectible>();
            if (collectible != null)
            {
                collectible.Collect();
            }
        }
    }

}