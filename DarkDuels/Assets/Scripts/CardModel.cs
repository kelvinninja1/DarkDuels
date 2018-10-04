using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class CardModel : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
	GameObject zoomSpace;
	GameZoom gameZoom;
	Sprite zoomSprite;
	Sprite originalSprite;
	GameController gcScript;
	GameObject gameController;
	GameObject playerHand;
	GameObject playerHeroes;
	RectTransform thisRect;

	SpriteRenderer spriteRenderer;
	public Sprite[] faces;
	public Sprite cardBack;
	private Draggable undrag;

	public int cardIndex;

	public void ToggleFace(bool showFace)
	{
		if (showFace) 
		{
			spriteRenderer.sprite = faces [cardIndex];
		} 

		else 
		
		{
			spriteRenderer.sprite = cardBack;
			Destroy (undrag);
		}
	}

	void Awake ()
	{
		spriteRenderer = GetComponent<SpriteRenderer> ();
		undrag = this.gameObject.GetComponent<Draggable>();
		gameController = GameObject.Find ("GameController");
		zoomSpace = GameObject.Find ("ZoomSpace");
		playerHand = GameObject.Find ("PHand");
		playerHeroes = GameObject.Find ("PHeroes");
	}

	void Start()
	{
		zoomSprite = zoomSpace.GetComponent<SpriteRenderer> ().sprite;
		gameZoom = playerHeroes.GetComponentInChildren<GameZoom> ();
		gcScript = gameController.GetComponent<GameController> ();
		thisRect = this.GetComponent<RectTransform> ();
	}

	void OnMouseDown()
	{
		if (this.transform.parent.name == "PDiscardPile" && gcScript.dropDownObject.activeSelf == false) 
		{
			gcScript.dropDownObject.SetActive (true);
			gcScript.UpdateDiscardDropdown ();
			gcScript.DraggableOffOrNot (true);
		}

		else if (this.transform.parent.name == "PDiscardPile" && gcScript.dropDownObject.activeSelf == true) 
		{
			if (gcScript.dropDownObject.transform.childCount == 4) 
			{
				GameObject dropdownList = gcScript.dropDownObject.transform.GetChild (3).gameObject;
				Destroy (dropdownList);	
			}
			gcScript.dropDownObject.SetActive (false);
			gcScript.UpdateDiscardDropdown ();
			gcScript.DraggableOffOrNot(false);
		}

	}

	void Update ()
	{
		if (this.transform.parent.name == "PDiscardPile" && thisRect.transform.localPosition.z != 0)
			thisRect.transform.localPosition = new Vector3(this.transform.localPosition.x,this.transform.localPosition.y,0);
	}

	public void OnPointerEnter(PointerEventData eventData)
	{ 
		if (gcScript.handZoom == true) {
			originalSprite = this.gameObject.GetComponent<SpriteRenderer> ().sprite;
			if (this.gameObject.transform.parent == playerHand.transform) {
				gcScript.EffectsOff ();
				gameZoom.TreesOff ();
				zoomSprite = originalSprite;
				zoomSpace.GetComponent<SpriteRenderer> ().sprite = zoomSprite;
				zoomSpace.transform.localScale = new Vector3 (60, 60, 60);

				if (this.gameObject.name == "Fetch Quest") {
					gcScript.fqEffect.SetActive (true);
				}

				if (this.gameObject.name == "Health Vial") {
					gcScript.hvEffect.SetActive (true);
				}

				if (this.gameObject.name == "Mana Vial") {
					gcScript.mvEffect.SetActive (true);
				}

				if (this.gameObject.name == "HealthPotion") {
					gcScript.hpEffect.SetActive (true);
				}

				if (this.gameObject.name == "ManaPotion") {
					gcScript.mpEffect.SetActive (true);
				}

				if (this.gameObject.name == "SpecialPotion") {
					gcScript.mvEffect.SetActive (true);
					gcScript.hvEffect.SetActive (true);
				}

				if (this.gameObject.name == "JarOfGreed") {
					gcScript.fqEffect.SetActive (true);
					gcScript.drawEffect.SetActive (true);
				}

				if (this.gameObject.name == "StolenPouch") {
					gcScript.stolenpEffect.SetActive (true);
				}

				if (this.gameObject.name == "PotOfGreed") {
					gcScript.drawEffect.SetActive (true);
				}

				if (this.gameObject.name == "InnerPower") {
					gcScript.mvEffect.SetActive (true);
					gcScript.drawEffect.SetActive (true);
				}

				if (this.gameObject.name == "WealthIsHealth") {
					gcScript.hvEffect.SetActive (true);
					gcScript.fqEffect.SetActive (true);
				}
				if (this.gameObject.name == "TransmuteLife") {
					gcScript.mvEffect.SetActive (true);
					gcScript.fqEffect.SetActive (true);
				}
			}
		}
	}

	public void OnPointerExit(PointerEventData eventData)
	{

	}
}
