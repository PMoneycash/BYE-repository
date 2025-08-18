using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Animator))]
public class PlayerMovement2DIn3D : MonoBehaviour
{
    [Header("Movement")]
    public float walkSpeed = 6f;
    public float runSpeed = 10f;

    [Header("Jumping")]
    public float jumpForce = 12f;

    [Header("Ground Check")]
    public Transform groundCheck;      // empty at feet
    public float groundRadius = 0.15f;
    public LayerMask groundLayer;

    [Header("Run Input")]
    public KeyCode runKey = KeyCode.LeftShift;

    [Header("Optional Feel")]
    public bool strongerFall = true;
    public float fallGravityMultiplier = 2f; // 2 = twice as fast on the way down

    Rigidbody rb;
    Animator anim;

    float moveInputX;
    bool jumpPressed;
    public bool isGrounded;

    public Transform model;   // the visual child with the Animator

    Vector3 baseScale;



    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();

        // Lock to XY plane and keep the “paper-doll” upright.
        rb.constraints =
            RigidbodyConstraints.FreezePositionZ |
            RigidbodyConstraints.FreezeRotationX |
            RigidbodyConstraints.FreezeRotationY |
            RigidbodyConstraints.FreezeRotationZ;


        if (!model) model = transform;        // if you put this on the model itself
        if (!rb) rb = GetComponent<Rigidbody>();
        baseScale = model.localScale;


    }

    void Update()
    {

        float dir = 0f;

        // Use input OR velocity – pick one
        if (Input.GetKey(KeyCode.A)) dir = 1f;
        else if (Input.GetKey(KeyCode.D)) dir = -1f;
        else if (rb) dir = rb.velocity.x * -1f;     // fallback: face move direction

        if (dir > 0.05f)
            model.localScale = new Vector3(Mathf.Abs(baseScale.x), baseScale.y, baseScale.z);   // face right
        else if (dir < -0.05f)
            model.localScale = new Vector3(-Mathf.Abs(baseScale.x), baseScale.y, baseScale.z);


        // --- Read inputs in Update ---
        moveInputX = Input.GetAxisRaw("Horizontal");
        if (Input.GetButtonDown("Jump"))
            jumpPressed = true;

        // --- Lightweight checks that don’t touch physics ---
        if (groundCheck != null)
            isGrounded = Physics.CheckSphere(
                groundCheck.position, groundRadius, groundLayer, QueryTriggerInteraction.Ignore
            );

        // Animator params
        float horizontalSpeed = Mathf.Abs(rb.velocity.x) * 5f;
        anim.SetFloat("Speed", horizontalSpeed);
        anim.SetBool("IsGrounded", isGrounded);
    }

    void FixedUpdate()
    {
        // Horizontal movement (physics write happens in FixedUpdate)
        float targetSpeed = (Input.GetKey(runKey) && isGrounded) ? runSpeed : walkSpeed;

        Vector3 v = rb.velocity;
        v.x = moveInputX * targetSpeed;
        rb.velocity = v;

        // Jump
        if (jumpPressed && isGrounded)
        {
            rb.velocity = new Vector3(rb.velocity.x, jumpForce, 0f);
        }
        jumpPressed = false;

        // Stronger fall only while descending
        if (strongerFall && rb.velocity.y < 0f)
        {
            rb.AddForce(Physics.gravity * (fallGravityMultiplier - 1f), ForceMode.Acceleration);
        }
    }

    void OnDrawGizmosSelected()
    {
        if (groundCheck == null) return;
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(groundCheck.position, groundRadius);
    }
}
