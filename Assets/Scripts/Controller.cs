using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller : MonoBehaviour
{

    public GameObject puerta;
    Vector3 posicion;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        posicion = Player.instance.position();
        Debug.Log(posicion);
        if(posicion.z>180){
            puerta.SetActive(true);
        }
        
    }
}
