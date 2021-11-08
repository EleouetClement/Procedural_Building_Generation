using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Primitive
{
    public class PrimitivesGeneration : MonoBehaviour
    {
        private Vector3 StartingCenter;
        [SerializeField] private string axiom;
        public Material buildingMaterial;
        [SerializeField] [Range(1, 20)] private float length;
        [SerializeField] [Range(1, 10)] private float width;
        [SerializeField] [Range(3, 15)] private int polygoneMinSides;
        [SerializeField] [Range(3, 15)] private int polygoneMaxSides;
        [SerializeField] [Range(0, 360)] private int turnRadius;
        public GameObject buildingPrefab;

        private Vector3 direction;
        private Vector3 currentCenter;
        void Start()
        {
            StartingCenter = Vector3.zero;
            currentCenter = Vector3.zero;
            direction = Vector3.right.normalized;
            ApplyRule();
        }
        void ApplyRule()
        {
            foreach (char member in axiom)
            {

                switch (member)
                {
                    case '-':
                        this.direction = Tools.TurnRight(direction, turnRadius);
                        this.currentCenter = this.currentCenter + this.direction * (width - Random.Range(2, width/2));
                        break;
                    case '+':
                        this.direction = Tools.TurnLeft(direction, turnRadius);
                        this.currentCenter = this.currentCenter + this.direction * (width - Random.Range(2, width / 2));
                        break;
                    // P stands for Cylindre/polygone
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
    }
}
