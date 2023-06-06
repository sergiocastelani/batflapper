using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Flapper.Utils;
using Flapper.Utils.Transitions;

public class BatLife : MonoBehaviour {
	const int BatLayer = 8;
	const int BatInvulnerableLayer = 9;

	public HeartHUD heartDisplay;
	public PrefabFactory punchFactory;
	public GameObject gameOverGUI;

	public int Hp { get; private set; }

	public bool IsDead {
		get {
			return Hp < 0;
		}
	}

	private bool IsInvulnerable {
		get {
			return gameObject.layer == BatInvulnerableLayer;
		}
	}

	void OnEnable()
	{
		Hp = 1;
	}

	public void RecoverHp(){
		if(Hp < 1){
			Hp = 1;
			heartDisplay.show();
		}
	}

	void OnCollisionEnter2D(Collision2D collision)
	{
		if (!IsInvulnerable && !IsDead) {
			TakeDamage (collision.contacts.FirstOrDefault ());
			StartCoroutine (BeInvulnerable (2));
		}
	}

	private void TakeDamage(ContactPoint2D contact)
	{
		if(Hp < 0) return;

		if (Hp == 0) {
			Hp--;
			_showPunch(contact.point);
			Die ();
		}else{
			Hp--;
			heartDisplay.hide ();
			Camera.main.GetComponent<CameraFollow>().Shake();
			_showPunch(contact.point);
		}
	}

	private IEnumerator BeInvulnerable(int time) {
		var director = new Director();
		director.Add(Tweening.Blink(GetComponent<SpriteRenderer>(), true, (float)time));
		director.Play(this, null);

		gameObject.layer = BatInvulnerableLayer;
		yield return new WaitForSeconds(time);
		gameObject.layer = BatLayer;
	}

	private void _showPunch(Vector2 point) {
		var punchAsset = punchFactory.CreateRandom();
		punchAsset.SetActive(true);
		punchAsset.GetComponent<Punch>().Show(point, () => punchFactory.Despawn(punchAsset));
		if(IsDead){
	        punchAsset.GetComponent<Punch>().slowSound();
		}
	}

	private void Die()
	{
		GetComponent<Animator>().speed = 0;

		var bat = transform.parent.GetComponent<Bat>();
		bat.speed = 0;
		bat.jumpForce = 0;

		var sonar = bat.GetComponent<Sonar>();
		sonar.Stop();

		var director = new Director();
		director.Add(Tweening.Rotation2D(transform, 0, 3200, 1, Easing.Linear));
		director.Play(this, null);

		GameObject.FindWithTag("GameController").GetComponent<GameController>().GameOver();
	}
}
