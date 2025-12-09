using UnityEngine;

public class CameraSwitcher : MonoBehaviour
{
    public Camera[] cameras;
    public int currentCameraIndex = 0;

    public void SwitchCamera(int index)
    {
        if (index < 0 || index >= cameras.Length)
            return;

        // 모든 카메라 끄기
        foreach (Camera cam in cameras)
            cam.enabled = false;

        // 선택한 카메라 켜기
        cameras[index].enabled = true;
        currentCameraIndex = index;
    }
}
