using System.Collections;
using System.Collections.Generic;
using UnityAtoms.BaseAtoms;
using UnityEngine;

public class PlayerScaler : MonoBehaviour
{
    public float sizeScaleValue = 0.1f;

    public void IncreaseSize()
    {
        transform.localScale += new Vector3(sizeScaleValue,sizeScaleValue,sizeScaleValue);
    }
}
