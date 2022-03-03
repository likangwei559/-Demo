using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CapsuleCollider))]
public class BattleManager : IActorManagerInterface
{
    private CapsuleCollider defCol;

    private void Start()
    {
        defCol = GetComponent<CapsuleCollider>();
        defCol.center = Vector3.up * 1.0f;
        defCol.height = 2.0f;
        defCol.radius = 0.3f;
        defCol.isTrigger = true;
    }

    private void OnTriggerEnter(Collider col)
    {
        WeaponController targetWc = col.GetComponentInParent<WeaponController>();//取得攻击者的weaponController

        if(col.tag == "Weapon" )
        {
            am.TryDoDamage(targetWc);
        }
        //print(col.name);
        if(col.tag == "Monster")
        {
            am.TryDoDamage();
        }
        if(col.tag == "fireBall")
        {
            am.HitOrDie(true,-20);
        }
    }
}
