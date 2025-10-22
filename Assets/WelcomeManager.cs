using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class WelcomeManager : MonoBehaviour
{
    [Header("References")]
    public Transform startPosition;
    public GameObject xrRig;
    public GameObject welcomeCanvas;
    public Button visualNormalButton;
    public Button visualBlurredButton;
    public Button hapticAudioBlurredButton;
    public Timer timer;
    public TargetManager targetManager;
    public GameObject feedbackManager;
    public GameObject arrow;
    public GameObject rightHandRayInteractor;
    public GameObject rightUIInteractor;    // 🔄 Drag your RightHandUIInteractor GameObject here
    public GameObject blurVolume;
    public TargetScannerController_Hand rightScannerController;
    public GameObject rightScannerVisual;

    private bool hasStarted = false;

    void Start()
    {
        // Position the player
        if (xrRig != null && startPosition != null)
        {
            xrRig.transform.position = startPosition.position;
            xrRig.transform.rotation = startPosition.rotation;
        }

        // SHOW welcome UI & ENABLE UI‐ray interactor
        welcomeCanvas.SetActive(true);
        rightUIInteractor.SetActive(true);
        EventSystem.current.SetSelectedGameObject(null);

        // Hook up buttons
        visualNormalButton.onClick.RemoveAllListeners();
        visualBlurredButton.onClick.RemoveAllListeners();
        hapticAudioBlurredButton.onClick.RemoveAllListeners();

        visualNormalButton.onClick.AddListener(() => StartExperience("VisualNormal"));
        visualBlurredButton.onClick.AddListener(() => StartExperience("VisualBlurred"));
        hapticAudioBlurredButton.onClick.AddListener(() => StartExperience("HapticAudioBlurred"));
    }

    void StartExperience(string mode)
    {
        if (hasStarted) return;
        hasStarted = true;

        // HIDE the welcome UI and DISABLE UI‐ray interactor
        welcomeCanvas.SetActive(false);
        rightUIInteractor.SetActive(false);

        // Disable all feedback/visual systems
        feedbackManager.SetActive(false);
        arrow.SetActive(false);
        rightHandRayInteractor.SetActive(false);
        blurVolume.SetActive(false);
        rightScannerController.enabled = false;
        rightScannerVisual.SetActive(false);

        // Clear outlines
        var outlines = FindObjectsOfType<SelectionOutline>(true);
        foreach (var o in outlines) o.enabled = false;

        // Reactivate visuals or feedback by mode
        switch (mode)
        {
            case "VisualNormal":
                arrow.SetActive(true);
                rightHandRayInteractor.SetActive(true);
                rightScannerController.enabled = true;
                break;

            case "VisualBlurred":
                blurVolume.SetActive(true);
                arrow.SetActive(true);
                rightHandRayInteractor.SetActive(true);
                rightScannerController.enabled = true;
                break;

            case "HapticAudioBlurred":
                feedbackManager.GetComponent<FeedbackManager>().SetFeedbackEnabled(true);
                blurVolume.SetActive(true);
                feedbackManager.SetActive(true);
                break;
        }

        targetManager.AssignRandomTarget(mode == "HapticAudioBlurred");
    }
}
