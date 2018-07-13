using UnityEngine;
using System.Collections;

public class Crosshair : MonoBehaviour
{
    // Use this for initialization

    public enum preset { none, pistolPreset}
    public preset crosshairPreset = preset.none;

    public bool showCrosshair = true;
    public Texture2D verticalTexture;
    public Texture2D horizontalTexture;

    //Size of boxes
    public float cLength = 10.0f;
    public float cWidth = 3.0f;

    //Spreed setup
    public float minSpread = 45.0f;
    public float maxSpread = 250.0f;
    public float spreadPerSecond = 150.0f;

    //Rotation
    public float rotAngle = 0.0f;
    public float rotSpeed = 0.0f;

    [HideInInspector] public Texture2D temp;
    [HideInInspector] public float spread;

    void Start()
    {
        crosshairPreset = preset.none;
    }

    void Update()
    {
        //Rotation
        rotAngle += rotSpeed * Time.deltaTime;
    }

    public void IncreaseSpread(float multiplier)
    {
        Debug.Log("Increase spread with multiplier: " + multiplier);
        spread += spreadPerSecond * multiplier * Time.deltaTime;
    }

    public void DecreaseSpread(float multiplier)
    {
        Debug.Log("Decrease spread with multiplier: " + multiplier);
        spread -= spreadPerSecond * multiplier * Time.deltaTime;
    }

    void OnGUI()
    {
        if (showCrosshair && verticalTexture && horizontalTexture)
        {
            GUIStyle verticalT = new GUIStyle();
            GUIStyle horizontalT = new GUIStyle();
            verticalT.normal.background = verticalTexture;
            horizontalT.normal.background = horizontalTexture;
            spread = Mathf.Clamp(spread, minSpread, maxSpread);
            Vector2 pivot = new Vector2(Screen.width / 2, Screen.height / 2);

            if (crosshairPreset == preset.pistolPreset)
            {

                GUIUtility.RotateAroundPivot(45, pivot);

                //Horizontal
                GUI.Box(new Rect((Screen.width - 14) / 2, (Screen.height - spread) / 2 - 3, 14, 3), temp, horizontalT);
                GUI.Box(new Rect((Screen.width - 14) / 2, (Screen.height + spread) / 2, 14, 3), temp, horizontalT);
                //Vertical
                GUI.Box(new Rect((Screen.width - spread) / 2 - 3, (Screen.height - 14) / 2, 3, 14), temp, verticalT);
                GUI.Box(new Rect((Screen.width + spread) / 2, (Screen.height - 14) / 2, 3, 14), temp, verticalT);
            }

            if (crosshairPreset == preset.none)
            {

                GUIUtility.RotateAroundPivot(rotAngle % 360, pivot);

                //Horizontal
                GUI.Box(new Rect((Screen.width - cWidth) / 2, (Screen.height - spread) / 2 - cLength, cWidth, cLength), temp, horizontalT);
                GUI.Box(new Rect((Screen.width - cWidth) / 2, (Screen.height + spread) / 2, cWidth, cLength), temp, horizontalT);
                //Vertical
                GUI.Box(new Rect((Screen.width - spread) / 2 - cLength, (Screen.height - cWidth) / 2, cLength, cWidth), temp, verticalT);
                GUI.Box(new Rect((Screen.width + spread) / 2, (Screen.height - cWidth) / 2, cLength, cWidth), temp, verticalT);
            }
        }
    }
}
