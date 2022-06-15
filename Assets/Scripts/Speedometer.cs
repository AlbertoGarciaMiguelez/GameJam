using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Speedometer : MonoBehaviour {
    private Vector3 previousPosition;
    public Vector3 velocity;
    public float speed;
    // Start is called before the first frame update
    void Start() {
        previousPosition = transform.position;        
    }

    // Update is called once per frame
    void FixedUpdate()  {
        velocity = (transform.position - previousPosition) / Time.fixedDeltaTime;
        speed = velocity.magnitude;     

        previousPosition = transform.position;   
    }
}
