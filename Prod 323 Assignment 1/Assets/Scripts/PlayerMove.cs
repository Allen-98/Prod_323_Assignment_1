using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{

    Rigidbody rb;
    [SerializeField] float force = 5;
    [SerializeField] Transform start;
    [SerializeField] float maxForce;

    private Vector2 currentPos;
    private PathVisualizer pathVis;
    private List<Node> route;



    int node;
    int nextNode;
    Vector3 moveDirection;
    Vector3 nextDirection;
    float nodeDistance = 0;
    bool atGoal=false;

    // Start is called before the first frame update
    void Start()
    {
        pathVis = GetComponent<PathVisualizer>();
        route = pathVis.route;
        rb = GetComponent<Rigidbody>();

        node = 0;
        nextNode = node + 1;
        currentPos = new Vector2(this.transform.position.x, this.transform.position.z);


        moveDirection = new Vector3(route[node].Position.x - this.transform.position.x, 0, route[node].Position.y - this.transform.position.y);
        nextDirection = new Vector3(route[nextNode].Position.x - route[node].Position.x, 0, route[nextNode].Position.y - route[node].Position.y);
        nodeDistance = Mathf.Sqrt(Mathf.Pow((route[node].Position.x - currentPos.x), 2) + Mathf.Pow((route[node].Position.y - currentPos.y), 2));



    }

    // Update is called once per frame
    void Update()
    {
        currentPos = new Vector2(this.transform.position.x, this.transform.position.z);
        //nodeDistance = Mathf.Sqrt(Mathf.Pow((route[node].Position.x - currentPos.x), 2) + Mathf.Pow((route[node].Position.y - currentPos.y), 2));


    }



    private void FixedUpdate()
    {

        nodeDistance = Mathf.Sqrt(Mathf.Pow((route[node].Position.x - currentPos.x), 2) + Mathf.Pow((route[node].Position.y - currentPos.y), 2));

        if (!atGoal)
        {
            rb.AddForce(moveDirection * force, ForceMode.Force);
        }

        if (nextDirection != moveDirection)
        {

            if (nodeDistance < 2)
            {
                rb.Sleep();

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
            if(nextNode < route.Count - 1)
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
            atGoal = true;

        }
    }

}
