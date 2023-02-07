using System.Collections;
using System.Collections.Generic;
using Mono.CompilerServices.SymbolWriter;
using UnityAtoms.BaseAtoms;
using UnityEngine;

public class EndZone : MonoBehaviour
{
    //[SerializeField] private Transform playerPosition;
    [SerializeField] private IntVariable MaxTargets;
    [SerializeField] private GameObject[] objectsToEnable;

    //public float offset = 6f;

    public void OpenEndZone(int targetNum)
    {
        if (targetNum < MaxTargets.Value)
            return;

        //var playerPos = playerPosition.position;
        //playerPos.y += offset;
        ///transform.position = playerPos;
        
        
        foreach (var item in objectsToEnable)
        {
            item.SetActive(true);
        }
    }

}
