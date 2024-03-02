using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrassPatches : MonoBehaviour
{
    public MapChunk SandMapChunk;
    public Texture texture;
    public MapGenerator MapGenerator;
    Material material;


    // Start is called before the first frame update
    void Start()
    {
        SandMapChunk = transform.GetComponentInParent<MapChunk>();
        MapGenerator = SandMapChunk.MapGenerator;
    }

    public void UpdateValues()
    {
        MapGenerator = SandMapChunk.MapGenerator;
        material = gameObject.GetComponent<MeshRenderer>().material;

        material.SetFloat("_NoiseScale", SandMapChunk.noiseScale);
        material.SetFloat("_IslandDencity", SandMapChunk.islandDencity);
        material.SetFloat("_IslandChance", MapGenerator.grassChance);
        material.SetVector("_NoiseOffset", SandMapChunk.noiseOffset);
        material.SetTexture("_SandTexture", texture);
    }
}
