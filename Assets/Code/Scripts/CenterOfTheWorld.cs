using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CenterOfTheWorld : MonoBehaviour
{
    [SerializeField] private float _maxposition = 200f;
    [SerializeField] private GameObject _worldBorder;
    [SerializeField] private GameObject _boat;
    [SerializeField] private float _waveOffset = 5f;
    [SerializeField] private float _spawnDelay = 1f;
    private float _currentDelay;

    public GameObject WorldBorder { get => _worldBorder; set => _worldBorder = value; }
    protected float Maxposition { get => _maxposition; set => _maxposition = value; }

    private void Start()
    {
        _currentDelay = 0f;
        _boat = GameObject.FindGameObjectWithTag("Boat");
    }

    private void FixedUpdate()
    {

        if ((_boat.transform.position - transform.position).magnitude > _maxposition)
        {
            if(_currentDelay < 0f)
            {
                Vector3 waveOffset = _waveOffset * (_boat.transform.position - transform.position).normalized;
                Vector3 wavePosition = _boat.transform.position + waveOffset;
                Instantiate(_worldBorder, wavePosition, Quaternion.identity);
                _currentDelay = _spawnDelay;
            }
            else
            {
                _currentDelay -= Time.fixedDeltaTime;
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(transform.position,Maxposition);
    }
}
