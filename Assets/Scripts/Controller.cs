using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller : MonoBehaviour
{

    public GameObject puerta;
    public GameObject portal;
    public GameObject plataforma;
    public GameObject point;
    public GameObject power;
    public GameObject Musica1;
    public GameObject Musica2;
    public GameObject Musica3;
    private bool estado;
    private bool estado2;
    private int puntos;
    Vector3 posicion;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        puntos=Player.instance.datos();
        estado=Player.instance.powerOn();
        estado2=Player.instance.pointOn();
        posicion = Player.instance.position();
        Debug.Log(posicion);
        if(posicion.z>180){
            puerta.SetActive(true);
            Musica1.SetActive(false);
            Musica2.SetActive(true);
            Musica3.SetActive(false);
        }
        if(posicion.z<170){
            puerta.SetActive(false);
            if(puntos!=2){
                Musica1.SetActive(true);
                Musica2.SetActive(false);
                Musica3.SetActive(false);
            }
        }
        if(estado){
            portal.SetActive(true);
            plataforma.SetActive(true);
            Musica1.SetActive(false);
            Musica2.SetActive(false);
            Musica3.SetActive(true);
        }
        if(posicion.y<-50){
            Player.instance.Respawn();
        }
        if(estado2){
            point.SetActive(true);
        }
        if(puntos==2){
            power.SetActive(true);
        }
    }
}
