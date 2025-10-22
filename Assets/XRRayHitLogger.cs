using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class XRRayHitLogger : MonoBehaviour
{
    private XRRayInteractor rayInteractor;

    void Start()
    {
        rayInteractor = GetComponent<XRRayInteractor>();
    }

    void Update()
    {
        if (rayInteractor == null)
            return;

        // Check 3D hits (for world-space UI and objects)
        if (rayInteractor.TryGetCurrent3DRaycastHit(out RaycastHit hit))
        {
            Debug.Log("🎯 XR Ray hit: " + hit.collider.gameObject.name);
        }
        else
        {
            Debug.Log("⭕ XR Ray is not hitting anything");
        }
    }
}
