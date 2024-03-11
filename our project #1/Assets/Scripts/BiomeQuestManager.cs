using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BiomeQuestManager : MonoBehaviour
{
    public List<BiomeQuest> equipedQuests = new List<BiomeQuest>();
    public BiomeQuest selectedQuest;
    private void Start()
    {
        
    }

    private void Update()
    {
        foreach (BiomeQuest quest in equipedQuests)
        {
            if (quest == selectedQuest && quest.isConditionMet()) { quest.OnCompleat(); return; }
            if (quest.isConditionMet()){ quest.OnCompleation.Invoke(); }
        }
    }

    public void Unequip(BiomeQuest quest)
    {
        equipedQuests.Remove(quest);
    }
    public void Equip(BiomeQuest quest)
    {
        quest.OnEquip.Invoke();
        equipedQuests.Add(quest);
    }
   
}


