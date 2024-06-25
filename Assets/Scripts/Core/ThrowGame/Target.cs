using System;
using UnityEngine;

public class Target : MonoBehaviour
{
    [SerializeField] int _health;
    [SerializeField] int _reward;
    [SerializeField] AudioClip _destroyTargetClip;
    [SerializeField] ParticleSystem _particleSystem;

    public event Action OnDie;

    public void TakeDamage()
    {
        if (!FindAnyObjectByType<DragAndThrowGameSystem>().EndGame)
        {
            _health--;
            if (_health <= 0)
            {
                Die();
            }
        }

    }

    public int GetHealth()
    {
        return _health;
    }

    private void Die()
    {
        AudioManager.Instance.PlayOneShotSound(_destroyTargetClip);
        GameObject particle = Instantiate(_particleSystem.gameObject, transform.position, transform.rotation);
        Destroy(particle, 0.5f);
        FindAnyObjectByType<ScoreSystem>().AddScore(_reward);
        OnDie?.Invoke();
        Destroy(gameObject);
    }

}
