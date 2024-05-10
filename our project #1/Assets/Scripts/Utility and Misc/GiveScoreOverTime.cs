using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GiveScoreOverTime : MonoBehaviour
{
    public int amount = 1;
    public float interval = 5;
    public ScoreManager scoreManager;
    float scoreTimer;
    // Start is called before the first frame update
    void Start()
    {
        scoreTimer = interval;
    }

    // Update is called once per frame
    void Update()
    {
        scoreTimer -= Time.deltaTime;
        if (scoreTimer <= 0)
        {
            scoreManager.AddScore(amount);
            scoreTimer = interval;
        }
    }
}
