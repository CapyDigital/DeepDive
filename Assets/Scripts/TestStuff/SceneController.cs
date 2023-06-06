using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneController : MonoBehaviour
{
    [Header("Scene Transition Settings")]
    [SerializeField] private CanvasGroup    _sceneTransitionOverlay;
    [SerializeField] private float          _sceneTransitionOverlayFadeTime;

    [Header("Sound Settings")]
    [SerializeField] private AudioSource    _cageRollSource;
    [SerializeField] private AudioClip      _cageRollSound;
    [SerializeField] private AudioClip      _levelCompleteVoice;

    private IEnumerator Start()
    {
        _sceneTransitionOverlay.alpha = 1.0f;
        _sceneTransitionOverlay.gameObject.SetActive(true);

        // Play cage roll sound
        _cageRollSource.PlayOneShot(_cageRollSound);

        // Wait for cage roll sound to finish
        yield return new WaitForSeconds(_cageRollSound.length);

        // Fade out overlay
        float elapsedTime = 0;
        while (elapsedTime < _sceneTransitionOverlayFadeTime)
        {
            _sceneTransitionOverlay.alpha = Mathf.Lerp(1.0f, 0.0f, elapsedTime /
                                                        _sceneTransitionOverlayFadeTime);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        _sceneTransitionOverlay.alpha = 0.0f;

    }

    public void LoadNextScene()
    {
        StartCoroutine(FadeOutScene());
    }

    private IEnumerator FadeOutScene()
    {
        _sceneTransitionOverlay.alpha = 0.0f;
        _sceneTransitionOverlay.gameObject.SetActive(true);

        // Play cage roll sound
        _cageRollSource.PlayOneShot(_levelCompleteVoice);

        // Wait for cage roll sound to finish
        yield return new WaitForSeconds(_levelCompleteVoice.length);

        // Fade out overlay
        float elapsedTime = 0;
        while (elapsedTime < _sceneTransitionOverlayFadeTime)
        {
            _sceneTransitionOverlay.alpha = Mathf.Lerp(0.0f, 1.0f, elapsedTime /
                                                        _sceneTransitionOverlayFadeTime);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        _sceneTransitionOverlay.alpha = 1.0f;

        _cageRollSource.PlayOneShot(_cageRollSound);
    }
}
