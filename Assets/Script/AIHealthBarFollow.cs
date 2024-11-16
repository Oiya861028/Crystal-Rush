using UnityEngine;
using UnityEngine.UI;

public class AIHealthBarFollow : MonoBehaviour
{
    // Reference to the capsule's Transform
    public Transform target;

    // Offset from the target's position
    public Vector3 offset = new Vector3(0, 2, 0);

    // Cached components
    public RectTransform rectTransform;
    private Camera mainCamera;
    // Reference to the CanvasGroup for controlling visibility
    private CanvasGroup canvasGroup;

    void Start()
    {
        //  Ensure rectTransform is assigned
        if (rectTransform == null)
        {
            Debug.LogError("RectTransform is not assigned.");
        }
        // Get the main camera
        mainCamera = Camera.main;
        canvasGroup = rectTransform.GetComponent<CanvasGroup>();
        if (canvasGroup == null)
        {
            canvasGroup = rectTransform.gameObject.AddComponent<CanvasGroup>();
        }
    }

    void Update()
    {
        // Check if the target is assigned
        if (target == null)
        {
            Debug.LogWarning("Target is not assigned.");
            return;
        }

        // Convert the target's world position to screen space
        Vector3 screenPos = mainCamera.WorldToScreenPoint(target.position + offset);

        // Check if the target is visible on the screen
        bool isVisible = screenPos.z > 0 && screenPos.x > 0 && screenPos.x < Screen.width && screenPos.y > 0 && screenPos.y < Screen.height;

        // Update the health bar's visibility
        canvasGroup.alpha = isVisible ? 1 : 0;

        // Update the health bar's position
        rectTransform.position = screenPos;

        // Maintain a consistent size on screen
        rectTransform.localScale = Vector3.one;
    }
}