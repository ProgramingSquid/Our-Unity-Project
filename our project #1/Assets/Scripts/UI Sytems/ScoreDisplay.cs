using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class ScoreDisplay : MonoBehaviour
{
    public TextMeshProUGUI finalScore;
    public TextMeshProUGUI highScore;
    public GameObject player;
    public GameObject screen;

    // Start is called before the first frame update
    void Start()
    {

    }

    
    public void Retry(int level)
    {
        Application.LoadLevel(level);
    }
    public void PlayerDeath(float delay)
    {
        Invoke("playerDeath", delay);
    }
    void playerDeath()
    {

        screen.SetActive(true);
        player.SetActive(false);
        int currentScore = FindObjectOfType<ScoreManager>().score;
        finalScore.SetText(currentScore.ToString());
        string fileName = Application.persistentDataPath + "/HighScore.txt";
        if (File.Exists(fileName))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(fileName, FileMode.Open);
            int oldHighScore = (int)formatter.Deserialize(stream);


            if (oldHighScore > currentScore)
            {
                highScore.text = oldHighScore.ToString();
                stream.Close();

            }
            else
            {
                highScore.text = currentScore.ToString();
                stream.Close();
                FileStream save = new FileStream(fileName, FileMode.Create);
                formatter.Serialize(save, currentScore);
                save.Close();


            }

        }
        else
        {
            highScore.text = currentScore.ToString();
            FileStream save = new FileStream(fileName, FileMode.Create);
            BinaryFormatter formatter = new BinaryFormatter();
            formatter.Serialize(save, currentScore);
            save.Close();

        }
    }
}
