using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingletonManager : MonoBehaviour
{
    [SerializeField] private GameObject[] monoBehaviourSingletons;
    [SerializeField] Transform container;


    private List<GameObject> _instantiatedSingletons;
    private static bool _created = false;

    // Use this for initialization
    void Awake()
    {
        if(!_created)
        {
            if (container == null)
            {
                container = this.transform;
            }

            DontDestroyOnLoad(container.gameObject);

            for (int i = 0; i < monoBehaviourSingletons.Length; i++)
            {
                if (_instantiatedSingletons == null)
                {
                    _instantiatedSingletons = new List<GameObject>();
                }

                GameObject item = Instantiate(monoBehaviourSingletons[i]);
                item.transform.parent = container;
                _instantiatedSingletons.Add(item);
            }

            _created = true;
        }
    }
}
