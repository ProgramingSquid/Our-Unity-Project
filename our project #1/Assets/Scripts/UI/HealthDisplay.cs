using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HealthDisplay : MonoBehaviour
{
    public TextMeshProUGUI text;
    public HealthSystem healthSystem;


    private void Awake()
    {
        text.text = "Health: " + healthSystem.currentHealth;
    }

    // Update is called once per frame
    void Update()
    {
        text.text = "Health: " + healthSystem.currentHealth;
    }


}
