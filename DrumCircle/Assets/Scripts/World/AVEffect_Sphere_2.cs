using UnityEngine;
using System.Collections;

public class AVEffect_Sphere_2 : MonoBehaviour
{
    bool isInit = false;
    Vector3 dir;
    float speed;
    float life;

	public void Init(Vector3 d, float s, float scale)
    {
        dir = d;
        speed = s;
        transform.localScale = Vector3.one * scale;
        life = 0f;
        isInit = true;
    }
	
	// Update is called once per frame
	void Update ()
    {
	    if (isInit)
        {
            life += Time.deltaTime;
            transform.position += dir * speed * Time.deltaTime;
            if (life > 10)
                Destroy(this.gameObject);
        }
	}
}
