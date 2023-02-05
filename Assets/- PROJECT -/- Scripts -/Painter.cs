using MyBox;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Painter : MonoBehaviour
{
    [SerializeField] private new Collider collider;
    [SerializeField] private MeshFilter meshFilter;
    [SerializeField] private float brushSize;
    [SerializeField] private Color colorToSet;

    private Camera _mainCam;
    private Mesh _mesh;
    private Vector3[] vertices;
    private Color[] colors;


    private void Awake()
    {
        _mainCam = Camera.main;
        _mesh = meshFilter.mesh;
    }


    [ButtonMethod]
    private void SetColor()
    {
        colors = new Color[meshFilter.mesh.vertices.Length];
        vertices = meshFilter.mesh.vertices;
        for (int i = 0; i < colors.Length; i++)
        {
            colors[i] = colorToSet;
        }
        meshFilter.mesh.colors = colors;

        //meshFilter.mesh.SetColors(colors);
    }


    private void Update()
    {
        if (Input.GetMouseButton(0))
        {
            if (collider.Raycast(_mainCam.ScreenPointToRay(Input.mousePosition), out RaycastHit hitInfo, Mathf.Infinity))
            {
                colors = meshFilter.mesh.colors;
                vertices = meshFilter.mesh.vertices;
                for (int i = 0; i < colors.Length; i++)
                {
                    float dist = Vector3.Distance(hitInfo.point, meshFilter.transform.TransformPoint(vertices[i]));
                    if (dist < brushSize)
                    {
                        colors[i] += Color.Lerp(Color.white, Color.clear, dist / brushSize);
                    }
                }

                meshFilter.mesh.colors = colors;
            }
        }
    }
}
