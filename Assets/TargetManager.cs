using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using TMPro;

public class TargetManager : MonoBehaviour
{
    public GameObject[] grabbableObjects;
    public ArrowPointer arrowPointer;
    public Timer timer;

    public HapticManager hapticManager;
    public AudioFeedbackManager audioFeedbackManager;

    public TargetScannerController_Hand rightScannerController;

    public GameObject grabbedMessageTextPrefab;

    private Transform currentTarget;

    public void AssignRandomTarget(bool enableFeedback)
    {
        if (grabbableObjects.Length == 0) return;

        DisableAllOutlines();

        int index = Random.Range(0, grabbableObjects.Length);
        GameObject selectedTarget = grabbableObjects[index];
        currentTarget = selectedTarget.transform;

        arrowPointer.SetTarget(selectedTarget);

        TargetIdentifier targetID = selectedTarget.GetComponent<TargetIdentifier>();
        if (targetID == null)
            targetID = selectedTarget.AddComponent<TargetIdentifier>();

        targetID.isTarget = true;
        targetID.timer = timer;

        if (enableFeedback)
        {
            targetID.hapticManager = hapticManager;
            targetID.audioFeedbackManager = audioFeedbackManager;
        }

        if (grabbedMessageTextPrefab != null)
        {
            GameObject textInstance = Instantiate(grabbedMessageTextPrefab, selectedTarget.transform);
            textInstance.transform.localPosition = new Vector3(-0.25f, 0.15f, 0);
            textInstance.transform.localRotation = Quaternion.identity;
            textInstance.SetActive(false);
            targetID.grabbedMessageText = textInstance.GetComponent<TMP_Text>();
        }

        if (!enableFeedback)
        {
            var outline = selectedTarget.GetComponent<SelectionOutline>();
            if (outline != null)
                outline.enabled = true;
        }

        var rightHand = GameObject.Find("RightHand Controller");
        var rightFeedback = rightHand?.GetComponent<PointingFeedbackController>();
        if (rightFeedback != null)
            rightFeedback.SetTarget(currentTarget);

        if (rightScannerController != null)
            rightScannerController.SetTarget(currentTarget);

        // 🔊 Play announcement audio (manually assigned in Inspector)
        if (enableFeedback && audioFeedbackManager != null && targetID.announceClip != null)
        {
            audioFeedbackManager.audioSource.PlayOneShot(targetID.announceClip);
        }
    }

    public Transform CurrentTarget => currentTarget;

    public void DisableAllOutlines()
    {
        foreach (var obj in grabbableObjects)
        {
            var outline = obj.GetComponent<SelectionOutline>();
            if (outline != null)
                outline.enabled = false;
        }
    }
}
