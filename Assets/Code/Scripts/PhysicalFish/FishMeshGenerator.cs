using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static MeshConfig;


[System.Serializable]
public class MeshConfig
{
    public enum Archetype
    {
        Flat,
        Linear,
        Exponential,
        Legacy
    }

    public enum FinShape
    {
        Standard,
        Sharp,
    }

    [Header("General")]
    [SerializeField] public float radious;
    [SerializeField] public Vector2 translate;
    [SerializeField] public Vector2 size;
    [SerializeField] public float rotate;

    [Header("Body")]
    [SerializeField] public int bodyResolution;
    [SerializeField] public float bodyLength;
    [SerializeField] public Archetype bodyArchetype;
    [SerializeField, Range(0f, 10f)] public float exponent;


    [Header("Head")]
    [SerializeField] public int headResolution;
    [SerializeField] public float multiplyX;
    [SerializeField] public float multiplyY;

    [Header("Fins")]
    [SerializeField] public int finAmount;
    [SerializeField] public List<Vector2> finPositions;
    [SerializeField] public List<FinShape> finArchetypes;
    [SerializeField] public List<float> finSize;
    [SerializeField] public List<Vector2> finOffset;
    [Header("FinsExtra (may not works for some archetypes)")]
    [SerializeField] public List<float> finFlattenX;
    [SerializeField] public List<float> finFlattenY;



    private MeshConfig() { }
    public MeshConfig(
        float radious,
        Vector2 translate,
        Vector2 size,
        float rotate,
        int bodyResolution,
        float bodyLength,
        Archetype bodyArchetype,
        float exponent,
        int headResolution,
        float multiplyX,
        float multiplyY)
    {
        //general
        this.radious = radious;
        this.translate = translate;
        this.size = size;
        this.rotate = rotate;


        //body
        this.bodyResolution = bodyResolution;
        this.bodyLength = bodyLength;
        this.bodyArchetype = bodyArchetype;
        this.exponent = exponent;

        //head
        this.headResolution = headResolution;
        this.multiplyX = multiplyX;
        this.multiplyY = multiplyY;
    }
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

    private int CreateHead(out int headTop, out int headBottom, out int headCenter)
    {
        headCenter = AddVertex(Vector3.zero);
        headTop = -1;
        headBottom = -1;
        for (int i = 0; i <= config.headResolution; ++i)
        {
            int v = AddVertex(
                Mathf.Sin((float)(i / (float)config.headResolution * Mathf.PI)) * config.radious * config.multiplyX,
                Mathf.Cos((float)(i / (float)config.headResolution * Mathf.PI)) * config.radious * config.multiplyY);
            if (i == 0) headTop = v;
            else if (i == config.headResolution) headBottom = v;
        }
        for (int i = 0; i < config.headResolution; ++i)
        {
            AddTriangle(0, i + 1, i + 2);
        }
        return vertices.Count;
    }

    private int CreateBody(int headTop, int headBottom, out int previousTop, out int previousBottom)
    {
        previousTop = headTop;
        previousBottom = headBottom;
        for (int i = 0; i <= config.bodyResolution; i++)
        {
            float width = CalculateBodyWidth(i, config.bodyResolution, config.radious);
            float length = CalculateBodyLength(i, config.bodyResolution, config.bodyLength);

            int top = AddVertex(-length, -width);
            int bottom = AddVertex(-length, width);
            AddTriangle(top, bottom, previousTop);
            AddTriangle(previousTop, previousBottom, bottom);

            previousTop = top;
            previousBottom = bottom;
        }
        return vertices.Count;
    }

    private int CalculateTail(int top, int bottom)
    {
        return 1;
    }

    private int CreateFins()
    {
        for(int i = 0; i < config.finAmount; i++)
        {
            CreateFin(config.finPositions[i], config.finArchetypes[i], config.finSize[i], config.finOffset[i], config.finFlattenX[i], config.finFlattenY[i], true);
            CreateFin(config.finPositions[i], config.finArchetypes[i], config.finSize[i], config.finOffset[i], config.finFlattenX[i], config.finFlattenY[i], false);
        }
        return vertices.Count;
    }

    private int CreateFin(Vector2 positions, FinShape finShape, float finSize, Vector2 finOffset, float finFlattenX, float finFlattenY, bool top)
    {
        int sign = top ? 1 : -1;
        switch (finShape) {
            case FinShape.Sharp:
                SharpFin(positions, finSize, finOffset, sign);
                break;
            case FinShape.Standard:
                StandardFin(positions, finSize, finOffset, sign, finFlattenX, finFlattenY);
                break;
        }
        return vertices.Count;
    }

    private void SharpFin(Vector2 positions, float finSzie, Vector2 finOffset, int sign)
    {
        int a = AddVertex(
            -positions.x * config.bodyLength,
            sign * CalculateBodyWidth(positions.x, 1, config.radious));
        int b = AddVertex(
            -positions.y * config.bodyLength,
            sign * CalculateBodyWidth(positions.y, 1, config.radious));
        int c = AddVertex(
            -((positions.x + positions.y) / 2 + finOffset.x) * config.bodyLength,
            sign * (CalculateBodyWidth((positions.x + positions.y) / 2, 1, config.radious) * (finSzie + 1) + finOffset.y));
        AddTriangle(a, b, c);
    }

