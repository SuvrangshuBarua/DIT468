using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This can be used to create dictionaries in the inspector.
/// </summary>
/// <typeparam name="T">Key</typeparam>
/// <typeparam name="U">Value</typeparam>
[System.Serializable]
public class InspectableDictionary<T, U>
{    
    [SerializeField] List<InspectablePair<T, U>> _primingData;
    Dictionary<T, U> _dictionary;
    
    public InspectableDictionary()
    {
        _primingData = new List<InspectablePair<T, U>>();
    }

    public Dictionary<T, U> GetDictionary()
    {
        if(_dictionary == null)
        {
            _dictionary = new Dictionary<T, U>();

            foreach(InspectablePair<T, U> data in _primingData)
            {
                _dictionary.Add(data.Key, data.Value);
            }
        }

        return _dictionary;
    }
    
}
