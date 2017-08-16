using UnityEngine;
using UnitySampleAssets.CrossPlatformInput;

namespace CompleteProject
{
    public class PlayerShooting : MonoBehaviour
    {
        public int damagePerShot = 50;                  // The damage inflicted by each bullet.
        public float timeBetweenBullets = 0.15f;        // The time between each shot.
        public float range = 100f;                      // The distance the gun can fire.
        private int weaponupdate;

        float timer;                                    // A timer to determine when to fire.
        Ray shootRay = new Ray();                       // A ray from the gun end forwards.
        RaycastHit shootHit;                            // A raycast hit to get information about what was hit.
        int shootableMask;                              // A layer mask so the raycast only hits things on the shootable layer.
        ParticleSystem gunParticles;                    // Reference to the particle system.
        LineRenderer gunLine;                           // Reference to the line renderer.
        AudioSource gunAudio;                           // Reference to the audio source.
        Light gunLight;                                 // Reference to the light component.
		public Light faceLight;								// Duh
        float effectsDisplayTime = 0.2f;                // The proportion of the timeBetweenBullets that the effects will display for.
        Vector3 loopposition;
        private float radius = 4.2f;
        public SerialController serialController;              // read and write

        void Awake ()
        {
            // Create a layer mask for the Shootable layer.
            shootableMask = LayerMask.GetMask ("Shootable");

            // Set up the references.
            gunParticles = GetComponent<ParticleSystem> ();
            gunLine = GetComponent <LineRenderer> ();
            gunAudio = GetComponent<AudioSource> ();
            gunLight = GetComponent<Light> ();
            radius = 4.2f;
			//faceLight = GetComponentInChildren<Light> ();
            serialController = GameObject.Find("SerialController").GetComponent<SerialController>();
        }

        void Update ()
        {
            // Add the time since Update was last called to the timer.
            timer += Time.deltaTime;

#if !MOBILE_INPUT
            // If the Fire1 button is being press and it's time to fire...
			if(Input.GetButton ("Fire1") && timer >= timeBetweenBullets && Time.timeScale != 0)
            {
                // ... shoot the gun.
                Shoot ();
            }
#else
            // If there is input on the shoot direction stick and it's time to fire...
            if ((CrossPlatformInputManager.GetAxisRaw("Mouse X") != 0 || CrossPlatformInputManager.GetAxisRaw("Mouse Y") != 0) && timer >= timeBetweenBullets)
            {
                // ... shoot the gun
                Shoot();
            }
#endif
            // If the timer has exceeded the proportion of timeBetweenBullets that the effects should be displayed for...
            if(timer >= timeBetweenBullets * effectsDisplayTime)
            {
                // ... disable the effects.
                DisableEffects ();
            }

            receivemsg();
        }


        public void DisableEffects ()
        {
            // Disable the line renderer and the light.
            gunLine.enabled = false;
			faceLight.enabled = false;
            gunLight.enabled = false;
        }


        void Shoot ()
        {
            // Reset the timer.
            timer = 0f;

            // Play the gun shot audioclip.
            gunAudio.Play ();

            // Enable the lights.
            gunLight.enabled = true;
			faceLight.enabled = true;

            // Stop the particles from playing if they were, then start the particles.
            gunParticles.Stop ();
            gunParticles.Play ();

            // Enable the line renderer and set it's first position to be the end of the gun.
            gunLine.enabled = true;
            weaponupdate = PlayerMovement.weaponupdate;
            if (weaponupdate == 1)                       // get the weapon
            {
                gunLine.SetWidth(0.3f, 0.3f);
                damagePerShot = 100;
                //Debug.Log("damage"+damagePerShot);
            }

            else
            {
                gunLine.SetWidth(0.06f, 0.06f);
                damagePerShot = 50;
            }
                
            gunLine.SetPosition (0, transform.position);

            // Set the shootRay so that it starts at the end of the gun and points forward from the barrel.
            shootRay.origin = transform.position;
            shootRay.direction = transform.forward;

            // Perform the raycast against gameobjects on the shootable layer and if it hits something...
            if(Physics.Raycast (shootRay, out shootHit, range, shootableMask))
            {
                // Try and find an EnemyHealth script on the gameobject hit.
                EnemyHealth enemyHealth = shootHit.collider.GetComponent <EnemyHealth> ();

                // If the EnemyHealth component exist...
                if (enemyHealth != null)
                {
                    // ... the enemy should take damage.
                    enemyHealth.TakeDamage(damagePerShot, shootHit.point);                   
                }


                else
                {
                    GameObject loop = GameObject.Find("loop");
                    if (loop != null)
                    {
                        loopposition = loop.transform.position;
                        float delta_x = shootHit.point[0] - loopposition[0];
                        float delta_z = shootHit.point[2] - loopposition[2];
                        float RR = Mathf.Sqrt(delta_x * delta_x + delta_z * delta_z);

                        double theta = Mathf.Acos(delta_x/RR)*180/3.1416;               //calculate the angle, send it
                        if (delta_z < 0)
                            theta *= -1;
                        serialController.SendSerialMessage(theta.ToString());
                        //Debug.Log("Send the angle"+theta);

                    }

                }

                // Set the second position of the line renderer to the point the raycast hit.
                gunLine.SetPosition (1, shootHit.point);
                
            }
            // If the raycast didn't hit anything on the shootable layer...
            else
            {
                // ... set the second position of the line renderer to the fullest extent of the gun's range.
                gunLine.SetPosition (1, shootRay.origin + shootRay.direction * range);      
            }
        }



        public void receivemsg()
        {
            string message;

            //receive the message from Arduino
            message = serialController.ReadSerialMessage();
            if (message == null)
            {
                //Debug.Log("Got nothing");
                return;
            }

            // Check if the message is plain data or a connect/disconnect event.
            if (ReferenceEquals(message, SerialController.SERIAL_DEVICE_CONNECTED))
                Debug.Log("Connection established");
            else if (ReferenceEquals(message, SerialController.SERIAL_DEVICE_DISCONNECTED))
                Debug.Log("Connection attempt failed or disconnection detected");
            else
            {
                double dda;
                    
                if (double.TryParse(message, out dda))
                {
                    Debug.Log("Got:" + dda + "...");
                }
                else
                {
                    //  Debug.Log("error in the string..." + message + "||");      // receive some dirty data
                }
            }

        }


        //
    }
}