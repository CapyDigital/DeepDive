using UnityEngine;

public abstract class Task : MonoBehaviour
{
    public TaskType TaskType => _taskType;

    [SerializeField] protected TaskType _taskType; 
}
