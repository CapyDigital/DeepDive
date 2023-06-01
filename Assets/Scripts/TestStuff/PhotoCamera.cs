using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PhotoCamera : MonoBehaviour
{
    [SerializeField] private Image      _photoDisplayArea;
    [SerializeField] private GameObject _photoFrame;
    [SerializeField] private Animator   _photoDisplayAnimator;
    [SerializeField] private Transform  _camera;
    [SerializeField] private LayerMask  _fishLayerMask;

    private Texture2D   _screenCapture;
    private TaskManager _taskManager;
    private bool        _displayingPhoto;


    [SerializeField] private Renderer _fishRenderer;
    [SerializeField] private Camera  _photoCam;

    private void Awake()
    {
        _screenCapture = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, false);
        _displayingPhoto = false;
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

    private void CheckForFish()
    {
        // _photoHits = null;

        // _photoHits = Physics.BoxCastAll(_camera.position, Vector3.one * 1.9f,
        //     _camera.forward, _camera.rotation, Mathf.Infinity, _fishLayerMask);
        
        // if (_photoHits.Length > 0)
        // {
        //     Debug.Log($"Hit fish | {_photoHits.Length}");
        // }   
        // else
        //     Debug.Log("Didn't hit fish");


    }
}
