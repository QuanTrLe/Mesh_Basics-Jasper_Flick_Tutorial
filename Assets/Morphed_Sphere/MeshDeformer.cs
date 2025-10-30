using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
public class MeshDeformer : MonoBehaviour
{
    Mesh deformingMesh; // the mesh we have
    Vector3[] originalVertices, displacedVertices; // keeping track of the vertices

    // data for when we deform it
    Vector3[] vertexVelocities;

    void Start()
    {
        deformingMesh = GetComponent<MeshFilter>().mesh; // to get a copy of the mesh 
        originalVertices = deformingMesh.vertices; // the vertices that are in the right position
        displacedVertices = new Vector3[originalVertices.Length]; // vertices that are displaced in the process

        for (int i = 0; i < originalVertices.Length; i++)
        {
            displacedVertices[i] = originalVertices[i]; // just copying them over
        }

        vertexVelocities = new Vector3[originalVertices.Length]; // just populating
    }
    
    
    // the thing for the input handler to call
    public void AddDeformingForce (Vector3 point, float force) {
		Debug.DrawLine(Camera.main.transform.position, point);
	}
}