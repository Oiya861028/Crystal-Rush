using UnityEngine;
using UnityEngine.UI;

public class HealthBarFollow : MonoBehaviour
{
    // Reference to the capsule's Transform
    public Transform target;

    // Offset from the target's position
    public Vector3 offset = new Vector3(0, 2, 0);

    // Cached components
    private RectTransform rectTransform;
    private Camera mainCamera;

    void Start()
    {
        // Get the RectTransform component
        rectTransform = GetComponent<RectTransform>();

        // Get the main camera
        mainCamera = Camera.main;
    }

    void Update()
    {
        // Check if the target is assigned
        if (target == null)
            return;

        // Convert the target's world position to screen space
        Vector3 screenPos = mainCamera.WorldToScreenPoint(target.position + offset);

        // Update the health bar's position
        rectTransform.position = screenPos;
    }
}