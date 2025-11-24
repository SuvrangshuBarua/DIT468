using UnityEngine;

[CreateAssetMenu(fileName = "ScriptablePointOfInterest", menuName = "Scriptable Objects/PointOfInterest")]
public class ScriptablePointOfInterest : ScriptableObject
{
    [SerializeField] ScriptableRoom _room;

    public ScriptableRoom Room { get => _room; }
}
