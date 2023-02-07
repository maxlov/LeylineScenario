using UnityEngine;

public class LookAtTarget : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private bool _flip;

    public void SetTarget(GameObject newTarget)
    {
        target = newTarget.transform;
    }
    
    void Update()
    {   
        if (_flip)
            transform.LookAt(2 * transform.position - target.position);
        else
            transform.LookAt(target);
    }
}
