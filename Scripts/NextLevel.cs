using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NextLevel : MonoBehaviour
{
    public GameObject[] monster;
    private BoxCollider wall;
    public bool next;
    private BoxCollider levelNotice;
    // Start is called before the first frame update
    void Start()
    {
        wall = GetComponent<BoxCollider>();
        levelNotice = GameObject.FindWithTag("Notice").GetComponent<BoxCollider>();
    }

    // Update is called once per frame
    void Update()
    {
        next = enterNext();//第一关条件；
        if (next)
        {
            wall.enabled = false;//空气墙开
            levelNotice.enabled = false;
        }
    }

    
    private bool enterNext()
    {
        int num = 0;
        for(int i = 0; i< monster.Length;i++)
        {
            if (monster[i] != null)//剩余怪物数量
                num++;
        }
        if(num == 0)
        {
            return true;
        }
        else
        {
            return false;
        }
        
    }
}
