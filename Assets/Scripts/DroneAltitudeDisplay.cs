using UnityEngine;
using UnityEngine.UI;

public class DroneAltitudeDisplay : MonoBehaviour
{
    public Text altitudeText; // Assign in Inspector

    void OnTriggerStay(Collider other)
    {
        if (other.name.Contains("_Plane"))
        {
            string[] parts = other.name.Split('_');
            if (parts.Length >= 2)
            {
                string heading = parts[0];
                string altitude = parts[1];
                altitudeText.text = $"Altitude: {altitude}   Heading: {heading}";
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.name.Contains("_Plane"))
        {
            // Clear the display when leaving a lane
            altitudeText.text = "Altitude: -   Heading: -";
        }
    }
}
