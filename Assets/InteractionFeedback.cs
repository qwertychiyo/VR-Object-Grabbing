using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class InteractionFeedback : MonoBehaviour
{
    public HapticManager hapticManager;
    public AudioFeedbackManager audioManager;

    private bool feedbackEnabled = false;
    private bool isTarget = false;

    public void SetFeedbackEnabled(bool enabled)
    {
        feedbackEnabled = enabled;
    }

    public void SetAsTarget(bool isTargetObject)
    {
        isTarget = isTargetObject;
    }

    // Now requires distance inputs
    public void TriggerProximity(float distance, float maxDistance)
    {
        if (feedbackEnabled && isTarget)
        {
            hapticManager?.TriggerProximityPulse(distance, maxDistance);
            audioManager?.PlayProximityBeep();
        }
    }

    public void StopProximity()
    {
        if (feedbackEnabled && isTarget)
        {
            hapticManager?.StopProximityPulse();
        }
    }

    public void TriggerHover()
    {
        if (feedbackEnabled && isTarget)
        {
            hapticManager?.TriggerHoverVibration();
            audioManager?.PlayHoverCue();
        }
    }

    public void TriggerGrab()
    {
        if (feedbackEnabled && isTarget)
        {
            hapticManager?.TriggerGrabConfirmation();
            audioManager?.PlayGrabConfirm();
            Timer.Instance?.StopTimer(); // Make sure Timer.cs has a static Instance
        }
    }
}
