using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActorController : MonoBehaviour
{
    public GameObject model;
    public CameraController cameraCon;
    public PlayerInput pi;
    public float walkSpeed = 1.4f;//调整动画播放速度与行走不一致
    public float runMultiplier = 2.7f;
    public float jumpVelocity = 3.0f;
    public float rollVelocity = 1.0f;
    public float jabVelocity = 3.0f;

    [Header("Friction Setting")]
    public PhysicMaterial frictionOne;
    public PhysicMaterial frictionZero;

    
    private Animator anim;
    private Rigidbody rigid;
    private Vector3 planarVec;
    private Vector3 thrustVec;
    private bool canAttack;
    private bool lockPlanar = false;
    private bool trackDirection = false;
    private CapsuleCollider col;
    //private float lerpTarget;
    private Vector3 deltaPos;

    //private bool leftIsShield = true;

    // Start is called before the first frame update
    void Awake()
    {
        pi = GetComponent<PlayerInput>();
        anim = model.GetComponent<Animator>();
        rigid = GetComponent<Rigidbody>();
        col = GetComponent<CapsuleCollider>();
        
    }

    // Update is called once per frame
    void Update()
    {
        if (pi.lockon)
        {
            cameraCon.LockUnlock();
        }

        if(cameraCon.lockState == false)
        {
            anim.SetFloat("forward", pi.Dmag * Mathf.Lerp(anim.GetFloat("forward"), ((pi.run) ? 2.0f : 1.0f), 0.5f));//线性插值，使混合树动画切换转变平滑
            anim.SetFloat("right", 0);
        }
        else
        {
            Vector3 localDvec = transform.InverseTransformDirection(pi.Dvec);//将世界坐标转成锁定视角后（2D混合树）的方向
            anim.SetFloat("forward", localDvec.z * ((pi.run) ? 2.0f : 1.0f));
            anim.SetFloat("right", localDvec.x * ((pi.run) ? 2.0f : 1.0f));
        }
        anim.SetBool("defense", pi.defense);
     
        if (pi.jump)
        {
            if ( anim.GetFloat("forward") < 1.1  || anim.GetFloat("right") != 0)
            {
                anim.SetTrigger("roll");
            }
            anim.SetTrigger("jump");
            canAttack = false;
        }

        if (rigid.velocity.magnitude > 8f)//高空下落
        {
            anim.SetTrigger("roll");
        }

        if (pi.attack &&( CheckState("ground") || CheckStateTag("attack")) && canAttack)
        {
            anim.SetTrigger("attack");
            
        }

        if(pi.run && ( pi.attack || pi.defense) && canAttack)
        {
            if (pi.attack)
            {
                //right heavy attack
            }
            else
            {
                anim.SetTrigger("counterBack");
            }
        }

        if (cameraCon.lockState == false)
        {
            if (pi.Dmag > 0.1f)
            {
                model.transform.forward = Vector3.Slerp(model.transform.forward, pi.Dvec, 0.3f);//球形插值，使转向动画平滑
            }

            if (lockPlanar == false)//防止跳跃时inputEnable为false，planarVec减到0原地跳
            {
                planarVec = pi.Dmag * model.transform.forward * walkSpeed * ((pi.run) ? runMultiplier : 1.0f);
            }
        }
        else
        {
            if(trackDirection == true )//进入roll状态设为true,原地roll，方向混乱
            {                
                    model.transform.forward = planarVec.normalized;                 
            }
            else
            {
                model.transform.forward = transform.forward;
            }
            if(lockPlanar == false)
            {
                planarVec = pi.Dvec * walkSpeed * ((pi.run) ? runMultiplier : 1.0f);

            }
        }

    }

    private void FixedUpdate()
    {
        rigid.position += deltaPos;
        rigid.velocity = new Vector3(planarVec.x,rigid.velocity.y,planarVec.z) + thrustVec;
        thrustVec = Vector3.zero;
        deltaPos = Vector3.zero;
    }

    public bool CheckState(string stateName, string layerName = "Base Layer")//是否在某层的某状态
    {
        int layIndex = anim.GetLayerIndex(layerName);//查找某图层的层级
        bool result = anim.GetCurrentAnimatorStateInfo(layIndex).IsName(stateName);
        return result;
    }

    public bool CheckStateTag(string tagName, string layerName = "Base Layer")
    {
        int layIndex = anim.GetLayerIndex(layerName);
        bool result = anim.GetCurrentAnimatorStateInfo(layIndex).IsTag(tagName);
        return result;
    }


    /// <summary>
    /// Message processing block
    /// </summary>
    public void OnJumpEnter()
    { 
        pi.inputEnable = false;
        lockPlanar = true;
        thrustVec = new Vector3(0, jumpVelocity, 0);
      
    }

    //public void OnJumpExit()
    //{      
    //    pi.inputEnable = true;
    //    lockPlanar = false;
    //    print("on jump exit");
    //}

    public void IsGround()
    {
        
        anim.SetBool("isGround", true);
    }
    
    public void IsNotGround()
    {
       
        anim.SetBool("isGround", false);
    }

    public void OnGroundEnter()
    {
        pi.inputEnable = true;
        lockPlanar = false;
        canAttack = true;
        col.material = frictionOne;
        trackDirection = false;
    }

    public void OnGroundExit()
    {
        col.material = frictionZero;
    }

    public void OnFallEnter()
    {
        pi.inputEnable = false;
        lockPlanar = true;
    }

    public void OnRollEnter()
    {
        thrustVec = new Vector3(0, rollVelocity, 0);
        pi.inputEnable = false;
        lockPlanar = true;
        trackDirection = true;
    }

    public void OnJabEnter()
    {
        pi.inputEnable = false;
        lockPlanar = true;
        
    }

    public void OnJabUpdate()
    {
        thrustVec = model.transform.forward * anim.GetFloat("jabVelocity");
    }

    public void OnAttack1hAEnter()
    {
        pi.inputEnable = false;
       
    }
    public void OnAttack1hAUpdate()
    {
        thrustVec = model.transform.forward * anim.GetFloat("attack1hAVelocity");
    
    }

    public void OnHitEnter()
    {
        pi.inputEnable = false;
        planarVec = Vector3.zero;
    }

    public void OnDieEnter()
    {
        pi.inputEnable = false;
        planarVec = Vector3.zero;
    }

    public void OnBlockedEnter()
    {
        pi.inputEnable = false;
    }

    public void OnStunnedEnter()
    {
        pi.inputEnable = false;
        planarVec = Vector3.zero;
    }

    public void OnCounterBack()
    {
        pi.inputEnable = false;
        planarVec = Vector3.zero;
    }

    public void OnUpdateRM(object _deltaPos)//用到模型的root motion使第三段攻击移动
    {
        if (CheckState("attack1hC"))
        {
            deltaPos += (Vector3)_deltaPos;
        }
    }

    public void IssueTrigger(string triggerName)
    {
        anim.SetTrigger(triggerName);
    }
}
