using UnityEngine;
using System.Collections;

public class CameraPoint : MonoBehaviour {

    public PlayerController player;
    public float zoom;
    float initY;
    float closeY;
	// Use this for initialization
	void Start () {
        player = FindObjectOfType<PlayerController>();
        initY = transform.position.y;
        closeY = initY - 0.7f;
	}

	
	// Update is called once per frame
	void Update () {
        if(player.name == "Player1")
            transform.position = new Vector3(player.transform.position.x + player.distance / 2,
                                          transform.position.y, player.transform.position.z - 10);
        else
            transform.position = new Vector3(player.transform.position.x - player.distance / 2,
                                          transform.position.y, player.transform.position.z - 10);

	
	}

    void FixedUpdate()
    {
        if(player.distance < 5 && player.distance > 0)
        {
            zoom = 3;
            transform.position = new Vector3(player.transform.position.x - player.distance / 2,
                                          closeY, player.transform.position.z - 10);
        }
        else if(player.distance > 5 && player.distance < 15)
        {
            transform.position = new Vector3(player.transform.position.x - player.distance / 2,
                                          initY, player.transform.position.z - 10);
            zoom = 5;
        }
        else
        {
            transform.position = new Vector3(player.transform.position.x - player.distance / 2,
                                          initY, player.transform.position.z - 10);
            zoom = 7;
        }
           
        
    }
}
