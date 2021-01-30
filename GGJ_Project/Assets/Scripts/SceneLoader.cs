using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public List<string> SceneList;
    // Start is called before the first frame update
    void Start()
    {
        LoadUI.hasLoadedUIScene = true; // Load.UI is a concienience dev script. Tell it not to do it's thing. hacky D:
        for (int i = 0; i < SceneList.Count; i++)
        {
            SceneManager.LoadScene(SceneList[i], LoadSceneMode.Additive);
        }
    }
}
