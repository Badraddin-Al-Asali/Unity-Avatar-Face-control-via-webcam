using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Change_camera_pos : MonoBehaviour
{
    public Camera cam;
    private Vector3[,] camPos = new Vector3[1,2];

    // Start is called before the first frame update
    void Start()
    {
        camPos[0, 0].Set(0.5f, 0.5f, 0.5f);
        camPos[0, 1].Set(0.0f, 0.2f, 0.0f);
    }

    public void setCameraPos(int position)
    {
        cam.transform.position = camPos[position, 0];
        cam.transform.Rotate(camPos[position, 1]);
    }
}
