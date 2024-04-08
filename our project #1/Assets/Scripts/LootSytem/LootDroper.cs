using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class LootDroper : MonoBehaviour
{
    public List<GameObject> Loot = new List<GameObject>();
    List<GameObject> LootObjects = new List<GameObject>();

    [MinMaxSlider(0,10)] public Vector2Int itemsPerDrop; /*min saved in X, Max saved in Y*/

    public Transform dropPos;

    public float dropAreaRadius;


    [Button("DropLoot")]
    public void DropLoot()
    {
        LootObjects.Clear();

        foreach (GameObject lootGO in Loot)
        {
            LootComponent lootComponent = lootGO.GetComponent<LootComponent>();
            IPickUpable pickUp = lootComponent.lootSO as IPickUpable;
            int rarity = 1;
            rarity = pickUp.Rarity;

            for (int i = 0; i < rarity; i++)
            {
                LootObjects.Add(lootGO);
            }
        }

        int itemAmount = Random.Range(itemsPerDrop.x, itemsPerDrop.y);

        for (int i = 0; i < itemAmount; i++)
        {
            int item = Random.Range(0, LootObjects.Count);
            Vector3 pos = (Random.insideUnitSphere * dropAreaRadius) + dropPos.position;
            Instantiate(LootObjects[item], pos, LootObjects[item].gameObject.transform.rotation);
        }
    }
    private void OnValidate()
    {
        for (int i = 0; i < Loot.Count; i++)
        {
            GameObject loot = Loot[i];
            if (loot.TryGetComponent(typeof(LootComponent), out Component component) == false)
            {
                Loot.Remove(loot);
                Debug.LogError("Loot[" + i + "] must impliment IPickUpable");
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(dropPos.position, dropAreaRadius);
    }
}
