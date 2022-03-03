using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : PlayerInput
{
    public GameObject player;
    public GameObject model;

    public float patrolSpeed; 
    public float patrolWaitTime;
    

    public float rotateSpeed;
    //public float moveSpeed;
    public Vector3 dir;

    public float warningRange = 15.0f;
    public float foundRange = 8.0f;
    public float attackRange = 2.0f;

    private NavMeshAgent agent;
    public float patrolTimer = 0f;//巡逻到当前目标点已等待时间    

    private float tarDistance;
    private bool inWarningRange = false;
    //private bool inAttackRange = false;
    private Animator anim;
       
    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        tarDistance = Vector3.Magnitude(player.transform.position - gameObject.transform.position);
        anim = model.GetComponent<Animator>();
    }

    IEnumerator attackAI()
    {
          if (isEnterRange(tarDistance, warningRange))
        {
            this.transform.rotation = Quaternion.Slerp(this.transform.rotation,
                                                    Quaternion.LookRotation(player.transform.position - this.transform.position),
                                                    rotateSpeed * Time.deltaTime);
            yield return new WaitForSeconds(2f);
            Dmag = 1.0f;
            anim.SetFloat("forward", 1);
        }  
       
        if (isEnterRange(tarDistance, foundRange))//bug:跑到某些地方会陷进去，原地跑
        {
            agent.isStopped = false;
            agent.SetDestination(player.transform.position);
            Dmag = 2.0f;
            anim.SetFloat("forward",2);
            attack = false; 
        }
         if(isEnterRange(tarDistance, attackRange))
        {           
            Dmag = 0f;
            anim.SetFloat("forward", 0.5f);
            attack = true;
        }              
        yield return null;                      
    }

    void patrolAI()
    {
       
        anim.SetFloat("forward",1);
        agent.isStopped = false;
        agent.speed = patrolSpeed;

        if (!agent.SetDestination(dir))
        {
            dir =  GetRanPos();
            patrolWaitTime = Random.Range(3f, 10f);
            
        }
        else
        {
            agent.destination = dir;
            patrolTimer += Time.deltaTime;//dir有可能会出现在NevMesh之外，导致在导航边界一直走，暂时把patrolTimer也用于路上的时间
        }
                             
        if(agent.remainingDistance < agent.stoppingDistance || patrolTimer > patrolWaitTime)
        {
            dir = GetRanPos();                              
            patrolTimer = 0;           
        }      
    }
    
    public Vector3 GetRanPos()
    {
        Vector3 _dir = new Vector3(0,0,0);
        _dir.x = transform.position.x * Random.Range(-1f, 1f) * 1.5f;
        _dir.y = transform.position.y;
        _dir.z = transform.position.z * Random.Range(-1f, 1f) * 1.5f;
        
            return _dir;
    }


    bool isEnterRange(float _tarDistance, float _range)
    {
        if ( _tarDistance < _range)
            return true;
        else
            return false;
    }

    // Update is called once per frame
    private void Update()
    {
        if (player != null)
        {
            tarDistance = Vector3.Magnitude(player.transform.position - gameObject.transform.position);

            inWarningRange = isEnterRange(tarDistance, warningRange);//警戒范围
                                                                     //print(inWarningRange);
            if (inWarningRange)
            {
                agent.isStopped = true;
                StartCoroutine(attackAI());//进入搜寻、战斗           
            }
            else
            {
                StopCoroutine(attackAI());
                patrolAI();//进入巡逻
                attack = false;
            }
        }
        else
        {
            StopCoroutine(attackAI());
            patrolAI();//进入巡逻
            attack = false;
        }    

            Dmag = Mathf.Sqrt((Dup * Dup) + (Dright * Dright));
            Dvec = Dright * transform.right + Dup * transform.forward;
        
        
    }
}
