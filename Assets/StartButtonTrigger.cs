using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.XR;
using UnityEngine.UI;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.UI;

public class StartButtonRayTrigger : MonoBehaviour
{
    public XRUIInputModule xrInputModule;      // On EventSystem
    public XRRayInteractor rayInteractor;      // Right-hand ray interactor

    private bool hasPressed = false;

    void Update()
    {
        if (rayInteractor == null || xrInputModule == null)
            return;

        InputDevice device = InputDevices.GetDeviceAtXRNode(XRNode.RightHand);
        bool triggerValue;

        if (device.TryGetFeatureValue(CommonUsages.triggerButton, out triggerValue))
        {
            if (triggerValue && !hasPressed)
            {
                hasPressed = true;

                if (rayInteractor.TryGetCurrentUIRaycastResult(out RaycastResult result))
                {
                    var button = result.gameObject?.GetComponent<Button>();
                    if (button != null && button.interactable)
                    {
                        button.onClick.Invoke();
                        Debug.Log("Triggered Button: " + button.name);
                    }
                }
            }
            else if (!triggerValue)
            {
                hasPressed = false;
            }
        }
    }
}
