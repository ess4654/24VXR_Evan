using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioPlaybackManager : MonoBehaviour
{
    public AudioSource audioPlayer;

    public void PlayAudio()
    {
        audioPlayer.Play();
    }

    public void StopAudio()
    {
        audioPlayer.Stop();
    }
}