    private void StandardFin(Vector2 positions, float finSzie, Vector2 finOffset, int sign, float finflattenX, float finflattenY)
    {
        int a = AddVertex(
            -positions.x * config.bodyLength,
            sign * CalculateBodyWidth(positions.x, 1, config.radious));
        int b = AddVertex(
            -positions.y * config.bodyLength,
            sign * CalculateBodyWidth(positions.y, 1, config.radious));

        int startPos = vertices.Count;
        generateCircle(10,
            new Vector2(
                 -((positions.x + positions.y) / 2 + finOffset.x) * config.bodyLength,
                sign * (CalculateBodyWidth((positions.x + positions.y) / 2, 1, config.radious) * (finSzie + 1)/2 + finOffset.y)
                ),
            finSzie/2,
            new Vector2(finflattenX, finflattenY)
            );

        //create circle triangles
        for (int i = 0; i < 9; i++)
        {
            AddTriangle(startPos, startPos + 1 + i, startPos + 2 + i);
            AddTriangle(a, b, startPos + 1 + i);
        }
        AddTriangle(startPos, startPos + 10, startPos + 1);
        AddTriangle(a, b, startPos + 10);

    }

    private int generateCircle(float count, Vector2 center, float radious, Vector2 finFlatten)
    {
        AddVertex(center.x, center.y);
        for (float i = 0; i < count; i++)
            AddVertex(
                Mathf.Sin((float)(i / count * 2 * Mathf.PI)) * radious * finFlatten.x + center.x,
                Mathf.Cos((float)(i / count * 2 * Mathf.PI)) * radious * finFlatten.y + center.y);
        return vertices.Count;
    }

    private void ApplyScaleAndRotation()
    {
        for(int i = 0; i < vertices.Count; i++)
        {
            Vector2 newVertex = vertices[i];
            newVertex = (newVertex * config.size) + config.translate;
            vertices[i] = new Vector2(
                    newVertex.x * Mathf.Cos(config.rotate) - newVertex.y * Mathf.Sin(config.rotate),
                    newVertex.x * Mathf.Sin(config.rotate) + newVertex.y * Mathf.Cos(config.rotate)
                );
        }
    }

    public Mesh calculateMesh(Mesh mesh)
    {
        //generation
        CreateHead(out int headTop,out int headBottom,out int headCenter);
        CreateBody(headTop, headBottom, out int top, out int bottom);
        CalculateTail(top, bottom);
        CreateFins();

        //transformation
        ApplyScaleAndRotation();

        mesh.vertices = vertices.ToArray();
        mesh.triangles = triangles.ToArray();
        return mesh;
    }

    private int AddVertex(Vector3 v)
    {
        vertices.Add(new Vector3(v.x, v.y, 0));
        return vertices.Count - 1;
    }

    private int AddVertex(Vector2 v)
    {
        vertices.Add(v);
        return vertices.Count - 1;
    }

    private int AddVertex(float x, float y)
    {
        vertices.Add(new Vector3(x, y, 0));
        return vertices.Count - 1;
    }

    private float CalculateBodyWidth(float i, float maxI, float width)
    {
        switch (config.bodyArchetype){
            case MeshConfig.Archetype.Linear:
                return (1 - i / maxI) * width;
            case MeshConfig.Archetype.Flat:
                return width;
            case MeshConfig.Archetype.Exponential:
                return (1 - Mathf.Pow((i / maxI),config.exponent)) * width;
            case MeshConfig.Archetype.Legacy:
                return width *
                    ((1 - (i * i) + maxI * maxI))
                    / (maxI * maxI);
            default:
                return 1;
        }
    }

    private float CalculateBodyLength(float i, float maxI, float length)
    {
        return i * length / maxI;
    }

    //check if 3 points create triangle in clockwise or counterclockwise direction
    private bool CheckIfClockwise(Vector3 a, Vector3 b, Vector3 c)
    {
        a = new Vector3(a.x, a.y, 0);
        b = new Vector3(b.x, b.y, 0);
        c = new Vector3(c.x, c.y, 0);
        //if cross product is facing the viewer it's clockwise
        Vector3 cross = Vector3.Cross(b - a, c - a);
        return cross.z < 0;
    }

    //creates clockwise triangle based on given indicies
    private void AddTriangle(int a, int b, int c)
    {
        foreach (int i in new int[] { a, b, c })
        {
            if (i < 0) throw new ArgumentException("Index cant be negative");
            if (i >= vertices.Count) throw new ArgumentException("Index outside the bounds of the vertices vector");
        }
        if (!CheckIfClockwise(vertices[a], vertices[b], vertices[c]))
        {
            (b, c) = (c, b);
        }
        triangles.Add(a);
        triangles.Add(b);
        triangles.Add(c);
    }
}
