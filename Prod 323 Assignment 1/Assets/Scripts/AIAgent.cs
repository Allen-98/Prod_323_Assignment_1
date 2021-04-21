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
    private int node = 0;
    private Vector2 startPos;
    private Vector3 futureDirection;
    private int futureNode = 0;
    private bool changeDirection = false;

    private void Start()
    {
        graph = new TerrainGraph();
        Visualised();

        rb = GetComponent<Rigidbody>();
        //this.transform.position = new Vector3(start.transform.position.x, 2, start.transform.position.z);
        lastDirection = new Vector3(0, 0, 0);
        currentDirection = new Vector3(0, 0, 0);

        startPos = new Vector2(this.transform.position.x, this.transform.position.z);
        futureDirection = new Vector3(route[node].Position.x - startPos.x, 0, route[node].Position.y - startPos.y);
        currentPos = new Vector2(this.transform.position.x, this.transform.position.z);
    }

    private void Update()
    {
        currentPos = new Vector2(this.transform.position.x, this.transform.position.z);
        //Debug.Log(currentPos);
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

        if (node > 0)
        {
            futureDirection = new Vector3(route[node+1].Position.x - route[node].Position.x, 0, route[node+1].Position.y - route[node].Position.y);
        }

        Vector3 direction = new Vector3(route[node].Position.x - startPos.x, 0, route[node].Position.y - startPos.y);

   
        float distance = Mathf.Sqrt(Mathf.Pow((route[node].Position.x - currentPos.x), 2) + Mathf.Pow((route[node].Position.y - currentPos.y), 2));
        
        rb.AddForce(direction * force, ForceMode.Force);
        Debug.Log(direction);
        Debug.Log(futureDirection);

        if (changeDirection)
        {
            Debug.Log("nnnnnnnnnnnnnnnnnnnnnnnn");
            //rb.AddForce(direction * -2, ForceMode.Impulse);
            rb.Sleep();
            changeDirection = false;
            node += 1;

        }



        if (distance < 2)
        {
            startPos = route[node].Position;
            node += 1;
        }
       
        if (futureDirection == direction)
        {
            futureNode = node + 1;
            
        }
        
        if (futureNode == node)
        {
            changeDirection = true;
        }

        
        


    }





}
