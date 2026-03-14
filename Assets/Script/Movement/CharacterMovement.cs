using System.Collections;
using Unity.Netcode;
using UnityEngine;

public class CharacterMovement : NetworkBehaviour
{
    [SerializeField] private Rigidbody rb;
    private CharacterController characterController; 
    
    private Vector2 movementInput = Vector2.zero; 

    [Header("Movement Stats")]
    [SerializeField] private float moveSpeed = 10f;
    [SerializeField] private float dashPower = 15f;
    [SerializeField] private float dashDuration = 0.25f;
    [SerializeField] private float rotationSpeed = 15f; 

    private bool canDash = true;
    private bool isDashing = false;

    private readonly NetworkVariable<Vector3> _networkPosition = new NetworkVariable<Vector3>(
        default, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server
    ); 
    private float lastNetworkUpdate;
    private const float NetworkUpdateInterval = 0.05f; 

    private void Awake()
    {
        characterController = GetComponent<CharacterController>();
        rb = GetComponent<Rigidbody>();
        
        if (rb == null) Debug.LogError("Rigidbody (3D) not found!");
        
        rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
    }

    private void Start()
    {
        characterController.OnMoveEvent.AddListener(OnMoveInput);
        characterController.OnDash.AddListener(RequestDashServerRpc);
    }

    private void Update()
    {
        if (!IsOwner)
        {
            transform.position = Vector3.Lerp(transform.position, _networkPosition.Value, Time.deltaTime * 15f);
        }
    }

    private void FixedUpdate()
    {
        if (!IsOwner) return;

        if (!isDashing)
        {
            HandleMovementAndRotation();
        }

        if (Time.time - lastNetworkUpdate >= NetworkUpdateInterval)
        {
            lastNetworkUpdate = Time.time;
            UpdatePositionServerRpc(rb.position);
        }
    }

    private void HandleMovementAndRotation()
    {
        Vector3 moveDir = new Vector3(movementInput.x, 0f, movementInput.y).normalized;

        rb.velocity = new Vector3(moveDir.x * moveSpeed, rb.velocity.y, moveDir.z * moveSpeed);

        if (moveDir != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(moveDir);
            rb.rotation = Quaternion.Slerp(rb.rotation, targetRotation, rotationSpeed * Time.fixedDeltaTime);
        }
    }

    private void OnMoveInput(Vector2 input)
    {
        movementInput = input; 
    }

    [ServerRpc]
    private void UpdatePositionServerRpc(Vector3 newPosition)
    {
        _networkPosition.Value = newPosition;
    }

    [ServerRpc(RequireOwnership = false)]
    private void RequestDashServerRpc()
    {
        PerformDashClientRpc();
    }

    [ClientRpc]
    private void PerformDashClientRpc()
    {
        if (!IsOwner) return;
        StartCoroutine(DashCoroutine());
    }

    private IEnumerator DashCoroutine()
    {
        if (!canDash) yield break;

        canDash = false;
        isDashing = true;

        Vector3 dashDir = new Vector3(movementInput.x, 0f, movementInput.y).normalized;
        
        if (dashDir == Vector3.zero) dashDir = transform.forward;

        rb.velocity = new Vector3(0, rb.velocity.y, 0); 
        rb.AddForce(dashDir * dashPower, ForceMode.Impulse); 

        yield return new WaitForSeconds(dashDuration);

        isDashing = false;
        canDash = true;
    }
}