using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIAgent : MonoBehaviour
{
    public enum Algorithm { Dijkstra, Astar, AstarUnder, AstarOver, AstarEuclid3D }
    public Algorithm algorithm = Algorithm.Astar;
    //public Terrain terrain;
    public GameObject startGO;
    public GameObject goalGO;
    public LineRenderer pathRenderer;
    private TerrainGraph graph;
    private float offset = 0.5f;

    public List<Node> route = null;

    Rigidbody rb;
    [SerializeField] float force = 5;
    [SerializeField] Transform start;
    [SerializeField] float maxForce;

    private Vector2 currentPos;
    private Vector3 lastDirection;
    private Vector3 currentDirection;
    private float currentForce=0;

    private void Start()
    {
        graph = new TerrainGraph();
        Visualised();

        rb = GetComponent<Rigidbody>();
        //this.transform.position = new Vector3(start.transform.position.x, 2, start.transform.position.z);
        lastDirection = new Vector3(0, 0, 0);
        currentDirection = new Vector3(0, 0, 0);
    }

    private void Update()
    {
        currentPos = new Vector2(this.transform.position.x, this.transform.position.z);
        //if(startGO.transform.hasChanged || goalGO.transform.hasChanged)
        // Visualised();
    }

    private void Visualised()
    {

        int x1 = (int)startGO.transform.position.x;
        int y1 = (int)startGO.transform.position.z;
        int x2 = (int)goalGO.transform.position.x;
        int y2 = (int)goalGO.transform.position.z;

        List<Node> path = null;

        switch (algorithm)
        {
            case Algorithm.Dijkstra:
                path = TerrainPathfinding.Search(graph, graph.t_grid[x1, y1], graph.t_grid[x2, y2], (int)Algorithm.Dijkstra);
                break;
            case Algorithm.Astar:
                path = TerrainPathfinding.Search(graph, graph.t_grid[x1, y1], graph.t_grid[x2, y2], (int)Algorithm.Astar);
                break;
            case Algorithm.AstarUnder:
                path = TerrainPathfinding.Search(graph, graph.t_grid[x1, y1], graph.t_grid[x2, y2], (int)Algorithm.AstarUnder);
                break;
            case Algorithm.AstarOver:
                path = TerrainPathfinding.Search(graph, graph.t_grid[x1, y1], graph.t_grid[x2, y2], (int)Algorithm.AstarOver);
                break;
            case Algorithm.AstarEuclid3D:
                path = TerrainPathfinding.Search(graph, graph.t_grid[x1, y1], graph.t_grid[x2, y2], (int)Algorithm.AstarEuclid3D);
                break;
        }

        route = path;
        Debug.Log(route.Count);
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

    private void FixedUpdate()
    {
        Vector3 direction = new Vector3(route[0].Position.x - this.transform.position.x, 0, route[0].Position.y - this.transform.position.z);
        
        if(currentForce < maxForce)
        {
            rb.AddForce(direction * force, ForceMode.Force);
        }


        if(currentPos == route[0].Position)
        {
            rb.Sleep();

        }




        //rb.AddForce(direction * force, ForceMode.Force);
        //currentPos = new Vector2(this.transform.position.x, this.transform.position.z);

        /*
        for (int i = 0; i < (route.Count - 1); i++)
        {

            Vector3 direction = new Vector3(route[i].Position.x - this.transform.position.x, 0, route[i].Position.y - this.transform.position.z);
            currentDirection = direction;

            do
            {
                if (currentDirection == lastDirection)
                {
                    continue;

                }
                rb.AddForce(currentDirection * force, ForceMode.Force);
                lastDirection = currentDirection;

            } while (currentPos != route[i].Position);

            rb.AddForce(currentDirection * -force * 2, ForceMode.Impulse);

        }
        */

    }








}
