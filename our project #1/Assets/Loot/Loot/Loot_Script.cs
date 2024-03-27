using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;
[CreateAssetMenu(fileName = "LootSO", menuName = "Loot Items/Loot")]
public class Loot : ScriptableObject, IPickUpable
{
    public float pickUpSpeed { get; set; }
    public GameObject prefab { get; set; }
    public float splineSize { get; set; }

    public void PickUp()
    {
        throw new System.NotImplementedException();
    }
}