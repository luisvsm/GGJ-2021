using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManageCredits : MonoBehaviour
{
    //dirty singleton :<
    public static ManageCredits Instance;
    public bool shown = false;
    float skipTime;
    public float timeToWaitUntilPlayerCanSkip = 1.5f;

    public void Show()
    {
        if (shown == false)
        {
            shown = true;
            skipTime = Time.time + timeToWaitUntilPlayerCanSkip;
            gameObject.SetActive(true);
        }
    }

    void Start()
    {
        Instance = this;
        gameObject.SetActive(false);
    }

    void OnMouseDown()
    {
        if (skipTime < Time.time)
        {
            gameObject.SetActive(false);
        }
    }
}
