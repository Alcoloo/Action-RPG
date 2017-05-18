using Rpg.Characters;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaAttack : MonoBehaviour {
    
    public int damage = 10;

    public float durationTime = 3f;

    public float animationTime = 1f;

    public float diameter = 15f;

    public float finalHeight = 5f;

    private GameObject player = null;

    private bool damageDeal = false;

    private float startTime;

	// Use this for initialization
	void Start () {
        player = GameObject.FindGameObjectWithTag("Player");

        transform.localScale = new Vector3(diameter, 1, diameter);

        startTime = Time.time;

        StartCoroutine("destroyAfterCooldown");
	}
	
	// Update is called once per frame
	void Update () {
        float percent = (Time.time - startTime) / animationTime;

		if(percent < 1) {
            transform.localScale = new Vector3(transform.localScale.x, finalHeight * percent, transform.localScale.z);
            transform.position = new Vector3(transform.position.x, finalHeight * percent, transform.position.z);
        }
        else {
            transform.localScale = new Vector3(transform.localScale.x, finalHeight, transform.localScale.z);
            transform.position = new Vector3(transform.position.x, finalHeight, transform.position.z);
        }
	}

    void OnTriggerStay (Collider other) {
        if (other.name == player.name && !damageDeal) {
            player.GetComponent<Caracteristic>().TakeDamage(damage, Rpg.KIND.both);
            damageDeal = true;
        }
    }

    IEnumerator destroyAfterCooldown() {
        yield return new WaitForSeconds(animationTime + durationTime);
        Destroy(gameObject);
    }
}
