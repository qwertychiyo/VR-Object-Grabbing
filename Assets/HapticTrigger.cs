using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class HapticTrigger : MonoBehaviour
{
    public HapticFeedbackManager hapticManager;
    public XRBaseController leftController;
    public XRBaseController rightController;

    private bool isHovered = false;

    private void OnTriggerEnter(Collider other)
    {
        if (IsHand(other))
        {
            hapticManager?.PulseProximity(GetControllerFromObject(other.gameObject));
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (!isHovered && IsHand(other))
        {
            hapticManager?.PulseHover(GetControllerFromObject(other.gameObject));
            isHovered = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (IsHand(other))
        {
            isHovered = false;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (IsHand(collision.collider))
        {
            hapticManager?.PulseGrab(GetControllerFromObject(collision.collider.gameObject));
        }
    }

    private bool IsHand(Collider collider)
    {
        return collider.CompareTag("LeftHand") || collider.CompareTag("RightHand");
    }

    private XRBaseController GetControllerFromObject(GameObject obj)
    {
        if (obj.CompareTag("LeftHand")) return leftController;
        if (obj.CompareTag("RightHand")) return rightController;
        return null;
    }
}
