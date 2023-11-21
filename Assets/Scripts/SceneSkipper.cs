using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSkipper : MonoBehaviour
{
    [SerializeField] private string _nextSceneName;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha0)) SceneManager.LoadScene(_nextSceneName);
    }
}
