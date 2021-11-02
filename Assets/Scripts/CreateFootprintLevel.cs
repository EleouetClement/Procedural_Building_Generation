using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateFootprintLevel : MonoBehaviour
{
    public GameObject WallPrefab;
    public Vector3[] positions;
    public int prefabUnit = 5;
    public LineRenderer Footprint;
    public Vector3 offset = new Vector3(5, 5, 0);
    // Start is called before the first frame update
    void Start()
    {
        positions = new Vector3[Footprint.positionCount];
        Footprint.GetPositions(positions);
        CreateLevel();
    }

    // Update is called once per frame
    void Update()
    {

    }

    void CreateLevel()
    {
        for (int i = 0; i < positions.Length; i++)
        {
            Vector3 dir;
            if(i == positions.Length-1)
                dir = positions[0] - positions[i];
            else 
                dir = positions[i + 1] - positions[i];
            var offsetDir = new Vector3(offset.x * dir.normalized.x, offset.y, offset.z * dir.normalized.z);
            var wallPosition = positions[i] + offsetDir;
            Instantiate(WallPrefab, wallPosition, Quaternion.LookRotation(dir));
        }
    }
}
