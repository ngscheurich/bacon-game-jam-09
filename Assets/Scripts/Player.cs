using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;

public class Player : MonoBehaviour
{
	public float speed = 10f;
	public float jumpPower = 30f;
	public bool grounded;
	public Rigidbody2D rb2d;
	public float groundCheckTolerance = .01f;

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
			float distanceFromGround = hitPointY - playerBottom;
			Debug.Log(distanceFromGround);
			if (distanceFromGround < groundCheckTolerance) {
				Debug.Log("grounded");
			}
		}

		if (Input.GetKeyDown(KeyCode.Space))
			rb2d.AddForce(Vector2.up * jumpPower);
	}
}
