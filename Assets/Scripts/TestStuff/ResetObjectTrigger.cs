using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetObjectTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        ResetableObject aux = TryGetResetableComponent(other.gameObject);

        if (aux != null) aux.ResetObject();
    }

    private ResetableObject TryGetResetableComponent(GameObject go)
    {
        ResetableObject aux;

        aux = go.GetComponent<ResetableObject>();

        if (aux == null) aux = go.GetComponentInParent<ResetableObject>();
        if (aux == null) aux = go.GetComponentInChildren<ResetableObject>();

        return aux;
    }
}
