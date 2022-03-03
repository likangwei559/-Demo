using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeftArmAnimFix : MonoBehaviour
{
    public Vector3 a;
    private Animator anim;
    
     void Awake()
    {
        anim = GetComponent<Animator>();    
    }

     void OnAnimatorIK()
    {
        if(anim.GetBool("defense") == false)//未举盾时，因骨骼不合适，调整使idle状态更美观
        {
            Transform leftLowerArm = anim.GetBoneTransform(HumanBodyBones.LeftLowerArm);
            leftLowerArm.localEulerAngles += a;
            anim.SetBoneLocalRotation(HumanBodyBones.LeftLowerArm, Quaternion.Euler(leftLowerArm.localEulerAngles));//将left Lower arm骨骼旋转a度
        }
        
    }
}
