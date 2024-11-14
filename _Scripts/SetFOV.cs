using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SetFOV : MonoBehaviour
{
    public Slider fovSlider; // Reference to the fov slider
    public float fovValue; // Value that will be entered into the "size" field of the main camera

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    public void SetCamera()
    {
        fovValue = (fovSlider.value * 0.6f) + 0.2f; // Do some math to transform from the slider scale (0 to 1) to the camera size scale (0.2 to 0.8)

        PlayerPrefs.SetFloat("fovValue", fovValue);
    }
}
