using System.Collections.Generic;
using UnityEngine;

namespace CityGeneration
{
    public class CityCenterGen : MonoBehaviour
    {
        // A list containing the nodes located in the city center
        public List<Node> cityCenterNodes;

        // Building amounts
        private int mBuilingsAm;
        private int lBuilingsAm;

        // Keeps track of the big builings spawned
        private int lBuildingsSpawned;

        public void Initialize()
        {
            cityCenterNodes = new List<Node>();
            lBuildingsSpawned = 0;
            lBuilingsAm = 0;
            mBuilingsAm = 0;
        }

        public List<Node> GetCityCenterNodes()
        {
            return cityCenterNodes;
        }

        public void SetNodes(ref Node[,] nodesRef)
        {
            Vector2 center = new Vector2(nodesRef.GetLength(0) / 2, nodesRef.GetLength(1) / 2);

            // Define random offset from center
            int randOffsetX = Random.Range(-5, 5);
            int randOffsetY = Random.Range(-5, 5);
            center.x += randOffsetX;
            center.y += randOffsetY;

            int centerX = (int)center.x;
            int centerY = (int)center.y;

            // Add node to city center nodes
            nodesRef[centerX, centerY].location = Node.Location.CITY_CENTER;
            cityCenterNodes.Add(nodesRef[centerX, centerY]);

            int randSize = Random.Range(6, 10);

            // Generate nodes around the center
            for (int x = 1; x < randSize + 1; x++)
            {
                for (int y = 1; y < randSize + 1; y++)
                {
                    if (centerX - x >= 0)
                    {
                        nodesRef[centerX - x, centerY].location = Node.Location.CITY_CENTER;
                        cityCenterNodes.Add(nodesRef[centerX - x, centerY]);
                    }


                    if (centerX + x < nodesRef.GetLength(0))
                    {
                        nodesRef[centerX + x, centerY].location = Node.Location.CITY_CENTER;
                        cityCenterNodes.Add(nodesRef[centerX + x, centerY]);
                    }

                    if (centerY - y >= 0)
                    {
                        nodesRef[centerX, centerY - y].location = Node.Location.CITY_CENTER;
                        cityCenterNodes.Add(nodesRef[centerX, centerY - y]);
                    }

                    if (centerY + y < nodesRef.GetLength(1))
                    {
                        nodesRef[centerX, centerY + y].location = Node.Location.CITY_CENTER;
                        cityCenterNodes.Add(nodesRef[centerX, centerY + y]);
                    }

                    if (centerX - x >= 0 && centerY - y >= 0)
                    {
                        nodesRef[centerX - x, centerY - y].location = Node.Location.CITY_CENTER;
                        cityCenterNodes.Add(nodesRef[centerX - x, centerY - y]);
                    }

                    if (centerX - x >= 0 && centerY + y >= 0)
                    {
                        nodesRef[centerX - x, centerY + y].location = Node.Location.CITY_CENTER;
                        cityCenterNodes.Add(nodesRef[centerX - x, centerY + y]);
                    }

                    if (centerX + x >= 0 && centerY - y >= 0)
                    {
                        nodesRef[centerX + x, centerY - y].location = Node.Location.CITY_CENTER;
                        cityCenterNodes.Add(nodesRef[centerX + x, centerY - y]);
                    }

                    if (centerX + x >= 0 && centerY + y >= 0)
                    {
                        nodesRef[centerX + x, centerY + y].location = Node.Location.CITY_CENTER;
                        cityCenterNodes.Add(nodesRef[centerX + x, centerY + y]);
                    }
                }
            }
        }

        public void GenerateBuildings()
        {
            int randIndex;
            // Choose random nodes and generate buildings
            for (int i = 0; i < mBuilingsAm; i++)
            {
                Node node;
                do
                {
                    randIndex = Random.Range(0, cityCenterNodes.Count);
                    node = cityCenterNodes[randIndex];
                } while (node.type != Node.Type.EMPTY);

                Assets.SpawnRandomObject(Node.Type.MED_BUILDING, ref node);
                cityCenterNodes[randIndex] = node;
            }

            // Cellular Automata used to spawn big objects
            for (int i = 0; i < cityCenterNodes.Count; i++)
            {
                Node currentNode = cityCenterNodes[i];

                if (currentNode.type == Node.Type.EMPTY)
                {
                    List<Node> naibors = currentNode.GetNaibors();
                    int mBuildingsCount = 0;
                    int lBuildingsCount = 0;

                    foreach (Node naibor in naibors)
                    {
                        if (naibor.type == Node.Type.MED_BUILDING)
                        {
                            mBuildingsCount++;
                        }
                        else if (naibor.type == Node.Type.MED_BUILDING)
                        {
                            lBuildingsCount++;
                        }
                    }

                    int randomPercentage = Random.Range(0, 100);

                    if (mBuildingsCount >= 6 && lBuildingsCount < 1 && randomPercentage > 70 && lBuildingsSpawned < lBuilingsAm)
                    {
                        Assets.SpawnRandomObject(Node.Type.LARGE_BUILDING, ref currentNode);
                        lBuildingsSpawned++;
                    }
                }
            }
        }

        private void OnDrawGizmos()
        {
            // When the game is running, spawn spheres on each city center node (Used for testing)
            if (Application.isPlaying)
            {
                foreach (Node n in cityCenterNodes)
                {
                    Gizmos.color = Color.yellow;
                    Gizmos.DrawSphere(n.transform.position, 10);
                }
            }
        }

        public void SetMediumBuilings(int amount)
        {
            this.mBuilingsAm = amount;
        }

        public void SetLargeBuildings(int amount)
        {
            this.lBuilingsAm = amount;
        }
    }
}

/*
 *             for (int i = 0; i < mBuilingsAm; i++)
            {
                int randIndex = Random.Range(0, cityCenterNodes.Count);

                if (cityCenterNodes[randIndex].type == Node.Type.EMPTY)
                {
                    Node node = cityCenterNodes[randIndex];
                    MapAssets.Assets.SpawnRandomObject(Node.Type.MED_BUILDING, ref node);
                    cityCenterNodes[randIndex] = node;
                }
            }
*/