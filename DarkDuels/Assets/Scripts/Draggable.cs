using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.EventSystems;

public class Draggable : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{	
	public Transform parentToReturnTo = null;
	public Transform placeholderParent = null;
	public Vector2 dragOffset = new Vector2(0f, 0f); 
	GameObject placeholder = null;
	GameObject playerHeroes;
	Image heroTint;

	void Awake()
	{
		playerHeroes = GameObject.Find ("PHeroes");
	}

	public void OnBeginDrag(PointerEventData eventData)
	{
		dragOffset = eventData.position - (Vector2)this.transform.position;
		placeholder = new GameObject();
		placeholder.transform.SetParent( this.transform.parent );
		LayoutElement le = placeholder.AddComponent<LayoutElement>();
		le.preferredWidth = this.GetComponent<LayoutElement>().preferredWidth;
		le.preferredHeight = this.GetComponent<LayoutElement>().preferredHeight;
		le.flexibleWidth = 0;
		le.flexibleHeight = 0;

		placeholder.transform.SetSiblingIndex( this.transform.GetSiblingIndex() );

		parentToReturnTo = this.transform.parent;
		placeholderParent = parentToReturnTo;
		this.transform.SetParent( this.transform.parent.parent );

		GetComponent<CanvasGroup>().blocksRaycasts = false;

		if (this.gameObject.name == "Health Vial" || this.gameObject.name == "HealthPotion") 
		{
			for (int i = 0; i < playerHeroes.transform.childCount; i++) 
			{
				heroTint = playerHeroes.transform.GetChild (i).GetComponentInChildren<Image> ();
				HeroDrop hdScript = playerHeroes.transform.GetChild (i).GetComponent<HeroDrop> ();
				if (hdScript.currentHealth < hdScript.totalHealth)
					heroTint.color = new Color (0, 1, 0, 0.3f);
			}
		}

		if (this.gameObject.name == "Mana Vial" || this.gameObject.name == "ManaPotion") 
		{
			for (int i = 0; i < playerHeroes.transform.childCount; i++) 
			{
				heroTint = playerHeroes.transform.GetChild (i).GetComponentInChildren<Image> ();
				HeroDrop hdScript = playerHeroes.transform.GetChild (i).GetComponent<HeroDrop> ();
				if (hdScript.currentMana < hdScript.totalMana)
					heroTint.color = new Color (0, 1, 0, 0.3f);
			}
		}

		if (this.gameObject.name == "Fetch Quest" || this.gameObject.name == "PotOfGreed") 
		{
			for (int i = 0; i < playerHeroes.transform.childCount; i++) 
			{
				heroTint = playerHeroes.transform.GetChild (i).GetComponentInChildren<Image> ();
				HeroDrop hdScript = playerHeroes.transform.GetChild (i).GetComponent<HeroDrop> ();
				if(hdScript.currentMana > 0)
					heroTint.color = new Color (0, 1, 0, 0.3f);
			}
		}

		if (this.gameObject.name == "SpecialPotion") 
		{
			for (int i = 0; i < playerHeroes.transform.childCount; i++) 
			{
				heroTint = playerHeroes.transform.GetChild (i).GetComponentInChildren<Image> ();
				HeroDrop hdScript = playerHeroes.transform.GetChild (i).GetComponent<HeroDrop> ();
				if (hdScript.currentMana < hdScript.totalMana || hdScript.currentHealth < hdScript.totalHealth)
					heroTint.color = new Color (0, 1, 0, 0.3f);
			}
		}

		if (this.gameObject.name == "JarOfGreed" || this.gameObject.name == "StolenPouch") 
		{
			for (int i = 0; i < playerHeroes.transform.childCount; i++) 
			{
				heroTint = playerHeroes.transform.GetChild (i).GetComponentInChildren<Image> ();
				HeroDrop hdScript = playerHeroes.transform.GetChild (i).GetComponent<HeroDrop> ();
				if(hdScript.currentMana > 1)
					heroTint.color = new Color (0, 1, 0, 0.3f);
			}
		}

		if (this.gameObject.name == "Lifesteal" || this.gameObject.name == "Manasteal" || this.gameObject.name == "Thief" || this.gameObject.name == "TargetBlock" || this.gameObject.name == "ValorOfHercules" || this.gameObject.name == "DresdensRobe") 
		{
			for (int i = 0; i < playerHeroes.transform.childCount; i++) 
			{
				heroTint = playerHeroes.transform.GetChild (i).GetComponentInChildren<Image> ();
				HeroDrop hdScript = playerHeroes.transform.GetChild (i).GetComponent<HeroDrop> ();
				if (hdScript.playZone.childCount < 2)
					heroTint.color = new Color (0, 1, 0, 0.3f);
			}
		}

		if (this.gameObject.name == "InnerPower" || this.gameObject.name == "TransmuteLife") 
		{
			for (int i = 0; i < playerHeroes.transform.childCount; i++) 
			{
				heroTint = playerHeroes.transform.GetChild (i).GetComponentInChildren<Image> ();
				HeroDrop hdScript = playerHeroes.transform.GetChild (i).GetComponent<HeroDrop> ();
				if(hdScript.currentHealth > 2)
					heroTint.color = new Color (0, 1, 0, 0.3f);
			}
		}

		if (this.gameObject.name == "WealthIsHealth") 
		{
			for (int i = 0; i < playerHeroes.transform.childCount; i++) 
			{
				heroTint = playerHeroes.transform.GetChild (i).GetComponentInChildren<Image> ();
				HeroDrop hdScript = playerHeroes.transform.GetChild (i).GetComponent<HeroDrop> ();
				if(hdScript.currentMana > 2)
					heroTint.color = new Color (0, 1, 0, 0.3f);
			}
		}

		if (this.gameObject.name == "VictoryMedal") 
		{
			for (int i = 0; i < playerHeroes.transform.childCount; i++) 
			{
				heroTint = playerHeroes.transform.GetChild (i).GetComponentInChildren<Image> ();
				HeroDrop hdScript = playerHeroes.transform.GetChild (i).GetComponent<HeroDrop> ();
				if(hdScript.currentMana > 4)
					heroTint.color = new Color (0, 1, 0, 0.3f);
			}
		}

	}

	public void OnDrag(PointerEventData eventData)
	{
		this.transform.position = eventData.position - dragOffset;

		if(placeholder.transform.parent != placeholderParent)
			placeholder.transform.SetParent(placeholderParent);

		int newSiblingIndex = placeholderParent.childCount;

		for(int i=0; i < placeholderParent.childCount; i++) {
			if(this.transform.position.x < placeholderParent.GetChild(i).position.x) {

				newSiblingIndex = i;

				if(placeholder.transform.GetSiblingIndex() < newSiblingIndex)
					newSiblingIndex--;

				break;
			}
		}

		placeholder.transform.SetSiblingIndex(newSiblingIndex);

	}

	public void OnEndDrag(PointerEventData eventData)
	{
		for (int i = 0; i < playerHeroes.transform.childCount; i++) 
		{
			heroTint = playerHeroes.transform.GetChild (i).GetComponentInChildren<Image> ();
			heroTint.color = new Color (0, 0, 0, 0);
		}
		this.transform.SetParent( parentToReturnTo );
		this.transform.SetSiblingIndex( placeholder.transform.GetSiblingIndex() );
		GetComponent<CanvasGroup>().blocksRaycasts = true;
		Destroy(placeholder);
	}
}
