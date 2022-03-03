using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterGetHit : MonoBehaviour
{
    private EnemyBlood blood;
    private GameObject parent;
    private Animator anim;
    // Start is called before the first frame update
    void Start()
    {
        blood = GetComponentInParent<EnemyBlood>();
        anim = GetComponentInParent<Animator>();        
        parent = transform.parent.gameObject;     
    }
    private void OnTriggerEnter(Collider col)
    {
        
        if (col.tag == "Weapon")//Monster的扣血在EnemyBlood进行
        {
            blood.HP -= 10;
            blood.HP = Mathf.Clamp(blood.HP, 0, blood.HPMax);
            if (blood.HP > 0)
            {
                anim.SetTrigger("hit"); 
                
            }
            else
            {
                anim.SetTrigger("die");
            }
        }
    }

}
