using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Templo : MonoBehaviour
{
    private float speed=0.1f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float sped= speed*Time.deltaTime;
        transform.position = transform.position+new Vector3(0,0,speed*Time.deltaTime);
    }
}
