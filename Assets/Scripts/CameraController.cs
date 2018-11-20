using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

    private float percentScroll = 1f;
    private float speedScroll = 0.5f;

    private bool spacePressed = false;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        Camera.main.orthographicSize = Mathf.Clamp(Camera.main.orthographicSize - Input.GetAxis("Mouse ScrollWheel") * (10f * Camera.main.orthographicSize * .01f), 2.5f, 50f);

        float triggerX = Screen.width * (percentScroll / 100f);
        float triggerY = Screen.height * (percentScroll / 100f);


        if(Input.GetKeyDown("space")){
            spacePressed = !spacePressed;
        }


        if(!spacePressed){
            //down
            if (Input.mousePosition.y < triggerY)
            {
                transform.position += new Vector3(0, -1 * speedScroll, 0);
            }
            //up
            else if (Input.mousePosition.y > Screen.height - triggerY)
            {
                transform.position += new Vector3(0, 1 * speedScroll, 0);
            }
            //left
            if (Input.mousePosition.x < triggerX)
            {
                transform.position += new Vector3(-0.7f * speedScroll, 0, 0.7f * speedScroll);
            }
            //right
            else if (Input.mousePosition.x > Screen.width - triggerX)
            {
                transform.position += new Vector3(0.7f * speedScroll, 0, -0.7f * speedScroll);
            }
        }

	}
}
