using UnityEngine;
using System.Collections;

public class Drone3Autopilot : MonoBehaviour
{
    public GameObject rogueDrone;

    public float speed = 5f;
    public float bufferRadius = 3f;
    public float waitTime = 5f;

    private Vector3[] path;
    private int targetStep = 0;
    private bool isWaiting = false;

    void Start()
    {
        // NW = Y=11, SE = Y=7
        path = new Vector3[]
        {
            new Vector3(-10f, 11f, 20f),  // Step 1: Ascend to NW lane
            new Vector3(-30f, 11f, 40f),  // Step 2: Fly NW
            new Vector3(-30f, 0f, 40f),   // Step 3: Land
            new Vector3(-30f, 7f, 40f),   // Step 4: Ascend to SE lane
            new Vector3(-10f, 7f, 20f),   // Step 5: Fly SE
            new Vector3(-10f, 0f, 20f)    // Step 6: Land again
        };
    }

    void Update()
    {
        if (isWaiting) return;

        // STEP 1: Yield logic
        if (rogueDrone != null && Vector3.Distance(transform.position, rogueDrone.transform.position) < bufferRadius)
        {
            Debug.Log("Drone_3 yielding to rogue drone.");
            return;
        }

        // STEP 2: Move to target
        Vector3 target = path[targetStep];
        float step = speed * Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, target, step);

        // STEP 3: If reached, update path index
        if (Vector3.Distance(transform.position, target) < 0.1f)
        {
            // Pause after landings
            if (targetStep == 2 || targetStep == 5)
                StartCoroutine(PauseBeforeNext());

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
