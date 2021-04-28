using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Priority_Queue;

public static class TerrainPathfinding
{
 public static List<Node> Search(TerrainGraph graph, Node start, Node goal, int mode)
    {
        Dictionary<Node, Node> came_from = new Dictionary<Node, Node>();
        Dictionary<Node, float> cost_so_far = new Dictionary<Node, float>();

        List<Node> path = new List<Node>();

        SimplePriorityQueue<Node> frontier = new SimplePriorityQueue<Node>();
        frontier.Enqueue(start, 0);

        came_from.Add(start, start);
        cost_so_far.Add(start, 0);

        Node current = new Node(0,0);
        while (frontier.Count > 0)
        {
            current = frontier.Dequeue();
            if (current == goal) break; // Early exit

            foreach (Node next in graph.Neighbours(current))
            {
                float new_cost = cost_so_far[current] + graph.Cost(current, next);
                if (!cost_so_far.ContainsKey(next) || new_cost < cost_so_far[next])
                {
                    cost_so_far[next] = new_cost;
                    came_from[next] = current;

                    float heuristicVal = 0;
                    if(mode==0)
                        heuristicVal = 0;
                    else if(mode == 1)
                        heuristicVal = Heuristic(next, goal);
                    else if(mode == 2)
                        heuristicVal = HeuristicUnder(next, goal);
                    else if(mode == 3)
                        heuristicVal = HeuristicOver(next, goal);
                    else if(mode == 4)
                        heuristicVal = HeuristicEuclid3D(next, goal);

                    float priority = new_cost + heuristicVal;
                    frontier.Enqueue(next, priority);
                    next.Priority = new_cost;

                }
            }
        }


        while (current != start)
        {
            path.Add(current);
            current = came_from[current];          
        }
        path.Reverse();

        return path;
    }

    public static float Heuristic(Node a, Node b)
    {
        return Vector2.Distance(a.Position, b.Position);
    }

    public static float HeuristicUnder(Node a, Node b)
    {
        return Vector2.Distance(a.Position, b.Position)/2;
    } 

    public static float HeuristicOver(Node a, Node b)
    {
        return Vector2.Distance(a.Position, b.Position)*2;
    } 

    public static float HeuristicEuclid3D(Node a, Node b)
    {
        return Vector3.Distance(new Vector3(a.Position.x, a.height, a.Position.y), new Vector3(b.Position.x, b.height, b.Position.y));
    }     

}
