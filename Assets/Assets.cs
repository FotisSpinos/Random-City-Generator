using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MapNode;

namespace MapAssets
{
    public class Assets : MonoBehaviour
    {
        [SerializeField] GameObject[] s_Buildings;
        [SerializeField] private GameObject[] m_Buildings;
        [SerializeField] private GameObject[] l_Buildings;
        [SerializeField] private GameObject[] groundObjects;
        [SerializeField] private GameObject[] _natureObjects;

        public static GameObject[] sBuildings;
        public static GameObject[] mBuildings;
        public static GameObject[] lBuildings;
        public static GameObject[] natureObjects;
        public static GameObject[] roads;
        public static Dictionary<string, GameObject> groundDictionary;

        // Initialize static collections
        private void OnEnable()
        {
            sBuildings = s_Buildings;
            mBuildings = m_Buildings;
            lBuildings = l_Buildings;
            natureObjects = _natureObjects;

            groundDictionary = new Dictionary<string, GameObject>();
            groundDictionary["Grass"] = groundObjects[0];
            groundDictionary["Rock"] = groundObjects[1];
        }

        // Spawn Random Model at the nodes ref parameter based on the node's type
        public static void SpawnRandomObject(Node.Type type, ref Node node)
        {
            GameObject[] selectedCollection = { };
            GameObject spawnObj = null;

            // Define available collection of prefubs based on node type
            switch (type)
            {
                case Node.Type.LARGE_BUILDING:
                    selectedCollection = lBuildings;
                    break;

                case Node.Type.MED_BUILDING:
                    selectedCollection = mBuildings;
                    break;

                case Node.Type.SMALL_BUILDING:
                    selectedCollection = sBuildings;
                    break;

                case Node.Type.GROUND:
                    selectedCollection = natureObjects;
                    break;
            }

            // Select and spawn random prefub in the selectedCollection
            if (selectedCollection.Length != 0)
            {
                int randIndex = Random.Range(0, selectedCollection.Length);
                spawnObj = selectedCollection[randIndex];
                Mesh mesh = spawnObj.GetComponent<MeshFilter>().sharedMesh;
                Material[] materials = spawnObj.GetComponent<Renderer>().sharedMaterials;
                Vector3 scale = spawnObj.transform.lossyScale;

                node.type = type;
                node.SpawnNode(mesh, materials, scale);
            }
        }
    }
}