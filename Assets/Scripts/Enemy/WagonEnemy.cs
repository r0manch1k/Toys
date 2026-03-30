using UnityEngine;
using UnityEngine.AI;

[RequireComponent( typeof( NavMeshAgent ) )]
public class WagonEnemy : Wagon
{
	[SerializeField] private float aggroRadius = 10f;
	[SerializeField] private float maxTurnSpeed = 120f;
	[SerializeField] private GameObject indicator;

	private SphereCollider _collider;
	private NavMeshAgent _agent;
	private WagonPlayer _player;
	private Vector3 _spawnPosition;
	private bool _isAggro;

	private void Awake()
	{
		_collider = gameObject.AddComponent<SphereCollider>();
		_collider.isTrigger = true;
		_collider.radius = aggroRadius;

		if ( indicator != null )
			indicator.SetActive( false );
	}

	void Start()
	{
		_agent = GetComponent<NavMeshAgent>();
		_agent.updateRotation = false;
		_agent.updatePosition = false;
		_spawnPosition = transform.position;
	}

	void Update()
	{
		if ( _isAggro )
		{
			_agent.SetDestination( _player.transform.position );
		}
		else if ( _agent.isOnNavMesh )
		{
			float dist = Vector3.Distance( transform.position, _spawnPosition );
			if ( dist > _agent.stoppingDistance )
				_agent.SetDestination( _spawnPosition );
		}

		if ( !_agent.hasPath || _agent.remainingDistance <= _agent.stoppingDistance )
		{
			_agent.nextPosition = transform.position;
			return;
		}

		Vector3 desiredDir = _agent.desiredVelocity;
		desiredDir.y = 0f;

		if ( desiredDir.sqrMagnitude > 0.01f )
		{
			Quaternion targetRot = Quaternion.LookRotation( desiredDir.normalized );
			transform.rotation = Quaternion.RotateTowards(
				transform.rotation, targetRot, maxTurnSpeed * Time.deltaTime );
		}

		float speed = Mathf.Min( _agent.desiredVelocity.magnitude, _agent.speed );
		Vector3 newPos = transform.position + transform.forward * speed * Time.deltaTime;

		if ( NavMesh.SamplePosition( newPos, out NavMeshHit hit, 1f, NavMesh.AllAreas ) )
			newPos = new Vector3( hit.position.x, newPos.y, hit.position.z );

		transform.position = newPos;

		_agent.nextPosition = transform.position;
	}

	private void OnTriggerEnter(Collider other)
	{
		if ( other.TryGetComponent<WagonPlayer>( out var player ) )
		{
			_player = player;
			_isAggro = true;

			if ( indicator != null )
				indicator.SetActive( true );
		}
	}

	private void OnTriggerExit(Collider other)
	{
		if ( other.TryGetComponent<WagonPlayer>( out var player ) )
		{
			_player = null;
			_isAggro = false;

			if ( indicator != null )
				indicator.SetActive( false );
		}
	}
}
