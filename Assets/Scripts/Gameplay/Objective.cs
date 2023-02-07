using UnityEngine;

public class Objective : MonoBehaviour
{
    [HideInInspector] public GameManager manager;
    [SerializeField] private SoundEffectSO closeSFX;

    private void Awake()
    {
        gameObject.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        
        closeSFX.Play();
        manager.OnTargetReached();
        gameObject.SetActive(false);
    }
}
