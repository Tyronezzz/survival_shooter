using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fire : MonoBehaviour {


	public float spreadAfter = 1;
	public float damagepp = 8;
	public float force = 100;


	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}



	void OnParticleCollision (GameObject other)
	{
		Rigidbody body = other.GetComponent<Rigidbody>();
		other.SendMessageUpwards("ApplyDamage", damagepp, SendMessageOptions.DontRequireReceiver);

		if (body)
		{
			Vector3 direction = other.transform.position - transform.position;
			direction = direction.normalized;
			body.AddForce (direction * force);
		}

		//yield return new WaitForSeconds(spreadAfter);
		if (other != null)
		{
			other.SendMessageUpwards("Fire", null, SendMessageOptions.DontRequireReceiver);
		}
	}
}