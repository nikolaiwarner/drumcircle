using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

//Manager Class for the Player. More info here later

public class PlayerManager : NetworkBehaviour
{
    public GameObject AVEffect_TestVariable;

    //Test case: Add movement in update and test it works across the network
    void Update()
    {
        if (isLocalPlayer)
        {
            float x = Input.GetAxis("Horizontal") * 0.1f;
            float z = Input.GetAxis("Vertical") * 0.1f;

            transform.Translate(x, 0, z);

            if (Input.GetKeyDown(KeyCode.Space))
            {
                //If player presses Space key down, will spawn sphere at their position. To test, can follow instructions here: https://docs.unity3d.com/Manual/UNetSetup.html
                CmdSpawnTestAVEffect(); 
            }
        }
    }

    //Called when a player connects. Use for initialization
    public override void OnStartLocalPlayer()
    {
        
    }

    [Command]
    void CmdSpawnTestAVEffect()
    {
        GameObject temp = (GameObject)Instantiate(AVEffect_TestVariable, transform.position, transform.rotation);
        temp.GetComponent<AVEffect_Sphere_1>().SpawnTime = System.DateTime.UtcNow.Millisecond; //Need to have a clock, either on server, or system time. 
        NetworkServer.Spawn(temp);
    }

    [ClientRpc]
    void RpcTestCall()
    {
        
    }
}
