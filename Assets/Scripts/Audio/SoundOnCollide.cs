using UnityEngine;

public class SoundOnCollide : MonoBehaviour
{
    [SerializeField] private SoundEffectSO collideSFX;

    private void OnCollisionEnter(Collision collision)
    {
        collideSFX.Play();
    }
}
