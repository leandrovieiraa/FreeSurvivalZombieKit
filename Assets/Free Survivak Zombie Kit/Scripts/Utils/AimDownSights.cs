using UnityEngine;

public class AimDownSights : MonoBehaviour
{
    public PlayerController playerController;

    public float defaultFOV = 60.0F;
    public float aimedFOV = 45.0F;
    public float smoothFOV = 10.0F;

    public Vector3 hipPosition;
    public Vector3 aimPosition;
    public float smoothAim = 12.5F;

    public Camera camera;

    void Update()
    {
        if (playerController.inventory.activeSelf)
            return;

        if (Input.GetMouseButton(1))
        {
            transform.localPosition = Vector3.Lerp(transform.localPosition, aimPosition, Time.deltaTime * smoothAim);
            camera.fieldOfView = Mathf.Lerp(camera.fieldOfView, aimedFOV, Time.deltaTime * smoothFOV);
        }
        else
        {
            transform.localPosition = Vector3.Lerp(transform.localPosition, hipPosition, Time.deltaTime * smoothAim);
            camera.fieldOfView = Mathf.Lerp(camera.fieldOfView, defaultFOV, Time.deltaTime * smoothFOV);
        }
    }
}