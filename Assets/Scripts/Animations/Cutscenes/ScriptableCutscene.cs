using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "ScriptableCutscene", menuName = "Scriptable Objects/ScriptableCutscene")]
public class ScriptableCutscene : ScriptableObject
{
    [SerializeField] List<CutsceneBeat> _cutsceneTrack;

    public List<CutsceneBeat> CutsceneTrack { get => _cutsceneTrack; }

    [System.Serializable]
    public struct CutsceneBeat
    {
        [SerializeField] int _time;
        [SerializeReference, SubclassSelector] IEventChange[] _outcome;

        public int Time { get => _time; }
        public IEventChange[] Outcome { get => _outcome; }
    }
}
