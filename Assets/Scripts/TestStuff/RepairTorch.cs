using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RepairTorch : MonoBehaviour
{
    [SerializeField] private Transform _checkForRepairPoint;

    private bool _collidingWithRepairPoint;

    private void Awake()
    {
        _collidingWithRepairPoint = false;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            // // if (_collidingWithRepairPoint)
            // // {
            // //     Debug.Log("Repair point inside radius");
            // // }
            // // else Debug.Log("Repair point not inside radius");

            if (Physics.Raycast(_checkForRepairPoint.position, _checkForRepairPoint.forward,
                out RaycastHit hitInfo, 1.0f))
            {
                RepairTask aux = hitInfo.transform.GetComponent<RepairTask>();
                if (aux != null)
                {
                    Debug.Log("Repair point was hit trigger");
                    aux.CompleteTask();
                }
                else Debug.Log("Hit something that is not repair point");
            }
            else Debug.Log("Repair gun didn't hit anything");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Something inside trigger");
        RepairTask aux = other.GetComponent<RepairTask>();
        if (aux != null)
        {
            _collidingWithRepairPoint = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        RepairTask aux = other.GetComponent<RepairTask>();
        if (aux != null)
        {
            _collidingWithRepairPoint = false;
        }
    }

    // private void OnDrawGizmosSelected()
    // {
    //     Gizmos.color = Color.blue;
    //     Gizmos.DrawWireSphere(_checkForRepairPointCollider.transform.position, _checkForRepairPointCollider.radius);
    // }
}
