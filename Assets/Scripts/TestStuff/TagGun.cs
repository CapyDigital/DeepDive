using UnityEngine;

public class TagGun : MonoBehaviour
{
    [SerializeField] private Transform                  _bulletSpawnPoint;
    [SerializeField] private LayerMask                  _gunHitLayerMask;
    [SerializeField] private GameObject                 _laserSight;
    

    private RaycastHit  _bulletHit;
    private TaskManager _taskManager;

    private void Awake()
    {
        _taskManager = FindObjectOfType<TaskManager>();
        _laserSight.SetActive(false);
    }

    
    // private void Update()
    // {
    //     if (Input.GetMouseButtonDown(0))
    //     {
    //         Shoot();
    //     }
    // }

    public void Shoot()
    {
        if (Physics.Raycast(_bulletSpawnPoint.position, _bulletSpawnPoint.forward,
            out _bulletHit, Mathf.Infinity, _gunHitLayerMask))
        {
            // FishTask fish = _bulletHit.transform.GetComponent<FishTask>();

            // if (fish != null)
            // {
            //     if (_taskManager.CheckTagFish(fish.FishType)) fish.UpdateTaskProgress();
            // }

            FishTask[] fishTasks = _bulletHit.transform.GetComponents<FishTask>();

            if (fishTasks.Length < 1) fishTasks = _bulletHit.transform.GetComponentsInParent<FishTask>();

            if (fishTasks.Length > 0)
            {
                foreach (FishTask task in fishTasks)
                {
                    if (task.TaskType == TaskType.TAG && _taskManager.CheckTagFish(task.FishType))
                        task.CompleteTask();
                }
            }

            if (_bulletHit.collider)
            {
                Debug.Log(_bulletHit.collider.name);
            }
        }
        else Debug.Log("Didn't hit anything");
    }

    public void ChangeLaserSightActiveStatus(bool status) => _laserSight.SetActive(status);
}
