using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class City : MonoBehaviour {

    public int Cash { get; set; }
    public int Day { get; set; }
    public float PopulationCurrent { get; set; }
    public float PopulationCeiling { get; set; }
    public int JobsCurrent { get; set; }
    public int JobsCeiling { get; set; }
    public float Food { get; set; }

    public int[] buildingCounts = new int[3];
    private UIController uiController;


	// Use this for initialization
	void Start () {
        uiController = GetComponent<UIController>();
        Cash = 10000;
        Food = 6;
        JobsCeiling = 10;
	}
	
	

    public void EndTurn(){
        Day++;
        CalculatePopulation();
        CalculateJobs();
        CalculateFood();
        CalculateCash();
        uiController.UpdateCityData();
        uiController.UpdateDayCount();
    }

    void CalculateJobs(){
        JobsCeiling = buildingCounts[2] * 10;
        JobsCurrent = Mathf.Min((int)PopulationCurrent, JobsCeiling);
    }

    void CalculateCash(){
        Cash += JobsCurrent * 2;
    }

    void CalculateFood(){
        Food += buildingCounts[1] * 4f;
    }

    void CalculatePopulation(){
        PopulationCeiling = buildingCounts[0]*5;
        if(Food >= PopulationCurrent && PopulationCurrent < PopulationCeiling){
            Food -= PopulationCurrent * 0.25f;
            PopulationCurrent = Mathf.Min(PopulationCurrent += Food * 0.25f, PopulationCeiling);
        }
        else if(Food < PopulationCurrent){
            PopulationCurrent -= (PopulationCurrent - Food) * 0.5f;
        }
    }
}
