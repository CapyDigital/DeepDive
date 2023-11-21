using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    [Header("Scene Transition Settings")]
    [SerializeField] private CanvasGroup                _sceneTransitionOverlay;
    [SerializeField] private float                      _sceneTransitionOverlayFadeTime;
    [SerializeField] private List<SkinnedMeshRenderer>  _vrHandRenderers;
    [SerializeField] private bool                       _isFirstScene;
    [SerializeField] private bool                       _isLastLevel;

    [Header("Sound Settings")]
    [SerializeField] private AudioSource    _cageRollSource;
    [SerializeField] private AudioClip      _cageRollSound;
    [SerializeField] private AudioClip      _levelCompleteVoice;
    [SerializeField] private AudioClip      _levelIndicationsVoice;

    private IEnumerator Start()
    {
        if (_isFirstScene) yield break;

        foreach (SkinnedMeshRenderer mr in _vrHandRenderers) mr.enabled = false;

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

        foreach (SkinnedMeshRenderer mr in _vrHandRenderers) mr.enabled = true;

        if (_levelIndicationsVoice != null) _cageRollSource.PlayOneShot(_levelIndicationsVoice);
    }

    public void LoadNextScene()
    {
        StartCoroutine(FadeOutScene());
    }

    private IEnumerator FadeOutScene()
    {
        if (!_isFirstScene)
            foreach (SkinnedMeshRenderer mr in _vrHandRenderers) mr.enabled = false;
        
        _sceneTransitionOverlay.alpha = 0.0f;
        _sceneTransitionOverlay.gameObject.SetActive(true);

        // Play level complete speech
        if (!_isFirstScene)
        {
            _cageRollSource.PlayOneShot(_levelCompleteVoice);

            // Wait for cage roll sound to finish
            yield return new WaitForSeconds(_levelCompleteVoice.length);
        }

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

        // Load next level
        if (!_isLastLevel)
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        else SceneManager.LoadScene(0); // Load the main menu
    }
}
