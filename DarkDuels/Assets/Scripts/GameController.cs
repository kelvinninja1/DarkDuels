using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class GameController : MonoBehaviour
{
	public Camera battleCamera;
	public Camera shopCamera;
	public bool camSwitch = false;
	public bool handZoom = true;
	public Dropdown discardDropdown;
	public GameObject dropDownObject;
	public Text dropdownText;

	public int numberOfBuys;
	public int goldValue;
	public int huntValue;
	public bool potenceFlag = false;
	public bool prosperityFlag = false;
	public bool hoardFlag = false;
	public bool didChooseScavenge = false;
	public List<string> discardList = new List<string>(51);

	[Header("UI Elements")]
	public Text goldText;
	public Text shopGoldText;
	public Text buyText;
	public Text shopBuyText;
	public Text huntText;
	public Text messageText;
	public Text shopMessageText;
	public Color turnColor;
	public Color defaultColor;

	[Header("Buttons")]
	public Button shopButton;
	public Button actionButton;
	public Button buyPhaseButton;
	public Button endButton;
	public Button backButton;
	public Button scavengeButton;

	[Header("Heroes")]
	public CardStack enemyHeroes;
	public CardStack playerHeroes;
	public CardStack allHeroes;

	[Header("Decks and Hands")]
	public CardStack enemyDeck;
	public CardStack enemyHand;
	public CardStack playerDeck;
	public CardStack playerHand;
	public CardStack discardPile;
	public CardStack allActions;
	public CardStack actions;
	public CardStack allCombat;
	public CardStack combatBonuses;

	int fetchesInHand = 0;
	HeroDrop hdScript;
	GameZoom gameZoom;
	GameObject cardsInDeck;
	GameObject cardsInDp;
	Text cardsInText;
	Text cardsInDpText;

	GameObject powerButtonLeft;
	GameObject powerButtonRight;
	public GameObject scavengeButtonObj;

	public int leftButton;
	public int rightButton;

	public Button pbLeft;
	public Button pbRight;
	public Image draggableIcon;

	public GameObject actionEffects;
	public GameObject hvEffect;
	public GameObject hpEffect;
	public GameObject mvEffect;
	public GameObject mpEffect;
	public GameObject fqEffect;
	public GameObject stolenpEffect;
	public GameObject drawEffect;

	public GameObject shophvEffect;
	public GameObject shophpEffect;
	public GameObject shopmvEffect;
	public GameObject shopmpEffect;
	public GameObject shopfqEffect;
	public GameObject shopstolenpEffect;
	public GameObject shopdrawEffect;

	void Awake ()
	{
		cardsInDeck = GameObject.Find ("CardsInDeck");
		cardsInDp = GameObject.Find ("CardsInDp");
		powerButtonLeft = GameObject.Find ("PButtonLeft");
		powerButtonRight = GameObject.Find ("PButtonRight");

		actionEffects = GameObject.Find ("ActionEffects");
		hvEffect = GameObject.Find ("HVEffect");
		hpEffect = GameObject.Find ("HPEffect");
		mvEffect = GameObject.Find ("MVEffect");
		mpEffect = GameObject.Find ("MPEffect");
		fqEffect = GameObject.Find ("FQEffect");
		stolenpEffect = GameObject.Find ("StolenPEffect");
		drawEffect = GameObject.Find ("DrawEffect");

		shophvEffect = GameObject.Find ("ShopHVEffect");
		shophpEffect = GameObject.Find ("ShopHPEffect");
		shopmvEffect = GameObject.Find ("ShopMVEffect");
		shopmpEffect = GameObject.Find ("ShopMPEffect");
		shopfqEffect = GameObject.Find ("ShopFQEffect");
		shopstolenpEffect = GameObject.Find ("ShopStolenPEffect");
		shopdrawEffect = GameObject.Find ("ShopDrawEffect");
		scavengeButtonObj = GameObject.Find ("ScavengeButton");

		DealHeroes ();
		DealActions ();
		DealCombats ();
	} 

	void Start () 
	{
		goldValue = 10;
		numberOfBuys = 11;

		actionButton.interactable = false;
		shopButton.interactable = false;
		backButton.interactable = false;
		endButton.interactable = false;

		ColorBlock cb = actionButton.colors;
		turnColor.a = 1;
		cb.disabledColor = turnColor;
		actionButton.colors = cb;

		KillExtraCards ();
		DealPlayerHand ();
		DealEnemyHand ();

		shopButton.onClick.AddListener (GoToShop);
		backButton.onClick.AddListener (GoBack);
		buyPhaseButton.onClick.AddListener (BuyPhase);
		endButton.onClick.AddListener (EndTurn);

		StartCoroutine (checkFetches (0.2f));
		StartCoroutine (finishDealing (0.4f));

		cardsInText = cardsInDeck.GetComponent<Text> ();
		cardsInDpText = cardsInDp.GetComponent<Text> ();

		pbLeft = powerButtonLeft.GetComponent<Button> ();
		pbRight = powerButtonRight.GetComponent<Button> ();

		hdScript = playerHeroes.GetComponentInChildren<HeroDrop> ();
		discardDropdown = dropDownObject.GetComponent<Dropdown> ();

		pbLeft.onClick.AddListener (PowerButtonLeft);
		pbRight.onClick.AddListener (PowerButtonRight);

		pbLeft.interactable = false;
		pbRight.interactable = false;

		leftButton = 0;
		rightButton = 0;

		discardDropdown.ClearOptions ();
		dropDownObject.SetActive (false);
		scavengeButtonObj.SetActive (false);
		scavengeButton.onClick.AddListener (ChooseScavenge);
	}

	public void DropdownIndexChanged(int index)
	{
		dropdownText.text = discardList [index];
	}

	void ChooseScavenge()
	{
		didChooseScavenge = true;
	}

	void DealHeroes()
	{
		for (int i = 0; i < 3; i++) 
		{
			playerHeroes.Push (allHeroes.Pop ());
			enemyHeroes.Push (allHeroes.Pop ());
		}
	}

	void DealActions()
	{
		for (int i = 0; i < 5; i++) 
		{
			actions.Push (allActions.Pop ());
		}
	}

	void DealCombats()
	{
		for (int i = 0; i < 4; i++) 
		{
			combatBonuses.Push (allCombat.Pop ());
		}
	}

	void KillExtraCards()
	{
		for (int i = 0; i < 3; i++) 
		{
			GameObject extraCard = allHeroes.transform.GetChild (i).gameObject;
			Destroy (extraCard);
		}

		for (int i = 0; i < 4; i++) 
		{
			GameObject extraCard = allActions.transform.GetChild (i).gameObject;
			Destroy (extraCard);
		}

		for (int i = 0; i < 2; i++) 
		{
			GameObject extraCard = allCombat.transform.GetChild (i).gameObject;
			Destroy (extraCard);
		}
	}

	public void EffectsOff()
	{
		hvEffect.SetActive(false);
		hpEffect.SetActive(false);
		mvEffect.SetActive(false);
		mpEffect.SetActive(false);
		fqEffect.SetActive(false);
		stolenpEffect.SetActive(false);
		drawEffect.SetActive(false);
	}

	public void ShopEffectsOff()
	{
		shophvEffect.SetActive(false);
		shophpEffect.SetActive(false);
		shopmvEffect.SetActive(false);
		shopmpEffect.SetActive(false);
		shopfqEffect.SetActive(false);
		shopstolenpEffect.SetActive(false);
		shopdrawEffect.SetActive(false);
	}

	public void PowerButtonLeft()
	{
		leftButton = 1;
	}

	public void PowerButtonRight()
	{
		rightButton = 1;
	}

	void Update()
	{
		goldText.text = goldValue.ToString ();
		shopGoldText.text = goldValue.ToString ();
		buyText.text = numberOfBuys.ToString ();
		shopBuyText.text = numberOfBuys.ToString ();
		huntText.text = huntValue.ToString ();
		cardsInText.text = playerDeck.transform.childCount.ToString ();
		cardsInDpText.text = discardPile.transform.childCount.ToString ();
	}

	public void UpdateDiscardDropdown()
	{
		discardDropdown.ClearOptions ();
		discardList.Clear ();
		for (int i = 0; i < discardPile.transform.childCount; i++) 
		{
			discardList.Add (discardPile.transform.GetChild (i).name);
		}

		discardDropdown.AddOptions (discardList);
	}

	IEnumerator checkFetches(float waitTime)
	{
		yield return new WaitForSeconds (waitTime);
		CheckFetchesInHand ();
	}

	IEnumerator finishDealing(float waitTime)
	{
		yield return new WaitForSeconds (waitTime);
		DealLastCard ();
	}
		
	public void GoToShop()
	{
		camSwitch = !camSwitch;
		shopCamera.gameObject.SetActive(camSwitch);
		battleCamera.gameObject.SetActive(!camSwitch);
		backButton.interactable = true;
		shopButton.interactable = false;
	}

	public void GoBack()
	{
		camSwitch = !camSwitch;
		shopCamera.gameObject.SetActive(camSwitch);
		battleCamera.gameObject.SetActive(!camSwitch);
		backButton.interactable = false;
		shopButton.interactable = true;
	}

	public void ZoomOff(bool zoomBool, int number)
	{
		for (int i = 0; i < 3; i++) 
		{
			Transform temp = playerHeroes.transform.GetChild (i).transform;
			gameZoom = temp.GetComponent<GameZoom> ();

			if(zoomBool == true)
			{
				if (i != number)
					gameZoom.enabled = false;
			}

			if(zoomBool == false)
				gameZoom.enabled = true;
		}

		for (int i = 0; i < 3; i++) 
		{
			Transform temp = enemyHeroes.transform.GetChild (i).transform;
			gameZoom = temp.GetComponent<GameZoom> ();
			if(zoomBool == true)
				gameZoom.enabled = false;

			if(zoomBool == false)
				gameZoom.enabled = true;
		}

		if (zoomBool == true)
			handZoom = false;
		if (zoomBool == false)
			handZoom = true;
	}

	void BuyPhase()
	{
		DraggableOffOrNot (true);

		shopButton.interactable = true;
		buyPhaseButton.interactable = false;
		endButton.interactable = true;

		ColorBlock buyCb = buyPhaseButton.colors;
		turnColor.a = 1;
		buyCb.disabledColor = turnColor;
		buyPhaseButton.colors = buyCb;

		ColorBlock cb = actionButton.colors;
		defaultColor.a = 1;
		cb.disabledColor = defaultColor;
		actionButton.colors = cb;
	}

	public void DraggableOffOrNot(bool isDrag)
	{
		for (int i = 0; i < playerHand.transform.childCount; i++) 
		{
			Draggable d = playerHand.transform.GetChild (i).GetComponent<Draggable> ();
			if (isDrag == true) 
			{
				d.enabled = false;
				draggableIcon.enabled = false;
			}
			if (isDrag == false)
			{
				d.enabled = true;
				draggableIcon.enabled = true;
			}
		}
	}

	public void EndTurn()
	{
		potenceFlag = false;
		hdScript.Potence (false);
		prosperityFlag = false;
		hdScript.Prosperity (false);
		hoardFlag = false;
		hdScript.Hoard (false);
		int cardsNeeded = 5 - playerHand.transform.childCount;

		DrawACard(cardsNeeded);
		endButton.interactable = false;
		ColorBlock cb = actionButton.colors;
		turnColor.a = 1;
		cb.disabledColor = turnColor;
		actionButton.colors = cb;
		numberOfBuys++;

		ColorBlock buyCb = buyPhaseButton.colors;
		turnColor.a = 1;
		buyCb.disabledColor = defaultColor;
		buyPhaseButton.colors = buyCb;

		buyPhaseButton.interactable = true;
		shopButton.interactable = false;
		DraggableOffOrNot (false);
	}

	int CheckFetchesInHand()
	{
		if (playerHand.transform.childCount > 0) 
		{
			for (int k = 0; k < 4; k++) 
			{
				if (playerHand.transform.GetChild (k).name == ("Fetch Quest")) 
				{
					fetchesInHand++;
				}
			}
		}

		return fetchesInHand;
	}

	void DealLastCard()
	{
		if (fetchesInHand < 1) 
		{
			if (playerDeck.transform.childCount > 0) 
			{
				for (int m = 0; m < 5; m++) 
				{
					if (playerDeck.transform.GetChild (m).name == ("Fetch Quest")) 
					{
						CardModel cardModel = playerDeck.transform.GetChild (m).GetComponent<CardModel> ();
						cardModel.ToggleFace (true);
						playerDeck.transform.GetChild (m).SetParent (playerHand.transform);
						break;
					}
				}

				playerHand.transform.GetChild (4).gameObject.AddComponent<Draggable>();
			}
		} 

		else 
		{
			playerHand.Push (playerDeck.Pop ());
		}
	}

	void DealPlayerHand()
	{
		for (int i = 0; i < 4; i++) 
		{
			playerHand.Push (playerDeck.Pop ());
		}
	}

	void DealEnemyHand()
	{
		for (int i = 0; i < 5; i++) 
		{
			enemyHand.Push (enemyDeck.Pop ());
		}
	}

	public void Reshuffle()
	{
		if (discardPile.transform.childCount > 0) 
		{
			for (int i = 0; i < discardPile.transform.childCount; i++) 
			{
				CardModel cardModel = discardPile.transform.GetChild (i).GetComponent<CardModel> ();
				cardModel.ToggleFace (false);
				GameObject discardCopy = discardPile.transform.GetChild (i).gameObject;
				MoveCard (discardCopy, playerDeck.transform);
			}

			playerDeck.transform.SetSiblingIndex(Random.Range(0, playerDeck.transform.childCount));
			UpdateDiscardDropdown ();
		}
	}

	public void MoveCard(GameObject originalCard, Transform destination)
	{
		GameObject deckCard = (GameObject)Instantiate (originalCard);
		deckCard.transform.position = playerDeck.transform.position;
		deckCard.transform.SetParent (destination.transform);
		Destroy (originalCard, 1f);

		if (deckCard.name == "Fetch Quest(Clone)") 
		{
			deckCard.name = "Fetch Quest";
		}

		if (deckCard.name == "Health Vial(Clone)") 
		{
			deckCard.name = "Health Vial";
		}

		if (deckCard.name == "HealthPotion(Clone)") 
		{
			deckCard.name = "HealthPotion";
		}

		if (deckCard.name == "Mana Vial(Clone)") 
		{
			deckCard.name = "Mana Vial";
		}

		if (deckCard.name == "ManaPotion(Clone)") 
		{
			deckCard.name = "ManaPotion";
		}

		if (deckCard.name == "SpecialPotion(Clone)") 
		{
			deckCard.name = "SpecialPotion";
		}

		if (deckCard.name == "JarOfGreed(Clone)") 
		{
			deckCard.name = "JarOfGreed";
		}

		if (deckCard.name == "StolenPouch(Clone)") 
		{
			deckCard.name = "StolenPouch";
		}

		if (deckCard.name == "Lifesteal(Clone)") 
		{
			deckCard.name = "Lifesteal";
		}

		if (deckCard.name == "Manasteal(Clone)") 
		{
			deckCard.name = "Manasteal";
		}

		if (deckCard.name == "Thief(Clone)") 
		{
			deckCard.name = "Thief";
		}

		if (deckCard.name == "TargetBlock(Clone)") 
		{
			deckCard.name = "TargetBlock";
		}

		if (deckCard.name == "PotOfGreed(Clone)") 
		{
			deckCard.name = "PotOfGreed";
		}

		if (deckCard.name == "InnerPower(Clone)") 
		{
			deckCard.name = "InnerPower";
		}

		if (deckCard.name == "WealthIsHealth(Clone)") 
		{
			deckCard.name = "WealthIsHealth";
		}

		if (deckCard.name == "TransmuteLife(Clone)") 
		{
			deckCard.name = "TransmuteLife";
		}

		if (deckCard.name == "ValorOfHercules(Clone)") 
		{
			deckCard.name = "ValorOfHercules";
		}

		if (deckCard.name == "DresdensRobe(Clone)") 
		{
			deckCard.name = "DresdensRobe";
		}

		if (deckCard.name == "VictoryMedal(Clone)") 
		{
			deckCard.name = "VictoryMedal";
		}

		if (destination == playerHand.transform) 
		{
			if (deckCard.GetComponent<Draggable> () == null) 
			{
				deckCard.AddComponent<Draggable> ();
			}
		}
		CanvasGroup cvGroup = deckCard.GetComponent<CanvasGroup> ();
		cvGroup.blocksRaycasts = true;
		RectTransform rectTransform = deckCard.GetComponent<RectTransform> ();
		rectTransform.sizeDelta = new Vector2 (7.64f, 10.48f);		
	}

	public void DrawACard(int number)
	{
		if (playerDeck.transform.childCount > number) 
		{
			for (int i = 0; i < number; i++) 
			{
				CardModel cardModel = playerDeck.transform.GetChild (0).GetComponent<CardModel> ();
				cardModel.ToggleFace (true);
				GameObject drawCard = playerDeck.transform.GetChild (0).gameObject;
				if (drawCard.GetComponent<Draggable> () == null) 
				{
					drawCard.AddComponent<Draggable> ();
				}
				CanvasGroup cvGroup = drawCard.GetComponent<CanvasGroup> ();
				cvGroup.blocksRaycasts = true;
				RectTransform rectTransform = drawCard.GetComponent<RectTransform> ();
				rectTransform.sizeDelta = new Vector2 (7.64f, 10.48f);		
				drawCard.transform.position = playerHand.transform.position;
				drawCard.transform.SetParent (playerHand.transform);
				if (drawCard.GetComponent<Draggable> () != null) 
				{
					Draggable d = drawCard.GetComponent<Draggable> ();
					d.enabled = true;
				}
			}
		}

		else if (playerDeck.transform.childCount <= number) 
		{
			Reshuffle ();

			for (int i = 0; i < number; i++) 
			{
				CardModel cardModel = playerDeck.transform.GetChild (0).GetComponent<CardModel> ();
				cardModel.ToggleFace (true);
				GameObject drawCard = playerDeck.transform.GetChild (0).gameObject;
				if (drawCard.GetComponent<Draggable> () == null) 
				{
					drawCard.AddComponent<Draggable> ();
				}
				CanvasGroup cvGroup = drawCard.GetComponent<CanvasGroup> ();
				cvGroup.blocksRaycasts = true;
				RectTransform rectTransform = drawCard.GetComponent<RectTransform> ();
				rectTransform.sizeDelta = new Vector2 (7.64f, 10.48f);		
				drawCard.transform.position = playerHand.transform.position;
				drawCard.transform.SetParent (playerHand.transform);
				if (drawCard.GetComponent<Draggable> () == null) 
				{
					Draggable d = drawCard.GetComponent<Draggable> ();
					d.enabled = true;
				}
			}
		}
	}
}