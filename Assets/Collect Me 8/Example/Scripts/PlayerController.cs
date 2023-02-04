using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {

	Vector3 Velocity;
	bool bScrolling;
	Vector3 LastPointerPosition;
	Rigidbody Body;

	public float MaxSpeed = 4f;
	public float DragForce = 25f;

	void Awake () 
	{
		Body = GetComponent<Rigidbody> ();
	}
	
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

		if (!Application.isMobilePlatform)
		{
			bool goingForward = Input.GetKey (KeyCode.W);
			bool goingBackward = false;//Input.GetKey (KeyCode.S);
			bool goingLeft = false; //Input.GetKey (KeyCode.A);
			bool goingRight = false; //Input.GetKey (KeyCode.D);

			if (goingForward)
				Body.AddForce (new Vector3 (0f, 0f, DragForce));
			
			if (goingBackward)
				Body.AddForce (new Vector3 (DragForce, 0f, 0f));
			
			if (goingLeft)
				Body.AddForce (new Vector3 (0f, 0f, -DragForce));
			
			if (goingRight)
				Body.AddForce (new Vector3 (0f, 0f, DragForce));
		} 

		Body.velocity = Vector3.ClampMagnitude (Body.velocity, MaxSpeed);
	}
}
