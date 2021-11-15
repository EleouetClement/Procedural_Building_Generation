using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Generation_curved_building : MonoBehaviour
{
    [SerializeField][Range(0, 20)] float width;
    [SerializeField][Range(0, 20)] float height;
    [SerializeField] float numberOfIterations = 1;
    [SerializeField] GameObject[] controlPoints;
    [SerializeField] GameObject testPrefab;


    private Vector3[] controlCoor;
    Vector3[] vertices;
    int[] triangles;

    // Start is called before the first frame update
    void Start()
    {
        BuildCurvedBuilding();
    }

    
    //Get the coordinates froms the control game objects to avoid multiples call to GetComponent<Transform>
    private void ControlCoordinatesInit()
    {
        controlCoor = new Vector3[controlPoints.Length];
        int i = 0;
        foreach(GameObject go in controlPoints)
        {
            controlCoor[i++] = go.transform.position;
        }
    }

    //Function that create aa curve using n control points
    private void CreateCurve()
    {
        if(controlPoints == null)
        {
            Debug.LogError("CreateCurve : Control points missing");
            return;
        }
        ControlCoordinatesInit();
        float u = 1f / 4f;
        float v = 3f / 4f;
        Debug.Log("u = " + u + " v = " + v);
        List<Vector3> newLine = new List<Vector3>();
        //Ajout du point de depart
        //newLine.Add(points[0]);
        for (int iteration = 0; iteration < numberOfIterations; iteration++)
        {
            for (int i = 0; i < controlCoor.Length - 1; i++)
            {
                //On doit instancier 2 nouveaux points
                Vector3 pi34 = v * controlCoor[i];
                //Debug.Log("Pi 3/4 : " + pi34);
                Vector3 pi14 = u * controlCoor[i + 1];
                //Debug.Log("Pi+1 1/4 : " + pi14);
                newLine.Add(pi34 + pi14);
                pi14 = u * controlCoor[i];
                pi34 = v * controlCoor[i + 1];
                newLine.Add(pi14 + pi34);
            }
            //pour la prochaine iteration on travail sur la nouvelle liste.
            controlCoor = new Vector3[newLine.Count];
            newLine.CopyTo(controlCoor);
            newLine.Clear();
        }
        //newLine.Add(points[points.Count - 1]);

        //for (int i = 0; i < controlCoor.Length; i++)
        //{
        //    GameObject go = Instantiate(testPrefab, controlCoor[i], Quaternion.identity);
        //    go.GetComponent<MeshRenderer>().material.color = Color.cyan;
        //}

    }

    //Use the the list of points created by CreateCurve() to add the new point.
    private void BuildCurvedBuilding()
    {
        //Create the initial curve
        CreateCurve();

        if(controlPoints.Length == controlCoor.Length)
        {
            Debug.LogError("BuildCurvedBuilding : CreateCurve returned the tab without upating it");
            return;
        }

        //Initiating vertices tab
        vertices = new Vector3[controlCoor.Length * 4];

        //Passing the 1st line
        controlCoor.CopyTo(vertices, 0);

        //Setup the boundries
        Vector3 bottomRightCorner = new Vector3(width, 0, 0);
        Vector3 topCorner = new Vector3(0, height, 0);
        Vector3 topRightCorner = new Vector3(width, height, 0);

        //We need 3 more lines in order to generate the triangles for the whole building.
        int origWidth = controlCoor.Length, origHeight = controlCoor.Length * 2, heightWidth = controlCoor.Length * 3;
        for (int orig = 0; orig < controlCoor.Length; orig++, origWidth++, origHeight++, heightWidth++)
        {
            vertices[origWidth] = vertices[orig] + bottomRightCorner;
            vertices[origHeight] = vertices[orig] + topCorner;
            vertices[heightWidth] = vertices[orig] + topRightCorner;
        }

        for (int i = 0; i < vertices.Length; i++)
        {
            GameObject go = Instantiate(testPrefab, vertices[i], Quaternion.identity);
            go.GetComponent<MeshRenderer>().material.color = Color.cyan;
        }
    }


    //Use the vertices tab to create the triangles
    private void CreatingTriangles()
    {
        if(vertices == null)
        {
            Debug.LogError("CreatingTriangles : vertices tab not defined");
            return;
        }

    }

}
