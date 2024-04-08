using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BiomeQuest : MonoBehaviour
{
    public Image biomeImage;
    public Slider progressBar;
    public TMP_Text discription;
    public BiomeQuestSO biomeQuest;
    

    private void Start()
    {

    }
    private void Update()
    {
        UpdateUI();
    }

    public void UpdateUI()
    {
        biomeImage.sprite = biomeQuest.biome.questImage;
        discription.text = biomeQuest.discription;
        progressBar.value = biomeQuest.condition.compleatness;

    }
}
