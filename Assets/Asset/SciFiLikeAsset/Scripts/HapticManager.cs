using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using System.Collections;
using UnityEngine.XR;

public class HapticManager : MonoBehaviour
{
    public XRBaseController leftController;
    public XRBaseController rightController;

    [Header("Proximity Settings")]
    public float proximityMinAmplitude = 0.1f;
    public float proximityMaxAmplitude = 0.3f;
    public float proximityMinInterval = 0.8f; // slower when far
    public float proximityMaxInterval = 0.15f; // faster when close

    [Header("Hover Settings")]
    public float hoverAmplitude = 0.5f;
    public float hoverDuration = 0.1f;

    [Header("Grab Settings")]
    public float grabAmplitude = 0.9f;
    public float grabDuration = 0.2f;

    [Header("Point Settings")]
    public float pointingAngleThreshold = 15f;
    public float pointingPulseInterval = 0.6f;
    public float pointingPulseAmplitude = 0.6f;
    public float pointingPulseDuration = 0.1f;

    private Coroutine proximityPulseRoutine;
    private Coroutine pointingPulseRoutine;
    private Transform pointingTarget;
    private Transform pointingSource;

    public void TriggerProximityPulse(float distance, float maxDistance)
    {
        if (!isActiveAndEnabled) return;
        if (proximityPulseRoutine != null) return;

        proximityPulseRoutine = StartCoroutine(ProximityPulse(distance, maxDistance));
    }

    public void StopProximityPulse()
    {
        if (proximityPulseRoutine != null)
        {
            StopCoroutine(proximityPulseRoutine);
            proximityPulseRoutine = null;
        }
    }

    private IEnumerator ProximityPulse(float distance, float maxDistance)
    {
        while (true)
        {
            float t = Mathf.Clamp01(1f - (distance / maxDistance));
            float amplitude = Mathf.Lerp(proximityMinAmplitude, proximityMaxAmplitude, t);
            float interval = Mathf.Lerp(proximityMinInterval, proximityMaxInterval, t);

            SendHaptic(amplitude, 0.05f);
            yield return new WaitForSeconds(interval);
        }
    }

    public void TriggerHoverVibration()
    {
        if (!isActiveAndEnabled) return;
        SendHaptic(hoverAmplitude, hoverDuration);
    }

    public void TriggerGrabConfirmation()
    {
        if (!isActiveAndEnabled) return;
        SendHaptic(grabAmplitude, grabDuration);
    }

    public void StartPointingFeedback(Transform target, Transform hand)
    {
        if (!isActiveAndEnabled || pointingPulseRoutine != null) return;

        pointingTarget = target;
        pointingSource = hand;
        pointingPulseRoutine = StartCoroutine(PointingPulse());
    }

    public void StopPointingFeedback()
    {
        if (pointingPulseRoutine != null)
        {
            StopCoroutine(pointingPulseRoutine);
            pointingPulseRoutine = null;
        }
    }

    private IEnumerator PointingPulse()
    {
        while (true)
        {
            if (pointingTarget != null && pointingSource != null)
            {
                Vector3 toTarget = (pointingTarget.position - pointingSource.position).normalized;
                float angle = Vector3.Angle(pointingSource.forward, toTarget);

                if (angle <= pointingAngleThreshold)
                {
                    SendHaptic(pointingPulseAmplitude, pointingPulseDuration);
                yield return new WaitForSeconds(pointingPulseDuration + 0.1f);

                // Optional second weaker pulse
                SendHaptic(pointingPulseAmplitude * 0.5f, pointingPulseDuration * 0.8f);
                yield return new WaitForSeconds(pointingPulseInterval);
                continue;
                }
            }
            yield return new WaitForSeconds(pointingPulseInterval);
        }
    }

    private void SendHaptic(float amplitude, float duration)
    {
        if (leftController != null)
            leftController.SendHapticImpulse(amplitude, duration);
        if (rightController != null)
            rightController.SendHapticImpulse(amplitude, duration);
    }
}
