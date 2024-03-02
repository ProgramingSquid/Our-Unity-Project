using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "", menuName = "Upgrades/Weapon Abilities/")]
public class  : WeaponUpgrade
{
    
    protected override void OnFire()
    {
        weapon.Shoot();
        //Write the code you want to happen when the player shoots here
    }
    public override void GenerateNewSO()
    {   
            ScriptableObject NewSO = CreateInstance<>();
            AssetDatabase.CreateAsset(NewSO, "Assets/UpgradeData/" + name + ".asset");  
    }
}