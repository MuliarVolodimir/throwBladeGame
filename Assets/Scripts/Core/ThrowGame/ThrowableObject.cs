using System;
using UnityEngine;

public class ThrowableObject : MonoBehaviour
{
    [SerializeField] LayerMask _layerMask;
    [SerializeField] float _gravity = -4.5f;
    [SerializeField] float _timeToDestroy;
    private float _fallThreshold = 0.1f;

    private Vector2 _direction;
    private Vector2 _velocity;

    private float _speed;
    private bool _isMoving = false;

    public event Action OnDie;

    private void OnDestroy()
    {
        OnDie?.Invoke();   
    }

    void Update()
    {
        if (_isMoving)
        {
            _velocity.y += _gravity * Time.deltaTime;
            Vector2 newPosition = (Vector2)transform.position + _velocity * Time.deltaTime;

            RaycastHit2D hit = Physics2D.Raycast(transform.position, transform.forward, _speed * Time.deltaTime, _layerMask);
            if (hit.collider != null)
            {
                Destroy(gameObject);
                hit.collider.GetComponent<Target>().TakeDamage(); 
                return;
            }

            Debug.DrawRay(transform.position, _direction, Color.red);

            transform.position = newPosition;
            if (_velocity.magnitude < _fallThreshold)
            {
                _direction = Vector2.zero;
            }

            if (_velocity != Vector2.zero)
            {
                RotateTowardsDirection(_velocity);
            }
        }
    }

    public void SetDirection(Vector2 direction, float speed)
    {
        Destroy(gameObject, _timeToDestroy);

        _direction = direction.normalized;
        _speed = speed;
        _velocity = _direction * _speed;
        _isMoving = true;

        RotateTowardsDirection(_direction);
    }

    public void Aim(Vector2 direction)
    {
        RotateTowardsDirection(direction);
    }

    private void RotateTowardsDirection(Vector2 direction)
    {
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
    }

    public void Stop()
    {
        _isMoving = false;
        _speed = 0;
    }
}
