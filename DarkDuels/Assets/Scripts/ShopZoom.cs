using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ShopZoom : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
	GameObject shopZoom;
	Sprite zoomSprite;
	Sprite originalSprite;
	GameController gcScript;
	GameZoom gameZoom;
	int goldCost;
	GameObject playerHeroes;
	GameObject gameController;
	GameObject clickedHero;
	GameObject currentLevelObject;
	GameObject powerLeft;
	GameObject powerRight;
	GameObject heroTree;
	GameObject heroPower;
	GameObject heroLevel;
	Text currentLevelText;
	Text leftText;
	Text rightText;
	public bool areAllHeroesMaxed;

	int owned = 0;

	void Awake()
	{
		gameController = GameObject.Find ("GameController");
		shopZoom = GameObject.Find ("ShopZoom");
		playerHeroes = GameObject.Find ("PHeroes");
	}

	void Start () 
	{
		zoomSprite = shopZoom.GetComponent<SpriteRenderer> ().sprite;
		originalSprite = this.gameObject.GetComponent<SpriteRenderer> ().sprite;
		gcScript = gameController.GetComponent<GameController> ();

		if(this.name == "VictoryMedal(Clone)")
			this.name = "VictoryMedal";

		areAllHeroesMaxed = false;
	}

	int whichChild()
	{
		int index = 0;
		for (int i = 0; i < 3; i++) 
		{
			if (gcScript.playerHeroes.transform.GetChild (i).gameObject == clickedHero.gameObject)
				index = i;
		}

		return index;
	}

	void AreAllHeroesMaxed()
	{
		int j = 0;
		for (int i = 0; i < 3; i++) 
		{
			HeroDrop hdScript = playerHeroes.transform.GetChild (i).GetComponent<HeroDrop> ();
			if (hdScript.currentLevel == 6)
				j++;
		}

		if (j == 3)
			areAllHeroesMaxed = true;
	}

	void OnMouseDown()
	{
		if (zoomSprite == originalSprite) 
		{
			if (gcScript.goldValue >= goldCost && gcScript.numberOfBuys > 0 && owned < 3) 
			{
				AreAllHeroesMaxed ();
				BuyCard (this.gameObject);
				GameObject ownedObj = this.transform.GetChild (0).gameObject;
				Text ownedText = ownedObj.GetComponent<Text> ();
				ownedText.text = "Owned: " + owned.ToString ();
				gcScript.numberOfBuys--;

				if (areAllHeroesMaxed == false) 
				{
					gcScript.GoBack ();
					LevelUpHero ();
				}
			}

			if (gcScript.goldValue < goldCost) 
			{
				gcScript.shopMessageText.text = "Not enough gold to buy " + this.gameObject;
			} 

			if (gcScript.numberOfBuys <= 0) 
			{
				gcScript.shopMessageText.text = "Cannot buy more cards this turn!";
			} 

			if (owned >= 3) 
			{
				gcScript.shopMessageText.text = "Cannot buy more of " + this.gameObject;
			}
		}
	}

	public int BuyCard(GameObject boughtCard)
	{
		gcScript.endButton.interactable = false;
		boughtCard = (GameObject)Instantiate (boughtCard);
		boughtCard.AddComponent<Draggable> ();
		Draggable d = boughtCard.GetComponent<Draggable>();
		d.enabled = false;
		boughtCard.transform.position = gcScript.playerHand.transform.position;
		boughtCard.transform.SetParent(gcScript.playerHand.transform);

		if (boughtCard.name == "Fetch Quest(Clone)") 
		{
			boughtCard.name = "Fetch Quest";
		}

		if (boughtCard.name == "Health Vial(Clone)") 
		{
			boughtCard.name = "Health Vial";
		}

		if (boughtCard.name == "HealthPotion(Clone)") 
		{
			boughtCard.name = "HealthPotion";
		}

		if (boughtCard.name == "Mana Vial(Clone)") 
		{
			boughtCard.name = "Mana Vial";
		}

		if (boughtCard.name == "ManaPotion(Clone)") 
		{
			boughtCard.name = "ManaPotion";
		}

		if (boughtCard.name == "SpecialPotion(Clone)") 
		{
			boughtCard.name = "SpecialPotion";
		}

		if (boughtCard.name == "JarOfGreed(Clone)") 
		{
			boughtCard.name = "JarOfGreed";
		}

		if (boughtCard.name == "StolenPouch(Clone)") 
		{
			boughtCard.name = "StolenPouch";
		}

		if (boughtCard.name == "Lifesteal(Clone)") 
		{
			boughtCard.name = "Lifesteal";
		}

		if (boughtCard.name == "Manasteal(Clone)") 
		{
			boughtCard.name = "Manasteal";
		}

		if (boughtCard.name == "Thief(Clone)") 
		{
			boughtCard.name = "Thief";
		}

		if (boughtCard.name == "TargetBlock(Clone)") 
		{
			boughtCard.name = "TargetBlock";
		}

		if (boughtCard.name == "PotOfGreed(Clone)") 
		{
			boughtCard.name = "PotOfGreed";
		}

		if (boughtCard.name == "InnerPower(Clone)") 
		{
			boughtCard.name = "InnerPower";
		}

		if (boughtCard.name == "WealthIsHealth(Clone)") 
		{
			boughtCard.name = "WealthIsHealth";
		}

		if (boughtCard.name == "TransmuteLife(Clone)") 
		{
			boughtCard.name = "TransmuteLife";
		}

		if (boughtCard.name == "ValorOfHercules(Clone)") 
		{
			boughtCard.name = "ValorOfHercules";
		}

		if (boughtCard.name == "DresdensRobe(Clone)") 
		{
			boughtCard.name = "DresdensRobe";
		}

		if (boughtCard.name == "VictoryMedal(Clone)") 
		{
			boughtCard.name = "VictoryMedal";
		}
			
		owned++;
		GameObject ownedObj = boughtCard.transform.GetChild (0).gameObject;
		Destroy (ownedObj);
		ShopZoom disableBuy = boughtCard.GetComponent<ShopZoom> ();
		Destroy (disableBuy);
		gcScript.goldValue = gcScript.goldValue - goldCost;

		return gcScript.goldValue;
	}

	public void LevelUpHero()
	{
		StartCoroutine(SelectHero (0.2f));
	}

	IEnumerator SelectHero(float waitTime)
	{
		gcScript.shopButton.interactable = false;
		yield return new WaitForSeconds (waitTime);
		gcScript.messageText.text = "Please left-click on a hero you wish to level up";
		while (true) 
		{
			if (Input.GetMouseButtonDown (0)) 
			{
				RaycastHit hit;
				Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);

				if (Physics.Raycast (ray, out hit))
				{
					clickedHero = hit.transform.gameObject;
					HeroDrop hdScript = clickedHero.GetComponent<HeroDrop> ();
					if (hit.transform.parent == playerHeroes.transform && hdScript.currentLevel<6) 
					{
						gcScript.pbLeft.interactable = true;
						gcScript.pbRight.interactable = true;
						gcScript.leftButton = 0;
						gcScript.rightButton = 0;
						gcScript.ZoomOff (true, whichChild());

						hdScript.currentLevel++;
						heroTree = hit.transform.gameObject.gameObject.transform.GetChild (4).gameObject;
						heroPower = heroTree.gameObject.transform.GetChild (0).gameObject;
						heroLevel = heroTree.gameObject.transform.GetChild (1).gameObject;
						if (hdScript.currentLevel == 2 || hdScript.currentLevel == 4 || hdScript.currentLevel == 6) 
						{
							gcScript.messageText.text = "Skill Choice for " + hit.transform.gameObject.name + " at level " + hdScript.currentLevel.ToString() + ": Select the power you wish to unlock";
							while (true) 
							{
								if (hdScript.currentLevel == 2) 
								{
									powerLeft = heroPower.gameObject.transform.GetChild (0).gameObject;
									powerRight = heroPower.gameObject.transform.GetChild (1).gameObject;
									currentLevelObject = heroLevel.gameObject.transform.GetChild (1).gameObject;
								}

								if (hdScript.currentLevel == 4) 
								{
									powerLeft = heroPower.gameObject.transform.GetChild (2).gameObject;
									powerRight = heroPower.gameObject.transform.GetChild (3).gameObject;
									currentLevelObject = heroLevel.gameObject.transform.GetChild (3).gameObject;
								}

								if (hdScript.currentLevel == 6) 
								{
									powerLeft = heroPower.gameObject.transform.GetChild (4).gameObject;
									powerRight = heroPower.gameObject.transform.GetChild (5).gameObject;
									currentLevelObject = heroLevel.gameObject.transform.GetChild (5).gameObject;
								}

								leftText = powerLeft.GetComponent<Text> ();
								rightText = powerRight.GetComponent<Text> ();
								currentLevelText = currentLevelObject.GetComponent<Text> ();
								leftText.color = Color.yellow;
								rightText.color = Color.yellow;
								currentLevelText.color = Color.green;
								hdScript.totalHealth++;
								hdScript.totalMana++;
								StartCoroutine(PowerChoice());
								yield break;
							}
						}

						if (hdScript.currentLevel == 3 || hdScript.currentLevel == 5) 
						{
							if (hdScript.currentLevel == 3) 
							{
								currentLevelObject = heroLevel.gameObject.transform.GetChild (2).gameObject;
								if (hit.transform.gameObject.name == "Thaeton" || hit.transform.gameObject.name == "Sven" || hit.transform.gameObject.name == "Triss") 
								{
									hdScript.totalHealth++;
									hdScript.totalMana++;
								}
							}

							if (hdScript.currentLevel == 5) 
							{
								currentLevelObject = heroLevel.gameObject.transform.GetChild (4).gameObject;
							} 

							if (hit.transform.gameObject.name == "Akasha" || hit.transform.gameObject.name == "Elwyn" || hit.transform.gameObject.name == "Madalyn") 
							{
								hdScript.totalHealth++;
							}

							if (hit.transform.gameObject.name == "Lina" || hit.transform.gameObject.name == "Zane" || hit.transform.gameObject.name == "Isador") 
							{
								hdScript.totalMana++;
							}

							currentLevelText = currentLevelObject.GetComponent<Text> ();
							currentLevelText.color = Color.green;
							gcScript.shopButton.interactable = true;
							gcScript.endButton.interactable = true;
							gcScript.pbLeft.interactable = false;
							gcScript.pbRight.interactable = false;
							gcScript.ZoomOff (false, whichChild());
							gcScript.messageText.text = hit.transform.gameObject.name + " is now level " + hdScript.currentLevel;
							gcScript.leftButton = 0;
							gcScript.rightButton = 0;
							yield break;
						}
					} 
					else 
					{
						gcScript.messageText.text = "Please left-click on one of your heroes to level him/her up";
					}
				}
				else gcScript.messageText.text = "Please left-click on one of your heroes to level him/her up";
			}
			yield return null;
		}
	}

	IEnumerator PowerChoice()
	{
		gcScript.pbLeft.interactable = true;
		gcScript.pbRight.interactable = true;
		gcScript.messageText.text = "Press left-click on either the " + leftText.text + " power to unlock it OR the " + rightText.text + " power";
		while (true) 
		{
			if (gcScript.leftButton == 1) 
			{
				leftText.color = Color.green;
				rightText.color = Color.white;
				gcScript.messageText.text = "You unlocked " + leftText.text + " on " + clickedHero.name;
				gcScript.shopButton.interactable = true;
				gcScript.endButton.interactable = true;
				gcScript.pbLeft.interactable = false;
				gcScript.pbRight.interactable = false;
				gcScript.ZoomOff (false, whichChild());
				gcScript.leftButton = 0;
				yield break;
			}

			if (gcScript.rightButton == 1) 
			{
				rightText.color = Color.green;
				leftText.color = Color.white;
				gcScript.messageText.text = "You unlocked " + rightText.text + " on " + clickedHero.name;
				gcScript.shopButton.interactable = true;
				gcScript.endButton.interactable = true;
				gcScript.pbLeft.interactable = false;
				gcScript.pbRight.interactable = false;
				gcScript.ZoomOff (false, whichChild());
				gcScript.rightButton = 0;
				yield break;
			}

			yield return null;
		}
	}

	public void OnPointerEnter(PointerEventData eventData)
	{
		gcScript.ShopEffectsOff ();
		zoomSprite = originalSprite;
		shopZoom.GetComponent<SpriteRenderer> ().sprite = zoomSprite;
		shopZoom.transform.localScale = new Vector3 (60, 60, 60);

		if (this.gameObject.name == "HealthPotion") 
		{
			goldCost = 2;
			gcScript.shophpEffect.SetActive (true);
		}

		if (this.gameObject.name == "ManaPotion") 
		{
			goldCost = 2;
			gcScript.shopmpEffect.SetActive (true);
		}

		if (this.gameObject.name == "SpecialPotion") 
		{
			goldCost = 2;
			gcScript.shopmvEffect.SetActive (true);
			gcScript.shophvEffect.SetActive (true);
		}

		if (this.gameObject.name == "JarOfGreed") 
		{
			goldCost = 2;
			gcScript.shopfqEffect.SetActive (true);
			gcScript.shopdrawEffect.SetActive (true);
		}

		if (this.gameObject.name == "StolenPouch") 
		{
			goldCost = 2;
			gcScript.shopstolenpEffect.SetActive (true);
		}

		if (this.gameObject.name == "Lifesteal") 
		{
			goldCost = 1;
		}

		if (this.gameObject.name == "Manasteal") 
		{
			goldCost = 1;
		}

		if (this.gameObject.name == "Thief") 
		{
			goldCost = 1;
		}

		if (this.gameObject.name == "TargetBlock") 
		{
			goldCost = 1;
		}

		if (this.gameObject.name == "ValorOfHercules") 
		{
			goldCost = 1;
		}

		if (this.gameObject.name == "DresdensRobe") 
		{
			goldCost = 1;
		}

		if (this.gameObject.name == "PotOfGreed") 
		{
			goldCost = 1;
			gcScript.shopdrawEffect.SetActive (true);
		}

		if (this.gameObject.name == "InnerPower") 
		{
			goldCost = 3;
			gcScript.shopmvEffect.SetActive (true);
			gcScript.shopdrawEffect.SetActive (true);
		}

		if (this.gameObject.name == "WealthIsHealth") 
		{
			goldCost = 3;
			gcScript.shopfqEffect.SetActive (true);
			gcScript.shophvEffect.SetActive (true);
		}

		if (this.gameObject.name == "TransmuteLife") 
		{
			goldCost = 3;
			gcScript.shopfqEffect.SetActive (true);
			gcScript.shopmvEffect.SetActive (true);
		}

		if (this.gameObject.name == "VictoryMedal") 
		{
			goldCost = 3;
		}
	}

	public void OnPointerExit(PointerEventData eventData) 
	{
		
	}
}
