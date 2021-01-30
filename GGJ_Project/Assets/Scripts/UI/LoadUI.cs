using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadUI : MonoBehaviour
{
    private static bool hasLoadedUIScene = false;

    // Start is called before the first frame update
    void Start()
    {
        if (!hasLoadedUIScene)
        {
            SceneManager.LoadScene("GameUI", LoadSceneMode.Additive);
            hasLoadedUIScene = true;
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
