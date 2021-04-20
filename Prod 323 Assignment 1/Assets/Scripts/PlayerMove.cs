using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{

    Rigidbody rb;
    [SerializeField] float force = 5;
    [SerializeField] Transform start;

    private Vector2 currentPos;
    private PathVisualizer pathVis;
    private List<Node> racePath;
    private Vector3 direction;


    // Start is called before the first frame update
    void Start()
    {
        pathVis = GetComponent<PathVisualizer>();
        racePath = pathVis.path;
        rb = GetComponent<Rigidbody>();
        this.transform.position = new Vector3(start.transform.position.x, 2, start.transform.position.z);

        direction = new Vector3(racePath[0].Position.x - this.transform.position.x, 0, racePath[0].Position.y - this.transform.position.z);
    }

    // Update is called once per frame
    void Update()
    {
        currentPos = new Vector2(this.transform.position.x, this.transform.position.z);
        
        rb.AddForce(direction * force, ForceMode.Force);

    }


    private void FixedUpdate()
    {
        /*
        for (int i = 0; i < racePath.Count - 1; i++)
        {
            Vector3 direction = new Vector3(racePath[i].Position.x - this.transform.position.x, 0, racePath[i].Position.y - this.transform.position.z);

            rb.AddForce(direction * force, ForceMode.Force);

            if (currentPos == racePath[i].Position)
            {
                continue;
            }



        }
        */
    }

}
