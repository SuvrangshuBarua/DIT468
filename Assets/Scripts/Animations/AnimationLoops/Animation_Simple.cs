using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class Animation_Simple : IAnimationLoop
{
    [SerializeField] List<Sprite> _frames;
    [SerializeField] float _holdTime;

    public (Sprite, float) GetNextFrame(int index)
    {
        return (_frames[index], _holdTime);
    }

    public bool HasFrame(int index)
    {
        return _frames.Count > index;
    }
}
