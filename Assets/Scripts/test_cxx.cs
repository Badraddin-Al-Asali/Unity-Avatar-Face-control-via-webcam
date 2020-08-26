using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.InteropServices;
using UnityEngine.UI;

public class test_cxx : MonoBehaviour
{
    public RawImage myDisplay;
    public RawImage myDisplay3DFace;
    public Transform myHead;
    public Transform myJaw;
    public Transform myLeftEyeLid;
    public Transform myRightEyeLid;
    public Transform myLeftEyeBrow;
    public Transform myRightEyeBrow;
    private Vector3 orgHead;
    private Vector3 orgJaw;
    private Vector3 orgLeftEyeLid;
    private Vector3 orgRightEyeLid;
    private Vector3 orgLeftEyeBrow;
    private Vector3 orgRightEyeBrow;
    
/*#if UNITY_IPHONE || UNITY_XBOX360
 
   // On iOS and Xbox 360 plugins are statically linked into
   // the executable, so we have to use __Internal as the
   // library name.
   [DllImport ("__Internal")]
 
#else

    // Other platforms load plugins dynamically, so pass the name
    // of the plugin's dynamic library.
    [DllImport(@"C:\Users\Andy BadMan\source\repos\Dll2\x64\Release\Dll2.dll")]
    private static extern int FooPluginFunction();
    [DllImport(@"C:\Users\Andy BadMan\source\repos\Dll2\x64\Release\Dll2.dll")]
    private static extern double getX();
    [DllImport(@"C:\Users\Andy BadMan\source\repos\Dll2\x64\Release\Dll2.dll")]
    private static extern double getY();
    [DllImport(@"C:\Users\Andy BadMan\source\repos\Dll2\x64\Release\Dll2.dll")]
    private static extern double getZ();
    [DllImport(@"C:\Users\Andy BadMan\source\repos\Dll2\x64\Release\Dll2.dll")]
    private static extern double getJawY();
    [DllImport(@"C:\Users\Andy BadMan\source\repos\Dll2\x64\Release\Dll2.dll")]
    private static extern void stopCapture(bool set);
#endif*/

    static IntPtr nativeLibraryPtr;

    struct MyImageStruct
    {
        public int width { get; set; }
        public int height { get; set; }
        public int channels { get; set; }
        public IntPtr data { get; set; }
        public IntPtr data2 { get; set; }
    }

    delegate int FooPluginFunction();
    delegate int FooPluginFunctionTemp();
    delegate double getX();
    delegate double getY();
    delegate double getZ();
    delegate double getJawY();
    delegate double getLeftEyelidY();
    delegate double getRightEyelidY();
    delegate double getLeftEyeBrowY();
    delegate double getRightEyeBrowY();
    delegate MyImageStruct getMyImage();
    delegate void stopCapture(bool set);
    bool stop = true;
    MyImageStruct myImage;
    void Awake()
    {
        if (nativeLibraryPtr != IntPtr.Zero) return;

        nativeLibraryPtr = Native.LoadLibrary(@"C:\Users\Andy BadMan\source\repos\Dll2\x64\Release\Dll2.dll");
        if (nativeLibraryPtr == IntPtr.Zero)
        {
            Debug.LogError("Failed to load native library");
        }
    }
    //private static extern int CapPluginFunction();
    public void callThis()
    {
        // Calls the FooPluginFunction inside the plugin
        // And prints 5 to the console
        //Debug.Log("Capture: " + CapPluginFunction().ToString());
        stop = !stop;
        Debug.Log("Capture: " + stop);
        Native.Invoke<stopCapture>(nativeLibraryPtr, stop);
    }

    void OnApplicationQuit()
    {
        Native.Invoke<stopCapture>(nativeLibraryPtr, true);
        if (nativeLibraryPtr == IntPtr.Zero) return;

        Debug.Log(Native.FreeLibrary(nativeLibraryPtr)
                      ? "Native library successfully unloaded."
                      : "Native library could not be unloaded.");
        Debug.Log("Quit");
    }

    void Start()
    {
        orgHead = myHead.localEulerAngles;
        //orgHead = myHead.eulerAngles;
        orgJaw = myJaw.localEulerAngles;
        orgLeftEyeLid = myLeftEyeLid.localEulerAngles;
        orgRightEyeLid = myRightEyeLid.localEulerAngles;

        orgLeftEyeBrow = myLeftEyeBrow.localPosition;
        orgRightEyeBrow = myRightEyeBrow.localPosition;
    }

