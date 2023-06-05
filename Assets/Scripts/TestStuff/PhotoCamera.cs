using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PhotoCamera : MonoBehaviour
{
    [SerializeField] private Image      _photoDisplayArea;
    [SerializeField] private GameObject _photoFrame;
    [SerializeField] private GameObject _photoCameraOverlay;
    [SerializeField] private Animator   _photoDisplayAnimator;
    [SerializeField] private Camera     _camera;
    [SerializeField] private LayerMask  _fishLayerMask;
    [SerializeField] private float      _photoCameraFov = 30.0f;

    private Texture2D   _screenCapture;
    private TaskManager _taskManager;
    private bool        _displayingPhoto;

    private void Awake()
    {
        _screenCapture = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, false);
        _displayingPhoto = false;

        _camera.fieldOfView = 30.0f;
    }

    private void Start()
    {
        _taskManager = FindObjectOfType<TaskManager>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            if (_displayingPhoto)
                HidePhoto();
            else
            {
                StartCoroutine(TakePhoto()); 

                _taskManager.CheckPhotoFish();
            }      
        }
        
    }

    private IEnumerator TakePhoto()
    {
        _photoCameraOverlay.SetActive(false);
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
        _photoCameraOverlay.SetActive(true);
        _displayingPhoto = false;
        _photoFrame.SetActive(false);
    }

    public void GrabCamera()
    {
        _photoCameraOverlay.SetActive(true);
        _camera.fieldOfView = _photoCameraFov;
    }

    public void DropCamera()
    {
        _photoCameraOverlay.SetActive(false);
        _camera.fieldOfView = 60.0f;
    }
}
