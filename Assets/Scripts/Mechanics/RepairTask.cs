using System.Collections.Generic;
using UnityEngine;

public class RepairTask : Task
{
    [SerializeField] private MeshRenderer _repairedObjectRenderer;
    [SerializeField] private MeshRenderer _brokenObjectRenderer;

    [SerializeField] private float _repairGoal = 5.0f;

    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private AudioClip _weldingSound;
    [SerializeField] private List<AudioClip> _metalBendSounds;

    private TaskManager         _taskManager;
    private RepairPointSpawner  _spawner;
    private float               _currentRepairAmount;
    private bool                _canBeRepaired;


    private float _timeSinceLastRepair;
    private bool _startedPlayingRepairSound;

    private void Awake()
    {
        _canBeRepaired = false;
        _startedPlayingRepairSound = false;
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
        if (_startedPlayingRepairSound)
        {
            float currentTime = Time.time;
            if (currentTime - _timeSinceLastRepair > 0.1f)
            {
                Debug.Log("Gonna stop playing welding sound");
                _audioSource.Stop();
                _audioSource.loop = false;
                _audioSource.clip = null;
                _startedPlayingRepairSound = false;
            }
            else Debug.Log("Still repairing, not gonna stop weld sound");
        }
    }

    private void PlayMetalBendSound()
    {
        AudioClip soundToPlay = _metalBendSounds[Random.Range(0, _metalBendSounds.Count)];
        _audioSource.PlayOneShot(soundToPlay);
    }

    public void ActivateRepairPoint()
    {
        PlayMetalBendSound();
        Debug.Log($"Broken point activated. ({Time.time})");
        _canBeRepaired          = true;
        _currentRepairAmount    = 0.0f;
        _brokenObjectRenderer.enabled = true;
        _repairedObjectRenderer.enabled = false;    
        _taskManager.IncreaseTasksToCompleteAmount();
    }

    public override void CompleteTask()
    {
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

            if (!_startedPlayingRepairSound)
            {
                Debug.Log("Started playing repair sound");
                _startedPlayingRepairSound = true;
                _audioSource.clip = _weldingSound;
                _audioSource.loop = true;
                _audioSource.Play();
            }

            if (_startedPlayingRepairSound) _timeSinceLastRepair = Time.time;

            if (_currentRepairAmount >= _repairGoal) CompleteTask();
        }
    }
}
