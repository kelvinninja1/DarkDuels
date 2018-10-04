using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;

public class HeroDrop : MonoBehaviour, IDropHandler, IPointerEnterHandler, IPointerExitHandler {

	public Image healthBar;
	public Image manaBar;
	Image firstCardTint;
	Image secondCardTint;
	GameObject discardPile;
	GameObject enemyDiscardPile;
	GameObject enemyHeroes;
	GameObject gameController;
	GameObject playerHeroes;
	GameObject heroTree;
	GameObject heroPower;
	GameObject leftPower;
	GameObject rightPower;
	GameObject playCards;
	GameObject clickedHero;
	GameObject playerHand;
	GameObject enemyHand;
	GameObject combinedCard;
	GameObject firstCard;
	GameObject secondCard;
	GameObject playerDeck;
	GameObject choiceIndicator;
	GameController gcScript;
	ShopZoom szScript;

	public Transform playZone;
	char costNumberChar;
	int costNumber;
	char costType;
	float costResource;
	bool enoughResources;
	bool powerUsed = false;
	bool didCombine = false;
	bool canCombo = true;
	public bool isCursed = false;

	int currentCombo = 0;
	int comboNumber = 0;

	[HideInInspector]
	public float totalHealth;
	public float totalMana;
	public float currentHealth;
	public float currentMana;
	public float currentLevel;
	public float totalLevel;

	public Text healthText;
	public Text manaText;
	Text leftPowerText;
	Text rightPowerText;
	Text powerToCheck;
	string powerName;

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

	void OnMouseDown()
	{
		if (currentLevel > 1 && canCombo)
		{
			if (this.gameObject.transform.GetChild (1).childCount == 2) 
			{
				StartCoroutine (ExecuteCombo (0.2f));
			}
		}

		if (currentLevel == 1) 
		{
			if (this.gameObject.transform.GetChild (1).childCount == 2) 
			{
				gcScript.messageText.text = "No powers unlocked on " + this.gameObject.name + " yet!";
			}
		}
	}

	void Awake()
	{
		enemyHeroes = GameObject.Find ("EHeroes");
		gameController = GameObject.Find ("GameController");
		discardPile = GameObject.Find ("PDiscardPile");
		playerHand = GameObject.Find ("PHand");
		enemyHand = GameObject.Find ("EHand");
		playerDeck = GameObject.Find ("PDeck");
		choiceIndicator = GameObject.Find ("ChoiceIndicator");
		playerHeroes = GameObject.Find ("PHeroes");
		enemyDiscardPile = GameObject.Find ("EDiscardPile");
	}

	void Start ()
	{
		if (this.gameObject.transform.parent == enemyHeroes.transform) 
		{
			Destroy(this);
		}
			
		gcScript = gameController.GetComponent<GameController> ();

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

		enoughResources = false;

		for (int i = 0; i < this.gameObject.transform.childCount ; i++) 
		{
			Transform temp = this.gameObject.transform.GetChild (i).transform;
			if (temp.gameObject.name == "CombatBonusArea") 
			{
				playZone = temp;
			}
		}

		choiceIndicator.SetActive (false);
	}

	public void OnPointerEnter(PointerEventData eventData) 
	{
		if(eventData.pointerDrag == null)
			return;

		Draggable d = eventData.pointerDrag.GetComponent<Draggable>();
		if(d != null) {
			d.placeholderParent = this.transform;
		}
	}

	public void OnPointerExit(PointerEventData eventData) 
	{
		if(eventData.pointerDrag == null)
			return;

		Draggable d = eventData.pointerDrag.GetComponent<Draggable>();
		if(d != null && d.placeholderParent==this.transform) 
		{
			d.placeholderParent = d.parentToReturnTo;
		}
	}

