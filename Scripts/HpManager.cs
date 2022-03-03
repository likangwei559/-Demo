using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HpManager : MonoBehaviour
{
    public Image HPCur;//扣血前
    public Image HPTar;//扣血后
    public float hurtSpeed = 0.03f;

    private StateManager playerState;
    // Start is called before the first frame update
    void Awake()
    {
        playerState = GameObject.FindGameObjectWithTag("Player").GetComponent<StateManager>();
    }


    private void Update()//性能不好，可以用协程
    {
        HPTar.fillAmount = playerState.HP / playerState.HPMax;
        if (HPCur.fillAmount >= HPTar.fillAmount)
        {
            HPCur.fillAmount -= hurtSpeed;
            
        }
        else
        {
            HPCur.fillAmount = HPTar.fillAmount;//补血时
        }
    }

}
