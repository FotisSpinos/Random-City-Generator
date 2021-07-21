using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MapAssets;
using MapNode;

namespace MapCitySide
{
    public class CitySideGen : MonoBehaviour
    {
        public List<Node> citySideNodes;
        private int amount;

        public void Initialize()
        {
            citySideNodes = new List<Node>();
            amount = 0;
        }

        public void SetCitySideNodes(ref Node[,] cityNodes)
        {
            foreach (Node n in cityNodes)
            {
                if (n.location != Node.Location.CITY_CENTER)
                {
                    n.location = Node.Location.CITY_SIDE;
                    citySideNodes.Add(n);
                }
            }
        }

        public void GenerateBuildings()
        {
            for (int i = 0; i < amount; i++)
            {
                Node currentNode = new Node();
                int randIndex;
                do
                {
                    randIndex = Random.Range(0, citySideNodes.Count);
                    currentNode = citySideNodes[randIndex];
                } while (citySideNodes[randIndex].type != Node.Type.EMPTY);

                //Rotate
                currentNode.RotateToPoint(currentNode.rotationPoint);

                Assets.SpawnRandomObject(Node.Type.SMALL_BUILDING, ref currentNode);
                citySideNodes[randIndex].GenerateChildNodes(4, 3);
            }
        }

        public void SetAmount(int amount)
        {
            this.amount = amount;
        }
    }
}