	public void OnDrop(PointerEventData eventData) 
	{
		Draggable d = eventData.pointerDrag.GetComponent<Draggable>();

		if (eventData.pointerDrag.name == "Mana Vial" && currentMana < totalMana && isCursed == false) 
		{
			currentMana++;
			if (gcScript.potenceFlag == true) 
			{
				if(currentMana != totalMana)
					currentMana++;
			}
			d.parentToReturnTo = discardPile.transform;
			Destroy (d, 1f);
			gcScript.messageText.text = this.gameObject.name + " used " + eventData.pointerDrag.name;
		}

		if (eventData.pointerDrag.name == "ManaPotion" && currentMana < totalMana && isCursed == false) 
		{
			if ((totalMana - currentMana) == 1) 
			{
				currentMana++;
			} 

			if ((totalMana - currentMana) != 1)  
			{
				currentMana = currentMana + 2;
			}

			if (gcScript.potenceFlag == true) 
			{
				if(currentMana != totalMana)
					currentMana++;
			}

			d.parentToReturnTo = discardPile.transform;
			Destroy (d, 1f);
			gcScript.messageText.text = this.gameObject.name + " used " + eventData.pointerDrag.name;
		}

		if (eventData.pointerDrag.name == "Health Vial" && currentHealth < totalHealth && isCursed == false)
		{
			currentHealth++;
			if (gcScript.potenceFlag == true) 
			{
				if(currentHealth != totalHealth)
					currentHealth++;
			}
			d.parentToReturnTo = discardPile.transform;
			Destroy (d, 1f);
			gcScript.messageText.text = this.gameObject.name + " used " + eventData.pointerDrag.name;
		}

		if (eventData.pointerDrag.name == "HealthPotion" && currentHealth < totalHealth && isCursed == false) 
		{
			if ((totalHealth - currentHealth) == 1) 
			{
				currentHealth++;
			} 

			if ((totalHealth - currentHealth) != 1) 
			{
				currentHealth = currentHealth + 2;
			}

			if (gcScript.potenceFlag == true) 
			{
				if(currentHealth != totalHealth)
					currentHealth++;
			}

			d.parentToReturnTo = discardPile.transform;
			Destroy (d, 1f);
			gcScript.messageText.text = this.gameObject.name + " used " + eventData.pointerDrag.name;
		}

		if (eventData.pointerDrag.name == "SpecialPotion" && (currentHealth < totalHealth || currentMana < totalMana) && isCursed == false) 
		{
			if ((totalHealth - currentHealth) == 0) 
			{
				currentMana++;
			}

			else if ((totalMana - currentMana) == 0) 
			{
				currentHealth++;
			} 

			else 
			{
				currentHealth++;
				currentMana++;
			}

			if (gcScript.potenceFlag == true) 
			{
				if(currentMana != totalMana)
					currentMana++;
			}
			if (gcScript.potenceFlag == true) 
			{
				if(currentHealth != totalHealth)
					currentHealth++;
			}

			d.parentToReturnTo = discardPile.transform;
			Destroy (d, 1f);
			gcScript.messageText.text = this.gameObject.name + " used " + eventData.pointerDrag.name;
		}

		if (eventData.pointerDrag.name == "Fetch Quest" && currentMana > 0) 
		{
			currentMana--;
			gcScript.goldValue++;
			if (gcScript.prosperityFlag == true) 
			{
				gcScript.goldValue++;
			}
			d.parentToReturnTo = discardPile.transform;
			Destroy (d, 1f);
			gcScript.messageText.text = this.gameObject.name + " used " + eventData.pointerDrag.name;
		}

		if (eventData.pointerDrag.name == "PotOfGreed" && currentMana > 0) 
		{
			currentMana--;
			gcScript.DrawACard(2);
			if (gcScript.hoardFlag == true) 
			{
				gcScript.DrawACard(1);
			}
			d.parentToReturnTo = discardPile.transform;
			Destroy (d, 1f);
			gcScript.messageText.text = this.gameObject.name + " used " + eventData.pointerDrag.name;
		}

		if (eventData.pointerDrag.name == "JarOfGreed" && currentMana > 1) 
		{
			currentMana = currentMana - 2;
			gcScript.goldValue++;
			gcScript.DrawACard(2);
			if (gcScript.hoardFlag == true) 
			{
				gcScript.DrawACard(1);
			}
			if (gcScript.prosperityFlag == true) 
			{
				gcScript.goldValue++;
			}
			d.parentToReturnTo = discardPile.transform;
			Destroy (d, 1f);
			gcScript.messageText.text = this.gameObject.name + " used " + eventData.pointerDrag.name;
		}

		if (eventData.pointerDrag.name == "StolenPouch" && currentMana > 1) 
		{
			currentMana = currentMana - 2;
			gcScript.goldValue = gcScript.goldValue + 2;
			if (gcScript.prosperityFlag == true) 
			{
				gcScript.goldValue++;
			}
			d.parentToReturnTo = discardPile.transform;
			Destroy (d, 1f);
			gcScript.messageText.text = this.gameObject.name + " used " + eventData.pointerDrag.name;
		}

		if (eventData.pointerDrag.name == "WealthIsHealth" && currentMana > 1) 
		{
			currentMana = currentMana - 2;
			gcScript.goldValue++;

			if (isCursed == false) {
				if (currentHealth == totalHealth) {
				}
				else if (currentHealth != totalHealth)
					currentHealth++;
			
				if (gcScript.potenceFlag == true) {
					if (currentHealth != totalHealth)
						currentHealth++;
				}
			}

			if (gcScript.prosperityFlag == true) 
			{
				gcScript.goldValue++;
			}

			d.parentToReturnTo = discardPile.transform;
			Destroy (d, 1f);
			gcScript.messageText.text = this.gameObject.name + " used " + eventData.pointerDrag.name;
		} 

		if (eventData.pointerDrag.name == "InnerPower" && currentHealth > 2) 
		{
			currentHealth = currentHealth - 2;
			if (isCursed == false) {
				if (currentMana == totalMana) {
				}
				else if (currentMana != totalMana)
					currentMana++;

				if (gcScript.potenceFlag == true) {
					if (currentMana != totalMana)
						currentMana++;
				}
			}

			gcScript.DrawACard(2);
			if (gcScript.hoardFlag == true) 
			{
				gcScript.DrawACard(1);
			}
			d.parentToReturnTo = discardPile.transform;
			Destroy (d, 1f);
			gcScript.messageText.text = this.gameObject.name + " used " + eventData.pointerDrag.name;
		}

		if (eventData.pointerDrag.name == "TransmuteLife" && currentHealth > 2) 
		{
			currentHealth = currentHealth - 2;
			gcScript.goldValue++;

			if (isCursed == false) {
				if (currentMana == totalMana) {
				}
				else if (currentMana != totalMana)
					currentMana++;

				if (gcScript.potenceFlag == true) {
					if (currentMana != totalMana)
						currentMana++;
				}
			}

			if (gcScript.prosperityFlag == true) 
			{
				gcScript.goldValue++;
			}
			d.parentToReturnTo = discardPile.transform;
			Destroy (d, 1f);
			gcScript.messageText.text = this.gameObject.name + " used " + eventData.pointerDrag.name;
		}

		if (eventData.pointerDrag.name == "VictoryMedal") 
		{
			currentMana -= 3;
			d.parentToReturnTo = discardPile.transform;
			Destroy (d, 1f);
			gcScript.messageText.text = this.gameObject.name + " used " + eventData.pointerDrag.name;
		}

		if (eventData.pointerDrag.name == "Mana Vial" && (currentMana >= totalMana || isCursed == true)) 
		{
			if (d.parentToReturnTo != discardPile.transform) 
			{
				if (isCursed == true) 
				{
					gcScript.messageText.text = this.gameObject.name + "is cursed! They cannot regenerate mana or health";
				}
				else
					gcScript.messageText.text = this.gameObject.name + " is already at full Mana!";
			}
		} 

		if (eventData.pointerDrag.name == "Health Vial" && (currentHealth >= totalHealth || isCursed == true)) 
		{
			if (d.parentToReturnTo != discardPile.transform) 
			{
				if (isCursed == true) 
				{
					gcScript.messageText.text = this.gameObject.name + "is cursed! They cannot regenerate mana or health";
				}
				else
					gcScript.messageText.text = this.gameObject.name + " is already at full Health!";
			}
		} 

		if ((eventData.pointerDrag.name == "Fetch Quest" || eventData.pointerDrag.name == "PotOfGreed") && currentMana <= 0) 
		{
			if (d.parentToReturnTo != discardPile.transform) 
			{
				gcScript.messageText.text = "Not enough Mana on " + this.gameObject.name;
			}
		} 

		if ((eventData.pointerDrag.name == "JarOfGreed" || eventData.pointerDrag.name == "StolenPouch") && currentMana <= 1) 
		{
			if (d.parentToReturnTo != discardPile.transform) 
			{
				gcScript.messageText.text = "Not enough Mana on " + this.gameObject.name;
			}
		}

		if (eventData.pointerDrag.name == "WealthIsHealth" && currentMana <= 2)
		{
			if (d.parentToReturnTo != discardPile.transform) 
			{
				gcScript.messageText.text = "Not enough Mana on " + this.gameObject.name;
			}
		}

		if ((eventData.pointerDrag.name == "InnerPower" || eventData.pointerDrag.name == "TransmuteLife") && currentHealth <= 3)
		{
			if (d.parentToReturnTo != discardPile.transform) 
			{
				gcScript.messageText.text = "Not enough Health on " + this.gameObject.name;
			}
		}

		if ((eventData.pointerDrag.name == "Lifesteal" || eventData.pointerDrag.name == "Manasteal" || eventData.pointerDrag.name == "Thief" || eventData.pointerDrag.name == "TargetBlock" || eventData.pointerDrag.name == "ValorOfHercules" || eventData.pointerDrag.name == "DresdensRobe") && playZone.childCount < 2)
		{
			d.parentToReturnTo = playZone;
			Destroy (d, 1f);
		}

		if ((eventData.pointerDrag.name == "Lifesteal" || eventData.pointerDrag.name == "Manasteal" || eventData.pointerDrag.name == "Thief" || eventData.pointerDrag.name == "TargetBlock" || eventData.pointerDrag.name == "ValorOfHercules" || eventData.pointerDrag.name == "DresdensRobe") && playZone.childCount > 1)
		{
			if (d.parentToReturnTo != playZone) 
			{
				gcScript.messageText.text = this.gameObject.name + " already has two combat bonuses!";
			}
		}

		gcScript.UpdateDiscardDropdown ();
	}

