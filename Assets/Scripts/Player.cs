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

	void Start()
	{
		rb2d = GetComponent<Rigidbody2D>();
	}

	void FixedUpdate()
	{
		RaycastHit2D hit = Physics2D.Raycast(transform.position, -Vector2.up);
		if (hit.collider != null) {
			float distanceFromGround = hit.point.y - transform.position.y;
			Debug.Log(hit.collider.gameObject.name);
			Debug.Log(distanceFromGround);
		}

		if (Input.GetKey(KeyCode.Space))
			rb2d.AddForce(Vector2.up * jumpPower);
	}
}
