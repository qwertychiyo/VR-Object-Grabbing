using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class RayColorChanger : MonoBehaviour
{
    public XRRayInteractor rayInteractor;
    public Color defaultColor = Color.white;
    public Color targetColor = Color.green;

    private LineRenderer lineRenderer;
    private Material lineMaterialInstance;

    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        if (lineRenderer != null)
        {
            // Clone material to avoid affecting other line visuals
            lineMaterialInstance = new Material(lineRenderer.material);
            lineRenderer.material = lineMaterialInstance;
        }
    }

    void Update()
    {
        if (rayInteractor == null || lineRenderer == null || lineMaterialInstance == null)
            return;

        if (rayInteractor.TryGetCurrent3DRaycastHit(out RaycastHit hit))
        {
            TargetIdentifier target = hit.collider.GetComponent<TargetIdentifier>();

            if (target != null && target.isTarget)
            {
                lineMaterialInstance.color = targetColor;
                return;
            }
        }

        // Set to default if not pointing at target
        lineMaterialInstance.color = defaultColor;
    }
}
