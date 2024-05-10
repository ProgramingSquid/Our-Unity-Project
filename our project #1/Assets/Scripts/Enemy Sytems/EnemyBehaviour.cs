using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

public class EnemyBehaviour : MonoBehaviour
{
    /*A unity component that gives each enemy gameobject
     its functionality from thire state machine, other components like the health sytem,
    values from the EnemyType SO, ect.
     */

    [Expandable]
    public EnemySO type;


    
}
