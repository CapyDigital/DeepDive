using UnityEngine;

public class FishTask : Task
{
    public FishType FishType => _fishType;

    [SerializeField] private FishType _fishType;
}
