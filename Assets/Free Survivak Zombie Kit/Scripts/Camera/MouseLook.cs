using UnityEngine;
using System.Collections;

public class MouseLook : MonoBehaviour
{
    public GameObject player;
    public float xSensitivity = 10;
    public float ySensitivity = 10;
    public float smoothing = 0.4f;
    public int min = -60;
    public int max = 60;

    private float mouseOffsetY;
    private float mouseOffsetX;

    private float xtargetRotation = 10;
    private float ytargetRotation = 10;

    void Update()
    {
        mouseOffsetY = Input.GetAxis("Mouse Y") * ySensitivity;
        ytargetRotation += -mouseOffsetY;
        ytargetRotation = ytargetRotation % 360;
        ytargetRotation = Mathf.Clamp(ytargetRotation, min, max);

        mouseOffsetX = Input.GetAxis("Mouse X") * xSensitivity;
        xtargetRotation += mouseOffsetX;
        xtargetRotation = xtargetRotation % 360;

        transform.localRotation = Quaternion.Lerp(transform.localRotation, Quaternion.Euler(ytargetRotation, 0, 0), Time.deltaTime * 10 / smoothing);
        player.transform.rotation = Quaternion.Lerp(player.transform.rotation, Quaternion.Euler(0, xtargetRotation, 0), Time.deltaTime * 10 / smoothing);
    }
}