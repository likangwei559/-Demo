using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class TransformHelpers 
{
   public static Transform DeepFind(this Transform parent,string targetname)
    {
        Transform tempTrans = null;

        foreach (Transform child in parent)
        {
            if(child.name == targetname)
            {
                return child;
            }
            else
            {
                tempTrans = DeepFind(child, targetname);
                if(tempTrans != null)
                {
                    return tempTrans;
                }
            }
        }
        return null;
    }
}
