using System;
using Unity.Mathematics;
using UnityEngine;

/**
 * Simple path description struct used to have a serializable game object to represent paths
 * Represented as float2 for easier editing and to prevent setting path under the terrain/in the sky
 */
public class PathStruct : MonoBehaviour
{
    public float2[] path = { };
}
// Maybe possible to replace this struct with something easier to manage from the scene,
// like a list of empty game objects where we use their transform as way points (easier to manipulate
// from the scene than creating float2s) and then using gizmos to link them and show the full pathway ?