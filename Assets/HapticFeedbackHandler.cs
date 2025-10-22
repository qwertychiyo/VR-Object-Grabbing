using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

public class HapticFeedbackHandler : MonoBehaviour
{
    public XRNode controllerNode = XRNode.RightHand; // Or LeftHand

    public float proximityAmplitude = 0.1f;
    public float hoverAmplitude = 0.3f;
    public float grabAmplitude = 0.6f;
    public float duration = 0.1f;

    public void TriggerProximityPulse()
    {
        SendHaptics(proximityAmplitude, duration);
    }

    public void TriggerHoverFeedback()
    {
        SendHaptics(hoverAmplitude, duration);
    }

    public void TriggerGrabConfirmation()
    {
        SendHaptics(grabAmplitude, duration);
    }

    private void SendHaptics(float amplitude, float duration)
    {
        InputDevice device = InputDevices.GetDeviceAtXRNode(controllerNode);
        if (device.isValid)
        {
            HapticCapabilities capabilities;
            if (device.TryGetHapticCapabilities(out capabilities) && capabilities.supportsImpulse)
            {
                uint channel = 0;
                device.SendHapticImpulse(channel, amplitude, duration);
            }
        }
    }
}
