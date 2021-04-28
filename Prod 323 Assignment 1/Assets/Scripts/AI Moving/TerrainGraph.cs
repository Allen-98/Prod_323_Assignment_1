using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainGraph
{
    int t_width;
    int t_height;
    public Node[,] t_grid;
    Vector3 t_size;
    float[,] t_hmap;
    float[,,] t_cost;
    TerrainData t_data;
    int t_nn = 8;

    public TerrainGraph()
    {
        //t_data = terrain.terrainData;
        t_data = Terrain.activeTerrain.terrainData;

        t_size = t_data.size;
        t_width = Mathf.FloorToInt(t_size.x);
        t_height = Mathf.FloorToInt(t_size.z);
        t_grid = new Node[t_width, t_height];
        t_cost = new float[t_width, t_height, t_nn];
        //Get terrain heightmap
        //t_hmap = t_data.GetHeights(0, 0, t_width, t_height); //return a normalized height 0-1

        //Populate our grid
        for(int x = 0; x < t_width; x++)
        {
            for(int y = 0; y < t_height; y++)
            {
                t_grid[x, y] = new Node(x, y);
                //t_grid[x, y].height = t_hmap[x, y] * t_data.size.y;
                t_grid[x, y].height = Terrain.activeTerrain.SampleHeight(new Vector3(x + 0.5f, 0, y + 0.5f));                
            }
        }

        for(int x = 0; x < t_width; x++)
        {
            for(int y = 0; y < t_height; y++)
            {
                if(x > 0 && y > 0 && x < t_width-1 && y < t_height-1)
                { //Store cost (height difference) of the terrain
                    t_cost[x, y, 0] = Mathf.Abs(t_grid[x, y].height - t_grid[x-1, y].height); //left  //t_data.GetSteepness
                    t_cost[x, y, 1] = Mathf.Abs(t_grid[x, y].height - t_grid[x-1, y+1].height); //top-left
                    t_cost[x, y, 2] = Mathf.Abs(t_grid[x, y].height - t_grid[x  , y+1].height); //top
                    t_cost[x, y, 3] = Mathf.Abs(t_grid[x, y].height - t_grid[x+1, y+1].height); //top-right
                    t_cost[x, y, 4] = Mathf.Abs(t_grid[x, y].height - t_grid[x+1, y].height); //right
                    t_cost[x, y, 5] = Mathf.Abs(t_grid[x, y].height - t_grid[x+1, y-1].height); //bottom-right
                    t_cost[x, y, 6] = Mathf.Abs(t_grid[x, y].height - t_grid[x  , y-1].height); //bottom
                    t_cost[x, y, 7] = Mathf.Abs(t_grid[x, y].height - t_grid[x-1, y-1].height); //bottom-left           
                }
            }
        }
    }

    /// Checks whether the neighbouring Node is within the grid bounds or not
    public bool InBounds(Vector2 v)
    {
        if (v.x >= 0 && v.x < this.t_width &&
            v.y >= 0 && v.y < this.t_height)
            return true;
        else
            return false;
    }

    /// Returns a List of neighbouring Nodes
    public List<Node> Neighbours(Node n)
    {
        List<Node> results = new List<Node>();

        List<Vector2> directions = new List<Vector2>()
        {
            new Vector2( -1, 0 ), // left
            new Vector2(-1, 1 ),  // top-left
            new Vector2( 0, 1 ),  // top
            new Vector2( 1, 1 ),  // top-right
            new Vector2( 1, 0 ),  // right
            new Vector2( 1, -1 ), // bottom-right
            new Vector2( 0, -1 ), // bottom
            new Vector2( -1, -1 ) // bottom-left
        };

        foreach (Vector2 v in directions)
        {
            Vector2 newVector = v + n.Position;
            if (InBounds(newVector))
            {
                results.Add(t_grid[(int)newVector.x, (int)newVector.y]);
            }
        }

        return results;
    }

    public float Cost(Node a, Node b)
    {
        float cost = 99;
        int dx = Mathf.FloorToInt(b.Position.x - a.Position.x);
        int dy = Mathf.FloorToInt(b.Position.y - a.Position.y);
        int nn = 0;
        if(Mathf.Abs(dx) >= 0 && Mathf.Abs(dx) <= 1 && Mathf.Abs(dy) >= 0 && Mathf.Abs(dy) <= 1)
        {
            if(dx == 1) 
            {
                if(dy == 1)
                    nn = 3;
                else if(dy == 0)
                    nn = 4;
                else if(dy == -1)
                    nn = 5;
            } 
            else if (dx == -1)
            {
                if(dy == 1)
                    nn = 1;
                else if(dy == 0)
                    nn = 0;
                else if(dy == -1)
                    nn = 7;
            }
            else // dx == 0
            {
                if(dy == 1)
                    nn = 2;
                else if(dy == -1)
                    nn = 6;
            }
            cost = 1 + t_cost[Mathf.FloorToInt(a.Position.x), Mathf.FloorToInt(a.Position.y), nn]*5;
        }
        
        return cost;
    }
}
