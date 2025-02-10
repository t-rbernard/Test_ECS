using System;
using UnityEngine;

/**
 * Simple path description struct used to have a serializable game object to represent paths
 * Represented as float2 for easier editing and to prevent setting path under the terrain/in the sky
 */
public class PathStruct : MonoBehaviour
{
    public GameObject[] path = Array.Empty<GameObject>();

    private void OnDrawGizmos()
    {
        if(path != null)
        {
            for (int i = 0; i < path.Length - 1; i++)
            {
                Gizmos.DrawLine(path[i].transform.position, path[i + 1].transform.position);
            }
        }
    }
}
// Maybe possible to replace this struct with something easier to manage from the scene,
// like a list of empty game objects where we use their transform as way points (easier to manipulate
// from the scene than creating float2s) and then using gizmos to link them and show the full pathway ?