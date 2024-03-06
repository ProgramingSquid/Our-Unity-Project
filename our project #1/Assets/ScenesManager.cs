using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ScenesManager : MonoBehaviour
{
    public static ScenesManager instance;
    public GameObject loadingScreen;
    public Slider loadingBar;
    //TEST

    public enum sceneBuildIndexes
    {
        PersistentScene = 0,
        MainMenu = 1,
        GameScene = 2

    }

    void Awake()
    {
        instance = this;

        SceneManager.LoadSceneAsync((int)sceneBuildIndexes.MainMenu, LoadSceneMode.Additive);
    
    }

    List<AsyncOperation> scenesLoading = new List<AsyncOperation>();
    public void SwitchScences(sceneBuildIndexes sceneToLoad, sceneBuildIndexes sceneToUnload)
    {
        
        loadingScreen.SetActive(true);
        scenesLoading.Add(SceneManager.UnloadSceneAsync((int)sceneToUnload));
        scenesLoading.Add(SceneManager.LoadSceneAsync((int)sceneToLoad, LoadSceneMode.Additive));
        StartCoroutine(GetSceneLoadProgress());
    }
    public float totalSceneProgress;
    IEnumerator GetSceneLoadProgress()
    {
        for (int i = 0; i < scenesLoading.Count; i++)
        {
            while (!scenesLoading[i].isDone)
            {
                totalSceneProgress = 0;

                foreach(AsyncOperation operation in scenesLoading)
                {
                    totalSceneProgress += operation.progress;
                }

                totalSceneProgress = (totalSceneProgress / scenesLoading.Count);

                loadingBar.value = totalSceneProgress;

                yield return null;
            }
        }

        loadingScreen.SetActive(false);
    }
}
