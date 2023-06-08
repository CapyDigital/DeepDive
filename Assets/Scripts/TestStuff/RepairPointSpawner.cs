using System.Collections.Generic;
using UnityEngine;

public class RepairPointSpawner : MonoBehaviour
{
    [SerializeField] private float _timeForFirstRepairPoint;
    [SerializeField] private float _minCooldownBetweenPointSpawn;
    [SerializeField] private float _maxCooldownBetweenPointSpawn;

    private List<RepairTask>    _brokenPoints;
    private List<RepairTask>    _repairedPoints;
    private TaskManager         _taskManager;
    private float               _currentCooldown;
    private bool                _firstPointHasSpawned;


    private void Awake()
    {
        _taskManager            = FindObjectOfType<TaskManager>();
        _brokenPoints           = new List<RepairTask>();
        _repairedPoints         = new List<RepairTask>();
        _currentCooldown        = _timeForFirstRepairPoint;
        _firstPointHasSpawned   = false;
    }

    private void Update()
    {
        if (_taskManager.AllTasksCompleted) return;

        if (!_firstPointHasSpawned)
        {
            if (_currentCooldown <= 0.0f)
            {
                SpawnBrokenPoint();
                _firstPointHasSpawned = true;
            }
        }
        else
        {
            if (_repairedPoints.Count > 0 && (_currentCooldown <= 0.0f))
            {
                SpawnBrokenPoint();
            }
            else if (_repairedPoints.Count <= 0 && _currentCooldown <= 0.0f)
            {
                Debug.Log("All points are broken. Can't spawn anymore. Resetting Cooldown");
                ResetCurrentCooldown();
            }
        }

        if (_currentCooldown > 0)
        {
            UpdateCurrentCooldown();
            Debug.Log($"Current cooldown = {_currentCooldown}");
        }

        if (Input.GetKeyDown(KeyCode.Alpha1)) Debug.Log($"{_repairedPoints.Count}");
    }

    public void AddRepairedPoint(RepairTask point)
    {
        _repairedPoints.Add(point);

        if (_brokenPoints.Contains(point)) _brokenPoints.Remove(point);
    }

    private void AddBrokenPoint(RepairTask point)
    {
        _brokenPoints.Add(point);
        
        if (_repairedPoints.Contains(point)) _repairedPoints.Remove(point);
    }

    private RepairTask GetRandomRepairedPoint()
    {
        int randomIndex = Random.Range(0, _repairedPoints.Count);
        return _repairedPoints[randomIndex];
    }

    private void SpawnBrokenPoint()
    {
        Debug.Log("Spawning repair point");
        RepairTask pointToSpawn = GetRandomRepairedPoint();
        pointToSpawn.ActivateRepairPoint();
        AddBrokenPoint(pointToSpawn);
        ResetCurrentCooldown();
    }

    private void UpdateCurrentCooldown() => _currentCooldown -= Time.deltaTime;
    private void ResetCurrentCooldown()
    {
        _currentCooldown = Random.Range(_minCooldownBetweenPointSpawn,
                                            _maxCooldownBetweenPointSpawn);
    }
}
