using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;

[RequireComponent( typeof( NavMeshAgent ) )]
public class WagonEnemy : Wagon
{
	[SerializeField] private float aggroRadius = 10f;
	[SerializeField] private GameObject indicator;

	private SphereCollider _collider;
	private NavMeshAgent _agent;
	private WagonPlayer _player;
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
	}

	void Update()
	{
		if ( !_isAggro )
			return;

		_agent.SetDestination( _player.transform.position );
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
