using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class AVEffect_Sphere_1 : NetworkBehaviour
{
    [SyncVar]
    public int SpawnTime;

    public float lifeTime;
    float timeElapsed;

    Vector3 startPosition;

	// Use this for initialization
	void Start ()
    {
        timeElapsed = 0f + (System.DateTime.UtcNow.Millisecond - SpawnTime) / 1000f; //use spawn time to set current elapsed time for object to be correct
        startPosition = transform.position;
    }
	
	// Update is called once per frame
	void FixedUpdate ()
    {
        timeElapsed += Time.fixedDeltaTime;

        transform.position = startPosition + Vector3.up * timeElapsed;

        if (timeElapsed > lifeTime)
        {
            Destroy(this.gameObject);
        }
    }

    
}
