using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    public int force = 10;
    public GameObject goal;

    Rigidbody rb;
    bool avoiding = false;
    RaycastHit hit;
    Vector3 moveDir;
    float disToAvoid;
    Vector3 goalDir;
    bool normal = true;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        moveDir = new Vector3(goal.transform.position.x - transform.position.x, 0, goal.transform.position.z - transform.position.z);
        //Debug.Log(moveDir);
    }

    // Update is called once per frame
    void Update()
    {
        
        goalDir = new Vector3(goal.transform.position.x - transform.position.x, 0, goal.transform.position.z - transform.position.z);

        //Debug.Log(goalDir);
        Avoid();
    }

    private void FixedUpdate()
    {
        if (normal)
        {
            moveDir = goalDir;
        }
        
        rb.AddForce(moveDir * force, ForceMode.Force);


    }

    void Avoid()
    {

        if(Physics.Raycast(transform.position, moveDir, out hit, 10) && hit.collider.gameObject.CompareTag("obstacle"))
        {
            normal = false;
            Vector3 hitNormal = hit.normal;
            hitNormal.y = 0.0f;
            disToAvoid = hit.distance;

            if (disToAvoid < 3)
            {
                avoiding = true;
                rb.Sleep();
                moveDir =  - Vector3.Cross(hitNormal, Vector3.up).normalized;
                force = 80;
            }


        } 

        if(avoiding)
        {
            

            Debug.Log("1111111111111111111111");
            //Debug.Log(Vector3.Distance(transform.position, goal.transform.position));
            if (!(Physics.Raycast(transform.position, goalDir, out hit, Vector3.Distance(goal.transform.position, transform.position)) && hit.collider.gameObject.CompareTag("obstacle")))
            {
                Debug.Log("2222222222222222222");
                force = 0;
                avoiding = false;
                moveDir = goalDir;
                force = 3;
                normal = true;
            }
                

        }


    }



}
