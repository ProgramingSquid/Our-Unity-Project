using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Background : MonoBehaviour
{
    public Material material;
    public float SmothingAmount;
    public Transform player;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        material.SetVector("_Offset", new Vector2(player.position.x / SmothingAmount, player.position.y / SmothingAmount));
    }
}
