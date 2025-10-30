using UnityEngine;

public class MeshDeformerInput : MonoBehaviour {

    public float force = 10f;

    // when we apply the force have to pull the origin of the force a bit away to push the vertices in
    // basically give the deformer script a point a bit away from its actual surface & the point of contact
    public float forceOffset = 0.1f; 

    void Update()
    {
        if (Input.GetMouseButton(0))
        { // left click
            HandleInput();
        }
    }
    
    void HandleInput () {
        // basically getting the dir / thing were clicking at
        Ray inputRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

		if (Physics.Raycast(inputRay, out hit)) { // if we did hit smthing
            MeshDeformer deformer = hit.collider.GetComponent<MeshDeformer>(); // if that smthing has a deformer script

            if (deformer) { // then give that deformer script the data it needs
                Vector3 point = hit.point;
                point += hit.normal * forceOffset;
				deformer.AddDeformingForce(point, force);
			}
		}
	}
}