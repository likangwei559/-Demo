using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CameraController : MonoBehaviour
{
    public PlayerInput pi;
    public float horizontalSpeed = 50.0f;
    public float verticalSpeed = 80.0f;
    public float cameraDampValue = 0.05f;
    public Image lockDot;
    public bool lockState;
    public bool isAI = false;

    private GameObject playerHandle;
    private GameObject cameraHandle;
    private float tempEulerx;
    private GameObject model;
    private GameObject camera;
    private Animator anim;

    private Vector3 camerDampVelocity;
    private Vector3 distance;
    [SerializeField]
    private LockTarget lockTarget;

    // Start is called before the first frame update
    void Start()
    {
        cameraHandle = transform.parent.gameObject;//只有tansform可以用来找父类，转换成gameObject
        playerHandle = cameraHandle.transform.parent.gameObject;
        tempEulerx = 20;
        model = playerHandle.GetComponent<ActorController>().model;
        anim = model.GetComponent<Animator>();

        if (!isAI)
        {
            camera = Camera.main.gameObject;
            lockDot.enabled = false;
            Cursor.lockState = CursorLockMode.Locked;//隐藏鼠标
        }

        lockState = false;  
               
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(lockTarget == null)
        {
            Vector3 tempModelEuler = model.transform.eulerAngles;

            playerHandle.transform.Rotate(Vector3.up, pi.Jright * horizontalSpeed * Time.fixedDeltaTime);

            tempEulerx -= pi.Jup * verticalSpeed * Time.fixedDeltaTime;
            tempEulerx = Mathf.Clamp(tempEulerx, -40, 30);

            cameraHandle.transform.localEulerAngles = new Vector3(tempEulerx, 0, 0);//转相机

            model.transform.eulerAngles = tempModelEuler;
        }
        else
        {
            Vector3 tempForward = lockTarget.obj.transform.position - model.transform.position;
            tempForward.y = 0;
            playerHandle.transform.forward = tempForward;
            cameraHandle.transform.LookAt(lockTarget.obj.transform);
            
        }

        if (!isAI)
        {
            if (pi.Jup != 0 || pi.Jright != 0)//相机追人，转动视角时补上distance
            {
                camera.transform.position = transform.position + distance;
            }
            else
            {
                camera.transform.position = Vector3.SmoothDamp(camera.transform.position, transform.position, ref camerDampVelocity, cameraDampValue);
                distance = camera.transform.position - transform.position;
            }
            camera.transform.LookAt(cameraHandle.transform);
        }
                     
    }

   void Update()
    {
        if(lockTarget != null)
        {
            if (!isAI)
            {
                if(lockTarget != null)//敌人死亡红点还在
                {
                    lockDot.rectTransform.position = Camera.main.WorldToScreenPoint(lockTarget.obj.transform.position + new Vector3(0, lockTarget.halfHeight, 0));
                    lockDot.enabled = true;
                }
                
            }
                                                    
            if (Vector3.Distance(model.transform.position, lockTarget.obj.transform.position) > 10.0f)//距离较远解除锁定
            {
                LockProcessA(null, false, false, isAI);
                anim.SetBool("isLock", false);
            }
        }
    }

    private void LockProcessA(LockTarget _lockTarget, bool _lockEnable, bool _lockState, bool _isAI)
    {
        lockTarget = _lockTarget;
        if (!_isAI)//玩家才能显示锁定红点
        {
            lockDot.enabled = _lockEnable;
        }
        lockState = _lockState;
    }

    public void LockUnlock()
    {
        
            Vector3 modelOrigin1 = model.transform.position;
            Vector3 modelOrigin2 = modelOrigin1+ new Vector3(0, 1, 0);
            Vector3 boxCenter = modelOrigin2 + model.transform.forward * 5.0f;
            Collider[] cols = Physics.OverlapBox(boxCenter, new Vector3(0.5f, 0.5f, 5f),model.transform.rotation,LayerMask.GetMask(isAI?"Player":"Enemy"));
            
            if(cols.Length == 0)
            {
            LockProcessA(null, false, false,isAI);
            anim.SetBool("isLock", false);
            }
            else
            {
                foreach (var col in cols)
                {
                    //print(col.name);
                    if(lockTarget != null &&lockTarget.obj == col.gameObject)//对相同目标再次锁定视为解锁
                    {
                    LockProcessA(null, false, false, isAI);
                    anim.SetBool("isLock", false);
                    break;
                     }
                
                anim.SetBool("isLock", true);
                LockProcessA(new LockTarget(col.gameObject, col.bounds.extents.y), false, true, isAI);//现在显示会看到红点位移,等update里更改完坐标再显示
                break;
                }
            }
                                       
    }

    private class LockTarget
    {
        public GameObject obj;
        public float halfHeight;

        public LockTarget(GameObject _obj,float _halfHeight)
        {
            obj = _obj;
            halfHeight = _halfHeight;
        }
    }
}
