using UnityEngine;
using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

class FBPicturesCache
{
    static private int MAX = 255;
    static private int DEFAULT_PICTURE_INDEX = 255;
    static private FBPicturesCache _instance;

	static private string _textureFilename = Application.persistentDataPath + "/fbPictures.tex";
	static private string _metaFilename = Application.persistentDataPath + "/fbPictures.tex";

    [Serializable]
    class Entry {
        public long id;
        public int atlasIndex;
    }

    private GridAtlas _atlas = new GridAtlas(1024, 1024, 64, 64, _textureFilename);

    private Dictionary<long, Entry> _entriesById = new Dictionary<long, Entry>();
	private List<Entry> _fifo = new List<Entry>();

    public static FBPicturesCache instance()
    {
        if (_instance == null) _instance = new FBPicturesCache();
        return _instance;
    }

    private FBPicturesCache()
    {}

    public void Load()
    {
        _atlas.Load();

		using (Stream stream = new FileStream(_metaFilename, FileMode.Open, FileAccess.Read, FileShare.Read))
        {
            IFormatter formatter = new BinaryFormatter();
			_fifo = (List<Entry>) formatter.Deserialize(stream);
            stream.Close();
        }
    }

	//Necessary call to persist data and update the GPU content
    public void Save()
    {
        _atlas.Save();

		using (Stream stream = new FileStream(_metaFilename, FileMode.Create, FileAccess.Write, FileShare.None))
        {
            IFormatter formatter = new BinaryFormatter();
            formatter.Serialize(stream, _fifo);
            stream.Close();
        }
    }

    public void SetDefaultPicture(Texture2D picture)
    {
        _atlas.Set(DEFAULT_PICTURE_INDEX, picture);
    }

    public void Set(long id, Texture2D picture)
    {
        Entry entry;
		if (_entriesById.TryGetValue (id, out entry)) {
			_fifo.Remove (entry);
			_fifo.Insert (_fifo.Count, entry);

		} else {
			if(_fifo.Count == MAX)
			{
				entry = _fifo[0];
				entry.id = id;
				_fifo.RemoveAt(0);
				_entriesById.Remove(id);
			} 
			else
			{
				entry = new Entry()
				{
					id = id,
					atlasIndex = _fifo.Count
				};
			}

			_fifo.Insert (_fifo.Count, entry);
			_entriesById.Add (id, entry);
		}

		_atlas.Set(entry.atlasIndex, picture);
    }

    public Sprite Get(long id)
    {
        try
        {
            var entry = _entriesById[id];

			_fifo.Remove (entry);
			_fifo.Insert (_fifo.Count, entry);

            return _atlas.Get(entry.atlasIndex);
        }
        catch
        {
            return _atlas.Get(DEFAULT_PICTURE_INDEX);
        }
    }
}