using UnityEngine;
using System.Collections;

public class DroneAutopilot : MonoBehaviour
{
    public GameObject rogueDrone; // Assign your rogue drone in the Inspector

    public float speed = 5f;
    public float bufferRadius = 3f;
    public float waitTime = 5f;

    private Vector3[] path;
    private int targetStep = 0;
    private bool isWaiting = false;

    void Start()
    {
        // Full roundtrip path: East lane → land → West lane → land → loop
        path = new Vector3[]
        {
            new Vector3(-10f, 6f, 0f),   // Step 1: Ascend to East lane
            new Vector3(10f, 6f, 0f),    // Step 2: Fly East
            new Vector3(10f, 0f, 0f),    // Step 3: Land
            new Vector3(10f, 10f, 0f),   // Step 4: Ascend to West lane (corrected Y=10)
            new Vector3(-10f, 10f, 0f),  // Step 5: Fly West
            new Vector3(-10f, 0f, 0f)    // Step 6: Land
        };
    }

    void Update()
    {
        if (isWaiting) return;

        // STEP 1: Interrupt — stop if rogue drone is within buffer radius
        if (rogueDrone != null && Vector3.Distance(transform.position, rogueDrone.transform.position) < bufferRadius)
        {
            Debug.Log("Yielding to rogue drone.");
            return;
        }

        // STEP 2: Move toward current target
        Vector3 target = path[targetStep];
        float step = speed * Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, target, step);

        // STEP 3: Reached current target
        if (Vector3.Distance(transform.position, target) < 0.1f)
        {
            // Pause only after first landing (Step 3) and second landing (Step 6)
            if (targetStep == 2 || targetStep == 5)
            {
                StartCoroutine(PauseBeforeNext());
            }

            // Advance step (loop back to 0 if at end)
            targetStep = (targetStep + 1) % path.Length;
        }
    }

    IEnumerator PauseBeforeNext()
    {
        isWaiting = true;
        yield return new WaitForSeconds(waitTime);
        isWaiting = false;
    }
}
