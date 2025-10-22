using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class TargetScannerController_Hand : MonoBehaviour
{
    [Header("Interactor & Visual")]
    public XRRayInteractor rayInteractor;  // Drag in your RightHandUIInteractor here
    public GameObject scannerVisual;
    public Transform target;

    [Header("Axis Settings")]
    public float axisThreshold = 0.2f;     // meters tolerance
    public float maxDistance   = 20f;     // ignore if too far away

    private bool isActive = false;

    void Update()
    {
        if (rayInteractor == null || scannerVisual == null || target == null)
            return;

        bool onAxis = IsRayOverYAxis();

        if (onAxis && !isActive)
        {
            isActive = true;
            scannerVisual.SetActive(true);
        }
        else if (!onAxis && isActive)
        {
            isActive = false;
            scannerVisual.SetActive(false);
        }
    }

    bool IsRayOverYAxis()
    {
        Vector3 origin = rayInteractor.transform.position;
        Vector3 dir    = rayInteractor.transform.forward;

        if (Vector3.Distance(origin, target.position) > maxDistance)
            return false;

        Vector2 o2 = new Vector2(origin.x, origin.z);
        Vector2 d2 = new Vector2(dir.x, dir.z);
        if (d2.sqrMagnitude < 1e-6f) return false;
        d2.Normalize();

        Vector2 t2 = new Vector2(target.position.x - origin.x,
                                 target.position.z - origin.z);

        float proj = Vector2.Dot(t2, d2);
        if (proj < 0f) return false;

        float perp = (t2 - d2 * proj).magnitude;
        return perp <= axisThreshold;
    }

    public void SetTarget(Transform newTarget)
    {
        target = newTarget;
    }
}
