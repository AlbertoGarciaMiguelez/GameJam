using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{

    [Header("Movimiento horizontal")]
    public float forwardSpeed= 5f;    

    [Header("Movimiento de rotación")]
    public float mouseRotationSensitivity = 2;
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
    public GameObject antorcha;
    public Transform posicionAntorcha;

    public static Player instance;
    
    public Transform Target;
    public Transform Target2;
    public Transform Target3;

    public Transform spawn;

    private bool power=false;
    private bool point=false;
    private bool objeto=false;

    void Awake(){
        instance=this;
    }
    // Start is called before the first frame update
    void Start() {
        charController = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
        SetState(PlayerState.Idle);
    }

    // Update is called once per frame
    void Update() {
        Debug.Log(puntos);
        //Gestión de Inputs
        if(puntos==5){
            FinNivel();
        }
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

        if(Input.GetButtonDown("Interaction")) {
            if(holdingObject == null) {
                PickObject();
            } else {
                DropObject();
            }

        }
        if(power){
            transform.localScale = new Vector3(0.18f,0.18f,0.18f);
            power=false;
        }

    }

    public bool powerOn(){
        if(power){
            return true;
        }else{
            return false;
        }
    }
    public bool pointOn(){
        if(point){
            return true;
        }else{
            return false;
        }
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
    public Vector3 position(){
        return transform.position;
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

    private void PickObject() {
        RaycastHit hit;
        if (Physics.SphereCast(transform.position, charController.height/20, transform.forward, out hit, 1))
         {
            Debug.Log("Dentro2");
            if(hit.transform.gameObject.CompareTag("Pickable")) {
                Debug.Log("Player.PickObject Tengo algo Pickable delante de mi");
                //Cogemos el objeto
                objeto=true;
                holdingObject = hit.transform.gameObject;
                holdingObject.transform.parent = transform;
                holdingObject.transform.localPosition = new Vector3(0f, 2f, 1f);
                holdingObject.transform.localEulerAngles = Vector3.zero;
                holdingObject.GetComponent<Rigidbody>().isKinematic = true;
            }
        }
        
    }
    
    private void DropObject() {
        holdingObject.transform.parent = null;
        holdingObject.GetComponent<Rigidbody>().isKinematic = false;
        holdingObject = null;
        objeto=false;
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
            if(puntos==3){
                transform.position = Target3.transform.position;
            }
        }
        else if(other.gameObject.CompareTag("Power")){
            power=true;
            Destroy(other.gameObject, 0.1f);
        }
        else if(other.gameObject.CompareTag("Platform")) {
            transform.parent = other.transform;
        }
        else if(other.gameObject.CompareTag("Portal")) {
            transform.position = Target.transform.position;
        }
        else if(other.gameObject.CompareTag("Point")) {
            point=true;
        }
        else if(other.gameObject.CompareTag("Portal2")) {
            transform.position = Target2.transform.position;
        }
        else if(other.gameObject.CompareTag("Vuelta")) {
            transform.position = Target.transform.position;
        }
        else{
            Debug.Log("Normal");
        }
    }
    
    public void OnTriggerExit(Collider other) {
        if(other.gameObject.CompareTag("Platform")) {
            transform.parent = null;

            Speedometer speedometer = other.GetComponent<Speedometer>();
            if(speedometer != null) {
                playerVelocity += speedometer.velocity;
            }
        }
    }

    public int datos(){
        return puntos;
    }
    public void Respawn(){
        transform.position = spawn.transform.position;
        if(!objeto){
            antorcha.transform.position=posicionAntorcha.transform.position;
        }
    }
    public void FinNivel(){
        SceneManager.LoadScene("Fin");
    }
}


public enum PlayerState {
    Idle,
    Run,
    Jump,
    Fall
}