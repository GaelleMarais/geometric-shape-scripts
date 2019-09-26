using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Cylinder : MonoBehaviour
{
    public Material mat;
    private Vector3[] vertices;
    private int[] triangles;

    void Start()
    {
        gameObject.AddComponent<MeshFilter>();          // Creation d'un composant MeshFilter qui peut ensuite être visualisé
        gameObject.AddComponent<MeshRenderer>();


        //create_cylinder(2,10,24,1);
        //create_rectangle(6,8);
        create_cylinder(24, 20, 6);

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

    // Start is called before the first frame update

    Vector3 rotateZ(Vector3 v, float degrees)
    {
        float r = degrees * Mathf.Deg2Rad;
        float c = Mathf.Cos(r);
        float s = Mathf.Sin(r);
        return new Vector3(
            c * v.x - s * v.y,
            s * v.x + c * v.y,
            v.z);
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
    
    void create_cylinder(int point_per_circle, int circle_count, float radius)
    {
        if (point_per_circle < 3) point_per_circle = 3;
        if (circle_count < 2) circle_count = 2;

        int vertex_count =  point_per_circle * circle_count;
        int index_count = 2 * 2 * 3 * (point_per_circle * (circle_count - 1));

        triangles = new int[index_count];
        vertices = new Vector3[vertex_count +2]; 

        for (int c = 0; c < circle_count; c++) {
            for (int p = 0; p < point_per_circle; p++) {
                float angle = p * (360 / point_per_circle);
                Vector3 v = new Vector3(radius, 0, c);
                v = rotateZ(v, angle);
                vertices[p + c * point_per_circle] = v;
            }
        }

        // Sides of the cylinder

        int triangle_index = 0;
        for (int cc = 0; cc < circle_count - 1; cc++) {
            for (int p = 0; p < point_per_circle; p++) {
                int a = ((p + 0) % point_per_circle + (cc + 0) * point_per_circle) % vertices.Length;
                int b = ((p + 1) % point_per_circle + (cc + 0) * point_per_circle) % vertices.Length;
                int c = ((p + 1) % point_per_circle + (cc + 1) * point_per_circle) % vertices.Length;
                int d = ((p + 0) % point_per_circle + (cc + 1) * point_per_circle) % vertices.Length;
                draw_quad(triangle_index, a, b, c, d);
                triangle_index += 6;
            }
        }

        // // Circles of the cylinder
        // vertices[vertex_count]= new Vector3(0, 0, 0);
        // vertices[vertex_count+1]= new Vector3(0, 0, 4);

        // for(int p = 0; p< point_per_circle; p++){
        //     triangles[triangle_index] = p;
        //     triangles[triangle_index + 1] = p+1;
        //     triangles[triangle_index + 2] = vertex_count;       
        // }
        // triangle_index +=3;
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
