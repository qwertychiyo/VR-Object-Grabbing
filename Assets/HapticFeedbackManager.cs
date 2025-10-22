using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class HapticFeedbackManager : MonoBehaviour
{
    [Header("Controller References")]
    public XRBaseController leftController;
    public XRBaseController rightController;

    [Header("Haptic Settings")]
    public float proximityAmplitude = 0.2f;
    public float proximityDuration = 0.1f;

    public float hoverAmplitude = 0.3f;
    public float hoverDuration = 0.1f;

    public float grabAmplitude = 0.7f;
    public float grabDuration = 0.3f;

    public void PulseProximity(XRBaseController controller)
    {
        controller?.SendHapticImpulse(proximityAmplitude, proximityDuration);
    }

    public void PulseHover(XRBaseController controller)
    {
        controller?.SendHapticImpulse(hoverAmplitude, hoverDuration);
    }

    public void PulseGrab(XRBaseController controller)
    {
        controller?.SendHapticImpulse(grabAmplitude, grabDuration);
    }
}
