using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    [Header("Scene Transition Settings")]
    [SerializeField] private CanvasGroup        _sceneTransitionOverlay;
    [SerializeField] private float              _sceneTransitionOverlayFadeTime;
    [SerializeField] private List<GameObject>   _handRays;

    [Header("Sound Settings")]
    [SerializeField] private AudioSource    _cageRollSource;
    [SerializeField] private AudioClip      _cageRollSound;
    [SerializeField] private AudioClip      _levelCompleteVoice;

    private IEnumerator Start()
    {
        foreach (GameObject go in _handRays) go.SetActive(false);

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

        foreach (GameObject go in _handRays) go.SetActive(true);
    }

    public void LoadNextScene()
    {
        StartCoroutine(FadeOutScene());
    }

    private IEnumerator FadeOutScene()
    {
        foreach (GameObject go in _handRays) go.SetActive(false);
        
        _sceneTransitionOverlay.alpha = 0.0f;
        _sceneTransitionOverlay.gameObject.SetActive(true);

        // Play level complete speech
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

        yield return new WaitForSeconds(_cageRollSound.length / 2);

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
