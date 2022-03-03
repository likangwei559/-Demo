using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterAnimatorEvents : MonoBehaviour
{
    private GameObject attackSensor;
    private Collider attackSensorCol;
    // Start is called before the first frame update
    void Start()
    {
        attackSensor = transform.Find("attackSensor").gameObject;
        attackSensorCol = attackSensor.GetComponent<Collider>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void attackEnable()
    {
        attackSensorCol.enabled = true;
    }

    public void attackDisable()
    {
        attackSensorCol.enabled = false;
    }

    public void die()
    {
        Destroy(gameObject);
    }
}
