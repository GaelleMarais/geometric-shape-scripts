using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Rectangle : MonoBehaviour
{
    public Material mat;
    private Vector3[] vertices;
    private int[] triangles;

    void Start()
    {
        gameObject.AddComponent<MeshFilter>();          // Creation d'un composant MeshFilter qui peut ensuite être visualisé
        gameObject.AddComponent<MeshRenderer>();


        create_rectangle(16,10);

        Mesh msh = new Mesh();
        msh.vertices = vertices;
        msh.triangles = triangles;
        gameObject.GetComponent<MeshFilter>().mesh = msh;
        gameObject.GetComponent<MeshRenderer>().material = mat;  
    }


    void draw_triangle(int index, int a, int b, int c)
    {
        triangles[index + 0] = a;
        triangles[index + 1] = b;
        triangles[index + 2] = c;
        //ne pas oublier d'ajouter 3 à index après ça
    }

    void draw_quad(int index, int a, int b, int c, int d)
    {
        draw_triangle(index + 0, a, b, c);
        draw_triangle(index + 3, c, d, a);
        //index+=6 après ça
    }


     void create_rectangle(int width, int height) {
        int nb_triangle = width*height*6;
        int nb_vertice = (width+1) * (height+1);

        triangles = new int[nb_triangle];
        vertices = new Vector3[nb_vertice];

        for (int y = 0; y < height; ++y) {
            for (int x = 0; x < width; ++x) {
                float z = 0;
                vertices[x + y * width] = new Vector3(x, y, z);
            }
        }

        int triangle_index = 0;
        for (int y = 0; y < height - 1; ++y) {
            for (int x = 0; x < width - 1; ++x) {
                int a = (x + 0) + (y + 0) * width;
                int b = (x + 1) + (y + 0) * width;
                int c = (x + 1) + (y + 1) * width;
                int d = (x + 0) + (y + 1) * width;

                draw_quad(triangle_index, a, b, c, d);
                triangle_index += 6;
            }
        }
    }


    // Update is called once per frame
    void Update()
    {
        
    }

    // private void OnDrawGizmos() {
    //     if (vertices == null) {
    //         return;
    //     }
        
    //     for (int i = 0; i < vertices.Length; ++i) {
    //         Gizmos.DrawSphere(vertices[i], 0.1f);
    //     }
    // }
}
