using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HouseIllumination : MonoBehaviour {


	public Slider energySlider;
	private SpriteRenderer spriteRenderer;
	public Sprite normalHouse;
	public Sprite darkHouse;
	public Sprite explodingHouse;
	public Sprite glowingHouse;
	public Sprite totallyExplodingHouse;


	// Use this for initialization
	void Start () {
		spriteRenderer = GetComponent<SpriteRenderer> ();
	}
	
	// Update is called once per frame
	void Update () {

		float percent = energySlider.value / energySlider.maxValue;
		if (percent <= 0.45) {
			spriteRenderer.sprite = darkHouse;
		} else if (percent <= 0.61) {
			spriteRenderer.sprite = normalHouse;
		} else if (percent <= 0.7) {
			spriteRenderer.sprite = glowingHouse;
		} else if (percent <= 0.8) {
			spriteRenderer.sprite = explodingHouse;
		} else {
			spriteRenderer.sprite = totallyExplodingHouse;
		}

	}
}
