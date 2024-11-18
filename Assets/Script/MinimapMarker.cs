using UnityEngine;

public class MinimapMarker : MonoBehaviour
{
    public Transform player; 
    public RectTransform minimapRect; 
    public Camera minimapCamera; 
    private RectTransform markerRect;

    void Start()
    {
        markerRect = GetComponent<RectTransform>();
    }

    void LateUpdate()
    {
        if (player != null && minimapCamera != null)
        {
            Vector3 playerPosition = player.position;
            Vector3 viewportPosition = minimapCamera.WorldToViewportPoint(playerPosition);

            Vector2 markerPosition = new Vector2(
                (viewportPosition.x * minimapRect.sizeDelta.x) - (minimapRect.sizeDelta.x * 0.5f),
                (viewportPosition.y * minimapRect.sizeDelta.y) - (minimapRect.sizeDelta.y * 0.5f)
            );

            markerRect.anchoredPosition = markerPosition;
        }
    }
}
