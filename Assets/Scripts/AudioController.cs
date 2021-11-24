using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioController : MonoBehaviour
{
    Rigidbody rigidbody;
    AudioSource audioSource;
    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
    }

    // Stops the audio if its rigidbody stops moving
    void FixedUpdate()
    {
        if (rigidbody.velocity.magnitude < 0.1f)
        {
            if (audioSource && audioSource.isPlaying)
                audioSource.Stop();
        }
            
    }
}
