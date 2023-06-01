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
}