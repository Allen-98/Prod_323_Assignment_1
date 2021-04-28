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
    [SerializeField] float maxForce;

    private Vector2 currentPos;

    int node;
    int nextNode;
    Vector3 moveDirection;
    Vector3 nextDirection;
    float nodeDistance = 0;
    bool atGoal = false;
    //Animator ani;

    private void Start()
    {
        graph = new TerrainGraph();
        Visualised();

        //rb = GetComponentInChildren<Rigidbody>();
        rb = GetComponent<Rigidbody>();
        //ani = GetComponent<Animator>();
        //this.transform.position = new Vector3(start.transform.position.x, 2, start.transform.position.z);
       


        node = 0;
        nextNode = node + 1;
        currentPos = new Vector2(this.transform.position.x, this.transform.position.z);

        moveDirection = new Vector3(route[node].Position.x - this.transform.position.x, 0, route[node].Position.y - this.transform.position.y);
        nextDirection = new Vector3(route[nextNode].Position.x - route[node].Position.x, 0, route[nextNode].Position.y - route[node].Position.y);
        nodeDistance = Mathf.Sqrt(Mathf.Pow((route[node].Position.x - currentPos.x), 2) + Mathf.Pow((route[node].Position.y - currentPos.y), 2));


    }

    private void Update()
    {
        currentPos = new Vector2(this.transform.position.x, this.transform.position.z);

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

        nodeDistance = Mathf.Sqrt(Mathf.Pow((route[node].Position.x - currentPos.x), 2) + Mathf.Pow((route[node].Position.y - currentPos.y), 2));

        if (!atGoal)
        {
            rb.AddForce(moveDirection * force, ForceMode.Force);
            //ani.SetBool("isWalking", true);
        }

        if (nextDirection != moveDirection)
        {

            if (nodeDistance < 2)
            {
                rb.Sleep();
                //ani.SetBool("isWalking", false);
                moveDirection = nextDirection;

                if (nextNode < route.Count - 1)
                {
                    node += 1;
                    nextNode += 1;
                }
            }

        }
        else
        {
            if (nextNode < route.Count - 1)
            {
                node += 1;
                nextNode += 1;

            }

        }


        if (nextNode < route.Count - 1 && node > 0)
        {
            nextDirection = new Vector3(route[nextNode].Position.x - route[node].Position.x, 0, route[nextNode].Position.y - route[node].Position.y);
        }





    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("goal"))
        {
            Debug.Log("Arrive the goal");
            rb.Sleep();
            //ani.SetBool("isWalking", false);
            atGoal = true;

        }
    }


}
