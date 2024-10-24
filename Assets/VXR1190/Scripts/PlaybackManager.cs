using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class PlaybackManager : MonoBehaviour
{
    public AudioSource audio1, audio2;
    public AudioClip myClip;


    public AudioMixerSnapshot Day;
    public AudioMixerSnapshot Night;

    public void TransitionDay()
    {
        //will take two seconeds to transition from one to the other
        Day.TransitionTo(2);

    }

    public void PlayOnce(AudioSource audioSource)
    {
        if (!audioSource.isPlaying)
        {
            audioSource.PlayOneShot(myClip);
        }
        Debug.Log("clip played");
    }

    public void TransitionNight()
    {
        Night.TransitionTo(2);
    }

}