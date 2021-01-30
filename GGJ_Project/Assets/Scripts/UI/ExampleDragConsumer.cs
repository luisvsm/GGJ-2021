using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExampleDragConsumer : MonoBehaviour
{
    public void In()
    {
        // Reference to the current object that is being dragged
        // DraggableObject.currentDraggableObject;
        Debug.Log(gameObject.name + ": DragIn");
    }
    public void Out()
    {
        // Reference to the current object that is being dragged
        // DraggableObject.currentDraggableObject;
        Debug.Log(gameObject.name + ": DragOut");
    }
    public void LetGo()
    {
        // Reference to the current object that is being dragged
        // DraggableObject.currentDraggableObject;
        Debug.Log(gameObject.name + ": DragLetGo");
    }
}
