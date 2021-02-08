using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ClockStone;
public class MusicController : MonoBehaviourSingleton<MusicController>
{
    public List<string> plantSongs;
    public List<string> plantName;
    public Dictionary<string, AudioObject> plantSongObjects = new Dictionary<string, AudioObject>();

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
        if (!plantSongObjects.ContainsKey(plant))
        {
            Debug.Log(string.Format("<color=red>****** MUSIC LIBRARY DOES NOT CONTAIN SOUNDS FOR PLANT {0} </color>", plant));
        }
        else
        {
            plantSongObjects[plant].volume = 0.28f;
        }
    }

    public void PauseSong(string plant)
    {
        plantSongObjects[plant].volume = 0f;
    }
}
