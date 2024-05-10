using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Events;


[CreateAssetMenu(fileName = "NewBiomeQuest", menuName = "Biomes/Biome Quests/Create New Biom Quest")]
public class BiomeQuestSO : ScriptableObject
{
    [Range(0, 1f), Tooltip("A value to influence the probability this quest may apear in a game, 0 = unlickly 1 = lickly.")]
    public float rarityPiority = .1f;
    [Tooltip("Biome that the quest is for."), Expandable]
    public Biome biome;
    public BiomeQuestCondition condition;

    [Tooltip("A bias for what dirrection the quest should lead the player once compleated (use 0,0 for no bias)")]
    public Vector2 dirrectionBias;

    [Tooltip("A bias for how far the quest should lead the player once compleated (use 0,0 for no bias)")]
    [Foldout("Distance Bias")] public float distanceBias;

    public bool isCompleat;


    [Foldout("Events")]public UnityEvent OnEquip = new UnityEvent();
    [Foldout("Events")]public UnityEvent OnCompleation = new UnityEvent();
    [Foldout("Events")]public UnityEvent OnSpawn = new UnityEvent();

    [Foldout("Quest UI")] public string discription = "Quest";

    public bool TogleCompleat = true;
    public bool TogleCompleation = true;

    public bool isConditionMet()
    {
        return condition.isConditionMet();
    }

    //When the selected quest is compleated
    public void OnCompleat()
    {

        if(dirrectionBias != Vector2.zero) 
        { 
          CompassUI.intance.questDirectionGoal.x = dirrectionBias.x + Random.Range(-0.5f, .5f);
          CompassUI.intance.questDirectionGoal.y = dirrectionBias.y + Random.Range(-0.5f, .5f); 
        }
        else
        {
            CompassUI.intance.questDirectionGoal.x = Random.Range(-1, 1);
            CompassUI.intance.questDirectionGoal.y = dirrectionBias.y + Random.Range(-1, 1);
        }

        if(distanceBias != 0) 
        { 
          CompassUI.intance.questDistanceGoal = distanceBias + Random.Range( -(distanceBias / 4), distanceBias / 4);
        }
        else { CompassUI.intance.questDistanceGoal = Random.Range(300, 450); }
        
        isCompleat = true;
        TogleCompleat = false;
        
    }
}
public class BiomeQuestCondition : ScriptableObject
{
    public float compleatness = 0;
    public string discription = "QUEST";

    public bool isConditionMet()
    {
        return compleatness >= 1;
    }

    public virtual float UpdateCompleatness()
    {
        return 1;
    }
}
