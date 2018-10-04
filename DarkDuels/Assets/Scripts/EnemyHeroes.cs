using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHeroes : MonoBehaviour {

	public Image healthBar;
	public Image manaBar;
	public Text healthText;
	public Text manaText;

	GameObject playerHeroes;
	public bool isCursed = false;

	public float totalHealth;
	public float totalMana;
	public float currentHealth;
	public float currentMana;
	public float currentLevel;
	public float totalLevel;

	void Start () 
	{
		if (this.gameObject.transform.parent == playerHeroes.transform) 
		{
			Destroy(this);
		}

		totalLevel = 6;
		currentLevel = 1;

		if (this.gameObject.name == "Akasha" || this.gameObject.name == "Elwyn" || this.gameObject.name == "Madalyn") 
		{
			totalHealth = 4;
			totalMana = 1;
			currentHealth = totalHealth;
			currentMana = totalMana;
		}

		if (this.gameObject.name == "Isador" || this.gameObject.name == "Lina" || this.gameObject.name == "Zane") 
		{
			totalHealth = 2;
			totalMana = 3;
			currentHealth = totalHealth;
			currentMana = totalMana;
		}

		if (this.gameObject.name == "Sven" || this.gameObject.name == "Thaeton" || this.gameObject.name == "Triss") 
		{
			totalHealth = 3;
			totalMana = 2;
			currentHealth = totalHealth;
			currentMana = totalMana;
		}
	}

	void Update () 
	{
		healthText.text = "H e a l t h : "+ currentHealth.ToString () + " / " + totalHealth.ToString ();
		manaText.text = "M a n a : "+ currentMana.ToString () + " / " + totalMana.ToString ();
		manaBar.fillAmount = currentMana / totalMana;
		healthBar.fillAmount = currentHealth / totalHealth;

		if (this.gameObject.transform.GetChild (1).childCount == 2) 
		{
			this.gameObject.transform.GetChild (2).gameObject.SetActive (true);
		}

		if (this.gameObject.transform.GetChild (1).childCount != 2) 
		{
			this.gameObject.transform.GetChild (2).gameObject.SetActive (false);
		}

		if (isCursed == true)
			this.gameObject.transform.GetChild (3).gameObject.SetActive (true);
		if (isCursed == false)
			this.gameObject.transform.GetChild (3).gameObject.SetActive (false);
	}

	void Awake()
	{
		playerHeroes = GameObject.Find ("PHeroes");
	}
}
