using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationStartRandomizer : MonoBehaviour
{
    [SerializeField] private float _minAnimationStartTime = 0.0f;
    [SerializeField] private float _maxAnimationStartTime = 1.0f;

    [SerializeField] private float animationStartTime = 1.0f;

    private Animator _animator;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    private IEnumerator Start()
    {
        //float animationStartTime = Random.Range(_minAnimationStartTime, _maxAnimationStartTime);
        Debug.Log($"Animation starting in: {animationStartTime}");
        yield return new WaitForSeconds(animationStartTime);

        _animator.SetTrigger("Swim");
    }
}
