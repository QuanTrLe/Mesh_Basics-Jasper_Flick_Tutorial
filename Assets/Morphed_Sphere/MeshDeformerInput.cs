using UnityEngine;

public class MeshDeformerInput : MonoBehaviour {

    public float force = 10f;

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
				deformer.AddDeformingForce(point, force);
			}
		}
	}
}