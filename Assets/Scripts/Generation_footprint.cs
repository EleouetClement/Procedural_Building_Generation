using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Generation_footprint : MonoBehaviour
{
    // Building's footprint generation using L-system
    [SerializeField][Range(0, 10)] private int width;
    [SerializeField][Range(0, 10)] private int depth;
    [SerializeField][Range(0, 10)] private int height;
    [SerializeField] [Range(0, 2)] private int step;


    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void MoveForward()
    {
        //Move the current cursor then put a new point at the location

    }

    private void TurnLeft()
    {
        //Apply a negative rotation to the cursor
    }

    private void TurnRight()
    {
        //Apply a positive rotation to the cursor
    }

}
