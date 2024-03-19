using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;

public class CompassUI : MonoBehaviour
{
    public TMP_Text distanceText;
    public RectTransform BiomeNeedle;
    public RectTransform PlayerNeedle;
    public float questDistance;
    public Vector2 questDirection;
    float playerZAngle;

    public static CompassUI intance;
    

    // Start is called before the first frame update
    private void Start()
    {
        intance = this;
    }

    // Update is called once per frame
    void Update()
    {
        
        float angle = Mathf.Atan2(-questDirection.x, questDirection.y) * Mathf.Rad2Deg;
        BiomeNeedle.rotation = Quaternion.Euler(0, 0, angle);

        distanceText.text = "Distance " + questDistance.ToString();

        playerZAngle = MovementControl.player.angle * Mathf.Rad2Deg - 90;
        PlayerNeedle.rotation = Quaternion.Euler(0, 0, playerZAngle);
        
    }
}
