using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;
using System;
using Sirenix.Serialization;
using Sirenix.OdinInspector;

public class EnemyBehaviour : SerializedMonoBehaviour
{
    /*A unity component that gives each enemy gameobject
     its functionality from thire state machine, other components like the health sytem,
    values from the EnemyType SO, ect.
     */

    [InlineEditor]
    public EnemySO type;

    [OdinSerialize]
    public List<IEnemyBehaviorNode> currentNodes;
    
    public List<BehaviorNodeTransition> transitions;

    [OdinSerialize]
    public List<IEnemyBehaviorNode> rootTransitions;

    private void Start()
    {
        foreach (IEnemyBehaviorNode node in rootTransitions)
        {
            currentNodes.Add(node);
        }
    }
    private void Update()
    {
        foreach (BehaviorNodeTransition transition in transitions)
        {
            transition.UpdateTransition();
        }
    }
}

public interface IEnemyBehaviorNode
{
    public BehaviorExitReturn behaviorExitReturn { get; set; }
    public BehaviorExitReturn previousbehaviorExitReturns { get; set; }
    public bool exit { get; set; }
    public void OnEnterBehavior();
    public void BehaviorUpdate();

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
    public T value;
}

[Serializable]
public class BehaviorNodeTransition
{
    public IEnemyBehaviorNode From;
    public IEnemyBehaviorNode To;
    public EnemyBehaviour objectBehaviour;

    public void UpdateTransition()
    {
        if(From.exit == true)
        {
            objectBehaviour.currentNodes.Remove(From);
            objectBehaviour.currentNodes.Add(To);
            From.exit = false;
        }
    }
}
