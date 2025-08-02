using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DespawnScript : MonoBehaviour
{
    [SerializeField] private float _speed;
    [SerializeField] private float _lifetime;
    [SerializeField, Range(0f, 1f)] private float _fadeIn = 0.8f;
    private float _currentLifetime;
    private Vector2 _movementDirection;

    private void Start()
    {
        _currentLifetime = _lifetime;
        Debug.Log("OffsetSpawned");
        Vector3 _boatPosition = GameObject.FindGameObjectWithTag("Boat").transform.position;
        Vector2 direction = (_boatPosition - transform.position).normalized;
        /*transform.rotation.SetEulerAngles(
            new Vector3(0f, 0f,Mathf.Atan2(direction.y, direction.x)) * 180f / Mathf.PI
            );*/
        Vector3 currentEulerAngles = new Vector3(0,0,Mathf.Atan2(direction.y,direction.x) * 180f / Mathf.PI);
        _movementDirection = direction;
        transform.eulerAngles = currentEulerAngles;
    }

    void Update()
    {
        _currentLifetime -= Time.deltaTime;
        if (_currentLifetime < 0)
        {
            Destroy(gameObject);
            return;
        }
        if( _currentLifetime < _lifetime*_fadeIn ) {
            GetComponent<SpriteRenderer>().color = new Color(1, 1, 1,
                1 - Mathf.Abs(_currentLifetime - _lifetime * _fadeIn) / (_lifetime * _currentLifetime));
        }
        else
        {
            GetComponent<SpriteRenderer>().color = new Color(1, 1, 1,
                1 - Mathf.Abs(_currentLifetime - _lifetime * _fadeIn));
        }

        Vector2 movement = _movementDirection * _speed * Time.deltaTime;
        transform.position += new Vector3(movement.x, movement.y, 0);
    }
}
