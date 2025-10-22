using UnityEngine;

public class DirectionalRayFeedback : MonoBehaviour
{
    public Transform target;           // Assigned at runtime from TargetManager
    public Transform rayVisual;        // Your cone object (child of this GameObject)
    public Material rayMaterial;       // Material used on the cone (must support Emission)

    public float maxAngle = 90f;       // Full fade out at this angle
    public float maxScale = 1.5f;
    public float minScale = 0.3f;

    private Transform head;
    private bool isActive = false;

    void Start()
    {
        // Safely get head reference even if Camera.main is null at start
        if (Camera.main != null)
            head = Camera.main.transform;
    }

    void Update()
    {
        if (!isActive || target == null || rayVisual == null || rayMaterial == null || head == null)
            return;

        Vector3 toTarget = (target.position - head.position).normalized;
        float angle = Vector3.Angle(head.forward, toTarget);
        float t = Mathf.Clamp01(1 - (angle / maxAngle));

        float width = Mathf.Lerp(minScale, maxScale, t);
        rayVisual.localScale = new Vector3(width, width, rayVisual.localScale.z);

        // Adjust brightness (ensure your shader supports _EmissionColor)
        rayMaterial.SetColor("_EmissionColor", Color.white * t);
    }

    public void SetTarget(Transform newTarget)
    {
        target = newTarget;
    }

    public void EnableFeedback(bool enabled)
    {
        isActive = enabled;
        if (rayVisual != null)
            rayVisual.gameObject.SetActive(enabled);
    }
}
