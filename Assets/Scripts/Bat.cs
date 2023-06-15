using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;
using Flapper.Utils;

public class Bat : MonoBehaviour {
	public float speed = 5f;
	public float jumpForce = 6.5f;
	public float maxFallSpeed = 7.0f;

	Transform _body;
	BatLife _life;
	Rigidbody2D _rigidBody;

	void Awake () {
		_body = transform.Find("body");
		_life = _body.GetComponent<BatLife>();
		_rigidBody = GetComponent<Rigidbody2D>();
		_rigidBody.velocity = Vector2.right * speed + Vector2.up * jumpForce;
	}
	
	void Update () {
		//jump and fall control
		if (FlyButtonHaveBeenPressed()) {
			_rigidBody.velocity = Vector2.right * speed + Vector2.up * jumpForce;
		}else{
			//keep a limit at fall speed
			var vel = _rigidBody.velocity;
			vel.y = Mathf.Max(-maxFallSpeed, vel.y);
			_rigidBody.velocity = vel;
		}

		//guaratee the normal forward speed even after crashing
		_rigidBody.velocity = new Vector2(speed, _rigidBody.velocity.y);

		if(!_life.IsDead){
			Vector2 spritePosition = transform.position;
			spritePosition.x += Mathf.Sin(Time.time*10)*0.2f;
			spritePosition.y += 0.1f + Mathf.Sin(Time.time*25)*0.15f;
			_body.position = spritePosition;
		}
	}

	private bool FlyButtonHaveBeenPressed()
	{
	#if UNITY_EDITOR || UNITY_STANDALONE || UNITY_WEBPLAYER || UNITY_WEBGL
		return Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0);
	#else
		return Input.touches.Any(touch => touch.phase == TouchPhase.Began);
	#endif
	}

}
