using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RepairTask : Task
{
    [SerializeField] private float _repairGoal = 5.0f;
    [SerializeField] private float _metalDistortionSpeed = 1.0f;

    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private AudioClip _weldingSound;
    [SerializeField] private List<AudioClip> _metalBendSounds;

    private TaskManager         _taskManager;
    private RepairPointSpawner  _spawner;
    private float               _currentRepairAmount;
    private bool                _canBeRepaired;


    private float _timeSinceLastRepair;
    private bool _startedPlayingRepairSound;


    private SkinnedMeshRenderer _skinnedMeshRenderer;
    private Material BarMaterial => _skinnedMeshRenderer.material;

    private float _heatAmount;

    [SerializeField] private ParticleSystem _bubblesPS;


    private void Awake()
    {
        _canBeRepaired = false;
        _startedPlayingRepairSound = false;
        _skinnedMeshRenderer = GetComponent<SkinnedMeshRenderer>();
        BarMaterial.SetColor("_EmissionColor", Color.black);

        ParticleSystem.EmissionModule emissionModule = _bubblesPS.emission;
        emissionModule.rateOverTime = 0;
    }

    private void Start()
    {
        _taskManager    = FindObjectOfType<TaskManager>();
        _spawner        = FindObjectOfType<RepairPointSpawner>();

        _spawner.AddRepairedPoint(this);
    }

    private void Update()
    {
        AddHeat(-Time.deltaTime / 2);
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
        StartCoroutine(DistortShape());


        PlayMetalBendSound();
        Debug.Log($"Broken point activated. ({Time.time})");
        _canBeRepaired          = true;
        _currentRepairAmount    = 0.0f;
        _taskManager.IncreaseTasksToCompleteAmount();
    }

    public override void CompleteTask()
    {
        _canBeRepaired = false;
        _taskManager.UpdateRepairPoints();
        _spawner.AddRepairedPoint(this);
    }

    public void Repair()
    {
        
        AddHeat(Time.deltaTime);

        if ((_currentRepairAmount < _repairGoal) && _canBeRepaired)
        {
            Debug.Log("Repair point is being repaired...");
            _currentRepairAmount += Time.deltaTime;

            float normalizedRepair = _currentRepairAmount / _repairGoal;
            _skinnedMeshRenderer.SetBlendShapeWeight(0, (1 - normalizedRepair) * 100);

            if (!_startedPlayingRepairSound)
            {
                Debug.Log("Started playing repair sound");
                _startedPlayingRepairSound = true;
                _audioSource.clip = _weldingSound;
                _audioSource.loop = true;
                _audioSource.Play();
            }

            if (_startedPlayingRepairSound) _timeSinceLastRepair = Time.time;

            if (_currentRepairAmount >= _repairGoal)
            {
                _skinnedMeshRenderer.SetBlendShapeWeight(0, 0);
                CompleteTask();
            }
        }
    }

    private IEnumerator DistortShape()
    {
        float elapsed = 0;

        float initialDistortValue = _skinnedMeshRenderer.GetBlendShapeWeight(0);
        float distortValue;

        while (elapsed < _metalDistortionSpeed)
        {
            elapsed += Time.deltaTime;
            distortValue = Mathf.Lerp(initialDistortValue, 100, elapsed / _metalDistortionSpeed);
            _skinnedMeshRenderer.SetBlendShapeWeight(0, distortValue);
            yield return null;
        }

        _skinnedMeshRenderer.SetBlendShapeWeight(0, 100);

    }

    private void AddHeat(float heat)
    {
        _heatAmount += heat;
        _heatAmount = Mathf.Clamp01(_heatAmount);
        SetColorFromHeat(_heatAmount);
        SetEmissionFromHeat(_heatAmount);
    }

    private void SetColorFromHeat(float progress)
    {
        Color c = BarMaterial.GetColor("_EmissionColor");
        c.r = progress;
        c.g = progress;
        c.b = progress;
        BarMaterial.SetColor("_EmissionColor", c);
    }

    private void SetEmissionFromHeat(float heat)
    {
        ParticleSystem.EmissionModule emissionModule = _bubblesPS.emission;
        emissionModule.rateOverTime = heat * 150; 
    }
}
