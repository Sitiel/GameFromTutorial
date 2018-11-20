using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnvironnementObject : MonoBehaviour {

    private float scaleX_;
    private float scaleY_;
    private float scaleZ_;




	// Use this for initialization
	void Start () {
		
	}

    public void setScale(float scaleX, float scaleY, float scaleZ){
        this.scaleX_ = scaleX;
        this.scaleY_ = scaleY;
        this.scaleZ_ = scaleZ;
        transform.localScale = new Vector3(scaleX, scaleY, scaleZ);
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
