using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DynamicObjectPool<TPoolOf> where TPoolOf : MonoBehaviour
{

    private GameObject _prefab;
    private RectTransform _container;
    private List<TPoolOf> _objectPool;
    private Dictionary<string, int> _humanReadableLookup;
    private int _itemCount;

    public delegate void ObjectCreatedDelegate(TPoolOf created);
    private ObjectCreatedDelegate _onObjectCreatedEvent;

    public void Init(RectTransform container, GameObject prefab = null, ObjectCreatedDelegate onObjectCreated = null)
    {
        //If you add a prefab here this can be your fallback default you can still add different items, but if you always want to specify the object to create don't worry about setting this
        _prefab = prefab;
        _container = container;
        if (onObjectCreated != null)
        {
            _onObjectCreatedEvent += onObjectCreated;
        }
    }

    //Getting the object at index so you can use pool[i] to set and get
    public TPoolOf this[int key]
    {
        get
        {
            return GetValue(key);
        }
        set
        {
            SetValue(key, value);
        }
    }
    
    //Getting the object at index so you can use pool[i] to set and get
    public TPoolOf this[string humanReadableID]
    {
        get
        {
            return GetPoolItemByHumanReadableID(humanReadableID);
        }
        set
        {
            SetPoolItemByHumanReadableID(humanReadableID, value);
        }
    }

    private void SetValue(int key, TPoolOf value)
    {
        _objectPool[key] = value;
    }

    private TPoolOf GetValue(int key)
    {
        return _objectPool[key];
    }
    
    private bool SetPoolItemByHumanReadableID(string humanReadableID, TPoolOf value)
    {
        if (_humanReadableLookup.ContainsKey(humanReadableID))
        {
            _objectPool[_humanReadableLookup[humanReadableID]] = value;
            return true;
        }
        //TODO: Add if not found have a bool flag and then if true add
        //object with human readable ID not found 
        return false;
    }

    private TPoolOf GetPoolItemByHumanReadableID(string humanReadableID)
    {
        if (_humanReadableLookup.ContainsKey(humanReadableID))
        {
            return _objectPool[_humanReadableLookup[humanReadableID]];
        }

        return null;
    }

    public List<TPoolOf> GetPooledObjects(bool activeOnly = true)
    {
        if(_objectPool != null)
        {
            if (activeOnly)
            {
                List<TPoolOf> activeItems = null;
                for (int i = 0; i < _objectPool.Count; i++)
                {
                    if(_objectPool[i].gameObject.activeInHierarchy)
                    {
                        if(activeItems == null)
                        {
                            activeItems = new List<TPoolOf>();
                        }

                        activeItems.Add(_objectPool[i]);
                    }
                }

                return activeItems;
            }
            else
            {
                return _objectPool;
            }

        }

        return null;
    }

    public void AddItem(ref int nextChildCount, string humanReadableID = "")
    {
        if (_prefab != null)
        {
            AddItem(ref nextChildCount, _prefab, humanReadableID);
        }
        else
        {
            Debug.LogError("Tried to add a default object to the pool but a default prefab object wasn't specified at initialisation.");
        }
    }

    public void AddItem(ref int nextChildCount, GameObject newObjectPrefab, string humanReadableID = "")
    {
        if(_objectPool == null)
        {
            _objectPool = new List<TPoolOf>();
        }

        if(_itemCount >= _objectPool.Count || _objectPool[_itemCount] == null)
        {
            GameObject item = UnityEngine.Object.Instantiate(newObjectPrefab, _container, false);
            item.transform.localScale = Vector3.one;
            item.transform.SetSiblingIndex(nextChildCount);
            item.name = string.Format("item_{0}_{1}", nextChildCount, (typeof(TPoolOf)));
            TPoolOf pooledObject = item.GetComponent<TPoolOf>();
            _objectPool.Add(pooledObject);
            
            if (!string.IsNullOrEmpty(humanReadableID))
            {
                if (_humanReadableLookup == null)
                {
                    _humanReadableLookup = new Dictionary<string, int>();
                }
                
                _humanReadableLookup.Add(humanReadableID, _itemCount);
            }
            
            _itemCount++;
            nextChildCount++;

            if(_onObjectCreatedEvent != null)
            {
                _onObjectCreatedEvent(pooledObject);
            }
        }
        else
        {
            TPoolOf item = _objectPool[_itemCount];
            item.transform.SetSiblingIndex(nextChildCount);
            item.name = string.Format("item_{0}_{1}", nextChildCount, (typeof(TPoolOf)));
            
            if (!string.IsNullOrEmpty(humanReadableID))
            {
                if (_humanReadableLookup == null)
                {
                    _humanReadableLookup = new Dictionary<string, int>();
                }
                
                _humanReadableLookup.Add(humanReadableID, _itemCount);
            }
            
            _itemCount++;
            nextChildCount++;

            item.gameObject.SetActive(true);

            if (_onObjectCreatedEvent != null)
            {
                _onObjectCreatedEvent(item);
            }
        }
    }

    public void RemoveAll()
    {
        if (_objectPool != null)
        {
            int destroyItemsCount = _objectPool.Count;
            for (int i = _objectPool.Count - 1; i >= 0; i--)
            {
                RemoveItem(i, ref destroyItemsCount);
            }
        }
    }

    public void RemoveItem(int index, ref int nextChildCount)
    {
        if(_objectPool == null)
        {
            return;
        }

        if(index >= _objectPool.Count || _objectPool[index] == null)
        {
            return;
        }

        if(_objectPool[index].isActiveAndEnabled)
        {
            TPoolOf item = _objectPool[index];
            item.gameObject.SetActive(false);

            _itemCount--;
            nextChildCount--;
        }

    }

    public void DestroyAll()
    {
        if(_objectPool != null)
        {
            int destroyItemsCount = _objectPool.Count;
            for (int i = _objectPool.Count - 1; i >= 0; i--)
            {
                DestroyItem(i, ref destroyItemsCount);
            }
        }
    }

    public void DestroyItem(int index, ref int nextChildCount)
    {
        if (_objectPool == null)
        {
            return;
        }

        if (index >= _objectPool.Count)
        {
            return;
        }

        if (_objectPool[index] != null)
        {
            TPoolOf item = _objectPool[index];
            _objectPool.RemoveAt(index);
            item.gameObject.SetActive(false);
            UnityEngine.Object.Destroy(item);

        }
        else
        {
            _objectPool.RemoveAt(index);
        }

        _itemCount--;
        nextChildCount--;
    }
}