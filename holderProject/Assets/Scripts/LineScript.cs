using UnityEngine;
using System.Collections;

public enum LineGameOrigin
{
    AccoppiaParole,
    AccoppiaImmagini,
}

public class LineScript : MonoBehaviour
{

	public GameObject gameObject1;          // Reference to the first GameObject
	public GameObject gameObject2;          // Reference to the second GameObject

	private LineRenderer line;                           // Line Renderer
	public LineGameOrigin gameOrigin;

	// Use this for initialization
	void Start()
	{

	}

	// Update is called once per frame
	void Update()
	{
		// Check if the GameObjects are not null
		/*if (gameObject1 != null && gameObject2 != null)
		{
			// Update position of the two vertex of the Line Renderer
			line.SetPosition(0, gameObject1.transform.position);
			line.SetPosition(1, gameObject2.transform.position);
		}*/
		if (Input.touchCount >= 1 && Input.GetTouch(0).phase == TouchPhase.Began)
		{
			// The pos of the touch on the screen
			Vector2 vTouchPos = Input.GetTouch(0).position;

			// The ray to the touched object in the world
			Ray ray = Camera.main.ScreenPointToRay(vTouchPos);

			// Your raycast handling
			RaycastHit vHit;
			if (Physics.Raycast(ray.origin, ray.direction, out vHit))
			{
				if (vHit.collider != null && vHit.transform.tag == "Line")
				{
					GameObject touchedObject = vHit.transform.gameObject;
					print("Line Touched");
					switch(gameOrigin){
						case LineGameOrigin.AccoppiaParole:
						AccoppiaLeParoleManager.Instance.DeleteConnection(touchedObject.GetComponent<LineScript>().gameObject1, touchedObject.GetComponent<LineScript>().gameObject2);
						break;
						case LineGameOrigin.AccoppiaImmagini:
						AccoppiaLeImmaginiManager.Instance.DeleteConnection(touchedObject.GetComponent<LineScript>().gameObject1, touchedObject.GetComponent<LineScript>().gameObject2);
						break;
					}
					Destroy(vHit.transform.gameObject);
				}
			}
		}
	}
	public void InitializeLine(GameObject obj1, GameObject obj2)
	{
		gameObject1 = obj1;
		gameObject2 = obj2;
		// Add a Line Renderer to the GameObject
		line = this.gameObject.AddComponent<LineRenderer>();
		//line.useWorldSpace = false;
		// Set the width of the Line Renderer
		line.startWidth = 1.0f;
		line.endWidth = 1.0f;
		// Set the number of vertex fo the Line Renderer
		line.positionCount = 2;
		line.startColor = Color.black;
		line.endColor = Color.black;
		line.SetPosition(0, obj1.transform.position);
		line.SetPosition(1, obj2.transform.position);
		/*MeshCollider meshCollider = this.gameObject.AddComponent<MeshCollider>();
		Mesh mesh = new Mesh();
		line.BakeMesh(mesh, Camera.main, false);
		meshCollider.sharedMesh = mesh;*/
		CapsuleCollider CapsuleCol = this.gameObject.AddComponent<CapsuleCollider>();
		CapsuleCol.transform.position = (obj1.transform.position + obj2.transform.position) * 0.5f;
		CapsuleCol.transform.LookAt(obj2.transform.position);
		CapsuleCol.GetComponent<CapsuleCollider>().height = (Vector3.Distance(obj1.transform.position, obj2.transform.position) * 7);
		CapsuleCol.GetComponent<CapsuleCollider>().radius = 5.0f;
		CapsuleCol.GetComponent<CapsuleCollider>().direction = 2;
		CapsuleCol.GetComponent<CapsuleCollider>().center = new Vector3(0f, 0f, 0f);

	}
}