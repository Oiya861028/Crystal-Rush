using UnityEngine;
using UnityEngine.UI;

public class AIHealthBarFollow : MonoBehaviour
{
    // Reference to the capsule's Transform
    public Transform target;

    // Offset from the target's position
    public Vector3 offset = new Vector3(0, 2, 0);

    public CanvasGroup canvasGroup;
    void LateUpdate()
    {
        transform.position = Camera.main.WorldToScreenPoint(target.position+offset);
        Vector3 screenPosition = Camera.main.WorldToScreenPoint(target.position + offset);

        // Check if the target is within the camera's view
        bool isOnScreen = screenPosition.z > 0 && 
                        screenPosition.x > 0 && screenPosition.x < Screen.width && 
                        screenPosition.y > 0 && screenPosition.y < Screen.height;

        // Set the health bar's visibility based on whether the target is on screen
        canvasGroup.alpha = isOnScreen ? 1 : 0;

        if (isOnScreen)
        {
            transform.position = screenPosition;
        }
    }
}