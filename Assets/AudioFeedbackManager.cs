using UnityEngine;

public class AudioFeedbackManager : MonoBehaviour
{
    public AudioSource audioSource;

    public AudioClip proximityBeep;
    public AudioClip pointingCue;
    public AudioClip grabConfirm;

    public void PlayProximityBeep()
    {
        PlayClip(proximityBeep);
    }

    public void PlayHoverCue()
    {
        PlayClip(pointingCue);
    }

    public void PlayGrabConfirm()
    {
        PlayClip(grabConfirm);
    }

    private void PlayClip(AudioClip clip)
{
    if (clip != null && audioSource != null)
    {
        audioSource.PlayOneShot(clip);
    }
}


    public void StartPointingAudio()
    {
        PlayClip(pointingCue);
    }

    public void StopPointingAudio()
    {
        if (audioSource != null && audioSource.isPlaying)
        {
            audioSource.Stop();
        }
    }
}
