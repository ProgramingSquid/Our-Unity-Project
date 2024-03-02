using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public ScenesManager.sceneBuildIndexes sceneToUnloadIndex;
    public ScenesManager.sceneBuildIndexes sceneToLoadIndex;
    public GameObject currentMenu;
    public GameObject nextMenu;

    public void swapScenes()
    {
        if (SceneManager.GetSceneByBuildIndex(2).isLoaded)
        {
            EnemyWaveManager waveManager = (EnemyWaveManager)FindObjectOfType(typeof(EnemyWaveManager));
            waveManager.StopAllCoroutines();
            Enemy[] enemies = (Enemy[])FindObjectsOfType(typeof(Enemy));
            for (int i = 0; i < enemies.Length; i++)
            {
                Destroy(enemies[i].gameObject);
            }
        }
        ScenesManager.instance.SwitchScences(sceneToLoadIndex, sceneToUnloadIndex);
    }

    public void SwapMenus()
    {
        currentMenu.SetActive(false);
        nextMenu.SetActive(true);
    }

    public void Quit()
    {
        Application.Quit();
    }



}
