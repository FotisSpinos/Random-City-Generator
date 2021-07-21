using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MapChildNodeGen;

namespace MapNode
{
    public class Node : MonoBehaviour
    {
        private List<Node> naibors; // List of naibor objects
        private ChildNodeGen childNodeGen;   // Object used to generate child nodes

        public Vector3 rotationPoint;   // point used to define rotation
        private Vector3 scale;   // object's scale 

        public enum Type    // Available node types 
        {
            LARGE_BUILDING,
            MED_BUILDING,
            SMALL_BUILDING,
            GROUND,
            EMPTY
        }

        public enum Location    // Available node location
        {
            EMPTY,
            CITY_CENTER,
            CITY_SIDE
        }

        public Location location;   // object's location
        public Type type;   // object's type

        // Method for initializing the object
        public void Initialize()
        {
            scale = GetComponent<MeshRenderer>().bounds.size;
            naibors = new List<Node>();
            rotationPoint = Vector3.zero;
            type = Type.EMPTY;
            location = Location.EMPTY;
        }

        // Generates rotationPoint
        public void GenerateRotationPoint()
        {
            Vector3 point;
            float radious = 30;

            float xOffset = Random.Range(-radious, radious);
            float yOffset = Mathf.Sqrt(Mathf.Pow(radious, 2) - Mathf.Pow(xOffset, 2));
            float ySign = Random.Range(-1, 1);

            if (ySign >= 0)
                point = new Vector3(xOffset, 0, yOffset);
            else
                point = new Vector3(xOffset, 0, -yOffset);

            rotationPoint = transform.position + point;
        }

        public void RotateToPoint(Vector3 rotationPoint)
        {
            Vector3 direction = (rotationPoint - transform.localPosition).normalized;
            transform.forward = new Vector3(direction.x, 0, direction.z);
        }

        // Set node naibors using a list<Node> parameter
        public void SetNaibors(List<Node> naibors)
        {
            this.naibors = naibors;
        }

        // Sets node type
        public void SetType(Type type)
        {
            this.type = type;
        }

        public void SpawnNode(Mesh mesh, Material[] materials, Vector3 scale)
        {
            GetComponent<MeshFilter>().mesh = mesh;
            GetComponent<MeshRenderer>().sharedMaterials = materials;
            transform.localScale = scale;

            Vector3 objSize = GetComponent<MeshRenderer>().bounds.size;
            transform.position += new Vector3(0, objSize.y / 2, 0);
        }

        // Returns a list to return naibor nodes
        public List<Node> GetNaibors()
        {
            return naibors;
        }

        // Generates child nodes using the childNodeGen Object
        public void GenerateChildNodes(int maxChildSize, int maxGridSize)
        {
            childNodeGen = GetComponent<ChildNodeGen>();
            childNodeGen.Initialize(maxChildSize, maxGridSize);
            childNodeGen.GenerateChildNodes(type);
            List<Vector2> points = childNodeGen.GeneratePoints(type);

            Queue orderedPoints = childNodeGen.OrderQueueu(points);
            childNodeGen.GenerateNodes(orderedPoints, type, scale);
        }

        // Sets naibors using the node's position in the 2D array
        public void SetNaibors(Node[,] nodes, int x, int y, int xSize, int ySize)
        {
            List<Node> naibors = new List<Node>();

            if (x - 1 >= 0)
                naibors.Add(nodes[x - 1, y]);
            if (x + 1 < xSize)
                naibors.Add(nodes[x + 1, y]);
            if (y - 1 >= 0)
                naibors.Add(nodes[x, y - 1]);
            if (y + 1 < ySize)
                naibors.Add(nodes[x, y + 1]);
            if (x - 1 >= 0 && y - 1 >= 0)
                naibors.Add(nodes[x - 1, y - 1]);
            if (x - 1 >= 0 && y + 1 < ySize)
                naibors.Add(nodes[x - 1, y + 1]);
            if (x + 1 < xSize && y - 1 >= 0)
                naibors.Add(nodes[x + 1, y - 1]);
            if (x + 1 < xSize && y + 1 < ySize)
                naibors.Add(nodes[x + 1, y + 1]);

            SetNaibors(naibors);
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.green;
            Gizmos.DrawSphere(rotationPoint, 3);
            if (rotationPoint != Vector3.zero)
                Gizmos.DrawLine(transform.position, rotationPoint);
        }
    }
}