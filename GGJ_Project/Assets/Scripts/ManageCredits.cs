using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManageCredits : MonoBehaviourSingleton<ManageCredits>
{
    public bool shown = false;
    float skipTime;
    public float timeToWaitUntilPlayerCanSkip = 1.5f;

    void OnEnable()
    {
        shown = true;
        skipTime = Time.time + timeToWaitUntilPlayerCanSkip;
    }

    void OnMouseDown()
    {
        if (skipTime < Time.time)
        {
            gameObject.SetActive(false);
        }
    }
}
