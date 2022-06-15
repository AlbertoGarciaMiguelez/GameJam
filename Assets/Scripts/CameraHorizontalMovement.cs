using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraHorizontalMovement : MonoBehaviour {
    public bool intermediatePositionsAllowed = true;
    public float cameraLerpSpeed = 20;

    public Transform playerHead;
    public float mouseScrollSensitivity = 10f;
    public Transform cameraHorizontalFront;
    public Transform cameraHorizontalBack;
    private Vector3 cameraTargetPosition;

    private RaycastHit[] hits;

    // Start is called before the first frame update
    void Start() {
        if(cameraHorizontalFront == null) {
            Debug.Log("CameraHorizontalMovement.Start cameraHorizontalFront no está asignado");
        }

        if(cameraHorizontalBack == null) {
            Debug.Log("CameraHorizontalMovement.Start cameraHorizontalBack no está asignado");
        }

        if(playerHead == null) {
            Debug.Log("CameraHorizontalMovement.Start playerHead no está asignado");
        }

        cameraTargetPosition = transform.localPosition;
    }

    // Update is called once per frame
    void Update() {
        //Gestión de inputs
        float mouseScroll = Input.GetAxisRaw("Mouse ScrollWheel");
        
        //Movimiento horizontal de la cámara
        if(intermediatePositionsAllowed) {
            cameraTargetPosition.z += mouseScroll * mouseScrollSensitivity * Time.deltaTime;
            cameraTargetPosition.z = Mathf.Clamp(cameraTargetPosition.z, cameraHorizontalBack.localPosition.z, cameraHorizontalFront.localPosition.z);
        } else {
            
            if(mouseScroll > 0) {
                cameraTargetPosition = cameraHorizontalFront.localPosition;
            } else if (mouseScroll < 0) {
                cameraTargetPosition = cameraHorizontalBack.localPosition;
            }
        }


        
        //Antes de buscar nuevos obstáculos, restauro los que haya puesto transparentes en el frame previo
        if(hits != null) {
            foreach(RaycastHit hit in hits) {
                if(hit.collider.gameObject.CompareTag("Obstacle")) {
                    //Transparentamos el obstáculo
                    Color color = hit.collider.GetComponent<Renderer>().material.color;
                    color.a = 1f;
                    hit.collider.GetComponent<Renderer>().material.color = color;
                }
            }
        }

        //La dirección del raycast es la dirección forward de la montura de la cámara
        Vector3 raycastDirection = (playerHead.position - transform.position);
        float raycastDistance = raycastDirection.magnitude;
        raycastDirection = raycastDirection.normalized;
        hits = Physics.RaycastAll(transform.position, raycastDirection, raycastDistance);


        foreach(RaycastHit hit in hits) {
            if(hit.collider.gameObject.CompareTag("Obstacle")) {
                //Transparentamos el obstáculo
                Color color = hit.collider.GetComponent<Renderer>().material.color;
                color.a = 0.4f;
                hit.collider.GetComponent<Renderer>().material.color = color;
            } else if(hit.collider.gameObject.CompareTag("Wall")) {
                //Acercamos la cámara a Amy lo necesario para que quede por delante del muro
                float maxCameraDistance = Vector3.Distance(playerHead.position, hit.point) - 0.5f;

                if(cameraTargetPosition.z < -maxCameraDistance){
                    cameraTargetPosition.z = -maxCameraDistance;
                }

            }

        }
  
        transform.localPosition = Vector3.Lerp(transform.localPosition, cameraTargetPosition, cameraLerpSpeed * Time.deltaTime);
    }
}
