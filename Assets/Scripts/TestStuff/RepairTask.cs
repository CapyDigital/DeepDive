using UnityEngine;

public class RepairTask : Task
{
    public Vector3 RepairPoint => _repairPoint;

    [SerializeField] private MeshRenderer   _meshRenderer;
    [SerializeField] private Material       _repairedMaterial;
    [SerializeField] private Material       _brokenMaterial;


    [SerializeField] private MeshRenderer _repairedObjectRenderer;
    [SerializeField] private MeshRenderer _brokenObjectRenderer;

    [SerializeField] private float _repairGoal = 5.0f;

    private Vector3             _repairPoint;
    private TaskManager         _taskManager;
    private RepairPointSpawner  _spawner;
    private float               _currentRepairAmount;

    private bool _canBeRepaired;

    private void Awake()
    {
        _repairPoint = transform.position;

        _canBeRepaired = false;
    }

    private void Start()
    {
        _taskManager    = FindObjectOfType<TaskManager>();
        _spawner        = FindObjectOfType<RepairPointSpawner>();
        _brokenObjectRenderer.enabled = false;

        _spawner.AddRepairedPoint(this);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha5)) ActivateRepairPoint();
    }

    public void ActivateRepairPoint()
    {
        Debug.Log($"Broken point activated. ({Time.time})");
        _canBeRepaired          = true;
        _currentRepairAmount    = 0.0f;
        //_meshRenderer.material  = _brokenMaterial;
        _brokenObjectRenderer.enabled = true;
        _repairedObjectRenderer.enabled = false;    
        _taskManager.IncreaseTasksToCompleteAmount();
    }

    public override void CompleteTask()
    {
        //_meshRenderer.material = _repairedMaterial;
        _repairedObjectRenderer.enabled = true;
        _brokenObjectRenderer.enabled = false;
        _canBeRepaired = false;
        _taskManager.UpdateRepairPoints();
        _spawner.AddRepairedPoint(this);
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
