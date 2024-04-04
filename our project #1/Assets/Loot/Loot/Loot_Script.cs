using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;
using System;
[CreateAssetMenu(fileName = "LootSO", menuName = "Loot Items/Loot")]
public class Loot : ScriptableObject, IPickUpable
{

    [SerializeField] int rarity;
    public int Rarity { get => rarity; set => rarity = value; }

    [SerializeField] bool isAutomaticalyCollected;
    public bool IsAutomaticalyCollected { get => isAutomaticalyCollected; set => isAutomaticalyCollected = value; }

    public void PickUp()
    {
        
    }
}