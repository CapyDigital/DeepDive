using UnityEngine;

public class TagGun : MonoBehaviour
{
    [SerializeField] private Transform  _bulletSpawnPoint;
    [SerializeField] private LayerMask  _gunHitLayerMask;

    private RaycastHit _bulletHit;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Shoot();
        }
    }

    private void Shoot()
    {
        if (Physics.Raycast(_bulletSpawnPoint.position, _bulletSpawnPoint.forward,
            out _bulletHit, Mathf.Infinity, _gunHitLayerMask))
        {
            if (_bulletHit.collider)
            {
                Debug.Log(_bulletHit.collider.name);
            }
        }
        else Debug.Log("Didn't hit anything");
    }
}
