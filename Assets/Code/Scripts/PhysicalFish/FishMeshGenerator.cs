using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEditor.Searcher.SearcherWindow.Alignment;

[System.Serializable]
public class MeshConfig
{
    [SerializeField] private float radious;
    [SerializeField] private int headDots;
    [SerializeField] private int bodyDots;
    [SerializeField] private float bodyLength;
    [SerializeField] private float frontFlatten;
    [SerializeField] private float sideFlatten;
    [SerializeField] private bool rotate;
    [SerializeField] private Vector2 size;

    private MeshConfig() { }
    public MeshConfig(float radious, int headDots, int bodyDots, float bodyLength, float frontFlatten, float sideFlatten, bool rotate, Vector2 size)
    {
        Radious = radious;
        HeadDots = headDots;
        BodyDots = bodyDots;
        BodyLength = bodyLength;
        FrontFlatten = frontFlatten;
        SideFlatten = sideFlatten;
        Rotate = rotate;
        Size = size;
    }
    public float Radious { get => radious; set => radious = value; }
    public int HeadDots { get => headDots; set => headDots = value; }
    public int BodyDots { get => bodyDots; set => bodyDots = value; }
    public float BodyLength { get => bodyLength; set => bodyLength = value; }
    public float FrontFlatten { get => frontFlatten; set => frontFlatten = value; }
    public float SideFlatten { get => sideFlatten; set => sideFlatten = value; }
    public bool Rotate { get => rotate; set => rotate = value; }
    public Vector2 Size { get => size; set => size = value; }
}

public class FishMeshGenerator
{
    private List<Vector3> vertices = new List<Vector3>();
    private List<int> triangles = new List<int>();
    private MeshConfig config;

    public FishMeshGenerator(MeshConfig config)
    {
        this.config = config;
    }

    private void createHead(out int headTop, out int headBottom, out int headCenter)
    {
        vertices.Add(Vector3.zero);
        headCenter = 0;
        headTop = 1;
        headBottom = config.HeadDots + 1;
        for (int i = 0; i < (config.HeadDots + 1); ++i)
        {
            vertices.Add(
                new Vector3(
                    Mathf.Sin((float)(i / (float)config.HeadDots * Mathf.PI)) * config.Radious * config.SideFlatten,
                    Mathf.Cos((float)(i / (float)config.HeadDots * Mathf.PI)) * config.Radious * config.FrontFlatten,
                    0)
                );
        }
        for (int i = 0; i < config.HeadDots; ++i)
        {
            triangles.Add(0);
            triangles.Add(i + 1);
            triangles.Add(i + 2);
        }
    }

    public Mesh calculateMesh(Mesh mesh)
    {
        createHead(out int headTop,out int headBottom,out int headCenter);

        //new current dot id = headDots+1
        
        int currentDot = config.HeadDots + 2;

        int previousDots = 0;

        for (int i = 0; i < config.BodyDots; ++i)
        {
            if (i == 0)
            {
                vertices.Add(new Vector3(
                    -config.BodyLength * (i + 1),
                    -CalculateVidth(i, config.BodyDots, config.Radious),
                    0
                ));

                vertices.Add(new Vector3(
                    -config.BodyLength * (i + 1),
                    0,
                    0
                ));
                vertices.Add(new Vector3(
                    -config.BodyLength * (i + 1),
                    CalculateVidth(i, config.BodyDots, config.Radious),
                    0
                ));

                //Debug.Log(currentDot + " " + (config.HeadDots + 1) + " " + 0);
                //1
                triangles.Add(0);
                triangles.Add(config.HeadDots + 1);
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
                    -config.BodyLength * (i + 1),
                    -CalculateVidth(i, config.BodyDots, config.Radious),
                    0
                ));

                vertices.Add(new Vector3(
                    -config.BodyLength * (i + 1),
                    0,
                    0
                ));
                vertices.Add(new Vector3(
                    -config.BodyLength * (i + 1),
                    CalculateVidth(i, config.BodyDots, config.Radious),
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


        for (int i = 0; i < vertices.Count; i++)
        {
            if (config.Rotate)
            {
                vertices[i] = new Vector3(
                    -vertices[i].x * config.Size.x,
                    vertices[i].y * config.Size.y,
                    vertices[i].z);
            }
            else
            {
                vertices[i] = new Vector3(
                    vertices[i].x * config.Size.x,
                    vertices[i].y * config.Size.y,
                    vertices[i].z);
            }
        }
        

        mesh.vertices = vertices.ToArray();
        mesh.triangles = triangles.ToArray();
        return mesh;
    }

    private float CalculateVidth(int currentElement, int maxElements, float width)
    {
        maxElements -= 1;
        return width *
            ((1 - (currentElement * currentElement) + maxElements * maxElements))
            / (maxElements * maxElements);
    }
}
