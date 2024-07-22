using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DoorEnter : MonoBehaviour
{
    public Transform door;

    void OnTriggerEnter(Collider other){
        if(other.gameObject.tag == "NPC"){
            door.Rotate(0,90,0);
        }
    }
}
