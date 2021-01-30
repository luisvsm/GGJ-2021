using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public class DragConsumer : MonoBehaviour
{
    public UnityEvent OnDragIn;
    public UnityEvent OnDragOut;
    public UnityEvent OnDragLetGo;
}
