using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingHandler : MonoBehaviour {
    [SerializeField]
    private Building[] buildings;
    [SerializeField]
    private Material constructingMaterial;
    [SerializeField]
    private Material constructingErrorMaterial;
    private Building selectedBuilding;
    private bool constructingBuilding, haveConstructedAtleastOneBuilding;
    private Building instanciatedBuilding;

    private List<Rect> collisionsRects = new List<Rect>();

    private bool wasColliding = false;

    private bool leftMouseButtonDown = false;
    private float lastBuildTime = 0;
    public City city;
    public UIController uiController;


	// Use this for initialization
	void Start () {
		
	}

    public void addCollision(Rect r){
        collisionsRects.Add(r);
    }

    private void setMaterialAvailable(){
        setConstructingMaterial(constructingMaterial);
    }

    private void setMaterialError()
    {
        setConstructingMaterial(constructingErrorMaterial);
    }


    private void cancelBuild(){
        if (constructingBuilding)
        {
            constructingBuilding = false;
            Destroy(instanciatedBuilding.gameObject);
        }
    }


    private void setConstructingMaterial(Material material){

        if(instanciatedBuilding == null){
            return;
        }

        Renderer rend = instanciatedBuilding.transform.GetChild(0).GetComponent<Renderer>();
        if (rend == null)
        {
            rend = instanciatedBuilding.transform.GetChild(0).GetChild(0).GetComponent<Renderer>();
            if(rend == null)
                return;
        }
        rend.material = material;
    }


    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        for (int i = 0; i < collisionsRects.Count; i++)
        {
            Gizmos.DrawCube(new Vector3(collisionsRects[i].x, 0, collisionsRects[i].y), new Vector3(collisionsRects[i].width, 2, collisionsRects[i].height));
        }

    }


	
	// Update is called once per frame
	void Update () {
        
        if(Input.GetMouseButtonDown(1)){
            cancelBuild();
        }
        else if(Input.GetMouseButtonDown(0)){
            if (UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject()){
                //cancelBuild();
            }
            leftMouseButtonDown = true;
        }

        if(Input.GetMouseButtonUp(0)){
            leftMouseButtonDown = false;
            haveConstructedAtleastOneBuilding = false;
        }

        if(constructingBuilding){
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            bool snaped = false;
            if(Physics.Raycast(ray, out hit)){
                Rect rect = new Rect(hit.point.x, hit.point.z, 1, 1);
                for (int i = 0; i < collisionsRects.Count; i++)
                {
                    if (
                        Mathf.Abs((collisionsRects[i].x + collisionsRects[i].width) - rect.x) < 1 &&
                        (rect.center.y > collisionsRects[i].y && rect.center.y < collisionsRects[i].y + collisionsRects[i].height)
                       )
                    {
                        rect.x = collisionsRects[i].x + collisionsRects[i].width;
                        rect.y = collisionsRects[i].y;
                        snaped = true;
                        break;
                    }

                    if (
                        Mathf.Abs((collisionsRects[i].x) - rect.x) < 1 &&
                        (rect.center.y > collisionsRects[i].y && rect.center.y < collisionsRects[i].y + collisionsRects[i].height)
                       )
                    {
                        rect.x = collisionsRects[i].x - rect.width;
                        rect.y = collisionsRects[i].y;
                        snaped = true;
                        break;
                    }

                    if (
                        Mathf.Abs((collisionsRects[i].y + collisionsRects[i].height) - rect.y) < 1 &&
                        (rect.center.x > collisionsRects[i].x && rect.center.x < collisionsRects[i].x + collisionsRects[i].width)
                       )
                    {
                        rect.x = collisionsRects[i].x;
                        rect.y = collisionsRects[i].y + collisionsRects[i].height;
                        snaped = true;
                        break;
                    }

                    if (
                        Mathf.Abs((collisionsRects[i].y) - rect.y) < 1 &&
                        (rect.center.x > collisionsRects[i].x && rect.center.x < collisionsRects[i].x + collisionsRects[i].width)
                       )
                    {
                        rect.x = collisionsRects[i].x;
                        rect.y = collisionsRects[i].y - collisionsRects[i].height;
                        snaped = true;
                        break;
                    }
                }

                bool collide = false;
                for (int i = 0; i < collisionsRects.Count; i++){
                    if(collisionsRects[i].Overlaps(rect)){
                        setMaterialError();
                        collide = true;
                        wasColliding = true;
                        break;
                    }
                }

                if(wasColliding && !collide){
                    wasColliding = false;
                    setMaterialAvailable();
                }
                float x = Mathf.Clamp(rect.x, 0.5f, 99.5f);
                float z = Mathf.Clamp(rect.y, 0.5f, 99.5f);
                instanciatedBuilding.transform.position = new Vector3(x, 1, z);
                if (leftMouseButtonDown && !collide &&  Time.time - lastBuildTime > 0.1f && ((snaped && haveConstructedAtleastOneBuilding) || (!haveConstructedAtleastOneBuilding)) && city.Cash >= selectedBuilding.cost)
                {
                    city.Cash -= selectedBuilding.cost;
                    uiController.UpdateCityData();
                    city.buildingCounts[selectedBuilding.id]++;


                    lastBuildTime = Time.time;
                    haveConstructedAtleastOneBuilding = true;
                    Instantiate(selectedBuilding, instanciatedBuilding.transform.position, instanciatedBuilding.transform.rotation);
                    collisionsRects.Add(new Rect(instanciatedBuilding.transform.position.x, instanciatedBuilding.transform.position.z, 1, 1));

                }
            }
        }
	}

    public void enableBuilder(int buildingId){
        haveConstructedAtleastOneBuilding = false;
        constructingBuilding = true;
        selectedBuilding = buildings[buildingId];
        instanciatedBuilding = Instantiate(selectedBuilding, new Vector3(-100, -100, -100), Quaternion.identity);
        setMaterialAvailable();
    }
}
