using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeController : MonoBehaviourSingleton<FadeController>
{
    public delegate void voidCallback();
    voidCallback halfWayCallback;
    voidCallback completedCallback;

    // Hopefully this will cover all buttons etc when fade is active
    private Collider2D fadeCollider;

    private bool fadeToBlack = false;
    private bool fadeToOpaque = false;
    private float opacity = 0f;

    public float fadeSpeed = 1f;

    private SpriteRenderer sRenderer;

    // Start is called before the first frame update
    public void Fade(voidCallback halfWay, voidCallback completed)
    {
        fadeCollider.enabled = true;
        halfWayCallback = halfWay;
        completedCallback = completed;

        fadeToBlack = true;
        opacity = 0f;
    }

    public void HalfWayFade()
    {
        if (halfWayCallback != null)
        {
            halfWayCallback();
            halfWayCallback = null;
        }
    }

    public void FinishFade()
    {
        fadeCollider.enabled = false;

        if (completedCallback != null)
        {
            completedCallback();
            completedCallback = null;
        }
    }

    private Color colorTemp;
    public void SetOpacity(float opacity)
    {
        colorTemp = Color.black;
        colorTemp.a = opacity;
        sRenderer.color = colorTemp;
    }

    void Start()
    {
        fadeCollider = gameObject.GetComponent<Collider2D>();
        sRenderer = gameObject.GetComponent<SpriteRenderer>();
    }

    // Lazy fade lol
    void Update()
    {
        if (fadeToBlack)
        {
            //Debug.Log("fadeToBlack: " + opacity);
            opacity += Time.deltaTime * fadeSpeed;
            SetOpacity(opacity);
            if (opacity >= 1f)
            {
                opacity = 1f;
                fadeToBlack = false;
                fadeToOpaque = true;
                HalfWayFade();
            }
        }
        else if (fadeToOpaque)
        {
            //Debug.Log("fadeToOpaque: " + opacity);
            opacity -= Time.deltaTime * fadeSpeed;
            SetOpacity(opacity);
            if (opacity < 0f)
            {
                opacity = 0f;
                fadeToOpaque = false;
                FinishFade();
            }
        }
    }
}
