using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DraggableObject : MonoBehaviour, IPointerDownHandler
{
    public string RaycastLayerName = "Default";
    bool dragging = false;
    public Sprite draggableSprite;
    public GameObject draggableObjectDisplay;
    private GameObject draggableObjectDisplayInstance;


    public static DraggableObject currentDraggableObject;

    private DragConsumer dragConsumer;
    private DragConsumer previousDragConsumer;
    private LayerMask mask;

    void Start()
    {
        draggableObjectDisplayInstance = Instantiate(draggableObjectDisplay, Vector3.zero, Quaternion.identity);
        draggableObjectDisplayInstance.transform.Find("display").GetComponent<SpriteRenderer>().sprite = draggableSprite;
        draggableObjectDisplayInstance.SetActive(false);
    }

    void Update()
    {
        if (dragging)
        {
            dragConsumer = RaycastForGameObject();
            UpdateDragPosition();
            SendDragInAndOutEvents();
            DetectStopDragging();
        }
    }

    void SendDragInAndOutEvents()
    {

        if (
            previousDragConsumer != null && // If the raycast hit something in the previous frame
            dragConsumer != previousDragConsumer // And if the /something/ is different from the last frame
        )
        {
            // The drag consumer has changed, tell the drag controller
            // from the previous frame that a drag out event happened
            previousDragConsumer.OnDragOut.Invoke();
        }

        if (
            dragConsumer != null && // If the raycast hit something this frame
            dragConsumer != previousDragConsumer // And if the /something/ is different from the last frame
        )
        {
            // The drag consumer has changed, tell the drag controller that a drag out event happened
            dragConsumer.OnDragIn.Invoke();
        }

        previousDragConsumer = dragConsumer;
    }

    void SendDragLetGoEvents()
    {
        if (dragConsumer != null)
        {
            dragConsumer.OnDragLetGo.Invoke();
        }
    }

    void UpdateDragPosition()
    {
        draggableObjectDisplayInstance.transform.position = GetWorldPositionFromInput();
    }

    void DetectStopDragging()
    {
        // Touch phase ended or mouse up
        if (
            (Input.touchCount == 1 && Input.GetTouch(0).phase == TouchPhase.Ended) ||
            Input.GetMouseButtonUp(0)
        )
        {
            StopDragging();
        }
    }

    void StartDragging()
    {
        //Debug.Log("Start");
        mask = LayerMask.GetMask(RaycastLayerName);
        dragging = true;
        draggableObjectDisplayInstance.SetActive(true);
    }

    void StopDragging()
    {
        //Debug.Log("Stop");
        dragging = false;
        draggableObjectDisplayInstance.SetActive(false);
        SendDragLetGoEvents();
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

    Vector3 GetWorldPositionFromInput()
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

        return new Vector3(worldPoint.x, worldPoint.y, 0f);
    }

    DragConsumer RaycastForGameObject()
    {
        Vector2 worldPoint;
        if (Input.touchCount == 1)
        {
            //Get the touch position on the screen and send a raycast into the game world from that position.
            worldPoint = Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position);
        }
        else
        {
            //Get the mouse position on the screen and send a raycast into the game world from that position.
            worldPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        }

        RaycastHit2D hit = Physics2D.Raycast(worldPoint, Vector2.zero, Mathf.Infinity, mask);

        //If something was hit, the RaycastHit2D.collider will not be null.
        if (hit.collider != null)
        {
            return hit.collider.gameObject.GetComponent<DragConsumer>();
        }

        return null;
    }
}
