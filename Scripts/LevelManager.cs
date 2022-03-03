using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public Transform level1;
    public Transform level2;
    public Transform level3;
    public GameObject player;

    //private GameObject player;
    private int level;
    private Transform healPoint;
    private NextLevel nextlevel1;
    private NextLevel nextlevel2;
    // Start is called before the first frame update
    private void Awake()
    {
        nextlevel1 = transform.Find("level1").GetComponent<NextLevel>();
        nextlevel2 = transform.Find("level2").GetComponent<NextLevel>();
        if (nextlevel2.next)
        {
            healPoint = level3;
        }
        else if (nextlevel1.next)
        {
            healPoint = level2;
        }
        else
        {
            healPoint = level1;
        }
        player.transform.position = healPoint.position;
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
