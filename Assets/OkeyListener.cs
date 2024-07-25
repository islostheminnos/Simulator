using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class OkeyListener : MonoBehaviour
{
    [SerializeField] AudioSource audioSource2;
    public NPCBehavior nPCBehavior;
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(nPCBehavior.isSitting == true && !audioSource2.isPlaying){
            audioSource2.Play();
        }
    }
}
