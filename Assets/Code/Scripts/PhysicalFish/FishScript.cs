using System;
using System.Collections.Generic;
using UnityEngine;

public class FishScript : MonoBehaviour
{
    private MeshFilter _meshFilter;
    [SerializeField] private float _radious = 1f;
    [SerializeField] private int _headDots = 6;
    [SerializeField] private int _bodyDots = 1;
    [SerializeField] private float _bodyLength = 1f;
    [SerializeField] private float _frontFlatten = 1f;
    [SerializeField] private float _sideFlatten = 1f;
    [SerializeField] private bool _rotate = false;
    [SerializeField] private Vector2 _size = new Vector2(0.05f, 0.05f);

    [SerializeField] private float _maxVelocity = 0.1f;
    [SerializeField] private float _maxAcceleration = 0.1f;

    private Vector2 _center = new Vector2(0f,0f);
    private float _distanceFromCenter = 1f;
    private Vector2 _destination = new Vector2(0f,0f);
    private Vector2 _velocity = new Vector2(0f,0f);
    private float _colidingTimer = 0.0f;
    [HideInInspector]internal FishSpawner _fishSpawner;
    [SerializeField] private Color _color = new Color(45f/255f, 47f/255f, 103f/255f);
    [Header("Value and stuff")]
    [SerializeField] private NewFishData _fishData;

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
    private void RecalculateGoal()
    {
        _destination = _center + UnityEngine.Random.insideUnitCircle * _distanceFromCenter;
    }
    private void EnforceConstraints()
    {
        if (_radious < 0.01) _radious = 0.01f;
        if (_headDots < 1) _headDots = 1;
        if (_bodyDots < 3) _bodyDots = 3;
        if (_frontFlatten < 0.1) _frontFlatten = 0.1f;
    }
    private float CalculateVidth(int currentElement, int maxElements, float width)
    {
        maxElements -= 1;
        return width * 
            ((1 - (currentElement * currentElement) + maxElements * maxElements)) 
            / (maxElements * maxElements);
    }
    private void CalculateMesh()
    {
        EnforceConstraints();
        _meshFilter = GetComponent<MeshFilter>();
        Mesh mesh = new Mesh();
        _meshFilter.mesh = mesh;

        List<Vector3> vertices = new List<Vector3>();
        vertices.Add(Vector3.zero);
        for (int i = 0; i < (_headDots + 1); ++i)
        {
            vertices.Add(
                new Vector3(
                    Mathf.Sin((float)(i / (float)_headDots * Math.PI)) * _radious * _sideFlatten,
                    Mathf.Cos((float)(i / (float)_headDots * Math.PI)) * _radious * _frontFlatten,
                    0)
                );
        }

        List<int> triangles = new List<int>();
        for (int i = 0; i < _headDots; ++i)
        {
            triangles.Add(0);
            triangles.Add(i + 1);
            triangles.Add(i + 2);
        }
        //new current dot id = headDots+1
        int currentDot = _headDots + 2;

        int previousDots = 0;

        for (int i = 0; i < _bodyDots; ++i)
        {
            if (i == 0)
            {
                vertices.Add(new Vector3(
                    -_bodyLength * (i + 1),
                    -CalculateVidth(i, _bodyDots, _radious),
                    0
                ));

                vertices.Add(new Vector3(
                    -_bodyLength * (i + 1),
                    0,
                    0
                ));
                vertices.Add(new Vector3(
                    -_bodyLength * (i + 1),
                    CalculateVidth(i, _bodyDots, _radious),
                    0
                ));

                //Debug.Log(currentDot + " " + (_headDots + 1) + " " + 0);
                //1
                triangles.Add(0);
                triangles.Add(_headDots + 1);
                triangles.Add(currentDot);
                //2
                //triangles.Add(0);
                //triangles.Add(currentDot);
                //triangles.Add(currentDot + 1);
                //3
                triangles.Add(0);
                triangles.Add(currentDot);
                triangles.Add(currentDot + 2);
                //4
                triangles.Add(0);
                triangles.Add(currentDot + 2);
                triangles.Add(1);

                previousDots = currentDot;
                currentDot = vertices.Count;
            }
            else
            {
                vertices.Add(new Vector3(
                    -_bodyLength * (i + 1),
                    -CalculateVidth(i, _bodyDots, _radious),
                    0
                ));

                vertices.Add(new Vector3(
                    -_bodyLength * (i + 1),
                    0,
                    0
                ));
                vertices.Add(new Vector3(
                    -_bodyLength * (i + 1),
                    CalculateVidth(i, _bodyDots, _radious),
                    0
                ));
                //1
                triangles.Add(previousDots + 1); //center
                triangles.Add(previousDots);
                triangles.Add(currentDot);
                //2
                triangles.Add(previousDots + 1);
                triangles.Add(currentDot);
                triangles.Add(currentDot + 1);
                //3
                triangles.Add(previousDots + 1);
                triangles.Add(currentDot + 1);
                triangles.Add(currentDot + 2);
                //4
                triangles.Add(previousDots + 1);
                triangles.Add(currentDot + 2);
                triangles.Add(previousDots + 2);
                previousDots = currentDot;
                currentDot += 3;
            }
        }


        for(int i = 0; i < vertices.Count; i++)
        {
            if (_rotate)
            {
                vertices[i] = new Vector3(
                    -vertices[i].x * _size.x,
                    vertices[i].y * _size.y,
                    vertices[i].z);
            }
            else {
                vertices[i] = new Vector3(
                    vertices[i].x * _size.x,
                    vertices[i].y * _size.y,
                    vertices[i].z);
            }
        }


        mesh.vertices = vertices.ToArray();
        mesh.triangles = triangles.ToArray();
        mesh.RecalculateNormals();
    }

    private void MoveTowards(Vector2 direction)
    {
        Vector2 normalised = direction.normalized;
        //transform.root.LookAt(normalised);
        transform.rotation = Quaternion.Euler(0, 0, Mathf.Atan2(_velocity.y, _velocity.x) * 180f / Mathf.PI);
        _velocity += normalised * _maxAcceleration * Time.fixedDeltaTime;
        float speedLimit = _velocity.magnitude / _maxAcceleration;
        if (speedLimit > 1.0f)
        {
            _velocity /= speedLimit;
        }
    }

    void Awake()
    {
        Material mat = GetComponent<MeshRenderer>().material;
        mat = new Material(mat);
        mat.color = _color;
        CalculateMesh();
    }

    private void Start()
    {
        GetComponent<MeshRenderer>().material.color = _color;
    }

    private void FixedUpdate()
    {
        if (_colidingTimer > 0) _colidingTimer -= Time.fixedDeltaTime;
        if(_colidingTimer <= 0)
        {
            Vector2 direction = _destination - (Vector2)transform.position;
            if (direction.magnitude < 0.2f) RecalculateGoal();
            else MoveTowards(direction);
            transform.position += new Vector3(_velocity.x, _velocity.y, 0) * Time.fixedDeltaTime;
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if(collision == null) return;
        if (collision.gameObject.tag == "Buoy")
        {
            _colidingTimer = 0.2f;
        }
    }

    internal void Catch()
    {
        // Logic for catching the fish
        Debug.Log("Fish caught: " + gameObject.name);

        _fishSpawner._spawnedFish.Remove(gameObject); // Remove the fish from the spawner's list
        GameObject.FindGameObjectWithTag("EquipementManager").GetComponent<EquipementScript>().AddFishData(_fishData); // Add fish data to the equipment manager

        // You can add additional logic here, such as playing an animation or sound
        Destroy(gameObject); // Destroy the fish object after catching it
    }
}
