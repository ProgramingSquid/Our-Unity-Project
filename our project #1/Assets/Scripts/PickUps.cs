using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPickUpable
{
    public float pickUpSpeed { get; set; }
    public GameObject prefab { get; set; }
    public float splineSize { get; set; }

    public void PickUp();
        
}

