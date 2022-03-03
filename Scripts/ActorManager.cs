using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ActorManager : MonoBehaviour
{
    public ActorController ac;

    [Header("Auto Generate if null")]
    public BattleManager bm;
    public WeaponManager wm;
    public StateManager sm;

    // Start is called before the first frame update
    void Awake()
    {
        ac = GetComponent<ActorController>();
        GameObject model = ac.model;
        GameObject sensor = transform.Find("sensor").gameObject;

        bm = Bind<BattleManager>(sensor);
        wm = Bind<WeaponManager>(model);
        sm = Bind<StateManager>(gameObject);
   
    }

    private T Bind<T>(GameObject go) where T : IActorManagerInterface//T只能是IActorManagerInterface及其子类
    {
        T tempInstance;
        tempInstance = go.GetComponent<T>();
        if(tempInstance == null)
        {
            tempInstance = go.AddComponent<T>();
        }
        tempInstance.am = this;
        return tempInstance;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetIsCounterBack(bool value)
    {
        sm.isCounterBackEnable = value;
    }
    

    public void TryDoDamage(WeaponController targetWc)
    {
        if (sm.isCounterBackSuccess)
        {
            targetWc.wm.am.Stunned();
        }
        else if (sm.isCounterBackFailure)
        {
            HitOrDie(false);
        }
        else if (sm.isImmortal)
        {
            //do nothing
        }
        else if (sm.isDefense)
        {
            //attack should be blocked
            Blocked();
        }
        else
        {
            sm.SuperArmor-=2;
            if (sm.SuperArmor <= 0)
            {
                HitOrDie(true);
            }
            else
            {
                HitOrDie(false);
            }
        }
        
        
    }

    public void TryDoDamage()//对于Monster类，不能盾反
    {

        if (sm.isImmortal)
        {
            //do nothing
        }
        else if (sm.isDefense)
        {
            //attack should be blocked
            Blocked();
        }
        else
        {
            sm.SuperArmor--;
            if (sm.SuperArmor <= 0)
            {
                HitOrDie(true);
            }
            else
            {
                HitOrDie(false);
            }

        }
    }

    public void HitOrDie(bool doHitAnimation,int _hurt = -5)
    {
        if (sm.HP <= 0)
        {
            //already dead
        }
        else
        {
            sm.AddHP(_hurt);
            if (sm.HP > 0)
            {
                if (doHitAnimation)//是否硬直
                {
                    Hit();
                    sm.SuperArmor = 5;//霸体值初始为5
                }
                //do some VFX,like splatter blood
            }
            else
            {
                Die();
            }
        }
    }

    public void Stunned()
    {
        ac.IssueTrigger("stunned");
    }

    public void Blocked()
    {
        ac.IssueTrigger("blocked");
    }

    public void Hit()
    {
        ac.IssueTrigger("hit");
    }

    public void Die()
    {
        ac.IssueTrigger("die");
        ac.pi.inputEnable = false;//高耦合
        if(ac.cameraCon.lockState == true)
        {
            ac.cameraCon.LockUnlock();
        }
        ac.cameraCon.enabled = false;//死亡时，摄像头也不许动
        if(gameObject.tag =="Player")
        {
            SceneManager.LoadScene(2);
        }
        
    }
}
