using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    [Header("Movimiento horizontal")]
    public float forwardSpeed= 5f;    

    [Header("Movimiento de rotación")]
    public float mouseRotationSensitivity = 10;
    private float rotationSpeed = 200f;

    [Header("Salto")]
    public float jumpHeight = 1.2f;

    [Header("Fuerza")]
    public float hitForce = 10;

    public float explosionForce = 100;
    public float explosionRadius = 5;
    
    [Header("Componentes")]
    public Animator animator;
    private CharacterController charController;
    private Vector3 playerVelocity;    
    private PlayerState state;
    public int puntos = 0;

    private GameObject holdingObject = null;

    public Text monedas;

    private GameObject player;

    void Awake()
    {
        player = GameObject.FindWithTag("Player");
    }
    // Start is called before the first frame update
    void Start() {
        charController = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
        SetState(PlayerState.Idle);
    }

    // Update is called once per frame
    void Update() {
        //Gestión de Inputs

        Vector3 movementInput = Input.GetAxisRaw("Vertical") * Vector3.forward;
        Vector2 mouseInput = Vector2.zero;
        mouseInput.x = Input.GetAxisRaw("Mouse X");

        movementInput = transform.TransformDirection(movementInput);

        //Gestión de salto
        if(Input.GetButtonDown("Jump") && charController.isGrounded) {
            //Establecemos la velocidad de salto necesaria para alcanzar la
            //altura definida en jumpHeight;

            playerVelocity.y = Mathf.Sqrt(jumpHeight * -2 * Physics.gravity.y);
            SetState(PlayerState.Jump);
        }

        if( ! charController.isGrounded) {
            if(playerVelocity.y < 0) {
                SetState(PlayerState.Fall);
            }
        }

        //Gestión de rotacion
        //Rotación del personaje en el plano horizontal
        transform.Rotate(Vector3.up * rotationSpeed * Time.deltaTime * mouseInput.x * mouseRotationSensitivity);

        //Gestión de movimiento

        if(charController.isGrounded) {
            playerVelocity.x = movementInput.x * forwardSpeed;
            playerVelocity.z = movementInput.z * forwardSpeed;
        } else {
            /*
            playerVelocity.x += movementInput.x * forwardSpeed;
            playerVelocity.x = Mathf.Clamp(playerVelocity.x, -forwardSpeed, forwardSpeed);
            playerVelocity.z += movementInput.z * forwardSpeed;
            playerVelocity.z = Mathf.Clamp(playerVelocity.z, -forwardSpeed, forwardSpeed);
            */
        }
        //Gestión de gravedad
        playerVelocity.y += Physics.gravity.y * Time.deltaTime;

       
        //Aplicamos el movimiento
        charController.Move(playerVelocity * Time.deltaTime);
        if(charController.isGrounded) {
            if((Mathf.Abs(playerVelocity.x) > 0 || Mathf.Abs(playerVelocity.z) > 0) ) {
                SetState(PlayerState.Run);
            } else {
                SetState(PlayerState.Idle);
            }
        } 
        monedas.text= puntos.ToString();
    }


    private void ApplyExplosion() {
        Collider[] affectedObjects = Physics.OverlapSphere(transform.position, explosionRadius);
        foreach(Collider c in affectedObjects) {
             Rigidbody rb = c.GetComponent<Rigidbody>();
             if(rb != null) {
                 rb.AddExplosionForce(explosionForce, transform.position, explosionRadius, 1, ForceMode.VelocityChange);
             }
        }
    }

    private void ApplyForce() {
        RaycastHit hit;
        if(Physics.Raycast(transform.position + transform.up * 0.7f, transform.forward, out hit, 1)) {
            Rigidbody rb = hit.transform.GetComponent<Rigidbody>();
            if(rb != null) {
                rb.AddForce(transform.forward * hitForce, ForceMode.Impulse);
            }
        }
    }

    private void DropObject() {
        holdingObject.transform.parent = null;
        holdingObject.GetComponent<Rigidbody>().isKinematic = false;
        holdingObject = null;
    }

    void SetState(PlayerState newState) {
        if(state != newState) {
            state = newState;  
            AnimatorClearTriggers();   
            animator.SetTrigger($"{state}");
        }
    }

    void AnimatorClearTriggers() {
        animator.ResetTrigger("Idle");
        animator.ResetTrigger("Run");
        animator.ResetTrigger("Jump");
        animator.ResetTrigger("Fall");
    }

    public void OnTriggerEnter(Collider other) {
        if(other.gameObject.CompareTag("Moneda")) {
            puntos++;
            Destroy(other.gameObject, 0.1f);
        }
    }
    public int datos(){
        return puntos;
    }
}


public enum PlayerState {
    Idle,
    Run,
    Jump,
    Fall
}
