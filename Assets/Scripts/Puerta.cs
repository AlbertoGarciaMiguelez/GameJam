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

    public Player p;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
        
            Invoke("desbloqueo", 4f);
            Invoke("reaparece", 6f);
            Debug.Log("Update");
        
    }
    public void desbloqueo(){
        efecto1.Play();
        efecto2.Play();
        efecto3.Play();
        efecto4.Play();
        efecto5.Play();
        this.gameObject.SetActive(true);
        Debug.Log("Desbloqueo");
    }
    public void reaparece(){
        this.gameObject.SetActive(false);
        Debug.Log("reaparece");
    }
}
