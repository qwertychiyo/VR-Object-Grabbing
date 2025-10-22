using UnityEngine;

public class FeedbackManager : MonoBehaviour
{
    public HapticManager hapticManager;
    public AudioFeedbackManager audioManager;

    private bool feedbackEnabled = false;

    public void SetFeedbackEnabled(bool enabled)
    {
        feedbackEnabled = enabled;
    }

    public void StartPointing(Transform target, Transform hand)
    {
        if (!feedbackEnabled) return;

        hapticManager?.StartPointingFeedback(target, hand);
        audioManager?.StartPointingAudio();
    }

    public void StopPointing()
    {
        if (!feedbackEnabled) return;

        hapticManager?.StopPointingFeedback();
        audioManager?.StopPointingAudio();
    }

    public void TriggerProximity(float distance, float maxDistance)
    {
        if (!feedbackEnabled) return;

        hapticManager?.TriggerProximityPulse(distance, maxDistance);
        audioManager?.PlayProximityBeep();
    }

    public void StopProximity()
    {
        if (!feedbackEnabled) return;

        hapticManager?.StopProximityPulse();
    }

    public void TriggerHover()
    {
        if (!feedbackEnabled) return;

        hapticManager?.TriggerHoverVibration();
        audioManager?.PlayHoverCue();
    }

    public void TriggerGrab()
    {
        if (!feedbackEnabled) return;

        hapticManager?.TriggerGrabConfirmation();
        audioManager?.PlayGrabConfirm();
    }
}
