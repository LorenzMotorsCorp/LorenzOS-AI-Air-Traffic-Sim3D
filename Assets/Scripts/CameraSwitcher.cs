using UnityEngine;
using UnityEngine.UI;

public class CameraSwitcher : MonoBehaviour
{
    public Camera[] droneCameras;
    public GameObject[] altitudeDisplays;

    private int currentDroneIndex = 0;

    void Start()
    {
        // Disable all cameras and all UI displays on game start
        for (int i = 0; i < droneCameras.Length; i++)
        {
            droneCameras[i].gameObject.SetActive(false);
            altitudeDisplays[i].SetActive(false);
        }

        // Then activate the first one
        SwitchToDrone(0);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1)) SwitchToDrone(0);
        if (Input.GetKeyDown(KeyCode.Alpha2)) SwitchToDrone(1);
        if (Input.GetKeyDown(KeyCode.Alpha3)) SwitchToDrone(2);
        if (Input.GetKeyDown(KeyCode.Alpha4)) SwitchToDrone(3);
        if (Input.GetKeyDown(KeyCode.Alpha5)) SwitchToDrone(4);
    }

    void SwitchToDrone(int index)
    {
        if (index < 0 || index >= droneCameras.Length) return;

        // Deactivate all
        for (int i = 0; i < droneCameras.Length; i++)
        {
            droneCameras[i].gameObject.SetActive(false);
            altitudeDisplays[i].SetActive(false);
        }

        // Activate selected
        droneCameras[index].gameObject.SetActive(true);
        altitudeDisplays[index].SetActive(true);

        currentDroneIndex = index;
    }
}
