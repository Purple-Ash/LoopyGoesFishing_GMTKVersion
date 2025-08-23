using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.UI;

public class FishScript : MonoBehaviour
{
    private MeshFilter _meshFilter;
    private Vector2 _center = new Vector2(0f, 0f);
    private float _distanceFromCenter = 1f;
    private Vector2 _destination = new Vector2(0f, 0f);
    protected Vector2 _velocity = new Vector2(0f, 0f);
    private float _colidingTimer = 0.0f;
    private GameObject boat;
    private CenterOfTheWorld _centerOfTheWorld;
    [HideInInspector] internal FishSpawner _fishSpawner;

    [Header("Shape")]
    [SerializeField] private MeshConfig config;

    [Header("Movement")]
    [SerializeField] protected float _maxVelocity = 0.1f;
    [SerializeField] protected float _maxAcceleration = 0.1f;
    [SerializeField] protected float _skedaddleVelocity = 0.2f;
    [SerializeField] protected float _skedaddleAcceleration = 0.5f;
    [SerializeField] protected float _goalProximity = 0.2f;

    [Header("Skedaddle")]
    [SerializeField] private float _skedaddleRange;

    [Header("Value and stuff")]
    [SerializeField] private NewFishData _fishData;
    [SerializeField] private float _despawnDistance = 70f;

    [Header("Notification Objects")]
    [SerializeField] private GameObject _notification;
    [SerializeField] private Vector2 _notificationScale = new Vector2(1, 1);

    public Vector2 Center 
    {
        get => _center;
        set 
        {
            _center = value;
            RecalculateGoal();
        } 
    }

    public float DistanceFromCenter {
        get => _distanceFromCenter;
        set
        {
            _distanceFromCenter = value;
            RecalculateGoal();
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
    private void RecalculateGoal()
    {
        do
        {
            _destination = _center + UnityEngine.Random.insideUnitCircle * _distanceFromCenter;
        } while(IsPointOnIsland(_destination));
    }
    private void EnforceConstraints()
    {
        if (config.Radious < 0.01) config.Radious = 0.01f;
        if (config.HeadDots < 1) config.HeadDots = 1;
        if (config.BodyDots < 3) config.BodyDots = 3;
        if (config.FrontFlatten < 0.1) config.FrontFlatten = 0.1f;
    }
    private void CalculateMesh()
    {
        EnforceConstraints();
        _meshFilter = GetComponent<MeshFilter>();
        Mesh mesh = new Mesh();
        FishMeshGenerator fishMeshGenerator = new FishMeshGenerator(config);
        _meshFilter.mesh = fishMeshGenerator.calculateMesh(mesh); 

        mesh.RecalculateNormals();
    }

    protected virtual void MoveTowards(Vector2 direction, float speedMultiplier)
    {
        Vector2 normalised;
        Vector2 acceleration;
        Vector2 boatTransformPosition = new Vector2(
                boat.transform.position.x,
                boat.transform.position.y
            );
        Vector2 thisPosition = new Vector2(
                transform.position.x,
                transform.position.y
            );
        float actualMaxVelocity = 1;
        if ((boatTransformPosition - thisPosition).magnitude < _skedaddleRange)
        {
            Vector2 boatDirection = thisPosition - boatTransformPosition;
            normalised = boatDirection.normalized;
            acceleration = normalised * _skedaddleAcceleration * Time.fixedDeltaTime * 1/boatDirection.magnitude * _skedaddleRange;
            actualMaxVelocity = _skedaddleVelocity;
        }
        else
        {
            normalised = direction.normalized;
            acceleration = normalised * _maxAcceleration * Time.fixedDeltaTime;
            //transform.root.LookAt(normalised);
            actualMaxVelocity = _maxVelocity;
        }
        transform.rotation = Quaternion.Euler(0, 0, Mathf.Atan2(_velocity.y, _velocity.x) * 180f / Mathf.PI);
        _velocity += acceleration * speedMultiplier;
        float speedLimit = _velocity.magnitude / actualMaxVelocity;
        if (speedLimit > 1.0f)
        {
            _velocity /= speedLimit;
        }
    }

    void Awake()
    {
        Material mat = GetComponent<MeshRenderer>().material;
        mat = new Material(mat);
        //mat.color = _color;
        CalculateMesh();
    }

    protected void Start()
    {
        _centerOfTheWorld = GameObject.FindGameObjectWithTag("Center").GetComponent<CenterOfTheWorld>();
        boat = GameObject.FindGameObjectWithTag("Boat");
        //GetComponent<MeshRenderer>().material.color = _color;
    }

    private void FixedUpdate()
    {
        _velocity = GetComponent<Rigidbody2D>().velocity;
        float speedMultiplier = 0;
        if (_colidingTimer > 0)
        {
            _colidingTimer -= Time.fixedDeltaTime;
            speedMultiplier = 1f;
        }
        if(_colidingTimer <= 0)
        {
            speedMultiplier = 1f;
        }
        Vector2 direction = _destination - (Vector2)transform.position;
        if (direction.magnitude < _goalProximity) RecalculateGoal();
        else MoveTowards(direction, speedMultiplier);
        GetComponent<Rigidbody2D>().velocity = _velocity;
    }

    void RemoveIfFar()
    {

    }

    private void Update()
    {
        if((boat.transform.position - transform.position).magnitude > _despawnDistance)
        {
            Destroy(gameObject);
        }
    }

    protected virtual void OnCollisionStay2D(Collision2D collision)
    {
        if(collision == null) return;
        if (collision.gameObject.tag == "Buoy")
        {
            _colidingTimer = 0.3f;
        }
    }

    private void OnValidate()
    {
        CalculateMesh();
    }

    internal void Catch()
    {
        if (!GameObject.FindGameObjectWithTag("EquipementManager").GetComponent<EquipementScript>().AddFishData(_fishData))
        {
            Debug.LogWarning("Failed to catch fish: " + _fishData.name + ". Equipment is full or fish data already exists.");
            return;
        }
        else
        {
            // Logic for catching the fish
            //Debug.Log("Fish caught: " + gameObject.name);

            // You can add additional logic here, such as playing an animation or sound
            GameObject notification = Instantiate(_notification, transform.position, Quaternion.identity);
            notification.GetComponent<SpriteRenderer>().sprite = _fishData.image;
            notification.transform.position = new Vector3(
                notification.transform.position.x,
                notification.transform.position.y,
                0);
            notification.transform.localScale *= _notificationScale;

            Destroy(gameObject); // Destroy the fish object after catching it
        }
    }

    private void OnDestroy()
    {
        _fishSpawner._spawnedFish.Remove(gameObject); // Remove the fish from the spawner's list
                                                      // Add fish data to the equipment manager
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, _skedaddleRange);
    }
}
