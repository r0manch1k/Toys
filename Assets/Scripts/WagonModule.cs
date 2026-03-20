using UnityEngine;

[RequireComponent( typeof( Rigidbody ) )]
public class WagonModule : Wagon
{
	[SerializeField] private float followDistance = 3f;
	[SerializeField] private float detectionRadius = 5f;
	[SerializeField] private GameObject indicator;

	private SphereCollider _collider;
	private Rigidbody _rb;
	private Wagon _masterWagon;
	private bool _isAttached;

	public bool IsAttached => _isAttached;

	private void Awake()
	{
		_collider = gameObject.AddComponent<SphereCollider>();
		_collider.isTrigger = true;
		_collider.radius = detectionRadius;

		_rb = gameObject.GetComponent<Rigidbody>();
		_rb.isKinematic = true;

		if ( indicator != null )
			indicator.SetActive( false );
	}

	private void OnTriggerEnter(Collider other)
	{
		if ( _isAttached ) return;

		if ( other.TryGetComponent<WagonPlayer>( out var player ) )
		{
			player.SetNearbyWagon( this );

			if ( indicator != null )
				indicator.SetActive( true );
		}
	}

	private void OnTriggerExit(Collider other)
	{
		if ( _isAttached ) return;

		if ( other.TryGetComponent<WagonPlayer>( out var player ) )
		{
			player.ClearNearbyWagon( this );

			if ( indicator != null )
				indicator.SetActive( false );
		}
	}

	public void AttachTo(Wagon master)
	{
		_masterWagon = master;

		_isAttached = true;
		_collider.enabled = false;

		if ( indicator != null )
			indicator.SetActive( false );

		transform.position = _masterWagon.transform.position - _masterWagon.transform.forward * followDistance;
		transform.rotation = _masterWagon.transform.rotation;

		Debug.Log( "Wagon attached" );
	}

	private void LateUpdate()
	{
		if ( !_isAttached || _masterWagon == null ) return;

		Vector3 toMaster = _masterWagon.transform.position - transform.position;
		float distance = toMaster.magnitude;

		if ( distance > followDistance )
			transform.position = _masterWagon.transform.position - toMaster.normalized * followDistance;

		Vector3 flatDir = toMaster;
		flatDir.y = 0f;
		if ( flatDir.sqrMagnitude > 0.01f )
			transform.rotation = Quaternion.LookRotation( flatDir.normalized );
	}
}
