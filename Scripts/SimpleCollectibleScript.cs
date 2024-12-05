using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleCollectibleScript : MonoBehaviour {

	public static event EventHandler OnCollected;
	public enum CollectibleTypes {Star};

	[SerializeField] private CollectibleTypes CollectibleType; 
	
	[SerializeField] private bool rotate; 

	[SerializeField] private float rotationSpeed;

	[SerializeField] private AudioClip collectSound;

	[SerializeField] private GameObject collectEffect;

	private int scoreValue = 1;
	void Update () {

		if (rotate)
			transform.Rotate (Vector3.up * (rotationSpeed * Time.deltaTime), Space.World);

	}
	void OnTriggerEnter(Collider other)
	{
		if (other.tag == "Player") {
			Collect ();
		}
	}

	public void Collect()
	{
		OnCollected?.Invoke(this, EventArgs.Empty);
		
		if(collectSound)
			AudioSource.PlayClipAtPoint(collectSound, transform.position);
		if(collectEffect)
			Instantiate(collectEffect, transform.position, Quaternion.identity);

		if (CollectibleType == CollectibleTypes.Star) {
	
			GameManager.Instance.AddScore(scoreValue);
		}
		
		Destroy (gameObject);
	}
}
