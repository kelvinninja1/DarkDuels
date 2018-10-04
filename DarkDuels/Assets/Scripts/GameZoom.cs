using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class GameZoom : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler 
{
	GameObject zoomSpace;
	Sprite zoomSprite;
	Sprite originalSprite;

	GameObject playerHeroes;
	RectTransform rectTransform;
	GameController gcScript;
	GameObject gameController;

	[HideInInspector]
	public GameObject akashaTree;
	public GameObject elwynTree;
	public GameObject madalynTree;
	public GameObject thaetonTree;
	public GameObject svenTree;
	public GameObject trissTree;
	public GameObject linaTree;
	public GameObject zaneTree;
	public GameObject isadorTree;

	GameObject aLevelOne;
	GameObject eLevelOne;
	GameObject mLevelOne;
	GameObject tHLevelOne;
	GameObject sLevelOne;
	GameObject tRLevelOne;
	GameObject lLevelOne;
	GameObject zLevelOne;
	GameObject iLevelOne;

	Text aLevelOneText;
	Text eLevelOneText;
	Text mLevelOneText;
	Text tHLevelOneText;
	Text sLevelOneText;
	Text tRLevelOneText;
	Text lLevelOneText;
	Text zLevelOneText;
	Text iLevelOneText;

	void Awake()
	{
		akashaTree = GameObject.Find ("AkashaTree");
		elwynTree = GameObject.Find ("ElwynTree");
		madalynTree = GameObject.Find ("MadalynTree");
		thaetonTree = GameObject.Find ("ThaetonTree");
		svenTree = GameObject.Find ("SvenTree");
		trissTree = GameObject.Find ("TrissTree");
		linaTree = GameObject.Find ("LinaTree");
		zaneTree = GameObject.Find ("ZaneTree");
		isadorTree = GameObject.Find ("IsadorTree");

		aLevelOne = GameObject.Find ("ALevelOne");
		eLevelOne = GameObject.Find ("ELevelOne");
		mLevelOne = GameObject.Find ("MLevelOne");
		tHLevelOne = GameObject.Find ("ThLevelOne");
		sLevelOne = GameObject.Find ("SLevelOne");
		tRLevelOne = GameObject.Find ("TrLevelOne");
		lLevelOne = GameObject.Find ("LLevelOne");
		zLevelOne = GameObject.Find ("ZLevelOne");
		iLevelOne = GameObject.Find ("ILevelOne");

		playerHeroes = GameObject.Find ("PHeroes");
		gameController = GameObject.Find ("GameController");
	}

	void Start()
	{
		zoomSpace = GameObject.Find ("ZoomSpace");
		zoomSprite = zoomSpace.GetComponent<SpriteRenderer> ().sprite;
		originalSprite = this.gameObject.GetComponent<SpriteRenderer> ().sprite;
		gcScript = gameController.GetComponent<GameController> ();

		gcScript.EffectsOff ();
		TreesOff ();

		aLevelOneText = aLevelOne.GetComponent<Text> ();
		eLevelOneText = eLevelOne.GetComponent<Text> ();
		mLevelOneText = mLevelOne.GetComponent<Text> ();
		tHLevelOneText = tHLevelOne.GetComponent<Text> ();
		sLevelOneText = sLevelOne.GetComponent<Text> ();
		tRLevelOneText = tRLevelOne.GetComponent<Text> ();
		lLevelOneText = lLevelOne.GetComponent<Text> ();
		zLevelOneText = zLevelOne.GetComponent<Text> ();
		iLevelOneText = iLevelOne.GetComponent<Text> ();

		aLevelOneText.color = Color.green;
		eLevelOneText.color = Color.green;
		mLevelOneText.color = Color.green;
		tHLevelOneText.color = Color.green;
		sLevelOneText.color = Color.green;
		tRLevelOneText.color = Color.green;
		lLevelOneText.color = Color.green;
		zLevelOneText.color = Color.green;
		iLevelOneText.color = Color.green;

		if (this.gameObject.name == "Akasha") 
		{
			akashaTree.transform.SetParent (this.gameObject.transform);
			rectTransform = akashaTree.GetComponent<RectTransform> ();
		}

		if (this.gameObject.name == "Elwyn") 
		{
			elwynTree.transform.SetParent (this.gameObject.transform);
			rectTransform = elwynTree.GetComponent<RectTransform> ();
		}

		if (this.gameObject.name == "Isador") 
		{
			isadorTree.transform.SetParent (this.gameObject.transform);
			rectTransform = isadorTree.GetComponent<RectTransform> ();
		}

		if (this.gameObject.name == "Triss") 
		{
			trissTree.transform.SetParent (this.gameObject.transform);
			rectTransform = trissTree.GetComponent<RectTransform> ();
		}

		if (this.gameObject.name == "Thaeton") 
		{
			thaetonTree.transform.SetParent (this.gameObject.transform);
			rectTransform = thaetonTree.GetComponent<RectTransform> ();
		}

		if (this.gameObject.name == "Lina") 
		{
			linaTree.transform.SetParent (this.gameObject.transform);
			rectTransform = linaTree.GetComponent<RectTransform> ();
		}

		if (this.gameObject.name == "Madalyn") 
		{
			madalynTree.transform.SetParent (this.gameObject.transform);
			rectTransform = madalynTree.GetComponent<RectTransform> ();
		}

		if (this.gameObject.name == "Zane") 
		{
			zaneTree.transform.SetParent (this.gameObject.transform);
			rectTransform = zaneTree.GetComponent<RectTransform> ();
		}

		if (this.gameObject.name == "Sven") 
		{
			svenTree.transform.SetParent (this.gameObject.transform);
			rectTransform = svenTree.GetComponent<RectTransform> ();
		}

		if (this.gameObject == playerHeroes.transform.GetChild (0).gameObject) 
		{
			rectTransform.offsetMin = new Vector2 (-339f, -229.5f); // (left, bottom)
			rectTransform.offsetMax = new Vector2 (282f, 247.5f); // (-right, -top)
		}

		if (this.gameObject == playerHeroes.transform.GetChild (2).gameObject) 
		{
			rectTransform.offsetMin = new Vector2 (-374f, -229.5f);
			rectTransform.offsetMax = new Vector2 (247f, 247.5f);
		}
	}

	public void OnPointerEnter(PointerEventData eventData)
	{
		zoomSprite = originalSprite;
		zoomSpace.GetComponent<SpriteRenderer> ().sprite = zoomSprite;
		zoomSpace.transform.localScale = new Vector3 (60, 60, 60);

		gcScript.EffectsOff ();
		TreesOff ();

		if (this.gameObject.name == "Akasha") 
		{
			akashaTree.SetActive (true);
		}

		if (this.gameObject.name == "Elwyn") 
		{
			elwynTree.SetActive (true);
		}

		if (this.gameObject.name == "Madalyn") 
		{
			madalynTree.SetActive (true);
		}

		if (this.gameObject.name == "Thaeton") 
		{
			thaetonTree.SetActive (true);
		}

		if (this.gameObject.name == "Sven") 
		{
			svenTree.SetActive (true);
		}

		if (this.gameObject.name == "Triss") 
		{
			trissTree.SetActive (true);
		}

		if (this.gameObject.name == "Zane") 
		{
			zaneTree.SetActive (true);
		}

		if (this.gameObject.name == "Lina") 
		{
			linaTree.SetActive (true);
		}

		if (this.gameObject.name == "Isador") 
		{
			isadorTree.SetActive (true);
		}
	}

	public void TreesOff()
	{
		elwynTree.SetActive (false);
		madalynTree.SetActive (false);
		thaetonTree.SetActive (false);
		svenTree.SetActive (false);
		trissTree.SetActive (false);
		linaTree.SetActive (false);
		zaneTree.SetActive (false);
		isadorTree.SetActive (false);
		akashaTree.SetActive (false);
	}

	public void OnPointerExit(PointerEventData eventData)
	{
		
	}
}
