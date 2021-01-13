using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(MeshFilter),typeof(MeshRenderer))]
public class DeskTest : MonoBehaviour
{

    MeshFilter meshFilter;
    MeshRenderer meshRenderer;

    public float height = 0.4f;
    public float radius = 1f;
    public int details = 20;

    static float EPS = 0.01f;
    // Start is called before the first frame update
    void Awake()
    {
        meshFilter = transform.GetComponent<MeshFilter>();
        meshRenderer = transform.GetComponent<MeshRenderer>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    [ContextMenu("GeneratePie")]
    public void GeneratePieByRadian()
    {
        var arcs = new List<float>();

        float r = UnityEngine.Random.Range(0.1f, 0.9f);

        r *= 2 * Mathf.PI;

        arcs.Add(0);
        arcs.Add(r);
        GeneratePie(arcs);

    }

    private void GeneratePie(List<float> arcs)
    {
        List<Vector3> verts = new List<Vector3>();
        List<Vector2> uvs = new List<Vector2>();
        List<int> tris = new List<int>();

        List<Vector3> _verts = new List<Vector3>();
        List<Vector2> _uvs = new List<Vector2>();
        List<int> _tris = new List<int>();

        for (int i = 0; i < arcs.Count; i +=2)
        {
            _verts.Clear();
            _uvs.Clear();
            _tris.Clear();

            _verts.Add(new Vector3(0, 0, 0));
            _verts.Add(new Vector3(0, -height, 0));

            AddArcMeshInfo(arcs[i], arcs[i + 1], _verts, _uvs, _tris);

            foreach (int n in _tris)
            {
                tris.Add(n + verts.Count);
            }

            verts.AddRange(_verts);
            uvs.AddRange(_uvs);
        }

        Mesh mesh = new Mesh();

        mesh.vertices = verts.ToArray();
        mesh.triangles = tris.ToArray();
        mesh.uv = uvs.ToArray();

        mesh.RecalculateBounds();
        mesh.RecalculateNormals();
        mesh.RecalculateTangents();
        meshFilter.mesh = mesh;


    }

    private void AddArcMeshInfo(float begin, float end, List<Vector3> verts, List<Vector2> uvs, List<int> tris)
    {
        float eachRad = 2 * Mathf.PI / details;
        float a;
        for ( a = begin; a <= end; a+=eachRad)
        {
            Vector3 v = new Vector3(radius * Mathf.Sin(a), 0, radius * Mathf.Cos(a));
            verts.Add(v);
            Vector3 v2 = new Vector3(radius * Mathf.Sin(a), -height, radius * Mathf.Cos(a));
            verts.Add(v2);
        }
        if (a < end + EPS)
        {
            Vector3 v = new Vector3(radius * Mathf.Sin(end), 0, radius * Mathf.Cos(end));
            verts.Add(v);
            Vector3 v2 = new Vector3(radius * Mathf.Sin(end), -height, radius * Mathf.Cos(end));
            verts.Add(v2);
        }
        // 顶面顶点序号
        int n = verts.Count;
        for (int i = 2; i < n - 2; i += 2)
        {
            tris.Add(i); tris.Add(i + 2); tris.Add(0);
        }

        // 侧面顶点序号
        for (int i = 2; i < n - 2; i += 2)
        {
            tris.Add(i); tris.Add(i + 1); tris.Add(i + 2);
            tris.Add(i + 2); tris.Add(i + 1); tris.Add(i + 3);
        }

        // 封住两个直线边
        tris.Add(2); tris.Add(0); tris.Add(1);
        tris.Add(3); tris.Add(2); tris.Add(1);
        tris.Add(n - 1); tris.Add(0); tris.Add(n - 2);
        tris.Add(1); tris.Add(0); tris.Add(n - 1);
    }
}
    

