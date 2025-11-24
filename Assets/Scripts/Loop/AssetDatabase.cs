using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Events;
using System.Collections.Generic;

// Preloads any assets at the start of the game. Does not reset
public class AssetDatabase : MonoBehaviour
{
    public bool HasDatabaseLoaded { get; private set; }
    public List<ScriptableTimelineEvent> AllEvents { get; private set; }
    public List<ScriptableNPC> AllNPCs { get; private set; }
    UnityEvent _onDatabaseLoaded = new UnityEvent();

    bool _haveEventsLoaded = false;
    bool _haveNPCsLoaded = false;

    void Start()
    {
        LoopingManagers.Instance.TimeSystem.Pause("LoadData");
        LoadEvents();
        LoadNPCs();
    }

    public void LoadEvents()
    {
        Addressables.LoadAssetsAsync<ScriptableTimelineEvent>("timelineEvent", null).Completed += objects =>
        {
            AllEvents = new List<ScriptableTimelineEvent>();
            foreach (ScriptableTimelineEvent data in objects.Result)
            {
                if (!data.SkipOnLoad)
                {
                    AllEvents.Add(data);
                }  
            }

            _haveEventsLoaded = true;
            OnAllLoaded();
        };
    }

    public void LoadNPCs()
    {
        Addressables.LoadAssetsAsync<ScriptableNPC>("character", null).Completed += objects =>
        {
            AllNPCs = new List<ScriptableNPC>();
            foreach (ScriptableNPC data in objects.Result)
            {
                if (!data.SkipOnLoad)
                {
                    AllNPCs.Add(data);
                }
            }

            _haveNPCsLoaded = true;
            OnAllLoaded();
        };
    }

    void OnAllLoaded()
    {
        if(_haveEventsLoaded && _haveNPCsLoaded)
        {
            HasDatabaseLoaded = true;
            _onDatabaseLoaded.Invoke();
            LoopingManagers.Instance.TimeSystem.Unpause("LoadData");
        }
    }
        
    public void SubscribeToLoadedDatabase(UnityAction action)
    {
        _onDatabaseLoaded.AddListener(action);
    }

}
