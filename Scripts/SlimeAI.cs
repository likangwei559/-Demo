using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SlimeAI : MonoBehaviour
{
    public GameObject player;

    public float patrolSpeed = 1;
    public float patrolWaitTime = 5;
    public float rotateSpeed = 1;
    //public float moveTime = 100f;
    public Vector3 dir;

    public float warningRange = 15.0f;
    public float foundRange = 8.0f;
    public float attackRange = 2.0f;
    public float patrolTimer = 0f;

    [SerializeField]
    private GameObject attackSensor;
    private NavMeshAgent agent;
    private Animator anim; 
    private float tarDistance;
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        tarDistance = Vector3.Magnitude(player.transform.position - gameObject.transform.position);
        attackSensor = GameObject.Find("attackSensor");
    }

    // Update is called once per frame

    IEnumerator attackAI()
    {
        anim.ResetTrigger("patrol");
        if (isEnterRange(tarDistance, warningRange))
        {
            anim.SetBool("warning", true);//进入警戒范围，转向玩家
            agent.isStopped = true;
            this.transform.rotation = Quaternion.Slerp(this.transform.rotation,
                                                        Quaternion.LookRotation(player.transform.position - this.transform.position),
                                                        rotateSpeed * Time.deltaTime);
            move(true);
            yield return new WaitForSeconds(1f);
        }
        if (isEnterRange(tarDistance, attackRange))
        {
            move(false);
            anim.SetBool("seePlayer", true);
            anim.SetTrigger("attack");
            yield return new WaitForSeconds(1f);
            anim.ResetTrigger("attack");
            yield return new WaitForSeconds(2f);
        }
         if (isEnterRange(tarDistance, foundRange))
        {
            anim.SetBool("seePlayer", true);
            agent.isStopped = false;
            agent.SetDestination(player.transform.position);

        }
        


        
    }

    void patrolAI()
    {
        anim.ResetTrigger("attack");
        anim.SetTrigger("patrol");
        agent.isStopped = false;
        agent.speed = patrolSpeed;
        //print("patrol");

        if (!agent.SetDestination(dir))
        {
            dir = GetRanPos();
            patrolWaitTime = Random.Range(3f, 10f);
        }
        else
        {
            agent.destination = dir;
            patrolTimer += Time.deltaTime;
        }

        if (agent.remainingDistance < agent.stoppingDistance || patrolTimer > patrolWaitTime)
        {
            dir = GetRanPos();
            patrolTimer = 0;
        }
    }

    // Update is called once per frame
    private void Update()
    {
        if(player!= null)
        {
            tarDistance = Vector3.Magnitude(player.transform.position - gameObject.transform.position);
            if (isEnterRange(tarDistance, warningRange))
            {
                StartCoroutine(attackAI());//进入搜寻、战斗           
            }
            else
            {
                StopCoroutine(attackAI());
                patrolAI();//进入巡逻              
            }
        }
        else
        {
            StopCoroutine(attackAI());
            patrolAI();//进入巡逻  
        }
        

    }

    public void move(bool _canMove)
    {
        if (_canMove)
        {
            gameObject.transform.Translate(Vector3.forward * Time.deltaTime);
        }
    }

    public Vector3 GetRanPos()
    {
        Vector3 _dir = new Vector3(0, 0, 0);
        _dir.x = transform.position.x * Random.Range(-1f, 1f) * 1.5f;
        _dir.y = transform.position.y;
        _dir.z = transform.position.z * Random.Range(-1f, 1f) * 1.5f;
        return _dir;
    }

    bool isEnterRange(float _tarDistance, float _range)
    {
        if (_tarDistance < _range)
            return true;
        else
            return false;
    }


}
