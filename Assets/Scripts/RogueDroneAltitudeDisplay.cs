using UnityEngine;
using UnityEngine.UI;

public class RogueDroneAltitudeDisplay : MonoBehaviour
{
    public Text altitudeText;
    public Text warningText; // Assign in inspector

    private string currentHeading = "";

    void Start()
    {
        if (warningText != null)
            warningText.enabled = false;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.name.Contains("_Plane"))
        {
            string[] parts = other.name.Split('_');
            if (parts.Length >= 2)
            {
                string heading = parts[0];
                string altitude = parts[1];

                currentHeading = heading;
                altitudeText.text = $"Altitude: {altitude}  Heading: {heading}";

                CheckDirection(heading);
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.name.Contains("_Plane"))
        {
            // Hide warning when exiting the lane
            if (warningText != null)
                warningText.enabled = false;

            altitudeText.text = "Altitude: -  Heading: -";
        }
    }

    void CheckDirection(string heading)
    {
        Vector3 velocity = GetComponent<Rigidbody>().linearVelocity;

        bool wrongDir = false;

        switch (heading)
        {
            case "North": wrongDir = velocity.z < 0; break;
            case "South": wrongDir = velocity.z > 0; break;
            case "East": wrongDir = velocity.x < 0; break;
            case "West": wrongDir = velocity.x > 0; break;
            case "NorthEast": wrongDir = velocity.z < 0 || velocity.x < 0; break;
            case "NorthWest": wrongDir = velocity.z < 0 || velocity.x > 0; break;
            case "SouthEast": wrongDir = velocity.z > 0 || velocity.x < 0; break;
            case "SouthWest": wrongDir = velocity.z > 0 || velocity.x > 0; break;
        }

        // 👇 Add this debug log
    Debug.Log($"Velocity: {velocity} | Heading: {heading} | WrongDir: {wrongDir}");

        if (warningText != null)
            warningText.enabled = wrongDir;
    }
}
