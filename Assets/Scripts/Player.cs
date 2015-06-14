using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;

public class Player : MonoBehaviour
{
	public float speed = 30f;
	public float jumpPower = 300f;
	public bool grounded;
	public Rigidbody2D rb2d;
	public float groundCheckTolerance = .015f;

	void Start()
	{
		rb2d = GetComponent<Rigidbody2D>();
	}

	void FixedUpdate()
	{
		RaycastHit2D hit = Physics2D.Raycast(transform.position, -Vector2.up);
		if (hit.collider != null) {
			float hitPointY = hit.point.y;
			float playerBottom = transform.position.y + (transform.localScale.y / 2);
			float distanceFromGround = Mathf.Abs(playerBottom - hitPointY - 1);
			grounded = (distanceFromGround < groundCheckTolerance) ? true : false;
		}

		if (grounded) {
			if (Input.GetKeyDown(KeyCode.Space))
				rb2d.AddForce(Vector2.up * jumpPower);

			Vector2 sideForce = Vector2.zero;
			if (Input.GetKey(KeyCode.RightArrow))
				sideForce.x = 1;
			else if (Input.GetKey(KeyCode.LeftArrow))
				sideForce.x = -1;
			rb2d.AddForce(sideForce * speed);
		}
	}
}
