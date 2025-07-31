using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishSpawner : MonoBehaviour
{
    [SerializeField] private GameObject _fishToSpawn;
    [SerializeField] private uint _maxFishToSpawn;
    [SerializeField] private float _spawnTryDelay;
    [SerializeField] private float _spawnTryChance;
    [SerializeField] private float _spawnRadious;
    [SerializeField] private float _fishWanderRadious;
    [SerializeField] private float _minimumDistance;
    private float _spawnTryDelayCounter;
    [HideInInspector]internal List<GameObject> _spawnedFish;
    private GameObject _boat;


    void Start()
    {
        _boat = GameObject.FindGameObjectWithTag("Boat");
        _spawnTryDelayCounter = 0;
        _spawnedFish = new List<GameObject>();
    }

    void TrySpawning()
    {
        _spawnTryDelayCounter += _spawnTryDelay;
        float randomNumber = Random.value;
        if (randomNumber < _spawnTryChance)
        {
            Vector2 positionOffset = Random.insideUnitCircle * _spawnRadious;
            //TODO potentially make it circle
            FishScript newFish = Instantiate(_fishToSpawn,
                transform.position + new Vector3(
                    positionOffset.x,
                    positionOffset.y,
                    0),
                Quaternion.identity
                ).GetComponent<FishScript>();
            newFish.DistanceFromCenter = _fishWanderRadious;
            newFish.Center = transform.position;
            _spawnedFish.Add(newFish.gameObject);
            newFish._fishSpawner = this;
        }
        Debug.Log("amogus");
    }

    void Update()
    {
        if(_spawnTryDelayCounter > 0) _spawnTryDelayCounter -= Time.deltaTime;
        if(_spawnedFish.Count < _maxFishToSpawn &&
            _spawnTryDelayCounter <= 0 &&
            (_boat.transform.position - transform.position).magnitude > _minimumDistance) 
            TrySpawning();
    }
}
