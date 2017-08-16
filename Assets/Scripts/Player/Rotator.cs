using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotator : MonoBehaviour {

    private float startWait;
    private float waveWait;
	public GameObject pickupp;

    void Start()
    {
        startWait = 5;
        waveWait = 8;
        StartCoroutine(weaponWaves());
        
    }



    void Update()           // Update is called once per frame
    {
        transform.Rotate(new Vector3(15, 30, 45) * Time.deltaTime);
    }

    IEnumerator weaponWaves()
    {
        yield return new WaitForSeconds(startWait);

        while (true)
        {
			GameObject pckup2 = pickupp;

			Vector3 player_pos = GameObject.Find ("Player").transform.position;

			transform.position = new Vector3(Random.Range(player_pos[0]-5, player_pos[0]+5), 0, Random.Range(player_pos[2]-5, player_pos[2]+5));
			Instantiate (pckup2, transform.position, Quaternion.identity);

			yield return new WaitForSeconds(waveWait);
        }

    }
}
