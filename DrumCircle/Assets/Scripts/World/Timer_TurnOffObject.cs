using UnityEngine;
using System.Collections;

public class Timer_TurnOffObject : MonoBehaviour
{
    Vector3 defaultScale;
    
    void Awake()
    {
        defaultScale = transform.localScale;
    }

    public void DoEffect(float f)
    {
        this.gameObject.SetActive(true);
        StopAllCoroutines();
        transform.localScale = defaultScale * f;
        StartCoroutine(WaitToTurnOff());
    }
	
	
	IEnumerator WaitToTurnOff()
    {
        float tE = 0f;

        while (tE < 1)
        {
            tE += Time.deltaTime;
            transform.localScale *= .99f;
            yield return new WaitForEndOfFrame();
        }
        

        this.gameObject.SetActive(false);
    }
}
