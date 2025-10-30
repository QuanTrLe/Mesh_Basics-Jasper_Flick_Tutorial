using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
public class MeshDeformer : MonoBehaviour
{
    float uniformScale = 1f;

    Mesh deformingMesh; // the mesh we have
    Vector3[] originalVertices, displacedVertices; // keeping track of the vertices

    // data for when we deform it
    Vector3[] vertexVelocities;

    // how badly the vertices want to move back to its original position the further it is
    public float springForce = 20f;
    public float damping = 5f; // infinite forces arent a thing, how fast is the force gonna die out

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

    void Update()
    {
        uniformScale = transform.localScale.x; // make sure to take gameobj scale in mind

        for (int i = 0; i < displacedVertices.Length; i++)
        {
            UpdateVertex(i);
        }
        deformingMesh.vertices = displacedVertices;
        deformingMesh.RecalculateNormals(); // new normals since vertexes got moved around
    }

    // basically just get all the forces we need and displace the vertex based on that
    void UpdateVertex (int i) {
        Vector3 velocity = vertexVelocities[i]; // the velocity of a specific vertice

        // the velocity of that vertice for it to start bouncing back
        Vector3 displacement = displacedVertices[i] - originalVertices[i]; // back to its original position
        displacement *= uniformScale; // accounting for gameobj scale
        velocity -= displacement * springForce * Time.deltaTime;
        velocity *= 1f - damping * Time.deltaTime; // apply damp
        vertexVelocities[i] = velocity; // total force

        // the total force w time accounted
		displacedVertices[i] += velocity * (Time.deltaTime / uniformScale);
	}

    // the thing for the input handler to call
    public void AddDeformingForce (Vector3 point, float force) {
        point = transform.InverseTransformPoint(point); // jsut to make sure if we moved the sphere since we doing calc in local space
		for (int i = 0; i < displacedVertices.Length; i++) { // check every vertex
			AddForceToVertex(i, point, force);  // then apply appropriate force
		}
	}

    // to calc the amount of force for one vertex knowing the force and dir of it
    void AddForceToVertex (int i, Vector3 point, float force)
    {
        Vector3 pointToVertex = displacedVertices[i] - point; // the relative distance from this vertex to point of contact
        pointToVertex *= uniformScale; // making sure we account for gameobj scale

        // calc of the force
        // technically from inverse-square law it's just force / distance, but have 1 + distance to avoid inf the closer to the point
        float attenuatedForce = force / (1f + pointToVertex.sqrMagnitude);

        // a = F/m and delta_v = a * delta_t
        float velocity = attenuatedForce * Time.deltaTime; // since ignoring mass (1) just do delta_v = F * delta_t 

        vertexVelocities[i] += pointToVertex.normalized * velocity; // store the end result w/ dir of force in mind
    }
}