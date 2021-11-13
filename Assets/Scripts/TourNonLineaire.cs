using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TourNonLineaire : MonoBehaviour
{
    //[SerializeField] [Range(0, 10)] int floorsNumber = 1;
    [SerializeField] [Range(0, 10)] float step;
    [SerializeField] string floorsAxiom;
    [SerializeField] string footprintAxiom;
    [SerializeField] [Range(1, 10)] float floorWidth = 1f;
    [SerializeField] [Range(1, 10)] float floorHeight = 1f;
    [SerializeField] [Range(3, 15)] private int polygoneMinSides;
    [SerializeField] [Range(3, 15)] private int polygoneMaxSides;
    public GameObject buildingPrefab;
    private Vector3 center;
    private int currentFloor;
    //Will contain the list of all the floors excluding the entrance and the roof
    PrimitiveBuilding[] floors;
    // Start is called before the first frame update
    void Start()
    {
        //floors = new PrimitiveBuilding[floorsNumber];
        center = Vector3.zero;
        currentFloor = 0;
        ApplyRules();
    }

    private void ApplyRules()
    {
        //Read the axiom and build the building according to the rules given
        foreach(char member in floorsAxiom)
        {
            switch(member)
            {
                case '-':
                    //Decrease the floor width
                    floorWidth -= step;
                    break;

                case '+':
                    //Increase the floor width
                    floorWidth += step;
                    break;

                case 'P':
                    //Build a floor
                    BuildFloor(null);
                    break;
                case 'C':
                    //Build a cubical floor
                    Debug.Log("Etage cubique");
                    BuildFloor(4);
                    break;


            }
        }
    }

    private void BuildFloor(int? nbSides)
    {
        //Use the PrimitiveBuilding script to 
        Vector3 currentCenter = center + new Vector3(0, floorHeight * currentFloor, 0);
        Debug.Log("centre : " + currentCenter);
        PrimitiveBuilding pb = GameObject.Instantiate(buildingPrefab, center, Quaternion.identity).GetComponent<PrimitiveBuilding>();
        if (nbSides == null)
        {
            pb.Initialize(UnityEngine.Random.Range(polygoneMinSides, polygoneMaxSides),
                floorWidth,
                floorHeight,
                currentCenter);
        }
        else
        {
            pb.Initialize(nbSides, floorWidth, floorHeight, currentCenter);
        }
        currentFloor++;
        pb.BuildPrimitive();

    }
}
