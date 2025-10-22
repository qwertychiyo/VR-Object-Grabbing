using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class VisualFeedbackManager : MonoBehaviour
{
    [Header("Screen Border Flash")]
    public Image screenBorder;
    public float flashSpeed = 2f;

    [Header("Floating Alert Text")]
    public TextMeshProUGUI alertText;

    [Header("Directional Arrow")]
    public RectTransform arrow;
    public Transform exitTarget;
    public Transform playerHead;

    private bool flashing = false;
    private float flashAlpha = 0f;

    void Start()
    {
        if (arrow != null)
            arrow.gameObject.SetActive(true); // Always show arrow
    }

    void Update()
    {
        if (flashing && screenBorder != null)
        {
            flashAlpha = Mathf.PingPong(Time.time * flashSpeed, 1f);
            screenBorder.color = new Color(1f, 0f, 0f, flashAlpha);
        }

        // Keep rotating the arrow toward the exit
        if (arrow != null && exitTarget != null && playerHead != null)
        {
            Vector3 direction = (exitTarget.position - playerHead.position).normalized;
            float angle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
            arrow.rotation = Quaternion.Euler(0, 0, -angle);
        }
    }

    public void TriggerVisuals()
    {
        StartCoroutine(FlashForDuration(5f)); // Show screen flash and alert text for 5 seconds
    }

    private IEnumerator FlashForDuration(float duration)
    {
        flashing = true;

        if (screenBorder != null)
            screenBorder.gameObject.SetActive(true);

        if (alertText != null)
        {
            alertText.text = "FIRE ALARM – EVACUATE";
            Color color = alertText.color;
            color.a = 1f;
            alertText.color = color;
            alertText.gameObject.SetActive(true);
        }

        yield return new WaitForSeconds(duration);

        flashing = false;

        if (screenBorder != null)
        {
            screenBorder.color = new Color(1f, 0f, 0f, 0f);
            screenBorder.gameObject.SetActive(false);
        }

        if (alertText != null)
            alertText.gameObject.SetActive(false);

        // Do NOT hide arrow here — keep it visible
    }
}
