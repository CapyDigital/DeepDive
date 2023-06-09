using System.Collections;
using UnityEngine;

public class RepairTorch : MonoBehaviour
{
    [SerializeField] private Transform _checkForRepairPoint;
    [SerializeField] private ParticleSystem _fireParticles;
    [SerializeField] private ParticleSystem _bubblesParticles;
    [SerializeField] private AudioSource    _blowTorchAudioSource;
    [SerializeField] private AudioSource    _bubblesAudioSource;


    [SerializeField] private Transform _sphereCastPosition;
    [SerializeField] private LayerMask _repairPointLayerMask;

    private bool _collidingWithRepairPoint;
    private bool _activated;

    private void Awake()
    {
        _collidingWithRepairPoint   = false;
        _activated                  = false;
        StopParticles();
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
        PlayParticles();
        _blowTorchAudioSource.Play();
        _bubblesAudioSource.Play();
        StartCoroutine(Repair());
    }

    public void DeActivateTorch()
    {
        if (!_activated) return;
        
        _activated = false;
        StopParticles();
        _blowTorchAudioSource.Stop();
        _bubblesAudioSource.Stop();
        StopAllCoroutines();
    }

    private void StopParticles()
    {
        _fireParticles.Stop();
        _bubblesParticles.Stop();
    }

    private void PlayParticles()
    {
        _fireParticles.Play();
        _bubblesParticles.Play();
    }

    private IEnumerator Repair()
    {
        while (_activated)
        {
            // if (Physics.Raycast(_checkForRepairPoint.position, _checkForRepairPoint.forward,
            //     out RaycastHit hitInfo, 1.0f))
            // {
            //     RepairTask aux = hitInfo.transform.GetComponent<RepairTask>();
            //     if (aux != null)
            //     {
            //         //Debug.Log("Repair point was hit trigger");
            //         //aux.CompleteTask();
            //         aux.Repair();
            //     }
            //     else Debug.Log("Hit something that is not repair point");
            // }
            // else Debug.Log("Repair gun didn't hit anything");


            if (Physics.SphereCast(_sphereCastPosition.position, 0.20f, _sphereCastPosition.forward,
                out RaycastHit hitInfo, 0.35f, _repairPointLayerMask))
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

    private void OnDrawGizmosSelected()
    {
        if (_sphereCastPosition != null)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawSphere(_sphereCastPosition.position, 0.20f);
        }
    }
}
