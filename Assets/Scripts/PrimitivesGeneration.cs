using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Primitive
{
    public class PrimitivesGeneration : MonoBehaviour
    {
        private Vector3 StartingCenter;
        [SerializeField] private string axiom;
        public Material buildingMaterial;
        //Range of buildings' length
        [SerializeField] [Range(1, 100)] private float length;
        //Range of buildings' width
        [SerializeField] [Range(1, 10)] private float width;
        [SerializeField] [Range(3, 15)] private int polygoneMinSides;
        [SerializeField] [Range(3, 15)] private int polygoneMaxSides;
        //Range of angle's value separation two buildings at each step 
        [SerializeField] [Range(0, 360)] private int turnAngle;
        [SerializeField] private int buildingsDistance;

        //
        [SerializeField] [Range(0, 20)] private int nbIncrement;
        [SerializeField] [Range(0, 20)] private int nbIterations;
        public GameObject buildingPrefab;

        private Vector3 direction;
        private Vector3 currentCenter;
        void Start()
        {
            StartingCenter = Vector3.zero;
            currentCenter = Vector3.zero;
            direction = Vector3.right.normalized;
            for (int j = 0; j < nbIncrement; j++)
            {
                for (int i = 0; i < nbIterations; i++)
                {
                    ApplyRule();
                }
                currentCenter = StartingCenter - currentCenter;
                turnAngle -= 2;
            }
        }
        void ApplyRule()
        {
            foreach (char member in axiom)
            {

                switch (member)
                {
                    case '-':
                        this.direction = Tools.TurnRight(direction, turnAngle);
                        this.currentCenter = this.currentCenter + this.direction * buildingsDistance;
                        break;
                    case '+':
                        this.direction = Tools.TurnLeft(direction, turnAngle);
                        this.currentCenter = this.currentCenter + this.direction * buildingsDistance;
                        break;
                    // P stands for random Polygone
                    case 'P':
                        BuildBuilding(null);
                        break;
                    // C stands for Cube
                    case 'C':
                        BuildBuilding(4);
                        break;
                    default:
                        break;
                }
            }
        }
        void BuildBuilding(int? nbSides)
        {
            PrimitiveBuilding pb = GameObject.Instantiate(buildingPrefab, this.currentCenter, Quaternion.identity).GetComponent<PrimitiveBuilding>();
            if(nbSides == null)
                pb.Initialize(Random.Range(polygoneMinSides, polygoneMaxSides), Random.Range(width/2, width), Random.Range(length/6, length), currentCenter);
            else
                pb.Initialize(nbSides, width, length, currentCenter);
            pb.BuildPrimitive();
        }
        void Update()
        {
            if (Input.GetKey(KeyCode.R))
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
        }
    }
}
