using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMovement2DIn3D : MonoBehaviour
{
    [Header("Movement")]
    public float walkSpeed = 6f;          // base horizontal speed
    public float runSpeed = 10f;          // sprint speed when holding run key

    [Header("Jumping")]
    public float jumpForce = 12f;

    [Header("Ground Check")]
    public Transform groundCheck;         // empty object at the feet
    public float groundRadius = 0.15f;
    public LayerMask groundLayer;

    [Header("Run Input")]
    public KeyCode runKey = KeyCode.LeftShift;  // hold to run

    [Header("Optional Feel")]
    public bool strongerFall = true;      // snappier falling
    public float fallGravityMultiplier = 2f;

    private Rigidbody rb;
    private float moveInputX;
    private bool jumpPressed;
    public bool isGrounded { get; private set; }

    private float fixedZ;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();

        // Lock to an XY plane
        rb.constraints = RigidbodyConstraints.FreezePositionZ
                       | RigidbodyConstraints.FreezeRotationX
                       | RigidbodyConstraints.FreezeRotationY
                       | RigidbodyConstraints.FreezeRotationZ;

        fixedZ = transform.position.z;
    }

    void Update()
    {
        // Horizontal input only
        moveInputX = Input.GetAxisRaw("Horizontal");

        // Jump input
        if (Input.GetButtonDown("Jump"))
            jumpPressed = true;



        rb.AddForce(Physics.gravity * (fallGravityMultiplier - 1f), ForceMode.Acceleration);


        if (groundCheck != null)
            isGrounded = Physics.CheckSphere(groundCheck.position, groundRadius, groundLayer, QueryTriggerInteraction.Ignore);

        float currentSpeed = Input.GetKey(runKey) && isGrounded ? runSpeed : walkSpeed;


        Vector3 v = rb.velocity;
        v.x = moveInputX * currentSpeed;
        rb.velocity = v;

        if (jumpPressed && isGrounded)
        {
            rb.velocity = new Vector3(rb.velocity.x, jumpForce, 0f);
        }
        jumpPressed = false;
        //penis

        Vector3 pos = rb.position;
        if (!Mathf.Approximately(pos.z, fixedZ))
            rb.position = new Vector3(pos.x, pos.y, fixedZ);
    }

    void FixedUpdate()
    {

    }

    void OnDrawGizmosSelected()
    {
        if (groundCheck == null) return;
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(groundCheck.position, groundRadius);
    }
}
