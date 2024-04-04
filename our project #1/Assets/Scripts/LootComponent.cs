using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;

public class LootComponent : MonoBehaviour
{
    [Expandable] public ScriptableObject lootSO;

    [Label("Animation")]
    public float animationSpeed;
    public float animationMagnitude;
    public float lifeTime;
    float originalY;
    bool isVisible = true;
    public bool NotGettingPickedUp = true;


    // Start is called before the first frame update
    void Start()
    {
        originalY = transform.position.y;
    }

    // Update is called once per frame
    void Update()
    {
        if(NotGettingPickedUp == false)
        {
            transform.position = new Vector3(transform.position.x, originalY + (Mathf.Sin(Time.time * animationSpeed) * animationMagnitude),
                transform.position.z);

        }

    }

    private void OnBecameInvisible()
    {
        isVisible = false;
        Invoke("KillObj", lifeTime);
    }

    //Destroying after not visible for lifeTime:
    private void OnBecameVisible()
    {
        isVisible = true;
    }

    private void OnValidate()
    {
        if (lootSO is not IPickUpable)
        {
            lootSO = null;
            Debug.LogError("lootSO must impliment IPickUpable");
        }
        
    }
    void KillObj()
    {
        if (isVisible == false) { Destroy(gameObject); }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(transform.position, transform.localScale);
    }
}


