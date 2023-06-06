using UnityEngine;
using Flapper.Utils.Transitions;

public class Coin : MonoBehaviour {
	public FloatingText floatingText;

	private Collider2D _collider;
	private SpriteRenderer _renderer;

	void Awake()
	{
		_collider = GetComponent<Collider2D> ();
		_renderer = GetComponent<SpriteRenderer> ();
	}

	void OnEnable()
	{
		_collider.enabled = true;

		var color = _renderer.color;
		color.a = 1;
		_renderer.color = color;
	}

	void OnTriggerEnter2D(Collider2D collider)
	{
		_collider.enabled = false; //blocks more collisions events
		StageScore.instance.Coins += 1;

        floatingText.Show("+" + Score.MONEY_SCORE_VALUE, Color.white, transform.position);
		_collectAnimation();
	}

	void _collectAnimation(){
		var director = new Director();
		director.Add(Tweening.Blink(_renderer, false, 1f));
		director.Play(this, null);
	}
}
