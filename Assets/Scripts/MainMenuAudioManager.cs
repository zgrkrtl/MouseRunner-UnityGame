using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class MainMenuAudioManager : MonoBehaviour
{
    private AudioSource mainMenuMusicAudioSource;
    [SerializeField] private AudioClip buttonHighlightedSound;
    [SerializeField] private AudioClip buttonClickedSound;
    [SerializeField] private AudioSource audioSource;
    void Start()
    {
        mainMenuMusicAudioSource = GetComponent<AudioSource>();
        mainMenuMusicAudioSource.Play(); 
    }
    
    public void SetVolume(float volume)
    {
        if (mainMenuMusicAudioSource != null)
        {
            mainMenuMusicAudioSource.volume = volume;
        }
    }
   
    
    public void PlayHighlightSound()
    {
        if (buttonHighlightedSound != null && audioSource != null)
        {
            mainMenuMusicAudioSource.PlayOneShot(buttonHighlightedSound);
        }
    }
    
    public void PlayClickedSound()
    {
        if (buttonClickedSound != null && audioSource != null)
        {
            AudioSource.PlayClipAtPoint(buttonClickedSound, transform.position);
        }
    }
}
