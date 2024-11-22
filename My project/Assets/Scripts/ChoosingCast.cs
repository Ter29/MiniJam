using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChoosingCast : MonoBehaviour
{
    private AudioClip[] songs; //Massive for contain songs
    private AudioSource audioSource;
    private int currentSongIndex = 0;
    void Start()
    {
        audioSource = GetComponent<AudioSource>();

    }

    void Update()
    {
        
    }
}
