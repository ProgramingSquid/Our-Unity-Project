using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;
using System;
[CreateAssetMenu(fileName = "LootSO", menuName = "Loot Items/Health Pick Up")]
public class HealthPickUp : ScriptableObject, IPickUpable
{

    [SerializeField] int rarity;
    public int Rarity { get => rarity; set => rarity = value; }

    [SerializeField] bool isAutomaticalyCollected;
    public bool IsAutomaticalyCollected { get => isAutomaticalyCollected; set => isAutomaticalyCollected = value; }

    [SerializeField] bool isAddedToInventory;
    public bool IsAddedToInventory { get => isAddedToInventory; set => isAddedToInventory = value; }
    [MinMaxSlider(0, 10)]
    public Vector2 healAmount;

    public void PickUp()
    {
        float amount = UnityEngine.Random.Range(healAmount.x, healAmount.y);
        PlayerHealManager.instance.AddHeal(amount);
    }
}