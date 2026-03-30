using UnityEngine;
using UnityEngine.SceneManagement;

public class FightTrigger : MonoBehaviour
{
	[SerializeField] private float fightRadius = 3f;
	[SerializeField] private float fightTime = 3f;
	[SerializeField] private string fightScene = "FightScene";

	private WagonPlayer _player;
	private bool _isAggro;
	private float _timer = 0f;

	private void Update()
	{
		if ( !_isAggro ) return;

		float distance = Vector3.Distance( transform.position, _player.transform.position );

		if ( distance > fightRadius )
			return;

		_timer += Time.deltaTime;

		if ( _timer >= fightTime )
		{
			Fight();
		}
	}

	private void OnTriggerEnter(Collider other)
	{
		if ( other.TryGetComponent<WagonPlayer>( out var player ) )
		{
			_player = player;
			_isAggro = true;
			_timer = 0f;
		}
	}

	private void OnTriggerExit(Collider other)
	{
		if ( other.TryGetComponent<WagonPlayer>( out var player ) )
		{
			_isAggro = false;
			_player = null;
		}
	}

	private void Fight()
	{
		Debug.Log( "Fight is starting" );

		SceneManager.LoadScene( fightScene );
	}
}
