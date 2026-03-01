using UnityEngine;

public class ScreenToWorldPositionConverter
{    
    private Camera _camera;

    public ScreenToWorldPositionConverter(Camera camera)
    {
        _camera = camera;
    }

    public Vector3 GetPosition(Vector3 screenPosition, float targetY)
    {
        Ray ray = _camera.ScreenPointToRay(screenPosition);

        float distance = (targetY - ray.origin.y) / ray.direction.y;

        return ray.origin + ray.direction * distance;
    }
}
