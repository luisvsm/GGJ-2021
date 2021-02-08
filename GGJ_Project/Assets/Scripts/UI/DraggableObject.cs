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
    private Rigidbody2D draggableRB;


    public static DraggableObject currentDraggableObject;

    private DragConsumer dragConsumer;
    private DragConsumer previousDragConsumer;
    private LayerMask mask;

    private bool _debugLogging = false;

    public virtual void AudioHookGrab()
    {

    }

    public virtual void AudioHookHoverIn()
    {

    }

    public virtual void AudioHookHoverOut()
    {

    }

    public virtual void AudioHookLetGo()
    {

    }

    void Start()
    {
        if (draggableObjectDisplay == null)
        {
            Debug.Log(gameObject.name);
        }

        if (draggableObjectDisplay == null)
        {
            // Drag the current object
        }
        else
        {
            draggableObjectDisplayInstance = Instantiate(draggableObjectDisplay, Vector3.zero, Quaternion.identity);
            Transform displayTemp = draggableObjectDisplayInstance.transform.Find("display");
            if (displayTemp)
                displayTemp.GetComponent<SpriteRenderer>().sprite = draggableSprite;

            draggableRB = draggableObjectDisplayInstance.GetComponent<Rigidbody2D>();
            draggableObjectDisplayInstance.SetActive(false);
        }
    }

    void FixedUpdate()
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
            if (_debugLogging)
            {
                Debug.Log(string.Format("<color=magenta>DRAG OUT!!</color>"));
            }
            // The drag consumer has changed, tell the drag controller
            // from the previous frame that a drag out event happened
            previousDragConsumer.OnDragOut.Invoke();
            AudioHookHoverOut();
        }

        if (
            dragConsumer != null && // If the raycast hit something this frame
            dragConsumer != previousDragConsumer // And if the /something/ is different from the last frame
        )
        {
            if (_debugLogging)
            {
                Debug.Log(string.Format("<color=magenta>DRAG IN!!</color>"));
            }
            // The drag consumer has changed, tell the drag controller that a drag out event happened
            dragConsumer.OnDragIn.Invoke();
            AudioHookHoverIn();
        }

        previousDragConsumer = dragConsumer;
    }

    void SendDragLetGoEvents()
    {
        if (dragConsumer != null)
        {
            if (_debugLogging)
            {
                Debug.Log(string.Format("<color=yellow> DRAG DROP!!</color>"));
            }

            dragConsumer.OnDragLetGo.Invoke();
            AudioHookLetGo();
        }

        currentDraggableObject = null;
    }

    void UpdateDragPosition()
    {
        if (draggableRB == null)
        {
            if (draggableObjectDisplayInstance == null)
            {
                // Drag the current object
                transform.position = GetWorldPositionFromInput();
            }
            else
            {
                draggableObjectDisplayInstance.transform.position = GetWorldPositionFromInput();
            }
        }
        else
        {
            draggableRB.MovePosition(GetWorldPositionFromInput());
        }
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

    public virtual void StartDragging()
    {
        // Debug.Log("Start");
        currentDraggableObject = this;
        mask = LayerMask.GetMask(RaycastLayerName);
        dragging = true;

        if (draggableObjectDisplayInstance != null)
        {
            draggableObjectDisplayInstance.SetActive(true);
            draggableObjectDisplayInstance.transform.position = GetWorldPositionFromInput();
        }

        AudioHookGrab();
    }

    void StopDragging()
    {
        //Debug.Log("Stop");
        dragging = false;
        if (draggableObjectDisplayInstance != null)
        {
            draggableObjectDisplayInstance.SetActive(false);
        }
        SendDragLetGoEvents();
    }

    // When a world space game object is clicked
    public void OnMouseDown()
    {
        // Don't want to deal with multi-touch >_< lol
        if (Input.touchCount <= 1)
        {
            StartDragging();
        }
    }

    // When a UI element is clicked
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
