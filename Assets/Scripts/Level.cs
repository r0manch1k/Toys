using UnityEngine;

public class Level : MonoBehaviour
{
	[SerializeField] protected GameObject enemyPrefab;
	[SerializeField] protected GameObject modulePrefab;

	void Start()
	{
		SpawnEnemies();
	}

	public void SpawnEnemies()
	{
		Vector3 position = new Vector3( -18f, 0.5f, 14f );
		Quaternion rotation = new Quaternion( 0f, 0f, 0f, 0f );

		GameObject enemy = Instantiate( enemyPrefab, position, rotation );
		GameObject module = Instantiate( modulePrefab, position, rotation );

		WagonEnemy master = enemy.GetComponent<WagonEnemy>();
		WagonModule slave = module.GetComponent<WagonModule>();

		master.Connect( slave );

		Debug.Log( "Enemies spawned" );
	}
}
