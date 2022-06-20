using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformVaiven : MonoBehaviour {
    // Start is called before the first frame update
    public float displacement;
    public float period = 1;
    private Vector3 startPosition;
    void Start() {
        startPosition = transform.position;
    }

    // Update is called once per frame
    void Update() {
        //Queremos que la función PingPong varíe de 0 a 1 y de vuelta a 0
        //con un período marcado por la variable period
        //Como PingPong tiene un período que es el doble del valor máximo que alcanza
        //multiplicamos el primer parámetro por 2, para que su período pase a ser 1
        //y luego lo dividimos por period para ajustar el período definitivo a ese valor.
        float pingpong = Mathf.PingPong(Time.time * 2 / period, 1);

        float step = Mathf.SmoothStep(-displacement, displacement, pingpong);
        //float step = pingpong;

        transform.position = new Vector3(transform.position.x, transform.position.y, startPosition.z + step);
        /*
        if(Mathf.Abs(step + displacement) <= 0.001) {
            Debug.Log("PlatformVaiven.Update time " + Time.time + " pingpong" + pingpong + " step " + step);
        }*/
    }
}
