using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fire : MonoBehaviour {


	public float spreadAfter = 1;
	public float damagepp = 8;
	public float force = 100;
	public static int dmgFlag;

	Ray shootRay = new Ray();                       // A ray from the gun end forwards.
	RaycastHit shootHit;                            // A raycast hit to get information about what was hit.
	public int damagePerShot = 10;                  // The damage inflicted by each bullet.


	// Use this for initialization
	void Start () {
		dmgFlag = 0;
	}
	
	// Update is called once per frame
	void Update () {
		
	}



	void OnParticleCollision (GameObject other)
	{
		if (other.tag == "Enemy") 
		{
			
			dmgFlag = 1;
			EnemyHealth enemyHealth = other.GetComponent <EnemyHealth> ();

			// If the EnemyHealth component exist...
			if (enemyHealth != null)
			{
				// ... the enemy should take damage.
				enemyHealth.TakeDamage(20, new Vector3(0, -20, 0));   

				//GameObject.FindTag ("Emitter").GetComponent<ParticleSystem> ();
				Debug.Log ("HERE");
				               
			}
		}


//		Rigidbody body = other.GetComponent<Rigidbody>();
//		other.SendMessageUpwards("ApplyDamage", damagepp, SendMessageOptions.DontRequireReceiver);
//
//		if (body)
//		{
//			Vector3 direction = other.transform.position - transform.position;
//			direction = direction.normalized;
//			body.AddForce (direction * force);
//		}
//
//		//yield return new WaitForSeconds(spreadAfter);
//		if (other != null)
//		{
//			other.SendMessageUpwards("Fire", null, SendMessageOptions.DontRequireReceiver);
//		}
	}
}