using UnityEngine;

// Make a subclass of this class with T as the subclass to make a singleton
public class Singleton< T > : MonoBehaviour where T : MonoBehaviour
{
	private static T _instance = null;

	public static T instance
	{
		get
		{
			if ( _instance == null )
			{
				_instance = GameObject.FindObjectOfType< T >();

				if ( _instance == null )
					throw new UnityException( "No singleton object for " + typeof( T ).ToString() + " exists in the scene!" );
			}

			return _instance;
		}
	}

	// Use this to check if there's an instance of a singleton rather than an Instance null check, since Instance access requires that one exists
	// This generally shouldn't be called every frame, since not having one means FindObjectOfType gets called all the time
	// but once the instance is found this is fine
	public static bool DoesExist()
	{
		if ( _instance == null )
			_instance = GameObject.FindObjectOfType< T >();

		return _instance != null;
	}

	// Call this to upgrade a singleton to a persistent singleton.
	// This is most often done in Awake().
	// This will kill an instance that tries to be a persistent singleton but isn't the current instance.
	public static bool DontDestroyElseKill( Singleton< T > mb )
	{
		if ( mb == instance )
		{
			MonoBehaviour.DontDestroyOnLoad( instance.gameObject );
			return true;
		}
		else
		{
			MonoBehaviour.Destroy( mb.gameObject );
			return false;
		}
	}

	// Makes this object a persistent singleton unless the singleton already exists in which case
	// the current object is destroyed
	protected bool MakePersistent()
	{
		return DontDestroyElseKill( this );
	}

	public bool IsInstance()
	{ return instance == this; }
}
