using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class PlayerMovement : MonoBehaviour
{
    CharacterController character;
    private AudioSource audioSource;
    public float speed = 12f;
    void Start()
    {
        character = GetComponent<CharacterController>();
         audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        
    if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D))
    {
        if (!audioSource.isPlaying)
        {
            audioSource.Play();
        }
    }
    else
    {
        if (audioSource.isPlaying)
        {
            audioSource.Stop();
        }
    }
        Vector3 move = transform.right * x + transform.forward * z;

        character.Move(move * speed * Time.deltaTime);
    }

    
}
