using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TabGroup : MonoBehaviour
{
    public List<TabButton> tabs;
    public Color idleColor;
    public Color hoverColor;
    public Color SeletedColor;
    public List<GameObject> gameObjectsToSwap;
    [HideInInspector] public TabButton selectedTab;

    public void Subscribe(TabButton button)
    {
        if(tabs == null)
        {
            tabs = new List<TabButton>();
        }
        tabs.Add(button);
    }
    
    public void OnTabEnter(TabButton button)
    {
        RestTabs();
        if(selectedTab == null || button != selectedTab)
        {
            button.background.color = button.defualtColor;
            button.background.color *= hoverColor;
        }
    }
    public void OnTabSelected(TabButton button)
    {
        selectedTab = button;
        RestTabs();
        int index = button.transform.GetSiblingIndex();
        for(int i = 0; i < gameObjectsToSwap.Count; i++)
        {
            if(i == index)
            {
                gameObjectsToSwap[i].SetActive(true);
            }
            else
            {
                gameObjectsToSwap[i].SetActive(false);
            }
        }
        button.background.color = button.defualtColor;
        button.background.color *= SeletedColor;
    }

    public void OnTabExit(TabButton button)
    {
        RestTabs();
    }

    public void RestTabs()
    {
        foreach(TabButton button in tabs)
        {
            if(selectedTab != null && button == selectedTab) { continue; }
            button.background.color = button.defualtColor;
            button.background.color *= idleColor;
        }
    }
}
