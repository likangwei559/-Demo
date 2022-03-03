using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : IActorManagerInterface
{
    private AudioSource attackSound;
    private Collider weaponColL;
    private Collider weaponColR;

    public GameObject whL;
    public GameObject whR;

    public WeaponController wcL;
    public WeaponController wcR;

    private void Start()
    {
        whL = transform.DeepFind("weaponHandleL").gameObject;
        whR = transform.DeepFind("weaponHandleR").gameObject;

        wcL = BindWeaponController(whL);
        wcR = BindWeaponController(whR);

        weaponColL = whL.GetComponentInChildren<Collider>();
        weaponColR = whR.GetComponentInChildren<Collider>();
        attackSound = GetComponent<AudioSource>();
    }

    public WeaponController BindWeaponController(GameObject targetObj)//在weaponHandle上添加weaponController
    {
        WeaponController tempWc;
        tempWc = targetObj.GetComponent<WeaponController>();
        if(tempWc == null)
        {
            tempWc = targetObj.AddComponent<WeaponController>();
        }
        tempWc.wm = this;
        return tempWc;
    }

    public void WeaponEnable()
    {
        weaponColR.enabled = true;
    }

    public void WeaponDisable()
    {
        weaponColR.enabled = false;
    }

    public void CounterBackEnable()
    {
        am.SetIsCounterBack(true);
    }

    public void CounterBackDisable()
    {
        am.SetIsCounterBack(false);
    }

    public void die()
    {
        Destroy(gameObject.transform.parent.gameObject);//玩家死亡bug
    }

    public void sound()
    {
        if (attackSound != null)
        {
             attackSound.Play();
        }     
    }
}
