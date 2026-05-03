using UnityEngine;

[RequireComponent(typeof(Camera))]
public class AspectRatioHandler : MonoBehaviour
{
    [SerializeField] private float targetWidth = 1280f;
    [SerializeField] private float targetHeight = 720f;

    void Start()
    {
        Camera camera = GetComponent<Camera>();
        Rect rect = camera.rect;

        float targetRatio = targetWidth / targetHeight;
        float windowRatio = (float)Screen.width / (float)Screen.height;
        float scaleHeight = windowRatio / targetRatio;

        if (scaleHeight < 1.0f)
        {
            // Letterbox (top/bottom)
            rect.width = 1.0f;
            rect.height = scaleHeight;
            rect.x = 0;
            rect.y = (1.0f - scaleHeight) / 2.0f;
        }
        else
        {
            // Pillarbox (sides)
            float scaleWidth = 1.0f / scaleHeight;
            rect.width = scaleWidth;
            rect.height = 1.0f;
            rect.x = (1.0f - scaleWidth) / 2.0f;
            rect.y = 0;
        }
        camera.rect = rect;
    }
}
