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
    private List<Node> racePath;


    private Vector3 lastDirection;
    private Vector3 currentDirection;


    // Start is called before the first frame update
    void Start()
    {
        pathVis = GetComponent<PathVisualizer>();
        racePath = pathVis.route;
        rb = GetComponent<Rigidbody>();
        //this.transform.position = new Vector3(start.transform.position.x, 2, start.transform.position.z);
        lastDirection = new Vector3(0, 0, 0);
        currentDirection = new Vector3(0, 0, 0);
        Debug.Log(pathVis.route.Count);
    }

    // Update is called once per frame
    void Update()
    {
        currentPos = new Vector2(this.transform.position.x, this.transform.position.z);
        
        //rb.AddForce(direction * force, ForceMode.Force);

    }


    private void FixedUpdate()
    {
        //rb.AddForce(direction * force, ForceMode.Force);
        //currentPos = new Vector2(this.transform.position.x, this.transform.position.z);
        
        for (int i = 0; i < (racePath.Count - 1); i++)
        {

            Vector3 direction = new Vector3(racePath[i].Position.x - this.transform.position.x, 0, racePath[i].Position.y - this.transform.position.z);
            currentDirection = direction;

            do
            {
                if (currentDirection == lastDirection)
                {
                    continue;

                }
                rb.AddForce(currentDirection * force, ForceMode.Force);
                lastDirection = currentDirection;

            } while (currentPos != racePath[i].Position);

            rb.AddForce(currentDirection * -force * 2, ForceMode.Impulse);



        }
      
    }

}
