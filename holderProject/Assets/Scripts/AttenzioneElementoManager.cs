using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttenzioneElementoManager : MonoBehaviour
{
	private static AttenzioneElementoManager instance;
	public static AttenzioneElementoManager Instance { get { return instance; } }
	[SerializeField]
	GameObject ElementToTouch;
	[SerializeField]
	GameObject GridLayout;
	[SerializeField]
	GameObject ElementoPrefab;
	[SerializeField]
	GameObject endgame;
	List<Color> colors;
	List<string> imageNames;
	string imageNameToTouch;
	Color colorOfImageToTouch;
	public static int GridSizeX = 4;
	public static int GridSizeY = 4;
	public static int nSchede = 50;
	public static int occurrences;
	int correctAnswersRegistered;
	int answersRegistered;

	// Start is called before the first frame update
	void Start()
	{
		Application.targetFrameRate = 60;
		imageNames = new List<string>();
		colors = new List<Color>();
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
		ElementToTouch.GetComponent<UnityEngine.UI.Image>().sprite = Resources.Load<Sprite>("testImages/" + imageNameToTouch);
		ElementToTouch.GetComponent<UnityEngine.UI.Image>().color = colorOfImageToTouch;
		occurrences = Random.Range(1,(GridSizeX*GridSizeY)/2);
		correctAnswersRegistered = 0;
		answersRegistered = 0;
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
		int i = 0;
		int correctAnswersGenerated = 0;
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
		while (i < GridSizeX * GridSizeY)
		{
			int rnd = Random.Range(0, 2);
			var tmp = Instantiate(ElementoPrefab, GridLayout.transform);
			if (rnd == 0 && correctAnswersGenerated != occurrences)
			{
				tmp.transform.GetChild(0).GetComponent<UnityEngine.UI.Image>().sprite = Resources.Load<Sprite>("testImages/" + imageNameToTouch);
				tmp.transform.GetChild(0).GetComponent<UnityEngine.UI.Image>().color = colorOfImageToTouch;
				tmp.GetComponent<ElementoPrefabScript>().correctAnswer = true;
				correctAnswersGenerated++;
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
				tmp.transform.GetChild(0).GetComponent<UnityEngine.UI.Image>().sprite = Resources.Load<Sprite>("testImages/" + imageNames[rndImg]);
				tmp.transform.GetChild(0).GetComponent<UnityEngine.UI.Image>().color = colors[rndColor];
			}
			i++;
		}
	}
	public void GetClick(bool correct)
	{
		if (correct)
			correctAnswersRegistered++;
		answersRegistered++;
		print(correctAnswersRegistered + " di " + answersRegistered + " risposte sono corrette");
		if (correctAnswersRegistered == occurrences){
			if (nSchede>0){
			newScheda();
		} else {
			nSchede = 5;
			endgame.GetComponent<EndGame>().startEndGame();
			print("FINITA SESSIONE");
		}
		}
	}

	void newScheda(){
		ClearEverything();

		int rndImg = Random.Range(0, imageNames.Count);
		int rndColor = Random.Range(0, colors.Count);
		imageNameToTouch = imageNames[rndImg];
		colorOfImageToTouch = colors[rndColor];
		ElementToTouch.GetComponent<UnityEngine.UI.Image>().sprite = Resources.Load<Sprite>("testImages/" + imageNameToTouch);
		ElementToTouch.GetComponent<UnityEngine.UI.Image>().color = colorOfImageToTouch;
		occurrences = Random.Range(1,(GridSizeX*GridSizeY)/2);
		correctAnswersRegistered = 0;
		SetupGrid();
		nSchede -= 1;
	}

	void ClearEverything(){
		for (var i = GridLayout.transform.childCount - 1; i >= 0; i--){
  			Object.Destroy(GridLayout.transform.GetChild(i).gameObject);
		}
	}
}
