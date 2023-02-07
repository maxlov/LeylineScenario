using System.Collections.Generic;
using System.Linq;
using UnityAtoms.BaseAtoms;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private FloatVariable gameTime;
    [SerializeField] private IntVariable targetsReached;
    [SerializeField] private IntVariable targetCount;
    [SerializeField] private GameObjectVariable currentTarget;

    public List<Objective> Targets;

    private void Start()
    {
        foreach (var objective in Targets)
            objective.manager = this;

        targetCount.Value = Targets.Count;
        
        currentTarget.Value = Targets.First().gameObject;
        Targets.Remove(Targets.First());
        currentTarget.Value.SetActive(true);
    }

    private void Update()
    {
        if (Targets.Count == 0)
            return;
        gameTime.Add(Time.deltaTime);
    }

    public void OnTargetReached()
    {
        targetsReached.Value += 1;

        if (Targets.Count == 0)
        {
            Debug.Log($"Win! Time: {gameTime}");
            return;
        }
        
        currentTarget.Value = Targets.First().gameObject;
        currentTarget.Value.gameObject.SetActive(true);
        Targets.Remove(Targets.First());
    }
}
