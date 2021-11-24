using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FootSteps : MonoBehaviour
{
    AudioSource audioSource;
    public AudioClip clipLeft;
    public AudioClip clipRight;
    int num = 0;
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    /// <summary>
    /// Connects the footstep sound to the walking animation of the player
    /// </summary>
   private void Step()
    {
        num++;
        num %= 2;
        if (num == 0)
            audioSource.PlayOneShot(clipLeft);
        else
            audioSource.PlayOneShot(clipRight);
    }
}
