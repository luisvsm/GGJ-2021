using UnityEngine;
using UnityEngine.SceneManagement;

public class OpenSceneButton : MonoBehaviour
{
    [Tooltip("The name of the scene file exactly as it is in editor and remember if it is a new scene to add it in the build settinsg")]
    public string _sceneName = "GreenHouseGameScene";

    public void TriggerOpenScene()
    {
        SceneManager.LoadScene(_sceneName);
    }
}
