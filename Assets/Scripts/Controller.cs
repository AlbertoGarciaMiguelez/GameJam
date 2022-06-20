using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller : MonoBehaviour
{

    public GameObject puerta;
    public GameObject portal;
    public GameObject plataforma;
    private bool estado;
    Vector3 posicion;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        estado=Player.instance.powerOn();
        posicion = Player.instance.position();
        Debug.Log(posicion);
        if(posicion.z>180){
            puerta.SetActive(true);
        }
        if(estado){
            portal.SetActive(true);
            plataforma.SetActive(true);
        }
        if(posicion.y<-50){
            Player.instance.Respawn();
        }
        
    }
}
