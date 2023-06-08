using UnityEngine;

public class RepairTask : Task
{
    public Vector3 RepairPoint => _repairPoint;

    [SerializeField] private MeshRenderer   _meshRenderer;
    [SerializeField] private Material       _repairedMaterial;
    [SerializeField] private Material       _brokenMaterial;

    [SerializeField] private float _repairGoal = 5.0f;

    private Vector3     _repairPoint;
    private TaskManager _taskManager;
    private float       _currentRepairAmount;

    private bool _canBeRepaired;

    private void Awake()
    {
        _repairPoint = transform.position;

        _canBeRepaired = false;
    }

    private void Start()
    {
        _taskManager = FindObjectOfType<TaskManager>();
        
        
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha5)) ActivateRepairPoint();
    }

    private void ActivateRepairPoint()
    {
        _canBeRepaired          = true;
        _currentRepairAmount    = 0.0f;
        _meshRenderer.material  = _brokenMaterial;
        _taskManager.IncreaseTasksToCompleteAmount();
    }

    public override void CompleteTask()
    {
        _meshRenderer.material = _repairedMaterial;
        _canBeRepaired = false;
        _taskManager.UpdateRepairPoints();
    }

    public void Repair()
    {
        if ((_currentRepairAmount < _repairGoal) && _canBeRepaired)
        {
            Debug.Log("Repair point is being repaired...");
            _currentRepairAmount += Time.deltaTime;

            if (_currentRepairAmount >= _repairGoal) CompleteTask();
        }
    }
}
