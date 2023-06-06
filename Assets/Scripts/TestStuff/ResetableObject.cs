using UnityEngine;

public class ResetableObject : MonoBehaviour
{
    private Quaternion  _initialRotation;
    private Vector3     _initialPosition;

    private void Awake()
    {
        _initialPosition = transform.position;
        _initialRotation = transform.rotation;
    }

    public void ResetObject()
    {
        transform.rotation = _initialRotation;
        transform.position = _initialPosition;
    }
}
