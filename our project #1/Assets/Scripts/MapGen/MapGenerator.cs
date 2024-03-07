using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;


public class MapGenerator : MonoBehaviour
{
    
    public List<Biom> bioms = new List<Biom>();
    public List<Biom> biomsAvalable = new List<Biom>();
    public int currentBiomIndex;
    Biom lastBiom;
    public AnimationCurve noiseScaleCurve;
    public AnimationCurve islandDencityCurve;
    public AnimationCurve islandChanceCurve;
    public AnimationCurve islandGrassCurve;

    public Vector2 startNoiseOffset;
    public Vector2 noiseOffset;
    [Range(.1f, 10)] public float noiseScale = 3.75f;
    [Range(.01f, 5)] public float islandDencity = .38f;
    [Min(.1f)] public float islandChance = .55f;
    [Min(.1f)] public float grassChance = .55f;

    public const int renderDist = 50;
    int chunksVisibleInDist = 2;
    public int chunkSize = 50;
    [SerializeField]public Dictionary<Vector3, MapChunk> MapChunks = new Dictionary<Vector3, MapChunk>();
    public List<MapChunk> ChunksVisibleLastUpdate = new List<MapChunk>();
    public Transform Viewer;
    public GameObject ChunkPrefab;
    public static Vector3 VeiwerPos;

    Material material;

    Vector3 chunkPosition;
    public Vector3 VeiwedChunkChord;

    public static MapGenerator Generator;

    private void Start()
    {
        Generator = this;
        for (int i = 0; i < bioms.Count; i++)
        {
            biomsAvalable.Add(bioms[i]);
           
        }
        chunksVisibleInDist = Mathf.RoundToInt(renderDist / chunkSize);
        GenerateStartValues(true);
    }

    [Button]
    void GenrateButton() { GenerateStartValues(true); }

      public void GenerateStartValues(bool useRandomIndex = false, int biomeIndex = 0)
    {
        //Setting The avalible options
        biomsAvalable.Clear();
        for (int i = 0; i < bioms.Count; i++)
        {
            biomsAvalable.Add(bioms[i]);
            
        }

        biomsAvalable.Remove(lastBiom);

        // Picking random out of avalible options
        if (useRandomIndex) { currentBiomIndex = Random.Range(0, biomsAvalable.Count); }
        else { currentBiomIndex = biomeIndex; }

        startNoiseOffset.x = Random.Range(-500f, 500f);
        startNoiseOffset.y = Random.Range(-500f, 500f);
        noiseScale = biomsAvalable[currentBiomIndex].noiseScaleCurve.Evaluate(Random.Range(0f, 1f));
        islandDencity = biomsAvalable[currentBiomIndex].islandDencityCurve.Evaluate(Random.Range(0f, 1f));
        islandChance = biomsAvalable[currentBiomIndex].islandChanceCurve.Evaluate(Random.Range(0f, 1f));

        material = gameObject.GetComponent<SpriteRenderer>().material;
        material.SetFloat("_NoiseScale", noiseScale);
        material.SetFloat("_IslandDencity", islandDencity);
        material.SetFloat("_IslandChance", islandChance);
        material.SetVector("_NoiseOffset", startNoiseOffset);

        grassChance = islandChance - biomsAvalable[currentBiomIndex].islandGrassCurve.Evaluate(Random.Range(0, 1));

        //Setting up for next function call
        lastBiom = biomsAvalable[currentBiomIndex];
    }

    private void Update()
    {
        VeiwerPos = new Vector3(Viewer.position.x, 0, Viewer.position.z);
        UpdateVisibleChunks();
    }

    void UpdateVisibleChunks()
    {
        // refresh Chunks visible last frame
        for (int i = 0; i < ChunksVisibleLastUpdate.Count; i++)
        {
            ChunksVisibleLastUpdate[i].SetVisible(false); ;
        }
        ChunksVisibleLastUpdate.Clear();

        int currentChordX = Mathf.RoundToInt(Viewer.position.x / chunkSize);
        int currentChordZ = Mathf.RoundToInt(Viewer.position.z / chunkSize);

        // loop through all Chunks
        for (int ZOffset = -chunksVisibleInDist; ZOffset <= chunksVisibleInDist; ZOffset++) {
            for (int XOffset = -chunksVisibleInDist; XOffset <= chunksVisibleInDist; XOffset++) 
            {
                
                VeiwedChunkChord = new Vector3(currentChordX + XOffset, 0, currentChordZ + ZOffset);

                Bounds bounds;
                chunkPosition = VeiwedChunkChord * chunkSize;
                bounds = new Bounds(chunkPosition, Vector3.one * chunkSize);

                if (MapChunks.ContainsKey(VeiwedChunkChord))
                {
                    MapChunks[VeiwedChunkChord].UpdateChunk(VeiwerPos, renderDist, bounds);
                    if (MapChunks[VeiwedChunkChord].IsVisible()) { ChunksVisibleLastUpdate.Add(MapChunks[VeiwedChunkChord]); }
                }
                else
                {
                    //Create new chunk if needed
                    GameObject ChunkGo;
                    ChunkGo = Instantiate(ChunkPrefab);
                    Vector3 newPos = chunkPosition;
                    newPos.y = -19;
                    ChunkGo.transform.position = newPos;
                    MapChunk chunk = ChunkGo.GetComponent<MapChunk>();
                    chunk.UpdateValues();
                    chunk.SetVisible(false);
                    MapChunks.Add(VeiwedChunkChord, chunk);
                    
                }
            }
        }


    }

}

[System.Serializable]
public class Biom
{
    public string name;
    public AnimationCurve noiseScaleCurve;
    public AnimationCurve islandDencityCurve;
    public AnimationCurve islandChanceCurve;
    public AnimationCurve islandGrassCurve;
}