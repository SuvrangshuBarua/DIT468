using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CutsceneManager : MonoBehaviour
{
    TimeSystem _time;

    private void Start()
    {
        _time = LoopingManagers.Instance.TimeSystem;
    }

    public void PlayCutscene(ScriptableCutscene cutscene)
    {
        StartCoroutine(RunCutscene(cutscene));
    }

    IEnumerator RunCutscene(ScriptableCutscene cutscene)
    {
        List<ScriptableCutscene.CutsceneBeat> beats = new List<ScriptableCutscene.CutsceneBeat>();
        beats = cutscene.CutsceneTrack;

        float timePassed = 0;
        while(beats.Count != 0)
        {
            if (!_time.IsPaused)
            {
                timePassed += Time.deltaTime;
            }

            List<ScriptableCutscene.CutsceneBeat> remainingBeats = new List<ScriptableCutscene.CutsceneBeat>();
            foreach (var beat in beats)
            {
                if(beat.Time < timePassed)
                {
                    foreach(var action in beat.Outcome)
                    {
                        action.OnEventOccured();
                    }
                }
                else
                {
                    remainingBeats.Add(beat);
                }
            }
            beats = remainingBeats;

            yield return null;
        }
    }


}
