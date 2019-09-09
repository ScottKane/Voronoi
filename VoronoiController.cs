using System.Collections.Generic;
using UnityEngine;

public class VoronoiController : MonoBehaviour
{
    public int seed = 0;

    public float halfMapSize = 10f;

    public float numberOfPoints = 20;

    private void OnDrawGizmos()
    {
        //Generate the random sites
        List<Vector3> randomSites = new List<Vector3>();

        //Generate random numbers with a seed
        Random.InitState(seed);

        float max = halfMapSize;
        float min = -halfMapSize;

        for (int i = 0; i < numberOfPoints; i++)
        {
            float randomX = Random.Range(min, max);
            float randomZ = Random.Range(min, max);

            randomSites.Add(new Vector3(randomX, 0f, randomZ));
        }


        //Points outside of the screen for voronoi which has some cells that are infinite
        float bigSize = halfMapSize * 5f;

        //Star shape which will give a better result when a cell is infinite large
        //When using other shapes, some of the infinite cells misses triangles
        randomSites.Add(new Vector3(0f, 0f, bigSize));
        randomSites.Add(new Vector3(0f, 0f, -bigSize));
        randomSites.Add(new Vector3(bigSize, 0f, 0f));
        randomSites.Add(new Vector3(-bigSize, 0f, 0f));


        //Generate the voronoi diagram
        List<VoronoiCell> cells = DelaunayToVoronoi.GenerateVoronoiDiagram(randomSites);


        //Debug
        //Display the voronoi diagram
        DisplayVoronoiCells(cells);

        //Display the sites
        Gizmos.color = Color.white;

        for (int i = 0; i < randomSites.Count; i++)
        {
            float radius = 0.2f;

            Gizmos.DrawSphere(randomSites[i], radius);
        }
    }

    //Display the voronoi diagram with mesh
    private void DisplayVoronoiCells(List<VoronoiCell> cells)
    {
        Random.InitState(seed);

        for (int i = 0; i < cells.Count; i++)
        {
            VoronoiCell c = cells[i];

            Vector3 p1 = c.sitePos;

            Gizmos.color = new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f), 1f);

            List<Vector3> vertices = new List<Vector3>();

            List<int> triangles = new List<int>();

            vertices.Add(p1);

            for (int j = 0; j < c.edges.Count; j++)
            {
                Vector3 p3 = c.edges[j].v1;
                Vector3 p2 = c.edges[j].v2;

                vertices.Add(p2);
                vertices.Add(p3);

                triangles.Add(0);
                triangles.Add(vertices.Count - 2);
                triangles.Add(vertices.Count - 1);
            }

            Mesh triangleMesh = new Mesh();

            triangleMesh.vertices = vertices.ToArray();

            triangleMesh.triangles = triangles.ToArray();

            triangleMesh.RecalculateNormals();

            Gizmos.DrawMesh(triangleMesh);
        }
    }
}