using System;
using UnityEngine;

public class Target : MonoBehaviour
{
    [SerializeField] int _reward;
    [SerializeField] AudioClip _destroyTargetClip;
    [SerializeField] ParticleSystem _particleSystem;

    public event Action OnDie;

    private void OnDestroy()
    {
        if (!FindAnyObjectByType<DragAndThrow>().EndGame && !FindAnyObjectByType<DragAndThrow>().PauseGame)
        {
            AudioManager.Instance.PlayOneShotSound(_destroyTargetClip);
            GameObject particle = Instantiate(_particleSystem.gameObject, transform.position, transform.rotation);
            Destroy(particle, 0.5f);
            FindAnyObjectByType<ScoreSystem>().AddScore(_reward);
            OnDie?.Invoke();
        }
    }
}
