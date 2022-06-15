using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Agujero : MonoBehaviour
{
    public Transform player;
    Renderer render;
    // Start is called before the first frame update
    void Start()
    {
        render = gameObject.GetComponent<Renderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (player != null) {
            // Pass the player location to the shader
            render.sharedMaterial.SetVector("_PlayerPosition", player.position);
        }
    }
}
