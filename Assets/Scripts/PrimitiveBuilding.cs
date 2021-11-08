using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrimitiveBuilding : MonoBehaviour
{
    public Material mat;
    public Vector3[] vertices;
    public int[] triangles;
    int nbSides;
    float r;
    float h;
    Vector3 center;
    public Texture tex;
    public void Initialize(int? nbSides, float r, float h, Vector3 center)
    {
        this.nbSides = nbSides.Value;
        this.r = r;
        this.h = h;
        this.center = center;
    }

    public void BuildPrimitive()
    {
        vertices = new Vector3[nbSides * 2 + 2];
        var pi = Mathf.PI;
        float teta;

        for (int i = 0; i < nbSides * 2; i = i + 2)
        {
            teta = pi * i / nbSides;
            vertices[i] = center + new Vector3(r * Mathf.Cos(teta), h, r * Mathf.Sin(teta));
            vertices[i + 1] = center + new Vector3(r * Mathf.Cos(teta), 0, r * Mathf.Sin(teta));
        }
        vertices[nbSides * 2] = center + new Vector3(0, h, 0);
        vertices[nbSides * 2 + 1] = center;

        triangles = new int[nbSides * 12];
        int ti = 0;
        for (int x = 0, vi = 0; x < nbSides; x++, vi += 2)
        {
            if (x == nbSides - 1)
            {
                triangles[ti] = vi;
                triangles[ti + 1] = vi + 1;
                triangles[ti + 2] = 0;
                triangles[ti + 3] = 0;
                triangles[ti + 4] = vi + 1;
                triangles[ti + 5] = 1;
            }
            else
            {
                triangles[ti] = vi;
                triangles[ti + 1] = vi + 1;
                triangles[ti + 2] = vi + 2;
                triangles[ti + 3] = vi + 2;
                triangles[ti + 4] = vi + 1;
                triangles[ti + 5] = vi + 3;
            }
            ti += 6;
        }

        for (int x = 0, vi = 0; x < nbSides; x++, vi += 2)
        {
            if (x == nbSides - 1)
            {
                triangles[ti] = vi;
                triangles[ti + 1] = 0;
                triangles[ti + 2] = nbSides * 2;
                triangles[ti + 3] = 1;
                triangles[ti + 4] = vi + 1;
                triangles[ti + 5] = nbSides * 2 + 1;
            }
            else
            {
                triangles[ti] = vi;
                triangles[ti + 1] = vi + 2;
                triangles[ti + 2] = nbSides * 2;
                triangles[ti + 3] = vi + 3;
                triangles[ti + 4] = vi + 1;
                triangles[ti + 5] = nbSides * 2 + 1;

            }

            ti += 6;
        }
        Mesh msh = new Mesh();

        msh.vertices = vertices;
        msh.triangles = triangles;
        
        gameObject.GetComponent<MeshFilter>().mesh = msh;
        gameObject.GetComponent<Renderer>().material = mat;
        gameObject.GetComponent<Renderer>().material.mainTexture = tex;
        msh.RecalculateNormals();
    }
}
