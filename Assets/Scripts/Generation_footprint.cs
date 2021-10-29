using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Generation_footprint : MonoBehaviour
{
    // Building's footprint generation using L-system
    [SerializeField][Range(0, 10)] private int width;
    [SerializeField][Range(0, 10)] private int depth;
    [SerializeField][Range(1, 10)] private int floorsCount;
    [SerializeField] [Range(0, 2)] private int widthStep;
    [SerializeField] [Range(0, 2)] private int heightStep;
    [SerializeField] [Range(0, 360)] private int turnRadius;
    [SerializeField] private string axiom;
    [SerializeField] private GameObject pointPrefab;
    private List<Vector3> points;
    private Dictionary<char, string> rules;
    private Vector3 direction;
    private Vector3 position;
    private int id = 0;
    private int size;


    void Start()
    {
        points = new List<Vector3>();
        Vector3 startPosition = Vector3.zero;
        Vector3 startDirection = Vector3.right;
        InitRules();

        GenerateFootPrint(startPosition, startDirection);
        this.size = this.points.Count;
        Build();
        //DrawFootPrint();
    }

    // Update is called once per frame
    void Update()
    {
        
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
        
        for(int i = 0; i < size; i++)
        {
            Vector3 newPos = this.points[i] + (floorNumber * heightStep * Vector3.up);
            this.points.Add(newPos);
            GameObject go = Instantiate(pointPrefab, newPos, Quaternion.identity);
            go.name = "" + id;
            id++;
        }
    }

    private void InitRules()
    {
        rules = new Dictionary<char, string>();
        //Initiate All the Rules According to the inputs
        rules.Add('F', "F");
        rules.Add('C', "F -f");
        //rules.Add("-f", "")
    }




    private void GenerateFootPrint(Vector3 startPosition, Vector3 startDirection)
    {
        //Read the Axiom then apply the rules if each member of the axiom
        this.position = startPosition;

        //Direction must be normalized
        this.direction = startDirection.normalized;
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
        this.rules.TryGetValue(member, out currentRules);
        string[] rules = currentRules.Split(' ');
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
