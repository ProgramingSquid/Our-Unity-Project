using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapChunk : MonoBehaviour
{
    public MapGenerator MapGenerator;
    Material material;
    public float noiseScale;
    public float islandDencity;
    public float islandChance;
    public float grassChance;
    public Vector2 noiseOffset;
    public GrassPatches grassPatches;

    public void UpdateValues()
    {
        MapGenerator = MapGenerator.Generator;  
        noiseScale = MapGenerator.noiseScale;
        islandDencity = MapGenerator.islandDencity;
        islandChance = MapGenerator.islandChance;
        grassChance = MapGenerator.grassChance;
        if (transform.position != Vector3.zero) { noiseOffset = MapGenerator.startNoiseOffset + FindOffset(transform); }
        else { noiseOffset = MapGenerator.startNoiseOffset; }
        
        material = gameObject.GetComponent<MeshRenderer>().material;
        material.SetFloat("_NoiseScale", noiseScale);
        material.SetFloat("_IslandDencity", islandDencity);
        material.SetFloat("_IslandChance", islandChance);
        material.SetVector("_NoiseOffset", noiseOffset);
        grassPatches.UpdateValues();
    }
    public void UpdateChunk(Vector3 VeiwerPos, int renderDist, Bounds bounds)
    {
        //Sets the visibility of the chunk bassed on render distance
        float VeiwerDistFromNearestEdge = Mathf.Sqrt(bounds.SqrDistance(VeiwerPos));
        bool visible = VeiwerDistFromNearestEdge <= renderDist;
        SetVisible(visible);
    }
    public void SetVisible(bool visible)
    {
        gameObject.SetActive(visible);
    }
    public bool IsVisible()
    {
        return gameObject.activeSelf;
    }
    public Vector2 FindOffset(Transform _transform)
    {
        Vector2 offset = Vector2.zero;
        offset = new Vector2(MapGenerator.VeiwedChunkChord.x * -7, MapGenerator.VeiwedChunkChord.z * -7);
        return offset;
    }
}
