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
    public float questDistanceGoal;
    public Vector2 questDirectionGoal;
    public float distanceTraveled;
    public float playerZAngle { get; private set; }
    public float BiomeAngle { get; private set; }

    public static CompassUI intance;
    

    // Start is called before the first frame update
    private void Start()
    {
        intance = this;
    }

    // Update is called once per frame
    void Update()
    {
        
        BiomeAngle = Mathf.Atan2(-questDirectionGoal.x, questDirectionGoal.y) * Mathf.Rad2Deg;
        BiomeNeedle.rotation = Quaternion.Euler(0, 0, BiomeAngle);

        distanceText.text = "Distance " +  distanceTraveled + "/" + questDistanceGoal.ToString();

        playerZAngle = MovementControl.player.angle * Mathf.Rad2Deg - 90;
        PlayerNeedle.rotation = Quaternion.Euler(0, 0, playerZAngle);
        
    }
}
