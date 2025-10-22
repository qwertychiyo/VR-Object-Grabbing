using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class PointingFeedbackController : MonoBehaviour
{
    [Header("Interactor & Target")]
    public XRRayInteractor rayInteractor;     // Drag in your RightHandUIInteractor here
    public FeedbackManager feedbackManager;
    public Transform target;

    [Header("Axis Settings")]
    public float axisThreshold = 0.2f;        // meters tolerance from the target’s XZ position
    public float maxDistance   = 20f;        // ignore if too far away

    private bool isPointing = false;

    void Update()
    {
        if (rayInteractor == null || feedbackManager == null || target == null)
            return;

        bool onAxis = IsPointingAtYAxis();

        if (onAxis && !isPointing)
        {
            isPointing = true;
            feedbackManager.StartPointing(target, rayInteractor.transform);
        }
        else if (!onAxis && isPointing)
        {
            isPointing = false;
            feedbackManager.StopPointing();
        }
    }

    /// <summary>
    /// Returns true if the XR ray (projected to XZ) passes within axisThreshold of the target's XZ.
    /// </summary>
    bool IsPointingAtYAxis()
    {
        Vector3 origin = rayInteractor.transform.position;
        Vector3 dir    = rayInteractor.transform.forward;

        // 1) Distance check
        if (Vector3.Distance(origin, target.position) > maxDistance)
            return false;

        // 2) Project to ground plane
        Vector2 o2 = new Vector2(origin.x, origin.z);
        Vector2 d2 = new Vector2(dir.x, dir.z);
        if (d2.sqrMagnitude < 1e-6f) return false;
        d2.Normalize();

        // 3) Vector from ray origin to target XZ
        Vector2 t2 = new Vector2(target.position.x - origin.x,
                                 target.position.z - origin.z);

        // 4) Ensure ray is pointing “forward” at target (t >= 0)
        float proj = Vector2.Dot(t2, d2);
        if (proj < 0f) return false;

        // 5) Perpendicular distance from ray line to target axis
        float perp = (t2 - d2 * proj).magnitude;
        return perp <= axisThreshold;
    }

    public void SetTarget(Transform newTarget)
    {
        target = newTarget;
    }
}
