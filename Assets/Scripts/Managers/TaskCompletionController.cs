using UnityEngine;

public class TaskCompletionController : MonoBehaviour
{
    [SerializeField] private GameObject _completionReward;

    private void Awake()
    {
        foreach (Transform t in transform) t.gameObject.SetActive(false);
    }

    private void Start()
    {
        _completionReward.SetActive(false);
    }

    private bool CheckForCompletion()
    {
        foreach (Transform t in transform)
        {
            if (!t.gameObject.activeSelf) return false;
        }

        return true;
    }

    public void UpdateCompletionStatus(string taskName)
    {
        Transform aux = transform.Find(taskName);

        if (aux != null)
        {
            aux.gameObject.SetActive(true);
            if (CheckForCompletion()) _completionReward.SetActive(true);
        }
        else
        {
            Debug.LogError($"Couldn't find child with name \"{taskName}\" in the object " +
                $"\"{gameObject.name}\"");
        }
    }
}