	void CheckPowerCost()
	{
		if (leftPowerText.color == Color.green) 
		{
			powerToCheck = leftPowerText;
		}

		if (rightPowerText.color == Color.green) 
		{
			powerToCheck = rightPowerText;
		}

		for (int i = 0; i < powerToCheck.text.Length; i++) 
		{
			if (powerToCheck.text [i] == '(') 
			{
				powerName = powerToCheck.text.Substring (0, (i - 1));
				break;
			}
		}

		for (int i = 0; i < powerToCheck.text.Length; i++) 
		{
			char c = powerToCheck.text [i];
			if (c == '(') 
			{
				costNumberChar = powerToCheck.text [i + 1];
				costNumber = int.Parse (costNumberChar.ToString());
				costType = powerToCheck.text [i + 3];
				if (costType == 'H') 
					costResource = currentHealth;

				if (costType == 'M')
					costResource = currentMana;
			
				if (costType == 'G')
					costResource = gcScript.goldValue;
			}
		}

		if (costResource == currentHealth) 
		{
			if (costNumber < costResource) 
				enoughResources = true;
		}

		else if (costResource == currentMana || costResource == gcScript.goldValue) 
		{
			if (costNumber <= costResource) 
				enoughResources = true;
		}

		if (costNumber > costResource) 
		{
			powerToCheck.color = Color.red;
			enoughResources = false;
		}
	}

	int whichChild()
	{
		int index = 0;
		for (int i = 0; i < 3; i++) 
		{
			if (gcScript.playerHeroes.transform.GetChild (i).gameObject == this.gameObject)
				index = i;
		}

		return index;
	}

