using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Music : MonoBehaviour
{

    AudioSource myAudio;

    [SerializeField]
    public AudioClip swipeSound, destroySound;

    public bool muted = false;

    void Start()
    {
        myAudio = GetComponent<AudioSource>();
    }

    public void MusicPlayStop()
    {
        if (myAudio.isPlaying)
        {
            muted = true;
            myAudio.Pause();
        }
        else
        {
            muted = false;
            myAudio.Play();
        }
    }

    public void SwipeSound()
    {
        if(!muted) myAudio.PlayOneShot(swipeSound);
    }

    public void DestroySound()
    {
        if (!muted) myAudio.PlayOneShot(destroySound);
    }
}
