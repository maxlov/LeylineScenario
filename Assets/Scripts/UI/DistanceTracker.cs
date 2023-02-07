using System;
using System.Collections;
using System.Collections.Generic;
using UnityAtoms.BaseAtoms;
using UnityEngine;

public class DistanceTracker : MonoBehaviour
{
    [SerializeField] private FloatVariable distance;
    [SerializeField] private GameObjectVariable target;

    private void LateUpdate()
    {
        if (target.Value)
            distance.Value = Vector3.Distance(transform.position, target.Value.transform.position);
    }
}
