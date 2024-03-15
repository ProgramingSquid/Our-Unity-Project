using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BiomeQuestManager : MonoBehaviour
{
    public SnapToItem QuestSnaping;
    public RectTransform questList;
    public List<BiomeQuest> equipedQuests = new List<BiomeQuest>();
    public BiomeQuest selectedQuest;
    private void Start()
    {
        
    }

    private void Update()
    {
        //TO DO: Update quest UI/Game Objects

        //Udating List with new changes
        for (int i = 0; i < questList.childCount; i++)
        {
            /*equipedQuests[i] = questList.GetChild(i);*/
        }
        
        //TO DO: Set Selected quest from UI
        //selectedQuest = equipedQuests[QuestSnaping.currentItem];

        //Checking for completion

        foreach (BiomeQuest quest in equipedQuests)
        {
            if (quest == selectedQuest && quest.biomeQuest.isConditionMet()) { quest.biomeQuest.OnCompleat(); return; }
            if (quest.biomeQuest.isConditionMet()){ quest.biomeQuest.OnCompleation.Invoke(); }
        }
    }

    public void Unequip(BiomeQuest quest)
    {
        equipedQuests.Remove(quest);
    }
    public void Equip(BiomeQuest quest)
    {
        quest.biomeQuest.OnEquip.Invoke();
        equipedQuests.Add(quest);
    }
   
}


