using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GiveScoreOnDeath : MonoBehaviour
{
    public int points = 1;

    public void GiveScore()
    {
        FindObjectOfType<ScoreManager>().AddScore(points);
    }
}