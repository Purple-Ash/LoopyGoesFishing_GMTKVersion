using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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

    void calculateMesh()
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

        if (_rotate)
        {
            for(int i = 0; i < triangles.Count; i++)
            {
                vertices[i] = new Vector3(
                    vertices[i].x,
                    vertices[i].y,
                    vertices[i].z);
            }
        }

        mesh.vertices = vertices.ToArray();
        mesh.triangles = triangles.ToArray();
        mesh.RecalculateNormals();
    }

    void Awake()
    {
        calculateMesh();
    }

    void Update()
    {
    }
}
