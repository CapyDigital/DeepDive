using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WhaleSounds : MonoBehaviour
{
    [SerializeField] private AudioSource        _audioSource;
    [SerializeField] private List<AudioClip>    _whaleSounds;
    [SerializeField] private float              _timeBetweenSounds = 15.0f;

    private float _soundTimer;

    private void Awake()
    {
        _soundTimer = _timeBetweenSounds;
    }

    private void Update()
    {
        if (_soundTimer <= 0.0f)
        {
            _audioSource.PlayOneShot(_whaleSounds[Random.Range(0, _whaleSounds.Count)]);
            _soundTimer = _timeBetweenSounds;
        }
        else _soundTimer -= Time.deltaTime;
    }
}
