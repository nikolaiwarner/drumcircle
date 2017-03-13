using UnityEngine;
using System.Collections;
using UnityEngine.UI;

[RequireComponent(typeof(SteamVR_TrackedObject))]
public class DrumInput : MonoBehaviour
{
    SteamVR_TrackedObject trackedObj; //Move tracked object to manager that loops through all drums and triggers hits

    bool initialized = false;

    bool collided = false;
    Vector3 colliderPosition;
    Vector3 Drum_StartScale;

    public GameObject Drum_Prefab;
    GameObject Drum;
    AudioSource Drum_AudioSource;
    public GameObject DrumHit_VisualEffect_Prefab;
    GameObject DrumHit_MagVisualization;

    public GameObject AVEffect_1;

    void Awake()
    {
        trackedObj = GetComponent<SteamVR_TrackedObject>();
    }

	void Update ()
    {
        var device = SteamVR_Controller.Input((int)trackedObj.index);
        TrackInput();

        if (!initialized)
        {
            if (device.GetPressDown(SteamVR_Controller.ButtonMask.Trigger))
            {
                Drum = Instantiate(Drum_Prefab, transform.position, transform.rotation) as GameObject;
                colliderPosition = Drum.transform.position;
                Drum_AudioSource = Drum.GetComponent<AudioSource>();
                Drum_StartScale = Drum.transform.localScale;
                DrumHit_MagVisualization = Instantiate(DrumHit_VisualEffect_Prefab, transform.position, transform.rotation) as GameObject;
                initialized = true;
            }
        }
        else
        {
            if (Vector3.Distance(transform.position, colliderPosition) < .04 && !collided)
            {
                collided = true;

                DrumHit_MagVisualization.GetComponent<Timer_TurnOffObject>().DoEffect(mag*.35f);
                Drum_AudioSource.PlayOneShot(Drum_AudioSource.clip, mag);

                Vector3 EffectPosition = Vector3.up + Vector3.right * -20f;
                GameObject temp = Instantiate(AVEffect_1, EffectPosition, Quaternion.identity) as GameObject;
                temp.GetComponent<AVEffect_Sphere_2>().Init(Vector3.right, 4f, 1.25f);

                //Rumble and bounce drum. Need to move out of coroutines.
                StartCoroutine(DoHapticPulse(.075f, (ushort)(mag * 700)));
                StartCoroutine(DoHitDrumCollider());
                
            }
            else if (Vector3.Distance(transform.position, colliderPosition) > .1)
            {
                collided = false;
            }
        }
	}

    float[] lastTenMagnitude = new float[5];
    Vector3 lastPosition;
    float mag;
    void TrackInput()
    {
        Vector3 newPos = transform.position;
        float delta = Vector3.Distance(newPos, lastPosition) * 500f;

        for (int i = 0; i < lastTenMagnitude.Length - 1; i++)
        {
            lastTenMagnitude[i] = lastTenMagnitude[i + 1];
        }

        lastTenMagnitude[lastTenMagnitude.Length - 1] = delta;

        mag = 0f;
        for (int i = 0; i < lastTenMagnitude.Length - 1; i++)
        {
            mag += lastTenMagnitude[i];
        }

        mag /= lastTenMagnitude.Length;
        lastPosition = newPos;
    }

    IEnumerator DoHapticPulse(float f, ushort mag)
    {
        while (f > 0)
        {
            f -= Time.deltaTime;
            SteamVR_Controller.Input((int)trackedObj.index).TriggerHapticPulse(2000);
            yield return new WaitForEndOfFrame();
        }
    }

    IEnumerator DoHitDrumCollider()
    {
        float t = 0f;
        while (t < 1)
        {

            Drum.transform.localScale = Vector3.Lerp(Drum_StartScale * 2f, Drum_StartScale, t);
            t += Time.deltaTime * 10f;
            yield return new WaitForEndOfFrame();
        }

        Drum.transform.localScale = Drum_StartScale;
    }
}
