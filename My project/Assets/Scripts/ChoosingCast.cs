using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class ChoosingCast : MonoBehaviour
{
    public AudioClip[] songs; //Massive for contain songs
    public Button[] buttons; //Massive for contain buttons
    private AudioSource audioSource;
    private Dictionary<Button, AudioClip> buttonSongMap = new Dictionary<Button, AudioClip>();
    private List<AudioClip> availableSongs; //list of available songs
    private Player player;
    void Start()
    {
        if (songs.Length == 0 || buttons.Length == 0)
        {
            Debug.LogError("Songs or Buttons array is empty!");
            return;
        }
        audioSource = GetComponent<AudioSource>();
        availableSongs = new List<AudioClip>(songs);
        AssignRandomSongs();
        player = FindObjectOfType<Player>();
    }
    void Update()
    {
        RefreshingSongs();
    }
    void AssignRandomSongs()
    {
        for(int i = 0; i < buttons.Length; i++)
        {
            Button button = buttons[i];
            // Choose random song
            int randomIndex = Random.Range(0, availableSongs.Count);
            AudioClip randomSong = availableSongs[randomIndex];

            // Assing sont to button
            buttonSongMap[button] = randomSong;

            // Remove song from avaible
            availableSongs.RemoveAt(randomIndex);

            // Add EventTrigger to button
            EventTrigger trigger = button.gameObject.AddComponent<EventTrigger>();


            int songId = i + 1; // Призначення ID пісні (i + 1)
            button.onClick.AddListener(() => 
            {
                player.cast = songId;
                StopSong();
                HideAllButtons();
            });
            // Cursor touch button
            EventTrigger.Entry onPointerEnter = new EventTrigger.Entry
            {
                eventID = EventTriggerType.PointerEnter
            };
            onPointerEnter.callback.AddListener((data) => PlaySong(randomSong, randomIndex + 1));
            trigger.triggers.Add(onPointerEnter);

            // Cursor don't touch button
            EventTrigger.Entry onPointerExit = new EventTrigger.Entry
            {
                eventID = EventTriggerType.PointerExit
            };
            onPointerExit.callback.AddListener((data) => StopSong());
            trigger.triggers.Add(onPointerExit);
        }   
    }
    void PlaySong(AudioClip song, int songId)
    {
        if (audioSource.isPlaying) audioSource.Stop(); // Stop current song
        audioSource.clip = song; // Assing current song
        audioSource.Play(); // Play song
    }

    void StopSong()
    {
        if(audioSource.isPlaying)
        { 
            audioSource.Stop(); // stop song
        }            
    }
    void HideAllButtons()
    {
        foreach (var button in buttons)
        {
            button.gameObject.SetActive(false); // Приховуємо кнопку
        }
    }
    void RefreshingSongs(){
        if(Input.GetKeyDown(KeyCode.R)){
            foreach (var button in buttons)
            {
                button.gameObject.SetActive(true); // Приховуємо кнопку
            }
            availableSongs = new List<AudioClip>(songs); //return all songs into list
            AssignRandomSongs();
        }
    }
}
