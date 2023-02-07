using UnityEngine;

public class StartZone : MonoBehaviour
{
    [SerializeField] private GameManager gameManager;
    
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
            gameManager.enabled = true;
    }
}
