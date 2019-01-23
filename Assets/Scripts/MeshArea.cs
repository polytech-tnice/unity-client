using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshArea : MonoBehaviour
{
    public float Area {
        get {
            return area;
        }
    }

    private float area;
    // Start is called before the first frame update
    void Start()
    {
        Mesh mesh = GetComponent<MeshFilter>().sharedMesh;
        area = AreaOfMesh(mesh);
    }


    public float AreaOfTriangle(Vector3 p1, Vector3 p2, Vector3 p3) //Heron's formula
    {
        float a = Vector3.Distance(p1, p2);
        float b = Vector3.Distance(p2, p3);
        float c = Vector3.Distance(p3, p1);

        float s = 0.5f * (a + b + c);
        return Mathf.Sqrt(s * (s - a) * (s - b) * (s - c));
    }

    public float AreaOfMesh(Mesh mesh)
    {
        float volume = 0;
        Vector3[] vertices = mesh.vertices;
        int[] triangles = mesh.triangles;
        for (int i = 0; i < mesh.triangles.Length; i += 3)
        {
            Vector3 p1 = vertices[triangles[i + 0]];
            Vector3 p2 = vertices[triangles[i + 1]];
            Vector3 p3 = vertices[triangles[i + 2]];
            volume += AreaOfTriangle(p1, p2, p3);
        }
        return Mathf.Abs(volume);
    }
}
