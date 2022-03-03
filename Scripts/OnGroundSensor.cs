using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnGroundSensor : MonoBehaviour
{
    public CapsuleCollider capco1;
    public float offset = 0.1f;

    private Vector3 point1;//胶囊上下球体的球心,经过offse向下偏移一点.
    private Vector3 point2;
    private float radius;

    // Start is called before the first frame update
    void Awake()
    {
        radius = capco1.radius - 0.05f;

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        point1 = transform.position + transform.up * (radius - offset);
        point2 = transform.position + transform.up * (capco1.height - offset) - transform.up * radius;
        

        Collider[] outputCols = Physics.OverlapCapsule(point1, point2, radius, LayerMask.GetMask("Ground"));
        if(outputCols.Length != 0)
        {
            //foreach (var col in outputCols)
            //{
            //    print("collision" + col.name);
            //}
            SendMessageUpwards("IsGround");
        }
        else
        {
            SendMessageUpwards("IsNotGround");
        }
    }
}
