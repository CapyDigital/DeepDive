using UnityEngine;

public class RepairTask : Task
{
    public Vector3 RepairPoint => _repairPoint;

    [SerializeField] private MeshRenderer   _meshRenderer;
    [SerializeField] private Material       _repairedMaterial;

    [SerializeField] private float _repairGoal = 5.0f;

    private Vector3     _repairPoint;
    private TaskManager _taskManager;
    private float       _currentRepairAmount;

    private void Awake()
    {
        _repairPoint = transform.position;
    }

    private void Start()
    {
        _taskManager = FindObjectOfType<TaskManager>();
    }

    public override void CompleteTask()
    {
        _meshRenderer.material = _repairedMaterial;
        _taskManager.UpdateRepairPoints(this);
        Destroy(this);
    }

    public void Repair()
    {
        if (_currentRepairAmount < _repairGoal)
        {
            Debug.Log("Repair point is being repaired...");
            _currentRepairAmount += Time.deltaTime;
        }
        else CompleteTask();
    }
}
