using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
	private float speed = 20f;

	public int damage = 50;

	public float explosionRadius = 0f;
	public GameObject impactEffect;
	public Vector3 dir;
	public Transform creator;
	
	// Update is called once per frame
	void Update()
	{
		float distanceThisFrame = speed * Time.deltaTime;
		transform.Translate(dir.normalized * distanceThisFrame, Space.World);

	}

	public void OnTriggerEnter(Collider other)
{
		if (other.transform.Equals(creator))
			return;
		if (other.transform.GetComponent<Entity>() != null) {
			other.transform.GetComponent<Entity>().BeAttacked(damage);
		}

		Destroy(gameObject);
	}

}
