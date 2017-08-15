using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotator : MonoBehaviour {

    private float startWait;
    private float waveWait;


    void Start()
    {
        startWait = 1;
        waveWait = 5;
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
            transform.position = new Vector3(Random.Range(-20, 20), 0, Random.Range(-20, 20));
          //  gameObject.SetActive(true);
            yield return new WaitForSeconds(waveWait);
            //
        }



    }
}
