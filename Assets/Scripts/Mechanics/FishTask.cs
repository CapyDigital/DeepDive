using UnityEngine;

public class FishTask : Task
{
    public FishType FishType => _fishType;

    [SerializeField] private FishType                   _fishType;
    [SerializeField] private TaskCompletionController   _taskController;

    public override void CompleteTask()
    {
        if (_taskController == null) return;

        Debug.Log($"Task name = {_taskType.ToString()}");
        _taskController.UpdateCompletionStatus(_taskType.ToString());
    }
}
