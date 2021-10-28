using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Generation_footprint : MonoBehaviour
{
    // Building's footprint generation using L-system
    [SerializeField][Range(0, 10)] private int width;
    [SerializeField][Range(0, 10)] private int depth;
    [SerializeField][Range(0, 10)] private int height;
    [SerializeField] [Range(0, 2)] private int step;
    [SerializeField] private string axiom;
    [SerializeField] private const char F = 'F';
    [SerializeField] private GameObject pointPrefab;
    private List<GameObject> points;
    private Dictionary<char, string> rules;
    private Vector3 direction;
    private Vector3 position;
    private int id = 0;


    void Start()
    {
        points = new List<GameObject>();
        Vector3 startPosition = Vector3.zero;
        Vector3 startDirection = Vector3.right;
        InitRules();
        GenerateFootPrint(startPosition, startDirection);
        DrawFootPrint();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void InitRules()
    {
        rules = new Dictionary<char, string>();
        //Initiate All the Rules According to the inputs
        rules.Add(F, "F+F-F");
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
        string currentRules = "";
        this.rules.TryGetValue(member, out currentRules);
        Debug.Log(member);
        Debug.Log(currentRules);
        
        foreach (char rule in currentRules)
        {
            switch (rule)
            {
                case '-':
                    this.direction = TurnRight();
                    break;
                case '+':
                    this.direction = TurnLeft();
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
        this.points.Add(go);
    }

    private Vector3 MoveForward()
    {
        //Move the current cursor then put a new point at the location
        Vector3 newPosition = this.position + (step * this.direction);
        return newPosition;
    }

    private Vector3 TurnLeft()
    {
        //Apply a negative rotation to the cursor 
        Vector3 newDirection = Quaternion.Euler(0, 90, 0) * this.direction.normalized;
        Debug.Log(newDirection);
        return newDirection;
    }

    private Vector3 TurnRight()
    {
        //Apply a positive rotation to the cursor
        Vector3 newDirection = Quaternion.Euler(0, -90, 0) * this.direction.normalized;
        Debug.Log(newDirection);
        return newDirection;
    }

    private void DrawFootPrint()
    {
        //To check is the points are ok
        LineRenderer line = gameObject.AddComponent<LineRenderer>();
        line.positionCount = this.points.Count + 1;
        line.startWidth = 1;
        line.endWidth = 1;
        List<Vector3> positions = new List<Vector3>();
        foreach(GameObject go in this.points)
        {
            positions.Add(go.transform.position);
        }
        positions.Add(points[0].transform.position);
        line.SetPositions(positions.ToArray());
    }


}
