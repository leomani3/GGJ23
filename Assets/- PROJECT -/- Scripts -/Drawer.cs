using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drawer : MonoBehaviour
{
    [SerializeField] private RenderTexture renderTextureRef;
    [SerializeField] private Camera textureCam;
    [SerializeField] private Camera outlineTextureCam;
    [SerializeField] private Transform tracer;
    [SerializeField] private TrailRenderer[] trailRenderers;

    private RenderTexture texture;
    private RenderTexture outlineTexture;
    private MaterialPropertyBlock block;


    public void Init(MeshRenderer meshRenderer)
    {
        texture = new RenderTexture(renderTextureRef);
        outlineTexture = new RenderTexture(renderTextureRef);

        block = new MaterialPropertyBlock();


        textureCam.targetTexture = texture;
        block.SetTexture("_Texture", texture);
        outlineTextureCam.targetTexture = outlineTexture;
        block.SetTexture("_TextureOutline", outlineTexture);

        meshRenderer.SetPropertyBlock(block);
    }

    public void SetTracerPos(Vector3 localPos)
    {
        tracer.localPosition = localPos;
    }

    public void Activate()
    {
        textureCam.gameObject.SetActive(true);
        outlineTextureCam.gameObject.SetActive(true);

        for (int i = 0; i < trailRenderers.Length; i++)
        {
            trailRenderers[i].Clear();
            trailRenderers[i].emitting = true;
        }
    }

    public void Deactivate()
    {
        textureCam.gameObject.SetActive(false);
        outlineTextureCam.gameObject.SetActive(false);

        //for (int i = 0; i < trailRenderers.Length; i++)
        //{
        //    trailRenderers[i].Clear();
        //}
    }
}
