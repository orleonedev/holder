using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DoveVistoManager : MonoBehaviour
{
	private static DoveVistoManager instance;
	public static DoveVistoManager Instance { get { return instance; } }
	[SerializeField]
	GameObject ElementToTouch;
	[SerializeField]
	GameObject GridLayout;
	[SerializeField]
	GameObject DoveVistoPrefab;
	[SerializeField]
	GameObject WaitCover;
	[SerializeField]
	GameObject Label;
	[SerializeField]
	GameObject DoveVistoSolButtonPrefab;
	[SerializeField]
	GameObject ButtonHLayout;
	List<Color> colors;
	List<string> imageNames;
	string imageNameToTouch;
	Color colorOfImageToTouch;
	int GridSizeX;
	int GridSizeY;
	int amountOfCorrectAnswersToGenerate;
	List<GameObject> gridObj;
	List<int> correctPositions;
	int secondsToWait;
	bool firstRun;
	Sprite selectedImage;
	Color selectedColor;
	List<GameObject> buttonObj;
	int imagesPlaced;

	public static int nSchede = 30;

	[SerializeField]
	GameObject endgame;

	// Start is called before the first frame update
	void Start()
	{
		Application.targetFrameRate = 60;
		imageNames = new List<string>();
		colors = new List<Color>();
		gridObj = new List<GameObject>();
		correctPositions = new List<int>();
		buttonObj = new List<GameObject>();
		selectedImage = null;
		colors.Add(Color.black);
		colors.Add(Color.green);
		colors.Add(Color.red);
		colors.Add(Color.blue);
		colors.Add(Color.magenta);
		colors.Add(Color.yellow);
		imageNames.Add("Circle");
		imageNames.Add("Cube");
		int rndImg = Random.Range(0, imageNames.Count);
		int rndColor = Random.Range(0, colors.Count);
		imageNameToTouch = imageNames[rndImg];
		colorOfImageToTouch = colors[rndColor];
		GridSizeX = 3;
		GridSizeY = 3;
		amountOfCorrectAnswersToGenerate = 3;
		secondsToWait = 3;
		firstRun = true;
		imagesPlaced = 0;
		SetupGrid();
		nSchede -= 1;
	}

	// Update is called once per frame
	void Update()
	{

	}

	private void Awake()
	{
		instance = this;
	}

	void SetupGrid()
	{
		if (GridSizeX <= GridSizeY)
		{
			GridLayout.GetComponent<UnityEngine.UI.GridLayoutGroup>().constraint = UnityEngine.UI.GridLayoutGroup.Constraint.FixedColumnCount;
			GridLayout.GetComponent<UnityEngine.UI.GridLayoutGroup>().constraintCount = GridSizeX;
		}
		else
		{
			GridLayout.GetComponent<UnityEngine.UI.GridLayoutGroup>().constraint = UnityEngine.UI.GridLayoutGroup.Constraint.FixedRowCount;
			GridLayout.GetComponent<UnityEngine.UI.GridLayoutGroup>().constraintCount = GridSizeY;
		}
		for (int i = 0; i < GridSizeX * GridSizeY; i++)
		{
			var tmp = Instantiate(DoveVistoPrefab, GridLayout.transform);
			tmp.GetComponent<UnityEngine.UI.Button>().enabled = false;
			tmp.GetComponent<DoveVistoPrefabScript>().index = i;
			gridObj.Add(tmp);
		}
		for (int i = 0; i < amountOfCorrectAnswersToGenerate; i++)
		{
			int rnd;
			do
			{
				rnd = Random.Range(0, GridSizeX * GridSizeY);
			} while (correctPositions.Contains(rnd));
			correctPositions.Add(rnd);
			gridObj[rnd].transform.GetChild(0).GetComponentInChildren<UnityEngine.UI.Image>().sprite = Resources.Load<Sprite>("testImages/" + imageNameToTouch);
			gridObj[rnd].transform.GetChild(0).GetComponentInChildren<UnityEngine.UI.Image>().color = colorOfImageToTouch;
		}
		StartCoroutine(WaitForSeconds(secondsToWait, false));
	}
	IEnumerator WaitForSeconds(int secondsRemaining, bool shouldShowCover)
	{
		if (shouldShowCover)
		{
			WaitCover.SetActive(true);
		}
		secondsRemaining--;
		WaitCover.GetComponentInChildren<TMP_Text>().text = "Aspetta " + secondsRemaining + " secondi";
		yield return new WaitForSeconds(1);
		if (secondsRemaining > 0)
		{
			StartCoroutine(WaitForSeconds(secondsRemaining, false));

		}
		else
		{
			if (firstRun)
			{
				firstRun = false;
				StartCoroutine(WaitForSeconds(secondsToWait, true));
			}
			else
			{
				for (int i = 0; i < gridObj.Count; i++)
				{
					gridObj[i].GetComponent<UnityEngine.UI.Button>().enabled = true;
					if (correctPositions.Contains(i))
					{
						gridObj[i].transform.GetChild(0).GetComponentInChildren<UnityEngine.UI.Image>().sprite = null;
					}
				}
				Label.GetComponentInChildren<TMP_Text>().text = "Riposiziona gli elementi come prima";
				Label.GetComponent<RectTransform>().sizeDelta = new Vector2(1500f, 80.5f);
				SetupButtons();
				WaitCover.SetActive(false);
			}
		}
	}
	void SetupButtons()
	{
		int rnd = Random.Range(1, 4);
		bool alreadyInserted = false;
		for (int i = 0; i < rnd || !alreadyInserted; i++)
		{
			int tmpRnd = Random.Range(0, 2);
			var tmp = Instantiate(DoveVistoSolButtonPrefab, ButtonHLayout.transform);
			if (i + 1 >= rnd)
				tmpRnd = 0;
			if (tmpRnd == 0 && !alreadyInserted)
			{
				tmp.transform.GetChild(0).GetComponentInChildren<UnityEngine.UI.Image>().sprite = Resources.Load<Sprite>("testImages/" + imageNameToTouch);
				tmp.transform.GetChild(0).GetComponentInChildren<UnityEngine.UI.Image>().color = colorOfImageToTouch;
				alreadyInserted = true;
			}
			else
			{
				int rndImg;
				int rndColor;
				do
				{
					rndImg = Random.Range(0, imageNames.Count);
					rndColor = Random.Range(0, colors.Count);
				} while (imageNames[rndImg] == imageNameToTouch && colors[rndColor] == colorOfImageToTouch);
				tmp.transform.GetChild(0).GetComponentInChildren<UnityEngine.UI.Image>().sprite = Resources.Load<Sprite>("testImages/" + imageNames[rndImg]);
				tmp.transform.GetChild(0).GetComponentInChildren<UnityEngine.UI.Image>().color = colors[rndColor];
			}
			tmp.GetComponent<DoveVistoSolPrefabScript>().selectedImage = tmp.transform.GetChild(0).GetComponentInChildren<UnityEngine.UI.Image>().sprite;
			tmp.GetComponent<DoveVistoSolPrefabScript>().selectedColor = tmp.transform.GetChild(0).GetComponentInChildren<UnityEngine.UI.Image>().color;
			buttonObj.Add(tmp);
		}
	}
	public void SelectImageToPlace(Sprite selectedImage, Color selectedColor)
	{
		this.selectedImage = selectedImage;
		this.selectedColor = selectedColor;
	}
	public void GetClick(int indexInGrid)
	{
		if (selectedImage != null)
		{
			if (gridObj[indexInGrid].transform.GetChild(0).GetComponentInChildren<UnityEngine.UI.Image>().sprite == null)
			{
				gridObj[indexInGrid].transform.GetChild(0).GetComponentInChildren<UnityEngine.UI.Image>().sprite = selectedImage;
				gridObj[indexInGrid].transform.GetChild(0).GetComponentInChildren<UnityEngine.UI.Image>().color = selectedColor;
				imagesPlaced++;
			}
			else
			{
				gridObj[indexInGrid].transform.GetChild(0).GetComponentInChildren<UnityEngine.UI.Image>().sprite = null;
				imagesPlaced--;
			}
			if (imagesPlaced == amountOfCorrectAnswersToGenerate)
				CheckResults();
		}
	}
	void CheckResults()
	{
		bool correct = true;
		for (int i = 0; i < correctPositions.Count; i++)
		{
			if (gridObj[correctPositions[i]].transform.GetChild(0).GetComponentInChildren<UnityEngine.UI.Image>().sprite == null)
				correct = false;
				
			else if (gridObj[correctPositions[i]].transform.GetChild(0).GetComponentInChildren<UnityEngine.UI.Image>().sprite.name != imageNameToTouch || gridObj[correctPositions[i]].transform.GetChild(0).GetComponentInChildren<UnityEngine.UI.Image>().color != colorOfImageToTouch)
				correct = false;
		}
		print("Il risultato è " + correct);
		if(nSchede > 0){
			newScheda();
		}
		else {
			nSchede = 4;
			endgame.GetComponent<EndGame>().startEndGame();
			print("FINITA SESSIONE");
		}
	}

	public void newScheda(){
		ClearEverything();

		imageNames = new List<string>();
		colors = new List<Color>();
		gridObj = new List<GameObject>();
		correctPositions = new List<int>();
		buttonObj = new List<GameObject>();
		selectedImage = null;
		colors.Add(Color.black);
		colors.Add(Color.green);
		colors.Add(Color.red);
		colors.Add(Color.blue);
		colors.Add(Color.magenta);
		colors.Add(Color.yellow);
		imageNames.Add("Circle");
		imageNames.Add("Cube");
		int rndImg = Random.Range(0, imageNames.Count);
		int rndColor = Random.Range(0, colors.Count);
		imageNameToTouch = imageNames[rndImg];
		colorOfImageToTouch = colors[rndColor];
		GridSizeX = 3;
		GridSizeY = 3;
		amountOfCorrectAnswersToGenerate = 3;
		secondsToWait = 3;
		firstRun = true;
		imagesPlaced = 0;
		SetupGrid();
		nSchede -= 1;
	}

	void ClearEverything(){
		Label.GetComponentInChildren<TMP_Text>().text = "Guarda attentamente";
		
		for (var i = GridLayout.transform.childCount - 1; i >= 0; i--){
  			Object.Destroy(GridLayout.transform.GetChild(i).gameObject);
		}
		for (var j = ButtonHLayout.transform.childCount - 1; j >= 0; j--){
  			Object.Destroy(ButtonHLayout.transform.GetChild(j).gameObject);
		}
	}
}
