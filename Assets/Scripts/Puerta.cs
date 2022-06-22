using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Puerta : MonoBehaviour
{
    public ParticleSystem efecto1;
    public ParticleSystem efecto2;
    public ParticleSystem efecto3;
    public ParticleSystem efecto4;
    public ParticleSystem efecto5;
    public bool bo=true;
    private int datos;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        datos= Player.instance.datos();
        if(bo=true && datos==4){
            bo = false;
            desbloqueo();
            Invoke("reaparece", 5f);
            Invoke("stop", 5f);
            Debug.Log("Update");
        }
    }
    public void desbloqueo(){
        efecto1.Play();
        efecto2.Play();
        efecto3.Play();
        efecto4.Play();
        efecto5.Play();
        Debug.Log("Desbloqueo");
    }
    public void reaparece(){
        this.gameObject.SetActive(false);
        Debug.Log("reaparece");
    }
    public void stop(){
        efecto1.Stop();
        efecto2.Stop();
        efecto3.Stop();
        efecto4.Stop();
        efecto5.Stop();
        Debug.Log("Desbloqueo");
    }
}
