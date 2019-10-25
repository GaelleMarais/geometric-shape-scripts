using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Globalization;
using System;

public class LoadMesh : MonoBehaviour
{
    public Material mat;

    private Vector3 center;

    void Start()
    {
        gameObject.AddComponent<MeshFilter>();
        gameObject.AddComponent<MeshRenderer>();

        String[] lines = System.IO.File.ReadAllLines(@"Assets/triceratops.off");

        String[] tmpVertex = lines[1].Split(' ');

        int maxVertices = (int)(float.Parse(tmpVertex[0], CultureInfo.InvariantCulture));
        int maxIndex = (int)(float.Parse(tmpVertex[1], CultureInfo.InvariantCulture));

        Vector3[] vertices = new Vector3[maxVertices];
        int[] index = new int[maxIndex*3];

        Vector3 sumPoint = new Vector3(0, 0, 0);
        for (int i = 2; i < maxVertices + 2 ; i++)
        {
            tmpVertex = lines[i].Split(' ');
            float coordX = (float.Parse(tmpVertex[0], CultureInfo.InvariantCulture));
            float coordY = (float.Parse(tmpVertex[1], CultureInfo.InvariantCulture));
            float coordZ = (float.Parse(tmpVertex[2], CultureInfo.InvariantCulture));
            vertices[i-2] = new Vector3(coordX ,coordY, coordZ);

           sumPoint += vertices[i - 2];
        }

        center = sumPoint / maxVertices;

        for (int i = 0; i < maxVertices ; i++)
        {
            vertices[i] -= center;
        }

        Vector3 maxMagnitude = new Vector3 ();
        for(int i = 0; i < maxVertices; i++)
            if (vertices[i].sqrMagnitude > maxMagnitude.sqrMagnitude)
                maxMagnitude = vertices[i];

        for (int i = 0; i < maxVertices; i++)
            vertices[i] /= maxMagnitude.magnitude;

        int lastindex = 0;
        for(int i = (2 + maxVertices); i < (maxVertices + maxIndex + 2); i++)
        {
            tmpVertex = lines[i].Split(' ');

            index[lastindex] = (int)(float.Parse(tmpVertex[1], CultureInfo.InvariantCulture));
            lastindex++;
            index[lastindex] = (int)(float.Parse(tmpVertex[2], CultureInfo.InvariantCulture));
            lastindex++;
            index[lastindex] = (int)(float.Parse(tmpVertex[3], CultureInfo.InvariantCulture));
            lastindex++;
        }


        Mesh msh = new Mesh();

        msh.vertices = vertices;
        msh.triangles = index;
        gameObject.GetComponent<MeshFilter>().mesh = msh;
        gameObject.GetComponent<MeshRenderer>().material = mat;

    }

    private void OnDrawGizmos()
    {
        //Gizmos.DrawSphere(center, 0.25f);
    }
}
