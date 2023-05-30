using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PhotoCamera : MonoBehaviour
{
    [SerializeField] private Image      _photoDisplayArea;
    [SerializeField] private GameObject _photoFrame;
    [SerializeField] private Animator   _photoDisplayAnimator;

    private Texture2D   _screenCapture;
    private bool        _displayingPhoto;

    private void Awake()
    {
        _screenCapture = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, false);
        _displayingPhoto = false;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            if (_displayingPhoto)
                HidePhoto();
            else
                StartCoroutine(TakePhoto()); 
        }
        
    }

    private IEnumerator TakePhoto()
    {
        _displayingPhoto = true;

        yield return new WaitForEndOfFrame();

        Rect photoRegion = new Rect(0, 0, Screen.width, Screen.height);

        _screenCapture.ReadPixels(photoRegion, 0, 0, false);
        _screenCapture.Apply();

        DisplayPhoto();
    }

    private void DisplayPhoto()
    {
        Sprite photoSprite = Sprite.Create(_screenCapture, new Rect(
            0.0f, 0.0f, _screenCapture.width, _screenCapture.height), new Vector2(0.5f, 0.5f), 100.0f);
        
        _photoDisplayArea.sprite = photoSprite;

        _photoFrame.SetActive(true);
        _photoDisplayAnimator.Play("PhotoFadeIn");
    }

    private void HidePhoto()
    {
        _displayingPhoto = false;
        _photoFrame.SetActive(false);
    }
}
