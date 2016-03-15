using UnityEngine;
using System.Collections;

public class Move : MonoBehaviour {

	public float speed = 6.0F;
	public float jumpSpeed = 8.0F;
	public float gravity = 20.0F;
	private Vector3 moveDirection = Vector3.zero;


	void Update() {

		CharacterController controller = GetComponent<CharacterController>();

		if (controller.isGrounded) {
			moveDirection = new Vector3(0, 0, Input.GetAxis("Vertical") * 5);
			moveDirection = transform.TransformDirection(moveDirection);
			moveDirection *= speed;
			if (Input.GetButton("Jump"))
			{
				moveDirection.y = jumpSpeed;
			}
			
		}

		controller.transform.Rotate(new Vector3(0f, Input.GetAxis("Horizontal"), 0f));

		moveDirection.y -= gravity * Time.deltaTime;
		controller.Move(moveDirection * Time.deltaTime);
        
	}
}

