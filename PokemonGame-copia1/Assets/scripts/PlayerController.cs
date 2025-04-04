using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    [Header("References")]
    public Camera playerCamera;
    private Animator animator;          // Referencia al Animator

    [Header("General")]
    public float gravityScale = -9.81f; // Gravedad realista

    [Header("Movement")]
    public float walkSpeed = 5f;
    public float runSpeed = 10f;

    [Header("Rotation")]
    public float rotationSensitivity = 200f;

    [Header("Jump")]
    public float jumpHeight = 1.9f;

    private float cameraVerticalAngle = 0f;
    private Vector3 moveInput = Vector3.zero;
    private CharacterController characterController;

    private void Awake()
    {
        characterController = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();   // Inicializa el Animator

        if (playerCamera == null)
        {
            Debug.LogError("No se ha asignado una cámara al jugador.");
        }

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        Look();
        Move();
    }

    private void Move()
    {
        if (characterController.isGrounded)
        {
            bool isRunning = Input.GetButton("Sprint");
            float speed = isRunning ? runSpeed : walkSpeed;

            // Obtener la dirección de movimiento
            Vector3 inputDirection = new Vector3(Input.GetAxis("Horizontal"), 0f, Input.GetAxis("Vertical"));

            // Normalizar el vector de entrada si su magnitud es mayor a 1
            if (inputDirection.magnitude > 1)
            {
                inputDirection.Normalize();
            }

            // Aplicar la dirección con la velocidad
            inputDirection = transform.TransformDirection(inputDirection) * speed;

            // Aplicar movimiento
            moveInput.x = inputDirection.x;
            moveInput.z = inputDirection.z;

            // 🔥 Actualizar parámetros del Animator
            animator.SetFloat("VelX", Input.GetAxis("Horizontal"));  // Movimiento lateral
            animator.SetFloat("VelY", Input.GetAxis("Vertical"));    // Movimiento hacia adelante/atrás
            animator.SetBool("isRunning", isRunning); // Cambia entre caminar y correr

            // Salto
            if (Input.GetButtonDown("Jump"))
            {
                moveInput.y = Mathf.Sqrt(jumpHeight * -2f * gravityScale);
                animator.SetBool("jump", true);  // Activa la animación de salto
            }
            else
            {
                animator.SetBool("jump", false); // Desactiva la animación de salto
            }
        }

        // Aplicar gravedad cuando el jugador no está en el suelo
        if (!characterController.isGrounded)
        {
            moveInput.y += gravityScale * Time.deltaTime;
        }

        characterController.Move(moveInput * Time.deltaTime);
    }

    private void Look()
    {
        float mouseX = Input.GetAxis("Mouse X") * rotationSensitivity * Time.deltaTime;
        transform.Rotate(Vector3.up * mouseX);

        float mouseY = Input.GetAxis("Mouse Y") * rotationSensitivity * Time.deltaTime;
        cameraVerticalAngle -= mouseY;
        cameraVerticalAngle = Mathf.Clamp(cameraVerticalAngle, -80f, 80f);

        if (playerCamera != null)
        {
            playerCamera.transform.localRotation = Quaternion.Euler(cameraVerticalAngle, 0f, 0f);
        }
    }
}