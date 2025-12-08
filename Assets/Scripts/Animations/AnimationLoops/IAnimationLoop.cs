using UnityEngine;

public interface IAnimationLoop
{
    bool HasFrame(int index);
    (Sprite, float) GetNextFrame(int index);
}
