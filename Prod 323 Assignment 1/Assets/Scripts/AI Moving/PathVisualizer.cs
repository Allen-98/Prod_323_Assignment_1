using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathVisualizer : MonoBehaviour
{
    public enum Algorithm{Dijkstra, Astar, AstarUnder, AstarOver, AstarEuclid3D}
    public Algorithm algorithm = Algorithm.Astar;
    //public Terrain terrain;
    public GameObject startGO;
    public GameObject goalGO;
    public LineRenderer pathRenderer;
    private  TerrainGraph graph;
    private float offset = 0.5f;

    public List<Node> route = null;



    private void Start()
    {
        graph = new TerrainGraph();
        Visualised();

        //PathImprovement();
    }

    private void Update() {
        //if(startGO.transform.hasChanged || goalGO.transform.hasChanged)
          //  Visualised();
    }

    private void Visualised() {

        int x1 = (int) startGO.transform.position.x;
        int y1 = (int) startGO.transform.position.z;
        int x2 = (int) goalGO.transform.position.x;
        int y2 = (int) goalGO.transform.position.z;

        List<Node> path = null;
        
        switch(algorithm)
        {
            case Algorithm.Dijkstra:
                    path = TerrainPathfinding.Search(graph, graph.t_grid[x1, y1], graph.t_grid[x2, y2], (int) Algorithm.Dijkstra);
                    break;            
            case Algorithm.Astar:
                    path = TerrainPathfinding.Search(graph, graph.t_grid[x1, y1], graph.t_grid[x2, y2], (int) Algorithm.Astar);
                    break;
            case Algorithm.AstarUnder:
                    path = TerrainPathfinding.Search(graph, graph.t_grid[x1, y1], graph.t_grid[x2, y2], (int) Algorithm.AstarUnder);
                    break;
            case Algorithm.AstarOver:
                    path = TerrainPathfinding.Search(graph, graph.t_grid[x1, y1], graph.t_grid[x2, y2], (int) Algorithm.AstarOver);
                    break;
            case Algorithm.AstarEuclid3D:
                    path = TerrainPathfinding.Search(graph, graph.t_grid[x1, y1], graph.t_grid[x2, y2], (int) Algorithm.AstarEuclid3D);
                    break;        
        }

        route = path;


        Vector3[] lv = new Vector3[path.Count];
        int i = 0;
        Debug.Log(path.Count);



        foreach (Node n in path)
        {
            lv[i++] = new Vector3(n.Position.x + offset, n.height + offset, n.Position.y + offset);
        }
        pathRenderer.positionCount = path.Count;
        pathRenderer.SetPositions(lv);
        
    }


    void PathImprovement()
    {
        RaycastHit hit;
        int lookFrom = 0;
        List<Node> corner = new List<Node>();
        corner.Add(route[0]);


        for (int node = 2; node <= route.Count - 1; node++)
        {

            Vector3 a = new Vector3(route[lookFrom].Position.x, 0, route[lookFrom].Position.y);
            Vector3 b = new Vector3(route[node].Position.x, 0, route[node].Position.y);
            Vector3 look = b - a;

            if (Physics.Raycast(a, look, out hit, Vector3.Distance(a, b)))
            {
                corner.Add(route[node - 1]);
                corner.Add(route[node]);
                lookFrom = node - 1;
            }


        }

        Debug.Log(corner.Count);

        Vector3[] lv = new Vector3[corner.Count];
        int i = 0;

        foreach (Node n in corner)
        {
            lv[i++] = new Vector3(n.Position.x + offset, n.height + offset, n.Position.y + offset);
        }
        pathRenderer.positionCount = corner.Count;
        pathRenderer.SetPositions(lv);


    }



}
