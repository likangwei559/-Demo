using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
public class DragonBabyAI : MonoBehaviour
{
    public GameObject fireBallPrefab;
    public GameObject player;
    public bool fireBallMove;
    public NextLevel level2;

    private GameObject fireBall;
    private StateManager sm;
    private Animator anim;
    private bool isGround;
    private CinemachineDollyCart dc;
    private Vector3 playerPos;
    private Vector3 playerForward;//玩家前面一段距离
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        dc = GetComponentInParent<CinemachineDollyCart>();
        sm = player.GetComponent<StateManager>();     
        StartCoroutine(AI());
           
    }

    // Update is called once per frame
    void Update()
    {
        if(player!= null)
        {
            playerPos = player.transform.position;
            playerForward = player.transform.position + player.transform.forward * 3;
            isGround = anim.GetBool("isGround");
            if (fireBallMove && fireBall != null )
            {
                fireBall.transform.Translate((player.transform.position - transform.position) * 0.5f * Time.deltaTime);
            }
        }             
    }

    IEnumerator AI()
    {
        while (sm.HP > 0 )
        {
            patrol();//空中巡逻
            yield return new WaitForSeconds(4f);
            attack();//空中攻击
            yield return new WaitForSeconds(2f);

            //下地
            //anim.SetBool("patrol", false);
            //dc.enabled = false;
            //transform.position = Vector3.Lerp(transform.position, playerForward, 0.5f);
            //print("flydown");
            //yield return new WaitForSeconds(10f);
            //朝玩家前方一段距离冲刺，然后run，attack，飞回轨道。

        }
        
    }

    void attack()
    {
        dc.enabled = false;
        this.transform.rotation = Quaternion.LookRotation(player.transform.position - this.transform.position);
                                                         
        anim.SetTrigger("attack");
        if (!isGround)
        {       if(fireBall == null && level2.next)
            {
                fireBall = Instantiate(fireBallPrefab) as GameObject;
                fireBall.transform.position = transform.position;
                fireBallMove = true;
                //喷火球
            }

        }
    }

    void patrol()
    {
        anim.SetBool("patrol", true);
        dc.enabled = true;
    }

    public void IsGround()
    {

        anim.SetBool("isGround", true);
    }

    public void IsNotGround()
    {

        anim.SetBool("isGround", false);
    }
}
