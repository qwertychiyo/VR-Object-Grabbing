using UnityEngine;

public class GlowingObject : MonoBehaviour
{
    public bool isGlowing = false;

    public void SetGlow(bool glow)
    {
        isGlowing = glow;

        // Optional: visual glow effect
        var renderer = GetComponent<Renderer>();
        if (renderer != null)
        {
            Color glowColor = glow ? Color.yellow : Color.white;
            renderer.material.SetColor("_EmissionColor", glowColor);
            DynamicGI.SetEmissive(renderer, glowColor);
        }
    }
}
