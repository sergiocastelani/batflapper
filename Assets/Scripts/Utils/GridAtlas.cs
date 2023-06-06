using UnityEngine;
using System;
using System.IO;

public class GridAtlas {
	private Texture2D _atlasTexture;
	private string _filePath;

	private int _spriteWidth;
	private int _spriteHeight;

	private int _spritesPerRow;

	public GridAtlas(int width, int height, int spriteWidth, int spriteHeight, string persistedFilename) {
		_filePath = persistedFilename;
		_spriteWidth = spriteWidth;
		_spriteHeight = spriteHeight;
		_spritesPerRow = width / spriteWidth;

		//create a texture for jpg images
		_atlasTexture = new Texture2D(width, height, TextureFormat.RGB24, false);
	}

	public void Load() {
		//load persisted atlas
		if (Application.platform != RuntimePlatform.WebGLPlayer){
			try {
				_atlasTexture.LoadRawTextureData( File.ReadAllBytes(_filePath) );
				_atlasTexture.Apply();
			} catch(Exception e) {
				Debug.Log(e);
			}
		}
	}

	public void Save(){
		if (Application.platform != RuntimePlatform.WebGLPlayer){
			try{
				File.WriteAllBytes( _filePath, _atlasTexture.GetRawTextureData() );
				_atlasTexture.Apply();
			} catch(Exception e) {
				Debug.Log(e);
			}
		}
	}

	public Sprite Get(int index) {
		return Get(index % _spritesPerRow, index / _spritesPerRow);
	}

	public void Set(int index, Texture2D picture) {
		Set(index % _spritesPerRow, index / _spritesPerRow, picture);
	}

	public Sprite Get(int x, int y){
		var region = new Rect(x*_spriteWidth, y*_spriteHeight, _spriteWidth, _spriteHeight);
		var pivot = new Vector2(0.5f, 0.5f);
		return Sprite.Create(_atlasTexture, region, pivot);
	}

	public void Set(int x, int y, Texture2D picture){
		if(picture.width != _spriteWidth || picture.height != _spriteHeight){
			Debug.LogError("Tamanho de imagem incorreto");
		}

		if (_atlasTexture.format != picture.format) {
			Debug.LogError("Formato de imagem incorreto");
		}

		_atlasTexture.SetPixels(x*_spriteWidth, y*_spriteHeight, _spriteWidth, _spriteHeight, picture.GetPixels());
	}

}
