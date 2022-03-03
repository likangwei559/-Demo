using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelMessage : MonoBehaviour
{
    public Text message;
    private NextLevel nl;

    private void Start()
    {
        nl = GetComponentInParent<NextLevel>();
    }

    private void OnTriggerStay(Collider other)
    {
        if(other.tag == "Player"&& !nl.next)
        {
            message.text = "前面的区域，以后再来探索吧";
        }     
    }
    private void OnTriggerExit(Collider other)
    {
        message.text = "";
    }
}
