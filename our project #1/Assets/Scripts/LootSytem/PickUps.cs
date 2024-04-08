using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPickUpable
{
     
     int Rarity { get; set; }

    bool IsAutomaticalyCollected { get; set; }
    bool IsAddedToInventory { get; set; }

    public void PickUp();
        
}

