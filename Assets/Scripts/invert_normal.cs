using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class invert_normal : MonoBehaviour
{
    public GameObject SferaPanoramica;

    void Awake()
    {
        InvertSphere();
    }

    void Update()
    {
        InvertSphere();
    }

    void InvertSphere()
    {
        Vector3[] normals = SferaPanoramica.GetComponent<MeshFilter>().mesh.normals;
        for (int i = 0; i < normals.Length; i++)
        {
            normals[i] = -normals[i];
        }
        SferaPanoramica.GetComponent<MeshFilter>().sharedMesh.normals = normals;

        int[] triangles = SferaPanoramica.GetComponent<MeshFilter>().sharedMesh.triangles;
        for (int i = 0; i < triangles.Length; i += 3)
        {
            int t = triangles[i];
            triangles[i] = triangles[i + 2];
            triangles[i + 2] = t;
        }

        SferaPanoramica.GetComponent<MeshFilter>().sharedMesh.triangles = triangles;
    }
}
