using UnityEngine;

[CreateAssetMenu(fileName = "ScriptableAnimationClip", menuName = "Scriptable Objects/AnimationClip")]
public class ScriptableAnimationClip : ScriptableObject
{
    [SerializeField] Sprite _staticSprite;
    [SerializeReference, SubclassSelector] IAnimationLoop[] _playOnStart;
    [SerializeReference, SubclassSelector] IAnimationLoop[] _playLoop;

    public Sprite StaticSprite { get => _staticSprite; }
    public IAnimationLoop[] PlayOnStart { get => _playOnStart; }
    public IAnimationLoop[] PlayLoop { get => _playLoop; }
}
