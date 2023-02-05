using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using System.Collections;
using Deckbuilder;

namespace C8 {

public class Collectible : MonoBehaviour {

	[System.Serializable]
	public class CallbackFunction : UnityEvent <GameObject> {}

	[SerializeField]
	GameObject MeshObject;

	[SerializeField]
	GameObject ShadowObject;

	[SerializeField] 
	bool PlaySounds = true;
	
	[SerializeField] 
	bool PlayParticles = true;

	[SerializeField]
	ParticleSystem PickupEffect;
	
	[SerializeField] // Will play random sound 	
	AudioSource[] PickupSounds;

	[SerializeField]
	string[] CollideWithTags;

	[SerializeField]
	bool IgnoreCollisions;

	[SerializeField]
	bool DestroyOnCollected;

		[SerializeField] private float gravityForce;

	public CallbackFunction OnCollected;
	public CallbackFunction OnRestored;
	public CallbackFunction OnDestroyed;

	bool bWasDestroyed;
	bool bCollected;
	bool bShadowWasActive;
	private RaycastHit _raycastHit;
		private Rigidbody _rb;

	AudioSource ActiveSound;

	void OnDestroy () 
	{
		bWasDestroyed = true;
		OnDestroyed.Invoke (gameObject);
	}

	void OnEnable () 
	{
		if (PickupSounds != null)
			foreach (var sound in PickupSounds)
				sound.gameObject.SetActive (false);
	}

	void Awake () 
	{
            _rb = GetComponent<Rigidbody> ();

        if (PickupEffect != null)
			PickupEffect.gameObject.SetActive (false);

		if (ShadowObject != null) 
			bShadowWasActive = ShadowObject.activeSelf;

			Ray ray = new Ray(transform.position, Vector3.zero - transform.position);
			if (Physics.Raycast(ray, out _raycastHit))
			{
				//transform.position = _raycastHit.point;
				transform.up = _raycastHit.normal;
            }
		
		bWasDestroyed = false;
		bCollected = false;
	}

	//void OnTriggerEnter (Collider collider) 
	//{
	//	if (IgnoreCollisions)
	//		return;

	//	GameObject colliderObject = collider.gameObject;

	//	if (CollideWithTags != null && CollideWithTags.Length > 0) 
	//	{
	//		foreach (string tag in CollideWithTags) 
	//			if (colliderObject.CompareTag (tag))
	//				Collect ();
	//	} 
	//	else 
	//		Collect ();
	//}

	public void Collect () 
	{
		if (bCollected)
			return;

			FloatingTextManager.Instance.SpawnText(transform.position, 100, FloatingTextType.Heal);
            GameManager.Instance.AddPoints(100);

            if (PlayParticles && PickupEffect != null) 
		{
			PickupEffect.gameObject.SetActive (true);
			var scaler = PickupEffect.GetComponent<ParticleScaler> ();

			if (scaler != null)
				scaler.ApplyScale ();

			PickupEffect.Play ();
		}

		int numOfSounds = PickupSounds.Length;
		if (PlaySounds && PickupSounds != null && numOfSounds > 0) 
		{
			int index = 0;

			if (numOfSounds > 1)
				index = Random.Range(0, PickupSounds.Length);

			var sound = PickupSounds [index];
			sound.gameObject.SetActive (true);
			sound.Play ();
		}

		MeshObject.SetActive (false);
		ShadowObject.SetActive (false);
	
		bCollected = true;

		OnCollected.Invoke (gameObject);
	}

	public void Restore ()
	{
		if (bWasDestroyed) 
		{
			Debug.LogError ("Unable to restore Collectible: GameObject was already destroyed");
			return;
		}

		bCollected = false;
		MeshObject.SetActive (true);

		if (bShadowWasActive)
			ShadowObject.SetActive (true);

		OnRestored.Invoke (gameObject);
	}

	// Use this for initialization
	void Start () 
	{
		Application.targetFrameRate = 60;
	}

	void Dispose ()
	{
		if (DestroyOnCollected) 
		{
			Destroy (gameObject);
		}
		else 
		{
			if (PickupEffect != null) 
				PickupEffect.gameObject.SetActive (false);
			
			gameObject.SetActive (false);
			
			if (PickupSounds != null)
				foreach (var sound in PickupSounds)
					sound.gameObject.SetActive (false);
		}
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (bCollected)
		{
			bool readyToDispose = true;

			bool effectsFinished = true;
			if (PickupEffect != null && PickupEffect.IsAlive ())
				effectsFinished = false;

			bool soundsFinished = true;
			if (PickupSounds != null && PickupSounds.Length > 0)
			{
				foreach (var sound in PickupSounds) 
				{
					if (sound.isPlaying)
						soundsFinished = false;
				}
			}

			readyToDispose = soundsFinished && effectsFinished;

			if (readyToDispose) 
				Dispose ();
		}

			//_rb.AddForce(-transform.up * gravityForce, ForceMode.Acceleration);

    }
}

}
