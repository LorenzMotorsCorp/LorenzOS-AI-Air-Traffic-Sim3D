using UnityEngine;

public class RogueDroneController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float verticalSpeed = 3f;

    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        if (rb == null)
        {
            Debug.LogError("Rigidbody not found on Player_Rogue_Drone");
        }
    }

    void FixedUpdate()
    {
        float horizontal = Input.GetAxis("Horizontal");  // A/D or Left/Right
        float vertical = Input.GetAxis("Vertical");      // W/S or Up/Down
        float height = 0;

        // Q to go down, E to go up
        if (Input.GetKey(KeyCode.E)) height = 1;
        else if (Input.GetKey(KeyCode.Q)) height = -1;

        // Build movement vector
        Vector3 moveDirection = new Vector3(horizontal, height * verticalSpeed / moveSpeed, vertical).normalized;

        // Move using Rigidbody
        rb.linearVelocity = moveDirection * moveSpeed;
    }
}
