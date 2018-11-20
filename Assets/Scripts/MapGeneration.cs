using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGeneration : MonoBehaviour {

    [SerializeField]
    private EnvironnementObject mainTree = null;
    [SerializeField]
    private EnvironnementObject mainRock = null;
    [SerializeField]
    private BuildingHandler buildingHandler = null;

    [SerializeField]
    private int width = 100;

    [SerializeField]
    private int height = 100;

    private System.Random rnd;

    private void addTree(float x, float y){
        float widthScale = rnd.Next(80, 220) / 100f;
        mainTree.setScale(widthScale, widthScale, widthScale);
        Instantiate(mainTree, new Vector3(x, 1, y), Quaternion.identity);
        buildingHandler.addCollision(new Rect(x, y, 2*widthScale, 2*widthScale));
    }

	// Use this for initialization
	void Start () {

        rnd = new System.Random(129139);

        int nb_forest = rnd.Next(6, 12);
        for (int i = 0; i < nb_forest; i++){
            int forestCenterX = rnd.Next(0, width);
            int forestCenterY = rnd.Next(0, height);

            int numberOfTrees = rnd.Next(8, 30);
            for (int j = 0; j < numberOfTrees; j++){
                float xAdd = (rnd.Next(10, 100) * 1f) / 10f;
                float treePosX = forestCenterX + xAdd;
                if (treePosX > width){
                    treePosX = forestCenterX - xAdd;
                }

                float yAdd = (rnd.Next(10, 100) * 1f) / 10f;
                float treePosY = forestCenterY + yAdd;
                if (treePosY > height){
                    treePosY = forestCenterY - yAdd;
                }

                addTree(treePosX, treePosY);
            }
        }

        int nb_rock = rnd.Next(6, 12);
        for (int i = 0; i < nb_rock; i++)
        {
            int rockCenterX = rnd.Next(0, width);
            int rockCenterY = rnd.Next(0, height);
            Instantiate(mainRock, new Vector3(rockCenterX, 1, rockCenterY), Quaternion.identity);
        }

	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
