using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class OLD_DrumInputTest_Component : MonoBehaviour
{
    bool trackInput = true;

    public enum InputState { tracking, waitingToDecel, trackDecel, triggered, failed};
    InputState currentInputState;
    float velMag;

    float[] lastTenMagnitude = new float[10];

    Vector3 lastPosition;

    public Text VelText;
    public GameObject RedSphere;
    public GameObject GreenSphere;

    void Start ()
    {
        velMag = 0f;
        currentInputState = InputState.tracking;

        for (int i = 0; i < lastTenMagnitude.Length; i++)
        {
            lastTenMagnitude[i] = 0f;
        }

        lastPosition = transform.position;
    }
	
	// Update is called once per frame
	void FixedUpdate ()
    {
	    if (trackInput)
        {
            switch (currentInputState)
            {
                case InputState.tracking:
                    TrackInput();
                    break;
                case InputState.waitingToDecel:
                    TrackDeceleration();
                    break;
                case InputState.trackDecel:
                    Decel();
                    break;
            }
            
        }
	}

    void TrackInput()
    {
        Vector3 newPos = transform.position;
        float delta = Vector3.Distance(newPos, lastPosition) * 100f;

        for (int i = 0; i < lastTenMagnitude.Length-1; i++)
        {
            lastTenMagnitude[i] = lastTenMagnitude[i + 1];
        }

        lastTenMagnitude[lastTenMagnitude.Length - 1] = delta;

        float mag = 0f;
        for (int i = 0; i < lastTenMagnitude.Length - 1; i++)
        {
            mag += lastTenMagnitude[i];
        }

        mag /= lastTenMagnitude.Length;
        
        VelText.text = "Vel: " + mag.ToString();
        if (mag > 1.5)
        {
            RedSphere.SetActive(true);
            currentInputState = InputState.waitingToDecel;
        }
        lastPosition = newPos;
    }


    void TrackDeceleration()
    {
        Vector3 newPos = transform.position;
        float delta = Vector3.Distance(newPos, lastPosition) * 100f;

        for (int i = 0; i < lastTenMagnitude.Length - 1; i++)
        {
            lastTenMagnitude[i] = lastTenMagnitude[i + 1];
        }

        lastTenMagnitude[lastTenMagnitude.Length - 1] = delta;

        float mag = 0f;
        for (int i = 0; i < lastTenMagnitude.Length - 1; i++)
        {
            mag += lastTenMagnitude[i];
        }

        mag /= lastTenMagnitude.Length;

        VelText.text = "Vel: " + mag.ToString();
        if (mag < 1.5)
        {
            RedSphere.SetActive(true);
            currentInputState = InputState.trackDecel;
            decelStart = Time.timeSinceLevelLoad;
        }
        lastPosition = newPos;
    }

    float decelStart;
    void Decel()
    {
        Vector3 newPos = transform.position;
        float delta = Vector3.Distance(newPos, lastPosition) * 100f;

        for (int i = 0; i < lastTenMagnitude.Length - 1; i++)
        {
            lastTenMagnitude[i] = lastTenMagnitude[i + 1];
        }

        lastTenMagnitude[lastTenMagnitude.Length - 1] = delta;

        float mag = 0f;
        for (int i = 0; i < lastTenMagnitude.Length - 1; i++)
        {
            mag += lastTenMagnitude[i];
        }

        mag /= lastTenMagnitude.Length;

        VelText.text = "Vel: " + mag.ToString();
        if (mag < 1)
        {
            if (Time.timeSinceLevelLoad - decelStart < .1)
                GreenSphere.SetActive(true);
            currentInputState = InputState.tracking;
        }
        
        lastPosition = newPos;
    }
}
