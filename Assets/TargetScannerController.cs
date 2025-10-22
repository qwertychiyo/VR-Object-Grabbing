using UnityEngine;

public class TargetScannerController : MonoBehaviour
{
    public Transform targetToTrack;             // Target assigned at runtime
    public Renderer scannerVisual;              // Assign Cylinder Renderer
    public float viewAngle = 45f;               // Sensitivity angle
    public float spinSpeed = 90f;               // Degrees per second


    private void Update()
    {
        if (targetToTrack == null || scannerVisual == null)
            return;

        // Rotate scanner like a radar sweep
        transform.Rotate(Vector3.up, spinSpeed * Time.deltaTime, Space.Self);

        // Compute angle between camera forward and target direction
        Vector3 directionToTarget = (targetToTrack.position - Camera.main.transform.position).normalized;
        float angle = Vector3.Angle(Camera.main.transform.forward, directionToTarget);

        float intensity = angle < viewAngle
            ? Mathf.Clamp01(1f - angle / viewAngle)
            : 0f;

        SetVisualIntensity(intensity);
    }

    private void SetVisualIntensity(float intensity)
    {
        if (scannerVisual.material.HasProperty("_EmissionColor"))
        {
            Color baseColor = Color.green; // You can change this
            baseColor.a = 0.2f + intensity * 0.8f;

            scannerVisual.material.SetColor("_EmissionColor", baseColor * intensity);
            scannerVisual.material.EnableKeyword("_EMISSION");
        }
    }

    public void SetTarget(Transform newTarget)
    {
        targetToTrack = newTarget;
    }
}
