using System;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent( typeof( Rigidbody ) )]
public abstract class Wagon : MonoBehaviour
{
	protected Wagon _slaveWagon;
	protected Wagon _masterWagon;
	public const int MaxSlaves = 10;

	public Wagon SlaveWagon => _slaveWagon;
	public Wagon MasterWagon => _masterWagon;

	public void Attach(Wagon wagon)
	{
		_slaveWagon = wagon;
	}

	public void Detach()
	{
		_slaveWagon = null;
	}

	public virtual void AttachTo(Wagon wagon)
	{
		throw new InvalidOperationException( "Wagon cannot be attached to other wagon" );
	}

	public virtual void DetachFrom()
	{
		throw new InvalidOperationException( "Wagon cannot be detached from other wagon" );
	}

	public void Connect(Wagon wagon)
	{
		Attach( wagon );
		wagon.AttachTo( this );
	}

	public void Disconnect()
	{
		if ( _slaveWagon == null )
			return;

		_slaveWagon.DetachFrom();
		Detach();
	}

	public IReadOnlyList<Wagon> GetConnectedWagons(bool master = false)
	{
		var slaves = new List<Wagon>( capacity: MaxSlaves );

		Wagon current = master ? this : SlaveWagon;

		while ( current != null )
		{
			slaves.Add( current );
			current = current.SlaveWagon;
		}

		return slaves;
	}
}
