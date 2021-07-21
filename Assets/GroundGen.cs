using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MapAssets;
using MapChildNodeGen;
using MapNode;

namespace MapGround
{
    public class GroundGen : MonoBehaviour
    {
        private Node[,] parentNodes;

        // Method for setting the parent node map using a reference
        public void SetParentNodes(ref Node[,] parentNodes)
        {
            this.parentNodes = parentNodes;
        }

        // For each parent node chech how many naibors are small_buildings or empty 
        public void SpawnGroundObj()
        {
            for (int x = 0; x < parentNodes.GetLength(0); x++)
            {
                for (int y = 0; y < parentNodes.GetLength(1); y++)
                {
                    List<Node> nodeNaibors = parentNodes[x, y].GetNaibors();
                    if (nodeNaibors.Count > 0)
                    {
                        int naiborhoods = 0;
                        int empty = 0;

                        foreach (Node n in nodeNaibors)
                        {
                            if (n.location == Node.Location.CITY_SIDE)
                            {
                                if (n.type == Node.Type.EMPTY)
                                {
                                    empty++;
                                }
                                else if (n.type == Node.Type.SMALL_BUILDING)
                                {
                                    naiborhoods++;
                                }
                            }
                        }

                        // Spawn Ground Object
                        SpawnObj(x, y, naiborhoods, empty);
                    }
                }
            }
        }

        // Cellular automata used to spawn appropriate gound object
        private void SpawnObj(int x, int y, int naiborhoodCount, int emptyNaibors)
        {
            Vector3 nodeSize = parentNodes[x, y].GetComponent<Renderer>().bounds.size;
            int persentage = Random.Range(0, 100);

            if (parentNodes[x, y].location == Node.Location.CITY_SIDE)
            {
                if (parentNodes[x, y].type == Node.Type.SMALL_BUILDING && persentage < 90)
                {
                    Instantiate(Assets.groundDictionary["Rock"], parentNodes[x, y].transform.position - new Vector3(0, nodeSize.y / 2, 0), Quaternion.identity);
                }
                else if (naiborhoodCount > 5 && persentage > 90)
                {
                    Instantiate(Assets.groundDictionary["Rock"], parentNodes[x, y].transform.position - new Vector3(0, nodeSize.y / 2, 0), Quaternion.identity);
                }
                else if (naiborhoodCount < 3)
                {
                    GameObject ground = Instantiate(Assets.groundDictionary["Grass"], parentNodes[x, y].transform.position - new Vector3(0, nodeSize.y / 2, 0), Quaternion.identity);
                    Node node = ground.AddComponent<Node>();
                    ChildNodeGen cng = ground.AddComponent<ChildNodeGen>();

                    node.Initialize();
                    node.SetType(Node.Type.GROUND);
                    node.GenerateChildNodes(Random.Range(1, 3), Random.Range(2,9));  //Randomize a bit
                }
                else
                {
                    Instantiate(Assets.groundDictionary["Rock"], parentNodes[x, y].transform.position - new Vector3(0, nodeSize.y / 2, 0), Quaternion.identity);
                }
            }

            else if (parentNodes[x, y].location == Node.Location.CITY_CENTER)
            {
                Instantiate(Assets.groundDictionary["Rock"], parentNodes[x, y].transform.position - new Vector3(0, nodeSize.y / 2, 0), Quaternion.identity);
            }

            else
            {
                Debug.Log("Undefined Node Location At:" + parentNodes[x, y].transform.position);
            }
        }
    }
}