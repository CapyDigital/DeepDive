using System.Collections.Generic;
using UnityEngine;

public class TaskManager : MonoBehaviour
{
    public bool AllTasksCompleted => _allTasksCompleted;

    [SerializeField] private List<FishType> _fishToPhotograph;
    [SerializeField] private List<FishType> _fishToTag;
    [SerializeField] private List<RepairTask> _repairPoints;

    [SerializeField] private List<Renderer> _fishRenderers;

    [SerializeField] private Camera _playerCam;

    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private AudioClip   _levelCompletionVoice;
    [SerializeField] private SceneController _sceneController;

    private int     _tasksToBeCompletedAmount;
    private int     _completedTasksAmount;
    private bool    _allTasksCompleted;

    private int _tasksLeft => _tasksToBeCompletedAmount - _completedTasksAmount;


    private List<Renderer> _fishRenderersToRemove;

    private void Awake()
    {
        _tasksToBeCompletedAmount   = _fishToPhotograph.Count + _fishToTag.Count + _repairPoints.Count;
        _completedTasksAmount       = 0;
        _fishRenderersToRemove      = new List<Renderer>(_fishToPhotograph.Count);
        _allTasksCompleted          = false;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha0)) Debug.Log($"Tasks left: {_tasksLeft}");
    }

    private void IncreaseCompletedTaskAmount()
    {
        _completedTasksAmount++;
        Debug.Log($"Tasks completed: {_completedTasksAmount}. Tasks left: {_tasksToBeCompletedAmount - _completedTasksAmount}");
        CheckLevelCompletion();
    }

    private void CheckLevelCompletion()
    {
        if (_completedTasksAmount >= _tasksToBeCompletedAmount)
        {
            Debug.Log("Level complete.");
            //_audioSource.PlayOneShot(_levelCompletionVoice);
            _allTasksCompleted = true;
            _sceneController.LoadNextScene();
        }   
    }

    public void CheckPhotoFish()
    {
        if (_fishToPhotograph.Count == 0)
        {
            Debug.Log("No fishes left to photograph");
            return;
        }

        Debug.Log("Check fish renderers");
        _fishRenderersToRemove.Clear();
        foreach (Renderer r in _fishRenderers)
        {
            if (VisibilityChecker.CheckObjectVisibility(_playerCam, r))
            {
                Debug.Log("Fish in photo");
                // FishTask aux = r.GetComponent<FishTask>();
                // {
                //     if (aux != null)
                //     {
                //         if (_fishToPhotograph.Contains(aux.FishType))
                //         {
                //             _fishToPhotograph.Remove(aux.FishType);
                //             _fishRenderersToRemove.Add(r);
                //             aux.UpdateTaskProgress();
                            
                //             Debug.Log("Fish wasn't yet photographed. +1 task completed." +
                //                 $"There are {_fishToPhotograph.Count} fish left to photograph");

                //             IncreaseCompletedTaskAmount();
                //         }
                //     }
                // }

                FishTask[] fishTasks = r.GetComponents<FishTask>();
                
                if (fishTasks.Length < 1) fishTasks = r.GetComponentsInParent<FishTask>();
                
                if (fishTasks.Length > 0)
                {
                    foreach (FishTask task in fishTasks)
                    {
                        if (task.TaskType == TaskType.PHOTOGRAPH &&
                                _fishToPhotograph.Contains(task.FishType))
                        {
                            _fishToPhotograph.Remove(task.FishType);
                            _fishRenderersToRemove.Add(r);
                            task.CompleteTask();
                            
                            Debug.Log("Fish wasn't yet photographed. +1 task completed." +
                                $"There are {_fishToPhotograph.Count} fish left to photograph");

                            IncreaseCompletedTaskAmount();
                        }
                    }
                }
                
            }
                
        }

        if (_fishRenderersToRemove.Count > 0)
        {
            foreach (Renderer r in _fishRenderersToRemove) _fishRenderers.Remove(r);
        }
    }

    public bool CheckTagFish(FishType fishType)
    {
        if (_fishToTag.Contains(fishType))
        {
            Debug.Log($"Tagged fish. There are {_fishToTag.Count} fish left to tag.");
            _fishToTag.Remove(fishType);
            
            IncreaseCompletedTaskAmount();
            return true;
        }
        else
        {
            Debug.Log("Tagged fish not on list to tag");
            return false;
        }
    }

    // public void UpdateRepairPoints(RepairTask repairTask)
    // {
    //     if (_repairPoints.Contains(repairTask))
    //     {
    //         Debug.Log("Repaired a point");
    //         _repairPoints.Remove(repairTask);
    //         IncreaseCompletedTaskAmount();
    //     }
    // }

    public void UpdateRepairPoints()
    {
        Debug.Log("Repaired a point");
        IncreaseCompletedTaskAmount();
    }

    public void IncreaseTasksToCompleteAmount() => _tasksToBeCompletedAmount++;
}
