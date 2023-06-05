using UnityEngine;

public abstract class Task : MonoBehaviour
{
    public TaskType TaskType => _taskType;

    [SerializeField] protected TaskType                 _taskType;
    [SerializeField] protected TaskCompletionController _taskController;

    public void UpdateTaskProgress()
    {
        if (_taskController == null) return;
        
        Debug.Log($"Task name = {_taskType.ToString()}");
        _taskController.UpdateCompletionStatus(_taskType.ToString());
    }
}
