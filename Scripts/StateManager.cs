using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StateManager : IActorManagerInterface
{

    public float HP;
    public float HPMax;
    public float MP;
    public float MPMax;
    public Image mp;
    public int SuperArmor = 5;//霸体值

    [Header("1st order state flags")]
    public bool isGround;
    public bool isJump;
    public bool isFall;
    public bool isRoll;
    public bool isJab;
    public bool isAttack;
    public bool isHit;
    public bool isDie;
    public bool isBlocked;
    public bool isDefense;
    public bool isCounterBack;//跟动画机相关
    public bool isCounterBackEnable;//跟动画事件相关，比counterback时间短

    [Header("2nd order state flags")]
    public bool isAllowDefense;
    public bool isImmortal;
    public bool isCounterBackSuccess;
    public bool isCounterBackFailure;

    private void Start()
    {
        HP = HPMax;
        MP = MPMax;
    }

    private void Update()
    {
        isGround = am.ac.CheckState("ground");
        isJump = am.ac.CheckState("jump");
        isFall = am.ac.CheckState("fall");
        isRoll = am.ac.CheckState("roll");
        isJab = am.ac.CheckState("jab");
        isAttack = am.ac.CheckStateTag("attack");
        isHit = am.ac.CheckState("hit");
        isDie = am.ac.CheckState("die");
        isBlocked = am.ac.CheckState("blocked");
        isCounterBack = am.ac.CheckState("counterBack");
        isCounterBackSuccess = isCounterBackEnable;
        isCounterBackFailure = isCounterBack && !isCounterBackEnable;
        
        isAllowDefense = isGround || isBlocked;
        isDefense = isAllowDefense && am.ac.CheckState("defense1h","defense");
        isImmortal = isRoll || isJab;
        
    }

    public void AddHP(float value)
    {
        HP += value;
        HP = Mathf.Clamp(HP, 0, HPMax);       
    }

    public void AddMP(float value)
    {
        MP += value;
        MP = Mathf.Clamp(MP, 0, MPMax);
        mp.fillAmount = MP / MPMax;
    }


}
