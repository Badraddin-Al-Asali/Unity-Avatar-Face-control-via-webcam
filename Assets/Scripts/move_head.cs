using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class move_head : MonoBehaviour
{
    public Transform myHead;
    private bool mySwitch = true;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ttestt()
    {
        if (mySwitch)
            myHead.localEulerAngles = new Vector3(0, 90, 0);
        else
            myHead.localEulerAngles = new Vector3(0.0f, -0.003f, -7.715f);
        mySwitch = !mySwitch;
    }
}
