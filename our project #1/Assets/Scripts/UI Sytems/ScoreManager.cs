using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    public int score = 0;
    public TextMeshProUGUI text;

    public void AddScore(int amount)
    {
        score += amount;
        text.SetText(score.ToString());
    }
}