	IEnumerator ExecuteCombo(float waitTime)
	{
		currentCombo = 0;
		RectTransform rect = choiceIndicator.GetComponent<RectTransform> ();
		canCombo = false;
		gcScript.ZoomOff (true, whichChild());
		gcScript.DraggableOffOrNot (true);
		gcScript.shopButton.interactable = false;
		gcScript.endButton.interactable = false;
		playCards = this.gameObject.transform.GetChild (1).gameObject;
		heroTree = this.gameObject.transform.GetChild (4).gameObject;
		heroPower = heroTree.gameObject.transform.GetChild (0).gameObject;

		gcScript.leftButton = 0;
		gcScript.rightButton = 0;
		gcScript.pbLeft.interactable = true;
		gcScript.pbRight.interactable = true;

		yield return new WaitForSeconds (waitTime);
		gcScript.messageText.text = "Left-click on each of the power(s) you wish to use. To skip a power, click on the corresponding locked power";
		if (currentLevel == 2 || currentLevel == 3)
			comboNumber = 1;
		if (currentLevel == 4 || currentLevel == 5)
			comboNumber = 2;
		if (currentLevel == 6)
			comboNumber = 3;
		
		for (int i = 0; i < 5; i+=2) 
		{
			gcScript.pbLeft.interactable = true;
			gcScript.pbRight.interactable = true;
			if (i == 0) 
			{
				choiceIndicator.SetActive(true);
				rect.anchoredPosition = new Vector2 (0, 134);
			}
			if (i == 2)
				rect.anchoredPosition = new Vector2 (0, -34);
			if (i == 4)
				rect.anchoredPosition = new Vector2 (0, -185);
			
			leftPower = heroPower.gameObject.transform.GetChild (i).gameObject;
			leftPowerText = leftPower.GetComponent<Text> ();
			rightPower = heroPower.gameObject.transform.GetChild (i + 1).gameObject;
			rightPowerText = rightPower.GetComponent<Text> ();
			CheckPowerCost ();
			if (enoughResources == false) 
			{
				if (i == 4) 
				{
					gcScript.messageText.text = "Combo execution complete";
					yield return new WaitForSeconds (0.6f);
					ComboCleanup ();
					if (didCombine) 
					{
						if (szScript.areAllHeroesMaxed == false) 
						{
							gcScript.messageText.text = "You used Combine! You get to Level up one of your heroes";
							yield return new WaitForSeconds (0.8f);
							szScript.LevelUpHero ();
							didCombine = false;
						}
					}
					canCombo = true;
					choiceIndicator.SetActive(false);
					yield break;
				}

				currentCombo++;
				continue;
			}

			while (enoughResources) 
			{
				if (gcScript.leftButton == 1) 
				{
					StartCoroutine(ComboTextColors(true));
					gcScript.pbLeft.interactable = false;
					gcScript.pbRight.interactable = false;
					yield return new WaitUntil (() => powerUsed == true);
					currentCombo++;
					gcScript.leftButton = 0;
					gcScript.rightButton = 0;
					powerUsed = false;
					break;
				}

				if (gcScript.rightButton == 1) 
				{
					StartCoroutine(ComboTextColors(false));
					gcScript.pbLeft.interactable = false;
					gcScript.pbRight.interactable = false;
					yield return new WaitUntil (() => powerUsed == true);
					currentCombo++;
					gcScript.rightButton = 0;
					gcScript.leftButton = 0;
					powerUsed = false;
					break;
				}

				yield return null;
			}

			if (currentCombo == comboNumber) 
			{
				gcScript.messageText.text = "Combo execution complete";
				yield return new WaitForSeconds (0.6f);
				ComboCleanup ();
				if (didCombine) 
				{
					if (szScript.areAllHeroesMaxed == false) 
					{
						gcScript.messageText.text = "You used Combine! You get to Level up one of your heroes";
						yield return new WaitForSeconds (0.8f);
						szScript.LevelUpHero ();
						didCombine = false;
					}
				}
				canCombo = true;
				choiceIndicator.SetActive(false);
				break;
			}
		}
	}

	void ComboCleanup ()
	{
		for (int i = 0; i < 6; i++) 
		{
			GameObject revertObj = heroPower.gameObject.transform.GetChild (i).gameObject;
			Text revertText = revertObj.GetComponent<Text> ();
			if (revertText.color == Color.blue || revertText.color == Color.red) 
			{
				revertText.color = Color.green;
			}
		}

		for (int i = 0; i < 2; i++) 
		{
			GameObject bonusCards = playCards.transform.GetChild (0).gameObject;
			bonusCards.transform.position = discardPile.transform.position;
			bonusCards.transform.SetParent (discardPile.transform);
		}

		gcScript.ZoomOff (false, whichChild());
		gcScript.DraggableOffOrNot (false);
		gcScript.buyPhaseButton.interactable = true;
		gcScript.endButton.interactable = true;
		gcScript.UpdateDiscardDropdown ();
	}

	IEnumerator ComboTextColors(bool leftRight)
	{
		Text leftOrRight = null;
		Text oppositeSide = null;

		if (leftRight == true) 
		{
			leftOrRight = leftPowerText;
			oppositeSide = rightPowerText;
		}
		
		if (leftRight == false) 
		{
			leftOrRight = rightPowerText;
			oppositeSide = leftPowerText;
		}
		
		if (leftOrRight.color == Color.green) 
		{
			leftOrRight.color = Color.blue;
			if (powerName == "HUNT") 
			{
				Hunt ();
				yield return new WaitUntil (() => powerUsed == true);
			}
				
			if (powerName == "ZAP") 
			{
				StartCoroutine (Zap(0.4f));
				yield return new WaitUntil (() => powerUsed == true);
			}
				
			if (powerName == "GREED") 
			{
				Greed ();
				yield return new WaitUntil (() => powerUsed == true);
			}
				
			if (powerName == "COMBINE") 
			{
				StartCoroutine(Combine(0.4f));
				yield return new WaitUntil (() => powerUsed == true);
			}
				
			if (powerName == "UNBURDEN") 
			{
				StartCoroutine(Unburden(0.4f));
				yield return new WaitUntil (() => powerUsed == true);
			}
				
			if (powerName == "POTENCE") 
			{
				Potence (true);
				yield return new WaitUntil (() => powerUsed == true);
			}
				
			if (powerName == "PROSPERITY") 
			{
				Prosperity (true);
				yield return new WaitUntil (() => powerUsed == true);
			}
				
			if (powerName == "HOARD") 
			{
				Hoard (true);
				yield return new WaitUntil (() => powerUsed == true);
			}
				
			if (powerName == "CURSE") 
			{
				StartCoroutine(Curse (0.2f));
				yield return new WaitUntil (() => powerUsed == true);
			}
				
			if (powerName == "SCAVENGE") 
			{
				StartCoroutine (Scavenge (0.2f));
				yield return new WaitUntil (() => powerUsed == true);
			}
				
			if (powerName == "RECYCLE")
			{
				StartCoroutine (Recycle (0.2f));
				yield return new WaitUntil (() => powerUsed == true);
			}
				
			if (powerName == "DURESS")
			{
				StartCoroutine(Duress(0.2f));
				yield return new WaitUntil (() => powerUsed == true);
			}
				
			SubtractPowerCost ();
		}

		else if (leftOrRight.color == Color.white) 
		{
			oppositeSide.color = Color.red;
			gcScript.messageText.text = "You skipped " + leftOrRight.text + ". Make next choice";
			powerUsed = true;
		}
	}

