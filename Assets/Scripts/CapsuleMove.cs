using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CapsuleMove : MonoBehaviour
{
    public float speed = 1000;
    private Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
       rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        float vert = Input.GetAxis("Vertical");
        float horizontal = Input.GetAxis("Horizontal");

        Vector3 move = new Vector3(vert, 0, horizontal) * speed * Time.deltaTime;
        rb.MovePosition(transform.position + move);
    }
}
