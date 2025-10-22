using UnityEngine;
using UnityEngine.InputSystem;

public class TriggerTimerStarter : MonoBehaviour
{
    public Timer timer;
    public InputActionProperty triggerAction;

    private bool hasStarted = false;

    void Update()
    {
        if (hasStarted || timer == null)
            return;

        float triggerValue = triggerAction.action.ReadValue<float>();
        if (triggerValue > 0.1f)
        {
            timer.StartTimer();
            hasStarted = true;
        }
    }
}
