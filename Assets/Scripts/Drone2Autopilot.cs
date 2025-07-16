using UnityEngine;
using System.Collections;

public class Drone2Autopilot : MonoBehaviour
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
        // North lane is Y=4, South lane is Y=8
        path = new Vector3[]
        {
            new Vector3(-10f, 4f, 10f),   // Step 1: Ascend to North lane
            new Vector3(-10f, 4f, 30f),   // Step 2: Fly North
            new Vector3(-10f, 0f, 30f),   // Step 3: Land
            new Vector3(-10f, 8f, 30f),   // Step 4: Ascend to South lane
            new Vector3(-10f, 8f, 10f),   // Step 5: Fly South
            new Vector3(-10f, 0f, 10f)    // Step 6: Land again
        };
    }

    void Update()
    {
        if (isWaiting) return;

        // STEP 1: Interrupt — stop if rogue drone is within buffer radius
        if (rogueDrone != null && Vector3.Distance(transform.position, rogueDrone.transform.position) < bufferRadius)
        {
            Debug.Log("Drone_2 yielding to rogue drone.");
            return;
        }

        // STEP 2: Move toward current target
        Vector3 target = path[targetStep];
        float step = speed * Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, target, step);

        // STEP 3: Reached target
        if (Vector3.Distance(transform.position, target) < 0.1f)
        {
            // Pause after first and second landings
            if (targetStep == 2 || targetStep == 5)
            {
                StartCoroutine(PauseBeforeNext());
            }

            // Advance to next step, loop forever
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
