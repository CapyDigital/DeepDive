using UnityEngine;

public class RepairTask : Task
{
    public Vector3 RepairPoint => _repairPoint;

    private Vector3 _repairPoint;

    private void Awake()
    {
        _repairPoint = transform.position;
    }
}
