using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Sphere : MonoBehaviour
{
    public Material mat;
    private Vector3[] vertices;
    private int[] triangles;

    void Start()
    {
        gameObject.AddComponent<MeshFilter>();          // Creation d'un composant MeshFilter qui peut ensuite être visualisé
        gameObject.AddComponent<MeshRenderer>();

        // rayon, nombre meridiens, nombre cercle
        create_sphere(10, 20, 20);      

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

    // Update is called once per frame
    void Update()
    {
        
    }

    void create_sphere(float radius, int nb_meridian, int circle_count){        
        //  Creation des sommets
        vertices = new Vector3[(nb_meridian+1) * circle_count + 2];
        
        vertices[0] = Vector3.up * radius;
        for( int lat = 0; lat < circle_count; lat++ ){
            float a1 = Mathf.PI * (float)(lat+1) / (circle_count+1); // l'angle du cercle 
            float sin1 = Mathf.Sin(a1);
            float cos1 = Mathf.Cos(a1);
        
            for( int lon = 0; lon <= nb_meridian; lon++ ){
                float a2 = 2*Mathf.PI * (float)(lon == nb_meridian ? 0 : lon) / nb_meridian;
                float sin2 = Mathf.Sin(a2);
                float cos2 = Mathf.Cos(a2);
        
                vertices[ lon + lat * (nb_meridian + 1) + 1] = new Vector3( sin1 * cos2, cos1, sin1 * sin2 ) * radius;
            }
        }
        vertices[vertices.Length-1] = Vector3.up * -radius;   

        // Creation des triangles
        int nb_face = vertices.Length;
        int nb_triangle = nb_face * 2;
        int nb_index = nb_triangle * 3;
        triangles = new int[ nb_index];
        
        // Fermeture du haut
        int triangle_index = 0;
        for(int lon = 0; lon<nb_meridian; lon++){
            triangles[triangle_index + 0] = lon+2;
            triangles[triangle_index + 1] = lon+1;
            triangles[triangle_index + 2] = 0;
            triangle_index += 3;
        }
        

        // Fermeture du bas 
        for( int lon = 0; lon < nb_meridian; lon++ ){
            triangles[triangle_index + 0]=vertices.Length-1;
            triangles[triangle_index + 1] =vertices.Length-(lon+2)-1;
            triangles[triangle_index + 2] =vertices.Length-(lon+1)-1;
            triangle_index += 3;
        }

        //Middle
        for( int lat = 0; lat < circle_count - 1; lat++ ){
            for( int lon = 0; lon < nb_meridian; lon++ ){
                int current = lon + lat * (nb_meridian + 1) + 1;
                int next = current + nb_meridian + 1;
        
                triangles[triangle_index + 0] = current;
                triangles[triangle_index + 1] = current + 1;
                triangles[triangle_index + 2] = next + 1;
        
                triangles[triangle_index + 3] = current;
                triangles[triangle_index + 4] = next + 1;
                triangles[triangle_index + 5] = next;

                triangle_index +=6;
            }
        }


        


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
