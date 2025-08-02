using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishSpawner : MonoBehaviour
{
    [SerializeField] private Camera _camera;

    [SerializeField] private GameObject _fishToSpawn;
    [SerializeField] private uint _maxFishToSpawn;
    [SerializeField] private float _spawnTryDelay;
    [SerializeField] private float _spawnTryChance;
    [SerializeField] private float _minSpawnRadious;
    [SerializeField] private float _fishWanderRadious;
    [SerializeField] private float _minimumDistance;
    [SerializeField] private float _maximumDistance;
    [SerializeField] private float _cameraSpawnBuffer = 1f;
    [SerializeField] private int _instaSpawn = 0;

    [SerializeField] private string _gizmoName;
    private float _spawnTryDelayCounter;
    [HideInInspector]internal List<GameObject> _spawnedFish;
    private GameObject _boat;


    void Start()
    {
        _camera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        _boat = GameObject.FindGameObjectWithTag("Boat");
        _spawnTryDelayCounter = 0;
        _spawnedFish = new List<GameObject>();
        while(_instaSpawn > 0)
        {
            TrySpawning();
        }
    }

    bool IsPointOnIsland(Vector2 point)
    {
        var hits = Physics2D.OverlapPointAll(point);
        foreach (var hit in hits)
        {
            if (hit.CompareTag("Island")) return true;
        }
        return false;
    }
    
    bool IsPointOnCamera(Vector2 point)
    {
        if(_instaSpawn > 0) return false;
        float height = _camera.orthographicSize;
        float aspektRatio = _camera.aspect;
        float width = aspektRatio * height;
        Vector2 leftBottom = new Vector2(
            _camera.transform.position.x - width - _cameraSpawnBuffer,
            _camera.transform.position.y - height - _cameraSpawnBuffer);
        Vector2 rightTop = new Vector2(
            _camera.transform.position.x + width + _cameraSpawnBuffer,
            _camera.transform.position.y + height + _cameraSpawnBuffer);
        if(point.x > leftBottom.x &&
            point.y > leftBottom.y &&
            point.x < rightTop.x &&
            point.y < rightTop.y)
        {
            return true;
        }
        return false;
    }

    void TrySpawning()
    {
        _spawnTryDelayCounter += _spawnTryDelay;
        float randomNumber = Random.value;
        if (randomNumber < _spawnTryChance)
        {
            Vector2 positionOffset;
            Vector2 testPoint;
            int tryToSpawn = 10;
            do
            {
                positionOffset = Random.insideUnitCircle * _minSpawnRadious;
                testPoint = transform.position + new Vector3(
                    positionOffset.x,
                    positionOffset.y,
                    1.5f);
                tryToSpawn--;
                if (tryToSpawn == 0)
                    return;
            } while (IsPointOnIsland(testPoint) || IsPointOnCamera(testPoint));

            //TODO potentially make it circle
            FishScript newFish = Instantiate(_fishToSpawn,
                transform.position + new Vector3(
                    positionOffset.x,
                    positionOffset.y,
                    1.5f),
                Quaternion.identity
                ).GetComponent<FishScript>();
            newFish.DistanceFromCenter = _fishWanderRadious;
            newFish.Center = transform.position;
            _spawnedFish.Add(newFish.gameObject);
            newFish._fishSpawner = this;
            _instaSpawn--;
        }
        //Debug.Log("amogus");
    }

    void Update()
    {
        if(_spawnTryDelayCounter > 0) _spawnTryDelayCounter -= Time.deltaTime;
        if(_spawnedFish.Count < _maxFishToSpawn &&
            _spawnTryDelayCounter <= 0 &&
            (_boat.transform.position - transform.position).magnitude > _minimumDistance &&
            (_boat.transform.position - transform.position).magnitude < _maximumDistance) 
            TrySpawning();
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, _minSpawnRadious);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, _fishWanderRadious);
        Gizmos.DrawIcon(transform.position, _gizmoName);
        /*
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, _minimumDistance);
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, _maximumDistance);*/
    }
}
