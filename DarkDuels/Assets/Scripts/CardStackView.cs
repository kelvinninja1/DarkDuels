using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CardStack))]
public class CardStackView : MonoBehaviour 
{
	CardStack deck;
	Dictionary<int, GameObject> fetchedCards;
	int lastCount;
	public float cardOffset;
	public bool faceUp = false;
	public bool reverserLayerOrder = false;
	public GameObject cardPrefab;
	public Transform newParent;
	GameObject hand;

	void Start()
	{
		hand = GameObject.Find ("PHand");
		fetchedCards = new Dictionary<int, GameObject>();
		deck = GetComponent<CardStack> ();
		ShowCards ();
		lastCount = deck.CardCount;

		deck.CardRemoved += Deck_CardRemoved;
	}

	void Deck_CardRemoved (object sender, CardRemovedEventArgs e)
	{
		if (fetchedCards.ContainsKey (e.CardIndex)) 
		{
			Destroy (fetchedCards [e.CardIndex]);
			fetchedCards.Remove (e.CardIndex);
		}
	}

	void Update()
	{
		if (lastCount != deck.CardCount) 
		{
			lastCount = deck.CardCount;
			ShowCards ();
		}

	}

	void ShowCards()
	{
		int cardCount = 0;
		if (deck.HasCards) 
		{
			foreach (int i in deck.GetCards()) 
			{
				float co = cardOffset * cardCount;
				Vector3 temp = newParent.position + new Vector3 (co, 0f);
				AddCard (temp, i, cardCount);
				cardCount++;
			}
		}
	}

	void AddCard(Vector3 position, int cardIndex, int positionalIndex)
	{
		if (fetchedCards.ContainsKey (cardIndex)) 
		{
			return;
		}
		GameObject cardCopy = (GameObject)Instantiate (cardPrefab);
		cardCopy.transform.position = position;
		cardCopy.transform.SetParent (newParent);

		if (cardCopy.transform.parent == hand.transform) 
		{
			cardCopy.AddComponent<Draggable> ();
		}

		if (cardPrefab.name == "StartingDeck") 
		{
			if (cardIndex == 7 || cardIndex == 8 || cardIndex == 9) 
			{
				cardCopy.name = "Mana Vial";
			}

			if (cardIndex == 4 || cardIndex == 5 || cardIndex == 6) 
			{
				cardCopy.name = "Health Vial";
			}

			if (cardIndex == 0 || cardIndex == 1 || cardIndex == 2 || cardIndex == 3) 
			{
				cardCopy.name = "Fetch Quest";
			}
		}

		if (cardPrefab.name == "CardHeroes") 
		{
			if (cardIndex == 0)
			{
				cardCopy.name = "Akasha";
			}

			if (cardIndex == 1)
			{
				cardCopy.name = "Elwyn";
			}

			if (cardIndex == 2)
			{
				cardCopy.name = "Isador";
			}

			if (cardIndex == 3)
			{
				cardCopy.name = "Lina";
			}

			if (cardIndex == 4)
			{
				cardCopy.name = "Madalyn";
			}

			if (cardIndex == 5)
			{
				cardCopy.name = "Sven";
			}

			if (cardIndex == 6)
			{
				cardCopy.name = "Thaeton";
			}

			if (cardIndex == 7)
			{
				cardCopy.name = "Triss";
			}

			if (cardIndex == 8)
			{
				cardCopy.name = "Zane";
			}
		}

		if (cardPrefab.name == "Action") 
		{
			if (cardIndex == 0)
			{
				cardCopy.name = "HealthPotion";
				cardCopy.transform.GetChild (0).name = "HPOwned";
			}

			if (cardIndex == 1)
			{
				cardCopy.name = "InnerPower";
				cardCopy.transform.GetChild (0).name = "IPPOwned";
			}

			if (cardIndex == 2)
			{
				cardCopy.name = "JarOfGreed";
				cardCopy.transform.GetChild (0).name = "JOGOwned";
			}

			if (cardIndex == 3)
			{
				cardCopy.name = "ManaPotion";
				cardCopy.transform.GetChild (0).name = "MPOwned";
			}

			if (cardIndex == 4)
			{
				cardCopy.name = "PotOfGreed";
				cardCopy.transform.GetChild (0).name = "POGOwned";
			}

			if (cardIndex == 5)
			{
				cardCopy.name = "SpecialPotion";
				cardCopy.transform.GetChild (0).name = "SPOwned";
			}

			if (cardIndex == 6)
			{
				cardCopy.name = "StolenPouch";
				cardCopy.transform.GetChild (0).name = "SPouchOwned";
			}

			if (cardIndex == 7)
			{
				cardCopy.name = "TransmuteLife";
				cardCopy.transform.GetChild (0).name = "TPOwned";
			}

			if (cardIndex == 8)
			{
				cardCopy.name = "WealthIsHealth";
				cardCopy.transform.GetChild (0).name = "WIHPOwned";
			}
		}

		if (cardPrefab.name == "CombatBonus") 
		{
			if (cardIndex == 0)
			{
				cardCopy.name = "ValorOfHercules";
				cardCopy.transform.GetChild (0).name = "VHOwned";
			}

			if (cardIndex == 1)
			{
				cardCopy.name = "DresdensRobe";
				cardCopy.transform.GetChild (0).name = "DROwned";
			}

			if (cardIndex == 2)
			{
				cardCopy.name = "Lifesteal";
				cardCopy.transform.GetChild (0).name = "LOwned";
			}

			if (cardIndex == 3)
			{
				cardCopy.name = "Manasteal";
				cardCopy.transform.GetChild (0).name = "MOwned";
			}

			if (cardIndex == 4)
			{
				cardCopy.name = "TargetBlock";
				cardCopy.transform.GetChild (0).name = "TBOwned";
			}

			if (cardIndex == 5)
			{
				cardCopy.name = "Thief";
				cardCopy.transform.GetChild (0).name = "TOwned";
			}
		}



		CardModel cardModel = cardCopy.GetComponent<CardModel> ();
		cardModel.cardIndex = cardIndex;
		cardModel.ToggleFace (faceUp);
		SpriteRenderer spriteRenderer = cardCopy.GetComponent<SpriteRenderer> ();
		if (reverserLayerOrder) 
		{
			spriteRenderer.sortingOrder = lastCount - positionalIndex;
		} 
		else 
		{
			spriteRenderer.sortingOrder = positionalIndex;
		}
		fetchedCards.Add (cardIndex, cardCopy);
	}
}
