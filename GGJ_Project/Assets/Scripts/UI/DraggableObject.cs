using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DraggableObject : MonoBehaviour, IPointerDownHandler
{
    bool dragging = false;

    void Update()
    {
        DetectStopDragging();
    }

    void DetectStopDragging()
    {
        // Touch phase ended or mouse up
        if (
            dragging &&
            ((Input.touchCount == 1 && Input.GetTouch(0).phase == TouchPhase.Ended) ||
            Input.GetMouseButtonUp(0))
        )
        {
            StopDragging();
        }
    }

    void StartDragging()
    {
        Debug.Log("Start");
        dragging = true;
    }

    void StopDragging()
    {
        Debug.Log("Stop");
        dragging = false;
        Raycast();
    }

    void Raycast()
    {
        Vector2 worldPoint;
        if (Input.touchCount == 1)
        {
            //Get the mouse position on the screen and send a raycast into the game world from that position.
            worldPoint = Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position);
        }
        else
        {
            //Get the mouse position on the screen and send a raycast into the game world from that position.
            worldPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        }


        RaycastHit2D hit = Physics2D.Raycast(worldPoint, Vector2.zero);

        //If something was hit, the RaycastHit2D.collider will not be null.
        if (hit.collider != null)
        {
            Debug.Log(hit.collider.name);
        }
    }

    // Fires when touch start or mouse down happens
    public void OnPointerDown(PointerEventData eventData)
    {
        // Don't want to deal with multi-touch >_< lol
        if (Input.touchCount <= 1)
        {
            StartDragging();
        }
    }
}
