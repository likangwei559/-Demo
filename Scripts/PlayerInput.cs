using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    [Header("Key settings")]
    public string keyUp = "w";
    public string keyDown = "s";
    public string keyLeft = "a";
    public string keyRight = "d";

    public string keyA = "left shift";
    public string keyB = "space";
    public string keyC = "mouse 0";
    public string keyD = "mouse 1";
    public string keyE = "mouse 2";

    public MyButton buttonA = new MyButton();
    public MyButton buttonB = new MyButton();
    public MyButton buttonC = new MyButton();
    public MyButton buttonD = new MyButton();
    public MyButton buttonE = new MyButton();

    public string keyJRight;
    public string KeyJLeft;
    public string keyJUp;
    public string keyJDown;


    [Header("Mouse settings")]
    public float mouseSensitivityX = 1.0f;
    public float mouseSensitivityY = 1.0f;



    [Header("Output signals")]
    public float Dup;
    public float Dright;
    public float Dmag;
    public Vector3 Dvec;
    public float Jup;
    public float Jright;

    //1.pressing signal
    public bool run;
    public bool defense;
    //2.trigger once sinal
    public bool jump;
    public bool attack;
    public bool lockon;
    //3.double trigger
    

    [Header("Others")]

    public bool inputEnable = true;

    private float targetDup;
    private float targetDright;
    private float velocityDup;
    private float velocityDright;

    private StateManager sm;
    private float time = 2f;
    private float duration = 0;

    // Start is called before the first frame update
    void Start()
    {
        sm = GetComponent<StateManager>();
    }

    // Update is called once per frame
    void Update()
    {
        //Jup = (Input.GetKey(keyJUp) ? 1.0f : 0) - (Input.GetKey(keyJDown) ? 1.0f : 0);方向键控制视角
        //Jright = (Input.GetKey(keyJRight) ? 1.0f : 0) - (Input.GetKey(KeyJLeft) ? 1.0f : 0);
        buttonA.Tick(Input.GetKey(keyA));
        buttonB.Tick(Input.GetKey(keyB));
        buttonC.Tick(Input.GetKey(keyC));
        buttonD.Tick(Input.GetKey(keyD));
        buttonE.Tick(Input.GetKey(keyE)); 
        
        

        Jup = Input.GetAxis("Mouse Y") * mouseSensitivityY;
        Jright = Input.GetAxis("Mouse X") * mouseSensitivityX;

        targetDup = (Input.GetKey(keyUp) ? 1.0f : 0) - (Input.GetKey(keyDown) ? 1.0f : 0);
        targetDright = (Input.GetKey(keyRight) ? 1.0f : 0) - (Input.GetKey(keyLeft) ? 1.0f : 0);

        if(inputEnable == false)
        {
            targetDup = 0;
            targetDright = 0;
        }

        Dup = Mathf.SmoothDamp(Dup, targetDup, ref velocityDup, 0.1f);
        Dright = Mathf.SmoothDamp(Dright, targetDright, ref velocityDright, 0.1f);

        Vector2 tempDAxis = SquareToCircle(new  Vector2(Dright, Dup));
        float Dright2 = tempDAxis.x;
        float Dup2 = tempDAxis.y;


        Dmag = Mathf.Sqrt((Dup2 * Dup2) + (Dright2 * Dright2));
        Dvec = Dright2 * transform.right + Dup2 * transform.forward;

        if (sm.MP > 10)
        {
           
            jump = buttonB.OnPressed;
            attack = buttonC.OnPressed;
            if  (jump || attack)
            {
                sm.AddMP(-10);
            }   
            
        }
        else
        {
            jump = false;
            attack = false;//解决bug:冲刺跳跃，跳跃后刚好没体力会一直跳
        }
        if (sm.MP > 1)
        {
            run = buttonA.IsPressing;
            if (run)
            {
                sm.AddMP(-0.1f);
            }
        }
        else
        {
            run = false;
        }

     
        defense = buttonD.IsPressing;//防御何时都可以按下，按下后不回复
        if (!defense && !run && !attack && !jump)
        {
            duration += Time.deltaTime;
            if (duration > time && sm.MP < sm.MPMax)
            {
                sm.AddMP(1);
            }

            if(sm.MP >= sm.MPMax)
            {
                duration = 0;
            }
        }
        lockon = buttonE.OnPressed;
    }

    private Vector2 SquareToCircle( Vector2 input)//坐标转换，解决斜着走速度更快
    {
        Vector2 output = Vector2.zero;

        output.x = input.x * Mathf.Sqrt(1 - (input.y * input.y) / 2.0f);
        output.y = input.y * Mathf.Sqrt(1 - (input.x * input.x) / 2.0f);

        return output;
    }
}
