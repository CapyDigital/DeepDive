using UnityEngine;

public static class VisibilityChecker
{
    public static bool CheckObjectVisibility(Camera camera, Renderer objectRenderer)
    {
        // Get the object's bounds
        Bounds objectBounds = objectRenderer.bounds;

        // Check if the object is visible by the camera
        bool isVisible = GeometryUtility.TestPlanesAABB(GeometryUtility.CalculateFrustumPlanes(camera), objectBounds);

        return isVisible;
    }

    public static bool CheckObjectVisibility(Camera camera, Renderer objectRenderer, GameObject targetObject, LayerMask layerMask)
    {
        // Get the object's bounds
        Bounds objectBounds = objectRenderer.bounds;

        // Check if the object is visible by the camera
        bool isVisible = GeometryUtility.TestPlanesAABB(GeometryUtility.CalculateFrustumPlanes(camera), objectBounds);

        if (!isVisible)
        {
            Debug.Log($"Fish not in frustum ({targetObject.name})");
            return false;
        }

        Debug.Log($"Fish is in frustum, checking Raycast ({targetObject.name})");
        if (Physics.Raycast(camera.transform.position, targetObject.transform.position - camera.transform.position,
                                out RaycastHit hitInfo, Mathf.Infinity, layerMask))
        {
            isVisible = (hitInfo.collider.gameObject == targetObject);
            //Debug.Log($"Target object visible status after raycast = {isVisible}");
            Debug.Log($"Target object was {targetObject.name}, hit object is {hitInfo.collider.name}");
        }
        else
        {
            Debug.Log("Raycast didn't hit anything");
            isVisible = false;
        }

        //Debug.Log($"Target object visible status = {isVisible}");
        return isVisible;
    }
}