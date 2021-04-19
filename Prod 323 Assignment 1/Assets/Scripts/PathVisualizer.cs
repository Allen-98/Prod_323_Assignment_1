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

    private void Start()
    {
        graph = new TerrainGraph();
        Visualised();
    }

    private void Update() {
        if(startGO.transform.hasChanged || goalGO.transform.hasChanged)
            Visualised();
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

        Vector3[] lv = new Vector3[path.Count];
        int i = 0;
        //Debug.Log(path.Count);
        foreach (Node n in path)
        {
            lv[i++] = new Vector3(n.Position.x + offset, n.height + offset, n.Position.y + offset);
        }
        pathRenderer.positionCount = path.Count;
        pathRenderer.SetPositions(lv);
        
    }
}
