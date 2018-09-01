using System.Collections.Generic;
using UnityEngine;

public class Game : PersistableObject
{
	[SerializeField] private PersistableObject _prefab;
	
	[SerializeField] private KeyCode _createKey = KeyCode.C;
	[SerializeField] private KeyCode _newGameKey = KeyCode.N;
	[SerializeField] private KeyCode _saveKey = KeyCode.S;
	[SerializeField] private KeyCode _loadKey = KeyCode.L;
	
	[SerializeField] private PersistentStorage _storage;
	
	private List<PersistableObject> _objects;

	private void Awake()
	{
		_objects = new List<PersistableObject>();
	}

	private void Update()
	{
		if (Input.GetKeyDown(_createKey))
		{
			CreateObject();
		}
		else if (Input.GetKeyDown(_newGameKey))
		{
			BeginNewGame();
		}
		else if (Input.GetKeyDown(_saveKey))
		{
			_storage.Save(this);
		}
		else if (Input.GetKeyDown((_loadKey)))
		{
			BeginNewGame();
			_storage.Load(this);
		}
	}

	private void BeginNewGame()
	{
		for (var i = 0; i < _objects.Count; i++)
		{
			Destroy(_objects[i].gameObject);
		}	
		
		_objects.Clear();
	}

	private void CreateObject()
	{
		PersistableObject o = Instantiate(_prefab);
		Transform t = o.transform;
		t.localPosition = Random.insideUnitSphere * 5f;
		t.rotation = Random.rotation;
		t.localScale = Vector3.one * Random.Range(0.1f, 1f);
		
		_objects.Add(o);
	}
	
	public override void Save (GameDataWriter writer) 
	{
		writer.Write(_objects.Count);
		
		for (var i = 0; i < _objects.Count; i++) 
		{
			_objects[i].Save(writer);
		}
	}

	public override void Load(GameDataReader reader)
	{
		var count = reader.ReadInt();
		
		for (var i = 0; i < count; i++)
		{
			PersistableObject o = Instantiate(_prefab);
			o.Load(reader);
			
			_objects.Add(o);
		}
	}
}
