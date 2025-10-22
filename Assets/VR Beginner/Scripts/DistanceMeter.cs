using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using TMPro;

public class HandDistanceMeter : MonoBehaviour
{
    [Header("References")]
    public XRRayInteractor rayInteractor;
    public TextMeshProUGUI distanceText;
    public Transform rayOrigin;

    [Header("Settings")]
    public float maxDistance = 10f;
    public Vector3 offset = new Vector3(0, 0.05f, 0.1f);

    private RaycastHit hit;

    void Update()
    {
        if (rayInteractor == null || distanceText == null || rayOrigin == null)
            return;

        if (rayInteractor.TryGetCurrent3DRaycastHit(out hit))
        {
            GlowingObject glow = hit.collider.GetComponent<GlowingObject>();

            if (glow != null && glow.isGlowing)
            {
                float distance = hit.distance;
                distanceText.text = $"{distance:F2} m";
                distanceText.gameObject.SetActive(true);
            }
            else
            {
                distanceText.gameObject.SetActive(false);
            }
        }
        else
        {
            distanceText.gameObject.SetActive(false);
        }

        // Position the text in front of the ray origin (hand)
        distanceText.transform.position = rayOrigin.position + rayOrigin.TransformDirection(offset);
        distanceText.transform.rotation = Quaternion.LookRotation(distanceText.transform.position - Camera.main.transform.position);
    }
}
