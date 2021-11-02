using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public class Generation_footprint : MonoBehaviour
{
    // Building's footprint generation using L-system
    [SerializeField][Range(0, 10)] private int width;
    [SerializeField][Range(0, 10)] private int depth;
    [SerializeField][Range(1, 10)] private int floorsCount;
    [SerializeField] [Range(0, 2)] private int widthStep;
    [SerializeField] [Range(0, 2)] private float heightStep;
    [SerializeField] [Range(0, 360)] private int turnRadius;
    [SerializeField] private string axiom;
    [SerializeField] private GameObject pointPrefab;
    [SerializeField] private GameObject wallPrefab;
    private List<Vector3> points;
    private Dictionary<char, string> rules;
    private Vector3 direction;
    private Vector3 position;
    private int id = 0;
    private int NbPointsPerFloor;


    void Start()
    {
        points = new List<Vector3>();
        Vector3 startPosition = Vector3.zero;
        Vector3 startDirection = Vector3.right;
        InitRules();

        GenerateFootPrint(startPosition, startDirection);
        this.NbPointsPerFloor = this.points.Count;
        Build();
        WallInsertion();
        //DrawFootPrint();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void InitRules()
    {
        rules = new Dictionary<char, string>();
        //Initiate All the Rules According to the inputs
        rules.Add('F', "F");
        rules.Add('C', "F -f");
        rules.Add('S', "F + F - F");
        rules.Add('W', "F F F + F F - F F F F + F F F F + F");
        //rules.Add("-f", "")
    }

    private void WallInsertion()
    {
        //Insert Wall between the points 
        if(this.points == null)
        {
            Debug.LogError("WallInsertion : there is no construction points");
            return;
        }

        if(this.wallPrefab == null)
        {
            Debug.LogError("WallInsertion : missing wall prefab");
            return;
        }
        for(int floor = 1; floor <= this.floorsCount; floor++)
        {
            BuildCurrentFloor(floor);
        }
        



    }

    private void BuildCurrentFloor(int floorId)
    {

        for (int i = (this.NbPointsPerFloor * floorId); i < this.NbPointsPerFloor; i++)
        {
            //Getting the direction between 2 points
            Vector3 wallDirection = Vector3.zero;
            if (i == this.NbPointsPerFloor - 1)
            {
                wallDirection = this.points[0] - this.points[i];
            }
            else
            {
                wallDirection = this.points[i + 1] - this.points[i];
            }
            wallDirection = wallDirection.normalized;
            GameObject go = Instantiate(wallPrefab, this.points[i], Quaternion.identity);

            //Rotating the wall to make sur it is  perpendicular to the direction of both points
            //Quaternion rot = Quaternion.Euler(0, 90, 0);
            go.transform.rotation = Quaternion.LookRotation(wallDirection);
            //go.transform.rotation *= rot;

            //Moving the wall to put it at the mid distance of both points 
            float distance = 0f;
            if (i == this.NbPointsPerFloor - 1)
            {
                distance = Vector3.Distance(this.points[i], this.points[0]);
            }
            else
            {
                distance = Vector3.Distance(this.points[i], this.points[i + 1]);
            }
            Vector3 newPosition = this.points[i] + (wallDirection * (distance / 2));
            newPosition.y = newPosition.y + (floorId * (heightStep / 2));
            go.transform.position = newPosition;
        }
    }

    private void Build()
    {
        //Build the building

        //1st step adding the floors
        for(int i = 1; i <= floorsCount; i++)
        {
            BuildFloor(i);
        }
    }

    private void BuildFloor(int floorNumber)
    {
        //Add all the points needed for the floor
        
        for(int i = 0; i < NbPointsPerFloor; i++)
        {
            Vector3 newPos = this.points[i] + (floorNumber * heightStep * Vector3.up);
            this.points.Add(newPos);
            GameObject go = Instantiate(pointPrefab, newPos, Quaternion.identity);
            go.name = "" + id;
            id++;
        }
    }

    




    private void GenerateFootPrint(Vector3 startPosition, Vector3 startDirection)
    {
        //Read the Axiom then apply the rules if each member of the axiom
        this.position = startPosition;

        //Direction must be normalized
        this.direction = startDirection.normalized;
        Debug.Log(this.axiom);
        string[] currentAxiom = this.axiom.Split(' ');
        
        foreach(char member in this.axiom)
        {
            switch(member)
            {
                case '-':
                    this.direction = TurnRight();
                    break;
                case '+':
                    this.direction = TurnLeft();
                    break;
                default:
                    //Apply the rules of the current member
                    ApplyRules(member);
                    break;
            }
          
        }
    }

    private void ApplyRules(char member)
    {
        //Apply the rules of a precise given member
        //string [] currentRules = member.Split(' ');
        string currentRules = "";
        if(!this.rules.TryGetValue(member, out currentRules))
        {
            Debug.LogError("R�gle inexistante : " + member);
            return;
        }
        string[] rules;
        if(currentRules.Length > 1)
        {
            rules = currentRules.Split(' ');
        }
        else
        {
            rules = new string [] { currentRules[0].ToString() };
        }
        Debug.Log(member);
        Debug.Log(currentRules);
        
        foreach (string rule in rules)
        {
            switch (rule)
            {
                case "-":
                    this.direction = TurnRight();
                    break;
                case "+":
                    this.direction = TurnLeft();
                    break;
                case "-f":
                    //Go back to the previous location whithout adding a point
                    this.position = MoveBackward();
                    break;
                default:
                    this.position = MoveForward();
                    newPoint(id);
                    this.id++;
                    break;
            }
        }
    }



    private void newPoint(int id)
    {
        //Instantiate a new point and add it to the List of points
        GameObject go = Instantiate(pointPrefab, this.position, Quaternion.identity);
        go.name = id.ToString();
        this.points.Add(this.position);
    }

    private Vector3 MoveBackward()
    {
        //Move the current cursor then put a new point at the location
        Vector3 newPosition = this.position - (widthStep * this.direction);
        return newPosition;
    }

    private Vector3 MoveForward()
    {
        //Move the current cursor then put a new point at the location
        Vector3 newPosition = this.position + (widthStep * this.direction);
        return newPosition;
    }


    private Vector3 TurnLeft()
    {
        //Apply a negative rotation to the cursor 
        Vector3 newDirection = Quaternion.Euler(0, turnRadius, 0) * this.direction.normalized;
        Debug.Log(newDirection);
        return newDirection;
    }

    private Vector3 TurnRight()
    {
        //Apply a positive rotation to the cursor
        Vector3 newDirection = Quaternion.Euler(0, -turnRadius, 0) * this.direction.normalized;
        Debug.Log(newDirection);
        return newDirection;
    }

    private void DrawFootPrint()
    {
        //To check is the points are ok
        LineRenderer line = gameObject.AddComponent<LineRenderer>();
        line.positionCount = this.points.Count + 1;
        line.startWidth = 0.3f;
        line.endWidth = 0.3f;
        List<Vector3> positions = new List<Vector3>();
        foreach(Vector3 pos in this.points)
        {
            positions.Add(pos);
        }
        positions.Add(points[0]);
        line.SetPositions(positions.ToArray());
    }


}
