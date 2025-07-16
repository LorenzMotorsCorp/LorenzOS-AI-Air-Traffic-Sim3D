using UnityEngine;
using System.Collections;

public class Drone4Autopilot : MonoBehaviour
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
        // NE lane = y=5, SW lane = y=9
        path = new Vector3[]
        {
            new Vector3(-10f, 5f, 30f),   // Step 1: Ascend to NE lane
            new Vector3(10f, 5f, 50f),    // Step 2: Fly NE
            new Vector3(10f, 0f, 50f),    // Step 3: Land
            new Vector3(10f, 9f, 50f),    // Step 4: Ascend to SW lane
            new Vector3(-10f, 9f, 30f),   // Step 5: Fly SW
            new Vector3(-10f, 0f, 30f)    // Step 6: Land again
        };
    }

    void Update()
    {
        if (isWaiting) return;

        // STEP 1: Yield if rogue drone is close
        if (rogueDrone != null && Vector3.Distance(transform.position, rogueDrone.transform.position) < bufferRadius)
        {
            Debug.Log("Drone_4 yielding to rogue drone.");
            return;
        }

        // STEP 2: Move toward current target
        Vector3 target = path[targetStep];
        float step = speed * Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, target, step);

        // STEP 3: Reached waypoint
        if (Vector3.Distance(transform.position, target) < 0.1f)
        {
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
