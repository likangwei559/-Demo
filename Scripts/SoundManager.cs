using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public AudioSource level1;
    public AudioSource level2;
    public NextLevel nl;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (nl.next)
        {
            level1.enabled = false;
            level2.enabled = true;
        }
        else
        {
            level1.enabled = true;
            level2.enabled = false;
        }
    }
}
