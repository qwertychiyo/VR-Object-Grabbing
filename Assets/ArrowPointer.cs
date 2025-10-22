using UnityEngine;

public class ArrowPointer : MonoBehaviour
{
    private GameObject target;
    public Vector3 offset = new Vector3(0, 0.3f, 0);

    void Update()
    {
        if (target)
        {
            transform.position = target.transform.position + offset;
            transform.LookAt(Camera.main.transform);
        }
    }

    public void SetTarget(GameObject newTarget)
    {
        if (target == null) // only set once
        {
            target = newTarget;
        }
    }
}
