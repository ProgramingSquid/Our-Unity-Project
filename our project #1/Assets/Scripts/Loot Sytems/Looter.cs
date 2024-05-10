using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Looter : MonoBehaviour
{
    public List<IPickUpable> inventory = new List<IPickUpable>();
    public List<lootObj> pickUpableLoot = new List<lootObj>();

    public float pickUpRadius = 3;
    public Vector3 pickUpOffset;
    public LayerMask pickUpMask;
    public Transform AnimationEndPos;

    public float AnimationSpeed = 1;
    bool input;



    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        UpdatePickUpableLoot();

        foreach (lootObj loot in pickUpableLoot)
        {
            if (loot.pickUp.IsAutomaticalyCollected)
            {
                PickUpLoot(loot);
            }
        }

        if (input)
        {
            //TO DO: Add the option for some loot to be picked up atomaticaly
            PickUpLoot(pickUpableLoot[0]);
            input = false;
        }
    }

    void UpdatePickUpableLoot()
    {

        Collider[] colliders = Physics.OverlapSphere(transform.position + pickUpOffset, pickUpRadius, pickUpMask, QueryTriggerInteraction.Collide);
        pickUpableLoot.Clear();

        foreach (Collider collider in colliders)
        {
            lootObj loot = new lootObj();
            pickUpableLoot.Add(loot);

            loot.lootGO = collider.gameObject;
            loot.lootComponent = loot.lootGO.GetComponent<LootComponent>();
            loot.pickUp = loot.lootComponent.lootSO as IPickUpable;
        }

    }

    void PickUpLoot(lootObj loot)
    {
        //TO DO: Make it for not all loot gets added to inventory
        if (loot.pickUp.IsAddedToInventory) { inventory.Add(loot.pickUp); }
        if (loot.lootComponent.NotGettingPickedUp == true) { loot.pickUp.PickUp(); }
        loot.lootComponent.NotGettingPickedUp = false;
        StartCoroutine(PickUpAnimation(pickUpableLoot[0].lootGO, AnimationSpeed));
        

    }

    IEnumerator PickUpAnimation(GameObject gameObject, float speed)
    {
        
        Vector3 Apos = gameObject.transform.position;
        Vector3 Bpos = UnityEngine.Random.insideUnitSphere * pickUpRadius * 1.25f;
        Debug.Log(Bpos);
        Vector3 Cpos = AnimationEndPos.position;
        Vector3 ABpos;
        Vector3 BCpos;
        Vector3 lerpPos;
        float lerp = 0;

        while (lerp < 1)
        {
            lerp = lerp += Time.deltaTime * speed % 1f;
            ABpos = Vector3.Lerp(Apos, Bpos, lerp);
            BCpos = Vector3.Lerp(Bpos, Cpos, lerp);
            lerpPos = Vector3.Lerp(ABpos, BCpos, lerp);
            gameObject.transform.position = lerpPos;

            

            yield return null;
        }
        if (lerp >= 1)
        {
            Destroy(gameObject);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = new Color(1, .92f, .016f, .5f);
        Gizmos.DrawWireSphere(transform.position + pickUpOffset, pickUpRadius);
    }

    public void PickUpLootInput(InputAction.CallbackContext context)
    {
        if (context.started) { input = true; }
    }
}
[Serializable]
public class lootObj
{
    public GameObject lootGO;
    public LootComponent lootComponent;
    public IPickUpable pickUp;
}