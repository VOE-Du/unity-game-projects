using UnityEngine;

[RequireComponent(typeof(RectTransform))]
public class UIAspectSafeArea : MonoBehaviour
{
    public Camera targetCamera;

    private RectTransform rectTransform;
    private Rect lastCameraRect;

    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        ApplySafeArea();
    }

    void LateUpdate()
    {
        if (targetCamera == null)
            targetCamera = Camera.main;

        if (targetCamera == null)
            return;

        if (targetCamera.rect != lastCameraRect)
        {
            ApplySafeArea();
        }
    }

    void ApplySafeArea()
    {
        if (targetCamera == null)
            targetCamera = Camera.main;

        if (targetCamera == null)
            return;

        Rect rect = targetCamera.rect;
        lastCameraRect = rect;

        rectTransform.anchorMin = new Vector2(rect.xMin, rect.yMin);
        rectTransform.anchorMax = new Vector2(rect.xMax, rect.yMax);

        rectTransform.offsetMin = Vector2.zero;
        rectTransform.offsetMax = Vector2.zero;

        rectTransform.pivot = new Vector2(0.5f, 0.5f);
        rectTransform.localScale = Vector3.one;
    }
}