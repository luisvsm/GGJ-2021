using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ClockStone;
public class MusicController : MonoBehaviourSingleton<MusicController>
{
    public List<string> plantSongs;
    public List<string> plantName;
    private Dictionary<string, AudioObject> plantSongObjects = new Dictionary<string, AudioObject>();

    void Start()
    {
        AudioController.Play("MUS_GameLoop_BackingTrack_Percussion");

        for (int i = 0; i < plantSongs.Count; i++)
        {
            AudioObject tempObj = AudioController.Play(plantSongs[i]);
            if (tempObj == null)
            {
                Debug.Log("Unable to load " + plantSongs[i]);
            }
            else
            {
                plantSongObjects.Add(plantName[i], AudioController.Play(plantSongs[i]));
                plantSongObjects[plantName[i]].volume = 0f;
            }
        }
    }

    public void PlaySong(string plant)
    {
        plantSongObjects[plant].volume = 1f;
    }

    public void PauseSong(string plant)
    {
        plantSongObjects[plant].volume = 0f;
    }
}
