using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Sirenix.OdinInspector;
using System.Linq;
using System.Reflection;

[RequireComponent(typeof(HealthSystem))]
public class EnemyBehaviour : MonoBehaviour
{
    /*A unity component that gives each enemy gameobject
     its functionality from thire state machine, other components like the health sytem,
    values from the EnemyType SO, ect.
     */

    [InlineEditor]
    public EnemyTypeSO type;

    [DisplayAsString]
    public List<IEnemyBehaviorNode> currentNodes = new List<IEnemyBehaviorNode>();
    public List<BehaviorNodeTransition> transitions = new List<BehaviorNodeTransition>();


    public List<IEnemyBehaviorNode> rootTransitions = new List<IEnemyBehaviorNode>();

    [LabelText("starting nodes")] public List<BehaviorNode<UnityEngine.Object>> inspectorRootTransitions = new List<BehaviorNode<UnityEngine.Object>>();


    private void Start()
    {
        foreach (IEnemyBehaviorNode node in rootTransitions)
        {
            currentNodes.Add(node);
            node.OnEnterBehavior();
        }
    }
    private void Update()
    {
        foreach (BehaviorNodeTransition transition in transitions)
        {
            transition.UpdateTransition();
        }

        foreach (IEnemyBehaviorNode node in currentNodes)
        {
            node.BehaviorUpdate();
        }

    }

    private void OnValidate()
    {
        SetTransitions();
        ValidateBehavior();
        SetStatistics();
    }

    public void SetStatistics()
    {
        //Setting Health:
        gameObject.GetComponent<HealthSystem>().maxHealth = type.maxHealth.value;


        //Setting Other Stats:
        var floatPrameters = new List<EnemyPamater<float>>();
        var intPrameters = new List<EnemyPamater<int>>();

        foreach (var node in GetAllNodes())
        {
            var nodeParameters = GetProperties<EnemyPamater<float>>(node);

            floatPrameters.AddRange(nodeParameters);
            
        }
        foreach (var node in GetAllNodes())
        {
            var nodeParameters = GetProperties<EnemyPamater<int>>(node);

            intPrameters.AddRange(nodeParameters);

        }

        foreach (var pram in floatPrameters)
        {
            var MatchingEnemyStats = type.EnemyStats.Where(i => i.tag == pram.tag);
            if(MatchingEnemyStats.Count() > 1 ) 
            { 
                Debug.LogError("Eneny float stat has multiple matching tags with:" + MatchingEnemyStats.ToString()); 
                return; 
            }
            if(MatchingEnemyStats.Count() == 0) 
            {
                Debug.LogWarning("No float stat tag matches with:" + pram.tag);
                return;
            }

            if(MatchingEnemyStats.Count() == 1)
            {
                pram.randomnessValue = MatchingEnemyStats.ElementAt(0).value;
            }

        } 
        foreach (var pram in intPrameters)
        {
            var MatchingEnemyStats = type.EnemyStats.Where(i => i.tag == pram.tag);
            if(MatchingEnemyStats.Count() > 1 ) 
            { 
                Debug.LogError("Eneny float stat has multiple matching tags with:" + MatchingEnemyStats.ToString()); 
                return; 
            }
            if(MatchingEnemyStats.Count() == 0) 
            {
                Debug.LogWarning("No float stat tag matches with:" + pram.tag);
                return;
            }

            if(MatchingEnemyStats.Count() == 1)
            {
                pram.randomnessValue = (RandomnessValue<int>)(object)MatchingEnemyStats.ElementAt(0).value;
            }
        }
    }

    public void ValidateBehavior()
    {
        for (int i = 0; i < inspectorRootTransitions.Count; i++)
        {
            BehaviorNode<UnityEngine.Object> behaviorNode = inspectorRootTransitions[i];


            if (inspectorRootTransitions[i].nodeObject is IEnemyBehaviorNode)
            {
                rootTransitions.Add(inspectorRootTransitions[i].nodeObject as IEnemyBehaviorNode);
            }
            else if (inspectorRootTransitions[i].nodeObject != null)
            {
                Debug.LogError("In Valid Input, Cannot Be: " + inspectorRootTransitions[i].nodeObject.GetType());
                inspectorRootTransitions.Remove(behaviorNode);
            }
        }

        for (int i = 0; i < transitions.Count; i++)
        {
            if (transitions[i].From is IEnemyBehaviorNode && transitions[i].To is IEnemyBehaviorNode)
            {
                transitions[i].interfaceFrom = (IEnemyBehaviorNode)transitions[i].From;
                transitions[i].interfaceTo = (IEnemyBehaviorNode)transitions[i].To;
                Debug.LogError("In Valid Input From or To, Cannot Be: " + transitions[i].From.GetType() + " And/Or " + transitions[i].To.GetType());
            }
            else
            {
                transitions[i].From = null;
                transitions[i].To = null;
            }
        }
    }
    public List<IEnemyBehaviorNode> GetAllNodes()
    {
        List<IEnemyBehaviorNode> nodes = new List<IEnemyBehaviorNode>();
        foreach (var transition in transitions)
        {
            nodes.Add(transition.interfaceFrom);
            nodes.Add(transition.interfaceTo);
        }
        foreach (var node in rootTransitions)
        {
            nodes.Add(node);
        }

        return nodes;
    }
    public List<T> GetProperties<T>(object obj)
    {
        List<PropertyInfo> properties = obj.GetType()
        .GetProperties()
        .Where(prop => prop.PropertyType == typeof(T))
        .ToList();

        var values = new List<T>();
        foreach (var prop in properties)
        {
            values.Add((T)prop.GetValue(obj));
        }

        return values;
    }
    public void SetTransitions()
    {
        foreach (var item in transitions)
        {
            item.objectBehaviour = this;
        }
    }
}

[Serializable]
public class BehaviorNode<T>
{
    public T nodeObject;
}

public interface IEnemyBehaviorNode
{
    public BehaviorExitReturn behaviorExitReturn { get; set; }
    public BehaviorExitReturn previousbehaviorExitReturns { get; set; }
    public bool exit { get; set; }
    public void OnEnterBehavior();
    public void BehaviorUpdate();

    public void OnEnemySpawn();

}

[Serializable]
public class BehaviorExitReturn
{
    public Dictionary<string, ReturnValue> Returns = new Dictionary<string, ReturnValue>();
    public struct ReturnValue
    {
        public static Type type;
        public object value;
    }
}

[Serializable]
public class EnemyPamater<T>
{
    public string tag;
    public RandomnessValue<T> randomnessValue;
}

[Serializable]
public class BehaviorNodeTransition
{
    public BehaviorNode<UnityEngine.Object> From;
    public BehaviorNode<UnityEngine.Object> To;
    public IEnemyBehaviorNode interfaceFrom;
    public IEnemyBehaviorNode interfaceTo;
    public EnemyBehaviour objectBehaviour;

    public void UpdateTransition()
    {
        if(interfaceFrom.exit == true)
        {
            objectBehaviour.currentNodes.Remove(interfaceFrom);
            objectBehaviour.currentNodes.Add(interfaceTo);
            interfaceTo.OnEnterBehavior();
            interfaceFrom.exit = false;
        }
    }
}

public struct Enemy
{
    public EnemyBehaviour enemyBehaviour;
    public EnemyTypeSO enemySO;
    public HealthSystem healthSystem;
    [AssetsOnly] public GameObject gameObject;

    public void SetupInfo()
    {
        //To Do
    }
}