	void SubtractPowerCost()
	{
		if(costResource == currentHealth)
			currentHealth = currentHealth - costNumber;
		if(costResource == currentMana)
			currentMana = currentMana - costNumber;
		if(costResource == gcScript.goldValue)
			gcScript.goldValue = gcScript.goldValue - costNumber;
	}

	void Hunt()
	{
		gcScript.huntValue++;
		gcScript.messageText.text = powerName + " executed. Make next choice";
		powerUsed = true;
	}

	void Greed()
	{
		gcScript.numberOfBuys++;
		gcScript.messageText.text = powerName + " executed. Make next choice";
		powerUsed = true;
	}

	IEnumerator Zap(float waitTime)
	{
		yield return new WaitForSeconds (waitTime);
		gcScript.messageText.text = "Please left-click on an enemy hero you wish to zap";
		while (true) 
		{
			if (Input.GetMouseButtonDown (0)) 
			{
				RaycastHit hit;
				Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);

				if (Physics.Raycast (ray, out hit)) 
				{
					clickedHero = hit.transform.gameObject;
					if (hit.transform.parent == enemyHeroes.transform) 
					{
						EnemyHeroes ehScript = clickedHero.GetComponent<EnemyHeroes> ();
						ehScript.currentHealth--;
						gcScript.messageText.text = powerName + " executed. Make next choice";
						powerUsed = true;
						yield break;
					}
					else
						gcScript.messageText.text = "Please left-click on an enemy hero you wish to zap";
				}
				else
					gcScript.messageText.text = "Please left-click on an enemy hero you wish to zap";
			}
			yield return null;
		}
	}

	IEnumerator Combine(float waitTime)
	{
		int n = 0;
		bool cancel = true;
		yield return new WaitForSeconds (waitTime);
		gcScript.messageText.text = "Click on the first card you wish to combine or on your deck to cancel Combining. Combat bonuses cannot be combined";

		while (cancel) {
			while (n < 2) {
				if (Input.GetMouseButtonDown (0)) {
					RaycastHit hit;
					Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);

					if (Physics.Raycast (ray, out hit)) {
						clickedHero = hit.transform.gameObject;
						if (hit.transform.parent == playerHand.transform && (hit.transform.name != "Lifesteal" && hit.transform.name != "Manasteal" && hit.transform.name != "Thief" && hit.transform.name != "TargetBlock" && hit.transform.name != "ValorOfHercules" && hit.transform.name != "DresdensRobe")) {
							if (n == 0) {
								firstCard = hit.transform.gameObject;
								firstCardTint = hit.transform.GetComponent<Image> ();
								firstCardTint.color = new Color (0, 1, 0, 0.3f);
								gcScript.messageText.text = "Please click on the second card you wish to combine";
							}

							if (n == 1) {
								secondCard = hit.transform.gameObject;
								secondCardTint = hit.transform.GetComponent<Image> ();
								secondCardTint.color = new Color (0, 1, 0, 0.3f);
							}

							n++;
						}

						if (hit.transform.parent == playerDeck.transform) {
							gcScript.messageText.text = "Combining cancelled";
							if(firstCardTint != null) 
								firstCardTint.color = new Color (0, 0, 0, 0);
							if(secondCardTint != null) 
								secondCardTint.color = new Color (0, 0, 0, 0);
							powerUsed = true;
							yield break;
						}

						else if (hit.transform.name == "Lifesteal" || hit.transform.name == "Manasteal" || hit.transform.name == "Thief" || hit.transform.name == "TargetBlock" || hit.transform.name == "ValorOfHercules" || hit.transform.name == "DresdensRobe")
							gcScript.messageText.text = "Combat bonuses cannot be combined. Choose another one, or click on your deck to cancel";

						else
							gcScript.messageText.text = "Click on a card in your hand to combine, or on your deck to cancel Combining";
					} else
						gcScript.messageText.text = "Click on a card in your hand to combine, or on your deck to cancel Combining";
				}
				yield return null;
			}
				
			if (firstCard.name == "Mana Vial" && secondCard.name == "Mana Vial") {
				combinedCard = GameObject.Find ("ManaPotion");
				if (combinedCard != null) {
					CombineCleanup ();
					gcScript.messageText.text = powerName + " executed. Make next choice";
					firstCardTint.color = new Color (0, 0, 0, 0);
					secondCardTint.color = new Color (0, 0, 0, 0);
					didCombine = true;
					powerUsed = true;
					break;
				}
			}

			if (firstCard.name == "Health Vial" && secondCard.name == "Health Vial") {
				combinedCard = GameObject.Find ("HealthPotion");
				if (combinedCard != null) {
					CombineCleanup ();
					gcScript.messageText.text = powerName + " executed. Make next choice";
					firstCardTint.color = new Color (0, 0, 0, 0);
					secondCardTint.color = new Color (0, 0, 0, 0);
					didCombine = true;
					powerUsed = true;
					break;
				}
			}

			if ((firstCard.name == "Mana Vial" && secondCard.name == "Health Vial") || (secondCard.name == "Mana Vial" && firstCard.name == "Health Vial")) {
				combinedCard = GameObject.Find ("SpecialPotion");
				if (combinedCard != null) {
					CombineCleanup ();
					gcScript.messageText.text = powerName + " executed. Make next choice";
					firstCardTint.color = new Color (0, 0, 0, 0);
					secondCardTint.color = new Color (0, 0, 0, 0);
					didCombine = true;
					powerUsed = true;
					break;
				}
			}

			if (firstCard.name == "Fetch Quest" && secondCard.name == "Fetch Quest") {
				combinedCard = GameObject.Find ("StolenPouch");
				if (combinedCard != null) {
					CombineCleanup ();
					gcScript.messageText.text = powerName + " executed. Make next choice";
					firstCardTint.color = new Color (0, 0, 0, 0);
					secondCardTint.color = new Color (0, 0, 0, 0);
					didCombine = true;
					powerUsed = true;
					break;
				}
			}

			if ((firstCard.name == "PotOfGreed" && secondCard.name == "Mana Vial") || (secondCard.name == "PotOfGreed" && firstCard.name == "Mana Vial")) {
				combinedCard = GameObject.Find ("InnerPower");
				if (combinedCard != null) {
					CombineCleanup ();
					gcScript.messageText.text = powerName + " executed. Make next choice";
					firstCardTint.color = new Color (0, 0, 0, 0);
					secondCardTint.color = new Color (0, 0, 0, 0);
					didCombine = true;
					powerUsed = true;
					break;
				}
			}

			if ((firstCard.name == "Fetch Quest" && secondCard.name == "Health Vial") || (secondCard.name == "Fetch Quest" && firstCard.name == "Health Vial")) {
				combinedCard = GameObject.Find ("WealthIsHealth");
				if (combinedCard != null) {
					CombineCleanup ();
					gcScript.messageText.text = powerName + " executed. Make next choice";
					firstCardTint.color = new Color (0, 0, 0, 0);
					secondCardTint.color = new Color (0, 0, 0, 0);
					didCombine = true;
					powerUsed = true;
					break;
				}
			}

			if ((firstCard.name == "Fetch Quest" && secondCard.name == "PotOfGreed") || (secondCard.name == "Fetch Quest" && firstCard.name == "PotOfGreed")) {
				combinedCard = GameObject.Find ("JarOfGreed");
				if (combinedCard != null) {
					CombineCleanup ();
					gcScript.messageText.text = powerName + " executed. Make next choice";
					firstCardTint.color = new Color (0, 0, 0, 0);
					secondCardTint.color = new Color (0, 0, 0, 0);
					didCombine = true;
					powerUsed = true;
					break;
				}
			}

			if ((firstCard.name == "Fetch Quest" && secondCard.name == "Mana Vial") || (secondCard.name == "Fetch Quest" && firstCard.name == "Mana Vial")) {
				combinedCard = GameObject.Find ("TransmuteLife");
				if (combinedCard != null) {
					CombineCleanup ();
					gcScript.messageText.text = powerName + " executed. Make next choice";
					firstCardTint.color = new Color (0, 0, 0, 0);
					secondCardTint.color = new Color (0, 0, 0, 0);
					didCombine = true;
					powerUsed = true;
					break;
				}
			} 

			else 
			{
				gcScript.messageText.text = "Those two cards cannot be combined in this match. Try again or click on your deck to cancel";
				firstCardTint.color = new Color (0, 0, 0, 0);
				secondCardTint.color = new Color (0, 0, 0, 0);
				n = 0;
			}
		}
	}

	void CombineCleanup()
	{
		szScript = combinedCard.GetComponent<ShopZoom> ();
		szScript.BuyCard (combinedCard);

		firstCard.transform.position = discardPile.transform.position;
		firstCard.transform.SetParent (discardPile.transform);

		secondCard.transform.position = discardPile.transform.position;
		secondCard.transform.SetParent (discardPile.transform);
		gcScript.UpdateDiscardDropdown ();
	}

	IEnumerator Unburden(float waitTime)
	{
		yield return new WaitForSeconds (waitTime);
		gcScript.messageText.text = "Click on a card in your hand you wish to discard";
		while (true) 
		{
			if (Input.GetMouseButtonDown (0)) 
			{
				RaycastHit hit;
				Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);

				if (Physics.Raycast (ray, out hit)) 
				{
					clickedHero = hit.transform.gameObject;
					if (hit.transform.parent == playerHand.transform) 
					{
						hit.transform.position = discardPile.transform.position;
						hit.transform.SetParent (discardPile.transform);
						gcScript.UpdateDiscardDropdown ();
						gcScript.DrawACard (1);
						gcScript.messageText.text = powerName + " executed. Make next choice";
						powerUsed = true;
						yield break;
					}
					else
						gcScript.messageText.text = "Click on a card in your hand you wish to discard";
				}
				else
					gcScript.messageText.text = "Click on a card in your hand you wish to discard";
			}
			yield return null;
		}
	}

	public void Potence(bool offOn)
	{
		GameObject temp;
		for (int i = 0; i < 7; i++) 
		{
			temp = gcScript.actionEffects.transform.GetChild (i).gameObject;
			if (temp.name == "HVEffect" || temp.name == "HPEffect" || temp.name == "MVEffect" || temp.name == "MPEffect") 
			{
				Text tempText = temp.GetComponent<Text> ();
				if (offOn == true) {
					gcScript.potenceFlag = true;
					tempText.color = Color.green;
					if (tempText.text [1] == '1') 
					{
						tempText.text = tempText.text.Remove (1, 1);
						tempText.text = tempText.text.Insert(1,"2");
					}

					else if (tempText.text [1] == '2') 
					{
						tempText.text = tempText.text.Remove (1, 1);
						tempText.text = tempText.text.Insert(1,"3");
					}
				}

				if (offOn == false) 
				{
					gcScript.potenceFlag = false;
					if (tempText.color == Color.green) {
						tempText.color = Color.white;
						if (tempText.text [1] == '2') {
							tempText.text = tempText.text.Remove (1, 1);
							tempText.text = tempText.text.Insert (1, "1");
						} else if (tempText.text [1] == '3') {
							tempText.text = tempText.text.Remove (1, 1);
							tempText.text = tempText.text.Insert (1, "2");
						}
					}
				}
			}
		}

		powerUsed = true;
	}

	public void Prosperity(bool offOn)
	{
		GameObject temp;
		for (int i = 0; i < 7; i++) 
		{
			temp = gcScript.actionEffects.transform.GetChild (i).gameObject;
			if (temp.name == "FQEffect" || temp.name == "StolenPEffect") 
			{
				Text tempText = temp.GetComponent<Text> ();
				if (offOn == true) {
					gcScript.prosperityFlag = true;
					tempText.color = Color.green;
					if (tempText.text [1] == '1') 
					{
						tempText.text = tempText.text.Remove (1, 1);
						tempText.text = tempText.text.Insert(1,"2");
					}

					else if (tempText.text [1] == '2') 
					{
						tempText.text = tempText.text.Remove (1, 1);
						tempText.text = tempText.text.Insert(1,"3");
					}
				}

				if (offOn == false) 
				{
					gcScript.prosperityFlag = false;
					if (tempText.color == Color.green) {
						tempText.color = Color.white;
						if (tempText.text [1] == '2') {
							tempText.text = tempText.text.Remove (1, 1);
							tempText.text = tempText.text.Insert (1, "1");
						} else if (tempText.text [1] == '3') {
							tempText.text = tempText.text.Remove (1, 1);
							tempText.text = tempText.text.Insert (1, "2");
						}
					}
				}
			}
		}

		powerUsed = true;
	}

	public void Hoard(bool offOn)
	{
		GameObject temp;
		for (int i = 0; i < 7; i++) 
		{
			temp = gcScript.actionEffects.transform.GetChild (i).gameObject;
			if (temp.name == "DrawEffect") 
			{
				Text tempText = temp.GetComponent<Text> ();
				if (offOn == true) {
					gcScript.hoardFlag = true;
					tempText.color = Color.green;
					tempText.text = tempText.text.Remove (1, 1);
					tempText.text = tempText.text.Insert(1,"3");
				}

				if (offOn == false) 
				{
					gcScript.hoardFlag = false;
					if (tempText.color == Color.green) 
					{
						tempText.color = Color.white;
						tempText.text = tempText.text.Remove (1, 1);
						tempText.text = tempText.text.Insert (1, "2");
					}
				}
			}
		}

		powerUsed = true;
	}

	IEnumerator Curse(float waitTime)
	{
		yield return new WaitForSeconds (waitTime);
		gcScript.messageText.text = "Please left-click on an enemy hero you wish to curse";
		while (true) 
		{
			if (Input.GetMouseButtonDown (0)) 
			{
				RaycastHit hit;
				Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);

				if (Physics.Raycast (ray, out hit)) 
				{
					clickedHero = hit.transform.gameObject;
					if (hit.transform.parent == enemyHeroes.transform) 
					{
						EnemyHeroes ehScript = clickedHero.GetComponent<EnemyHeroes> ();
						ehScript.isCursed = true;
						gcScript.messageText.text = powerName + " executed. Make next choice";
						powerUsed = true;
						yield break;
					}
					else
						gcScript.messageText.text = "Please left-click on an enemy hero you wish to curse";
				}
				else
					gcScript.messageText.text = "Please left-click on an enemy hero you wish to curse";
			}
			yield return null;
		}
	}

	IEnumerator Scavenge(float waitTime)
	{
		gcScript.didChooseScavenge = false;
		yield return new WaitForSeconds (waitTime);
		gcScript.messageText.text = "Click on a card in your hand you wish to discard";
		while (true) 
		{
			if (Input.GetMouseButtonDown (0)) 
			{
				RaycastHit hit;
				Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);

				if (Physics.Raycast (ray, out hit)) 
				{
					clickedHero = hit.transform.gameObject;
					if (hit.transform.parent == playerHand.transform) 
					{
						hit.transform.position = discardPile.transform.position;
						hit.transform.SetParent (discardPile.transform);
						gcScript.UpdateDiscardDropdown ();
						while (true) 
						{
							gcScript.messageText.text = "You discarded " + clickedHero.name + ". " + "Click on your discard pile and select a card from the dropdown list you wish to Scavenge, then click the 'Scavenge' button";
							gcScript.scavengeButtonObj.SetActive (true);
							if (gcScript.didChooseScavenge == true) 
							{
								Debug.Log (gcScript.dropdownText.text);
								for (int i = 0; i < discardPile.transform.childCount; i++) 
								{
									GameObject discardedCard = discardPile.transform.GetChild (i).gameObject;
									if (discardedCard.name == gcScript.dropdownText.text) 
									{
										gcScript.MoveCard (discardedCard, playerHand.transform);
										gcScript.messageText.text = powerName + " executed. Make next choice";
										powerUsed = true;
										yield break;
									}
								}
							}
							yield return null;
						}
					}
					else
						gcScript.messageText.text = "Click on a card in your hand you wish to discard";
				}
				else
					gcScript.messageText.text = "Click on a card in your hand you wish to discard";
			}
			yield return null;
		}
	}

	IEnumerator Recycle(float waitTime)
	{
		yield return new WaitForSeconds (waitTime);
		gcScript.messageText.text = "Click on a card in your hand you wish to Recycle";
		while (true) 
		{
			if (Input.GetMouseButtonDown (0)) 
			{
				RaycastHit hit;
				Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);

				if (Physics.Raycast (ray, out hit)) 
				{
					clickedHero = hit.transform.gameObject;
					if (hit.transform.parent == playerHand.transform) 
					{
						GameObject recycled = hit.transform.gameObject;
						if (hit.transform.name == "Health Vial" || hit.transform.name == "Mana Vial" || hit.transform.name == "HealthPotion" || hit.transform.name == "ManaPotion" || hit.transform.name == "SpecialPotion") {
							gcScript.messageText.text = "You selected " + hit.transform.name + " and gained 1 gold. Make next power choice";
							gcScript.goldValue++;
						} else {
							gcScript.messageText.text = "You selected " + hit.transform.name + ". Please click on one of your heroes whose health and mana you wish to increase";
							while (true) {
								if (Input.GetMouseButtonDown (0)) {
									RaycastHit hit2;
									Ray ray2 = Camera.main.ScreenPointToRay (Input.mousePosition);

									if (Physics.Raycast (ray2, out hit2)) {
										clickedHero = hit2.transform.gameObject;
										if (hit2.transform.parent == playerHeroes.transform) {
											HeroDrop heroDrop = hit2.transform.GetComponent<HeroDrop> ();
											heroDrop.currentHealth++;
											heroDrop.currentMana++;
											gcScript.messageText.text = hit2.transform.name + " gained 1 Health and Mana. Make next power choice";
											break;
										}
									}
								}
								yield return null;
							}
						}

						hit.transform.position = discardPile.transform.position;
						hit.transform.SetParent (discardPile.transform);
						gcScript.UpdateDiscardDropdown ();
						powerUsed = true;
						yield break;
					}
					else
						gcScript.messageText.text = "Click on a card in your hand you wish to discard";
				}
				else
					gcScript.messageText.text = "Click on a card in your hand you wish to discard";
			}
			yield return null;
		}
	}

	IEnumerator Duress(float waitTime)
	{
		GameObject duressedCard = null;
		yield return new WaitForSeconds (waitTime);
		gcScript.messageText.text = "Click on a card in your hand to duress the opponent with";
		while (true) 
		{
			if (Input.GetMouseButtonDown (0)) 
			{
				RaycastHit hit;
				Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);

				if (Physics.Raycast (ray, out hit)) 
				{
					clickedHero = hit.transform.gameObject;
					if (hit.transform.parent == playerHand.transform && hit.transform.name != "VictoryMedal") 
					{
						if (hit.transform.name == "Health Vial" || hit.transform.name == "Mana Vial" || hit.transform.name == "HealthPotion" || hit.transform.name == "ManaPotion" || hit.transform.name == "SpecialPotion") 
						{
							for (int i = 0; i < enemyHand.transform.childCount; i++) 
							{
								GameObject eCard = enemyHand.transform.GetChild (i).gameObject;
								if (eCard.name == "Health Vial" || eCard.name == "Mana Vial" || eCard.name == "HealthPotion" || eCard.name == "ManaPotion" || eCard.name == "SpecialPotion") 
								{
									duressedCard = eCard;
								}
							}
						}

						if (hit.transform.name == "Fetch Quest" || hit.transform.name == "PotOfGreed" || hit.transform.name == "JarOfGreed" || hit.transform.name == "StolenPouch" || hit.transform.name == "InnerPower" || hit.transform.name == "WealthIsHealth" || hit.transform.name == "TransmuteLife")
						{
							for (int i = 0; i < enemyHand.transform.childCount; i++) 
							{
								GameObject eCard = enemyHand.transform.GetChild (i).gameObject;
								if (eCard.name == "Fetch Quest" || eCard.name == "PotOfGreed" || eCard.name == "JarOfGreed" || eCard.name == "StolenPouch" || eCard.name == "InnerPower" || eCard.name == "WealthIsHealth" || eCard.name == "TransmuteLife") 
								{
									duressedCard = eCard;
								}
							}
						}

						if (hit.transform.name == "Lifesteal" || hit.transform.name == "Manasteal" || hit.transform.name == "Thief" || hit.transform.name == "TargetBlock" || hit.transform.name == "ValorOfHercules" || hit.transform.name == "DresdensRobe")
						{
							for (int i = 0; i < enemyHand.transform.childCount; i++) 
							{
								GameObject eCard = enemyHand.transform.GetChild (i).gameObject;
								if (eCard.name == "Lifesteal" || eCard.name == "Manasteal" || eCard.name == "Thief" || eCard.name == "TargetBlock" || eCard.name == "ValorOfHercules" || eCard.name == "DresdensRobe") 
								{
									duressedCard = eCard;
								}
							}
						}
					}

					duressedCard.transform.position = enemyDiscardPile.transform.position;
					duressedCard.transform.SetParent (enemyDiscardPile.transform);
					gcScript.messageText.text = "You selected " + duressedCard.name + ". Make next power choice";
					powerUsed = true;
					yield break;
				}
				else
					gcScript.messageText.text = "Click on a card in your hand to duress the opponent with";
			}
			yield return null;
		}
	}
}


