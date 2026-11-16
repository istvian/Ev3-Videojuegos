using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.SocialPlatforms.Impl;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 6f;
    [SerializeField] private float runSpeed = 10f;
    [SerializeField] private float rotationSpeed = 10f;
    [SerializeField] private float jumpForce = 5f;

    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundCheckRadius = 0.2f;
    [SerializeField] private LayerMask groundMask;
    [SerializeField] Animator animator;

    private Rigidbody rb;
    private Vector3 moveInput;
    private bool jumpRequested;
    private bool isGrounded;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        Vector3 forward = Vector3.forward;
        Vector3 right = Vector3.right;

        moveInput = forward * vertical + right * horizontal;
        if (moveInput.sqrMagnitude > 1f)
        {
            moveInput.Normalize();
        }

        if (Input.GetKeyDown(KeyCode.X) && isGrounded)
        {
            Vector3 velocity = rb.linearVelocity;
            velocity.y = 0f;
            rb.linearVelocity = velocity;
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }

        SetAnimations();
    }

    private void FixedUpdate()
    {
        CheckGrounded();
        Move();
    }
    private void SetAnimations()
    {
        if (isGrounded)
        {
            animator.SetBool("IsJumping", false);
            animator.SetFloat("Velocity", Mathf.Clamp01(rb.linearVelocity.magnitude));
        }
        else
        {
            animator.SetBool("IsJumping", true);
        }

    }

    private void Move()
    {
        Vector3 targetVelocity = Vector3.zero;
        if (Input.GetKey(KeyCode.Z)) // Si esta corriendo
        {
            targetVelocity = moveInput * runSpeed;
        }
        else
        {
            targetVelocity = moveInput * moveSpeed;
        }
        Vector3 velocity = rb.linearVelocity;
        rb.linearVelocity = new Vector3(targetVelocity.x, velocity.y, targetVelocity.z);

        rb.angularVelocity = Vector3.zero;
        if (moveInput.sqrMagnitude > 0.0001f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(moveInput, Vector3.up);
            Quaternion newRotation = Quaternion.Slerp(rb.rotation, targetRotation, rotationSpeed * Time.fixedDeltaTime);

            rb.MoveRotation(newRotation);
        }
    }



    private void CheckGrounded()
    {
        if (groundCheck != null)
        {
            isGrounded = Physics.CheckSphere(groundCheck.position, groundCheckRadius, groundMask, QueryTriggerInteraction.Ignore);
        }
    }

    void OnDrawGizmos()
    {
        if (groundCheck != null)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
        }
        Gizmos.DrawLine(transform.position, transform.position + transform.forward * .5f);
    }

    void OnTriggerEnter(Collider other)
    {
        Debug.Log("Objeto recolectado:" + other.gameObject.name);
        other.gameObject.SetActive(false);
        if (other.CompareTag("WorldLimit"))
        {
            ScoreManager.instance.ReloadScene();
        }
        if (other.CompareTag("Coleccionable"))
        {
            ScoreManager.instance.ColeccionableNormal();
        }
        if (other.CompareTag("ItemEspecial"))
        {
            ScoreManager.instance.ColeccionableEspecial();
        }
        if (other.CompareTag("ItemTrampa"))
        {
            ScoreManager.instance.ColeccionableTrampa();
        }

    }

}
