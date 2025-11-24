using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This is used in the InspectableDictionary as a Key-Value Pair
/// </summary>
/// <typeparam name="T">Key</typeparam>
/// <typeparam name="U">Value</typeparam>
[System.Serializable]
public class InspectablePair<T, U>
{    
    [SerializeField] T _key;
    [SerializeField] U _value;

    public InspectablePair(T key, U value)
    {
        _key = key;
        _value = value;
    }

    public T Key { get => _key; }
    public U Value { get => _value; }    
}
