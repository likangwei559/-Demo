using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class explosion : MonoBehaviour
{
    public GameObject boomPerfab;
    public GameObject boom;
    public AudioSource sound;
    private CapsuleCollider col;
    // Start is called before the first frame update
    void Start()
    {
        col = GetComponent<CapsuleCollider>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter(Collider col)
    {        
        
       if( col.tag == "ground" || col.tag =="Player")
        {
            if(boom == null)
            {
                print("boom");
                boom = Instantiate(boomPerfab) as GameObject;
                boom.transform.position = transform.position;
            }            
            sound.Play();
        }
        StartCoroutine(die());
    }
   
    IEnumerator die()
    {
        yield return new WaitForSeconds(4f);
        Destroy(gameObject);
        Destroy(boom);
    }

}
