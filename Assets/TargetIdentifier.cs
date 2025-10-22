using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using TMPro;

public class TargetIdentifier : MonoBehaviour
{
    public bool isTarget = false;

    public Timer timer;
    public HapticManager hapticManager;
    public AudioFeedbackManager audioFeedbackManager;

    public TMP_Text grabbedMessageText;
    public AudioClip announceClip; // 🔊 Manually assign in Inspector

    public float proximityMaxDistance = 5f;

    private Transform playerHand;
    private bool isHandInProximity = false;

    private Camera mainCamera;

    private void Start()
    {
        if (TryGetComponent<XRBaseInteractable>(out var interactable))
        {
            interactable.hoverEntered.AddListener(OnHoverEnter);
            interactable.selectEntered.AddListener(OnSelectEntered);
        }

        if (grabbedMessageText != null)
            grabbedMessageText.gameObject.SetActive(false);

        mainCamera = Camera.main;
    }

    private void Update()
    {
        if (!isTarget || playerHand == null) return;

        // Rotate the message to face the player
        if (grabbedMessageText != null && grabbedMessageText.gameObject.activeSelf && mainCamera != null)
        {
            Vector3 direction = (mainCamera.transform.position - grabbedMessageText.transform.position).normalized;
            direction.y = 0;
            grabbedMessageText.transform.rotation = Quaternion.LookRotation(-direction);
        }

        float distance = Vector3.Distance(transform.position, playerHand.position);

        if (distance <= proximityMaxDistance)
        {
            if (!isHandInProximity)
            {
                isHandInProximity = true;
                audioFeedbackManager?.PlayProximityBeep();
            }

            hapticManager?.TriggerProximityPulse(distance, proximityMaxDistance);
        }
        else
        {
            if (isHandInProximity)
            {
                isHandInProximity = false;
                hapticManager?.StopProximityPulse();
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!isTarget || !other.CompareTag("PlayerHand")) return;
        playerHand = other.transform;
    }

    private void OnTriggerExit(Collider other)
    {
        if (!isTarget || !other.CompareTag("PlayerHand")) return;

        isHandInProximity = false;
        playerHand = null;
        hapticManager?.StopProximityPulse();
    }

    private void OnHoverEnter(HoverEnterEventArgs args)
    {
        if (!isTarget) return;
        hapticManager?.TriggerHoverVibration();
        audioFeedbackManager?.PlayHoverCue();
    }

    private void OnSelectEntered(SelectEnterEventArgs args)
    {
        if (!isTarget) return;

        hapticManager?.TriggerGrabConfirmation();
        audioFeedbackManager?.PlayGrabConfirm();
        timer?.StopTimer();

        ShowGrabTextNextToObject();
    }

    private void ShowGrabTextNextToObject()
    {
        if (grabbedMessageText != null)
        {
            grabbedMessageText.text = "Object Grabbed!";
            grabbedMessageText.gameObject.SetActive(true);

            Vector3 offset = transform.right * -0.1f + Vector3.up * 0.1f;
            grabbedMessageText.transform.position = transform.position + offset;
        }
    }
}
