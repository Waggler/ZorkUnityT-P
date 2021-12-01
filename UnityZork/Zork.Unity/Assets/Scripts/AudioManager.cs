using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private AudioSource openSource;
    [SerializeField] private AudioSource forestSource;

    [SerializeField] private AudioSource sfxSource;

    [SerializeField] private AudioClip forestMusicClip;
    [SerializeField] private AudioClip openMusicClip;

    [SerializeField] private AudioClip inputCommandClip;
    [SerializeField] private AudioClip pickupItemClip;
    [SerializeField] private AudioClip dropItemClip;

    //---------------------//
    public void PlayMusic(int musicValue)
    //---------------------//
    {
        if (musicValue == 0)
        {
            openSource.Play();
            forestSource.Stop();
        }
        else if (musicValue == 1)
        {
            openSource.Stop();
            forestSource.Play();
        }

    }//END PlayMusic

    //---------------------//
    public void PlayInput()
    //---------------------//
    {
        sfxSource.PlayOneShot(inputCommandClip);

    }//END PlayInput

    //---------------------//
    public void PlayPickup()
    //---------------------//
    {
        sfxSource.PlayOneShot(pickupItemClip);

    }//END PlayPickup

    //---------------------//
    public void PlayDrop()
    //---------------------//
    {
        sfxSource.PlayOneShot(dropItemClip);

    }//END PlayDrop

}//END AudioManager
