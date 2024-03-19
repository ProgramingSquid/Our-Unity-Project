using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BiomeQuestManager : MonoBehaviour
{
    public SnapToItem QuestSnaping;
    public RectTransform questList;
    public List<BiomeQuest> equipedQuests = new List<BiomeQuest>();
    public BiomeQuest selectedQuest;
    public GameObject QuestUIPrefab;
    BiomeQuest lastSelected;
    private void Start()
    {
        foreach (BiomeQuest quest in equipedQuests)
        {
            quest.biomeQuest.TogleCompleat = true;
            quest.biomeQuest.TogleCompleation = true;
        }
    }

    private void Update()
    {
        
        selectedQuest = equipedQuests[QuestSnaping.currentItem];
        
        if(selectedQuest != lastSelected && lastSelected != null)
        {
            lastSelected.biomeQuest.TogleCompleat = true;
        }

        foreach (BiomeQuest quest in equipedQuests)
        {
            
            if (quest == selectedQuest && quest.biomeQuest.isConditionMet() && quest.biomeQuest.TogleCompleat) 
            {
                quest.biomeQuest.OnCompleat();
            }
            if (quest.biomeQuest.isConditionMet() && quest.biomeQuest.TogleCompleation) 
            {
                quest.biomeQuest.OnCompleation.Invoke(); 
                quest.biomeQuest.TogleCompleation = false;
            }
        }
        lastSelected = selectedQuest;
    }

    public void UnequipQuest(BiomeQuest quest)
    {

        equipedQuests.Remove(quest);
        Destroy(quest.gameObject);
    }
    public void EquipNewQust(BiomeQuestSO questSO)
    {
        GameObject GO = Instantiate(QuestUIPrefab, questList);
        BiomeQuest quest = GO.GetComponent<BiomeQuest>();
        quest.biomeQuest = questSO;
        quest.biomeQuest.OnEquip.Invoke();
        equipedQuests.Add(quest);
    }
   
}


