using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RepairTorch : MonoBehaviour
{
    [SerializeField] private Transform _checkForRepairPoint;

    private bool _collidingWithRepairPoint;
    private bool _activated;

    private void Awake()
    {
        _collidingWithRepairPoint   = false;
        _activated                  = false;
    }

    // private void Update()
    // {
    //     if (Input.GetMouseButtonDown(0))
    //     {
    //         // // if (_collidingWithRepairPoint)
    //         // // {
    //         // //     Debug.Log("Repair point inside radius");
    //         // // }
    //         // // else Debug.Log("Repair point not inside radius");

    //         if (Physics.Raycast(_checkForRepairPoint.position, _checkForRepairPoint.forward,
    //             out RaycastHit hitInfo, 1.0f))
    //         {
    //             RepairTask aux = hitInfo.transform.GetComponent<RepairTask>();
    //             if (aux != null)
    //             {
    //                 Debug.Log("Repair point was hit trigger");
    //                 //aux.CompleteTask();
    //                 aux.Repair();
    //             }
    //             else Debug.Log("Hit something that is not repair point");
    //         }
    //         else Debug.Log("Repair gun didn't hit anything");
    //     }
    // }

    public void ActivateTorch()
    {
        _activated = true;
        StartCoroutine(Repair());
    }

    public void DeActivateTorch()
    {
        _activated = false;
        StopAllCoroutines();
    }

    private IEnumerator Repair()
    {
        while (_activated)
        {
            if (Physics.Raycast(_checkForRepairPoint.position, _checkForRepairPoint.forward,
                out RaycastHit hitInfo, 1.0f))
            {
                RepairTask aux = hitInfo.transform.GetComponent<RepairTask>();
                if (aux != null)
                {
                    //Debug.Log("Repair point was hit trigger");
                    //aux.CompleteTask();
                    aux.Repair();
                }
                else Debug.Log("Hit something that is not repair point");
            }
            else Debug.Log("Repair gun didn't hit anything");

            yield return null;
        }

        Debug.Log("Repair torch stopped repairing");
    }
}
