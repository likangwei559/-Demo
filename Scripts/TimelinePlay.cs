using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;
using UnityEngine.UI;

public class TimelinePlay : MonoBehaviour
{
    public PlayableDirector director;
    public Text text;
    public GameObject dragon;
    public Animator anim;
    private bool hit;   
    

    void Update()
    {
        if (hit)
        {
            foreach (PlayableBinding bind in director.playableAsset.outputs)
            {
                if (bind.sourceObject.GetType() == typeof(PlayableTrack))
                {
                    PlayableTrack playableTrack = (PlayableTrack)bind.sourceObject;
                    foreach (var clip in playableTrack.GetClips())
                    {
                        object clipAsset = clip.asset;
                        if (clipAsset is PlayableAssetTest)
                        {
                            PlayableAssetTest test = clipAsset as PlayableAssetTest;
                            director.SetReferenceValue(test.m_DialogContainer.exposedName, text);
                        }
                    }
                }
            }
            director.Play();
        }
    }

    private void OnTriggerEnter(Collider col)
    {
        if (col.tag == "fireBall")
        {
            hit = true;
            anim.SetBool("fireBall", true);
            Destroy(dragon);
        }
    }
}
