using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerSFX : MonoBehaviour
{
    public AudioSource playSound;

    void OnTriggerEnter(Collider other)
    {
        playSound.PlayOneShot(playSound.clip);
    }

    void OnTriggerExit(Collider other)
    {
        playSound.Stop();
    }
}