using UnityEngine;

public class RepairTask : Task
{
    public Vector3 RepairPoint => _repairPoint;

    [SerializeField] private MeshRenderer   _meshRenderer;
    [SerializeField] private Material       _repairedMaterial;

    private Vector3     _repairPoint;
    private TaskManager _taskManager;

    private void Awake()
    {
        _repairPoint = transform.position;
    }

    private void Start()
    {
        _taskManager = FindObjectOfType<TaskManager>();
    }

    public void Repair()
    {
        _meshRenderer.material = _repairedMaterial;
        _taskManager.UpdateRepairPoints(this);
        Destroy(this);
    }
}