    public void nextFrame()
    {
        int ret = Native.Invoke<int, FooPluginFunctionTemp>(nativeLibraryPtr, null);
        Debug.Log("Capture: " + ret.ToString());
        if (ret >= 0)
        {
            myImage = Native.Invoke<MyImageStruct, getMyImage>(nativeLibraryPtr, null);
            TextureFormat textFormate = TextureFormat.RGB24;
            //if (myImage.channels == 1)
            //{
            //    textFormate = TextureFormat.R8;
            //}
            Texture2D text = new Texture2D(myImage.width, myImage.height, textFormate, false);
            // Load data into the texture and upload it to the GPU.
            text.LoadRawTextureData(myImage.data, myImage.width * myImage.height * myImage.channels);
            text.Apply();

            // Assign texture to renderer's material.
            myDisplay.texture = text;
        }
        else
        {
            Debug.Log("Capture: " + ret.ToString());
        }
    }

    void LateUpdate()
    {
        //@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@
        return;
        if(!stop)
        {
            int ret = Native.Invoke<int, FooPluginFunction>(nativeLibraryPtr, null);
            if (ret >= 0)
            {
                myImage = Native.Invoke<MyImageStruct, getMyImage>(nativeLibraryPtr, null);
                TextureFormat textFormate = TextureFormat.RGB24;
                //if (myImage.channels == 1)
                //{
                //    textFormate = TextureFormat.R8;
                //}
                Texture2D text = new Texture2D(myImage.width, myImage.height, textFormate, false);
                // Load data into the texture and upload it to the GPU.
                text.LoadRawTextureData(myImage.data, myImage.width * myImage.height * myImage.channels);
                text.Apply();

                // Assign texture to renderer's material.
                myDisplay.texture = text;

                textFormate = TextureFormat.RGBA32;
                Texture2D text2 = new Texture2D(myImage.width, myImage.height, textFormate, false);
                // Load data into the texture and upload it to the GPU.
                text2.LoadRawTextureData(myImage.data2, myImage.width * myImage.height * 4);
                text2.Apply();

                // Assign texture to renderer's material.
                myDisplay3DFace.texture = text2;

            }
            if (ret >= 0)
            {
                float x = (float)((180.0 / Math.PI) * Native.Invoke<double, getX>(nativeLibraryPtr, null));
                //x -= 180; 
                float y = (float)((180.0 / Math.PI) * Native.Invoke<double, getY>(nativeLibraryPtr, null));
                float z = (float)((180.0 / Math.PI) * Native.Invoke<double, getZ>(nativeLibraryPtr, null));
                myHead.localEulerAngles = new Vector3(orgHead.x + y, orgHead.y + z, orgHead.z + x);
                //myHead.eulerAngles = new Vector3(orgHead.x + z, orgHead.y - y, orgHead.z + x);
                float jawY = (float)Native.Invoke<double, getJawY>(nativeLibraryPtr, null);

                Debug.Log("jawY: " + jawY.ToString());
                //109.133f 122.518f;
                jawY = 13.385f * jawY;
                //Debug.Log("jawY: " + jawY.ToString());
                myJaw.localEulerAngles = new Vector3(myJaw.localEulerAngles.x, myJaw.localEulerAngles.y, orgJaw.z + jawY);
                
                float leftEyelidY = (float)Native.Invoke<double, getLeftEyelidY>(nativeLibraryPtr, null);
                leftEyelidY *= 3.0f;
                myLeftEyeLid.localEulerAngles = new Vector3(myLeftEyeLid.localEulerAngles.x, myLeftEyeLid.localEulerAngles.y, orgLeftEyeLid.z - leftEyelidY);
                
                float rightEyelidY = (float)Native.Invoke<double, getRightEyelidY>(nativeLibraryPtr, null);
                rightEyelidY *= 3.0f;
                myRightEyeLid.localEulerAngles = new Vector3(myRightEyeLid.localEulerAngles.x, myRightEyeLid.localEulerAngles.y, orgRightEyeLid.z - rightEyelidY);


                float leftEyeBrowY = (float)Native.Invoke<double, getLeftEyeBrowY>(nativeLibraryPtr, null);
                leftEyeBrowY /= 1500.0f;
                myLeftEyeBrow.localPosition = new Vector3(orgLeftEyeBrow.x - leftEyeBrowY, orgLeftEyeBrow.y, orgLeftEyeBrow.z);

                float rightEyeBrowY = (float)Native.Invoke<double, getRightEyeBrowY>(nativeLibraryPtr, null);
                rightEyeBrowY /= 1500.0f;
                myRightEyeBrow.localPosition = new Vector3(orgRightEyeBrow.x - rightEyeBrowY, orgRightEyeBrow.y, orgRightEyeBrow.z);
                
            }
            else
            {
                myHead.localEulerAngles = new Vector3(orgHead.x, orgHead.y, orgHead.z);
                //myHead.eulerAngles = new Vector3(orgHead.x, orgHead.y, orgHead.z);
                Debug.Log("Capture: " + ret.ToString());
            }
        }
    }
}
