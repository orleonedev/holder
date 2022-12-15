using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using TMPro;
using System.Text.RegularExpressions;
using System.Linq;

public class GiaVistoManager : MonoBehaviour
{
	private static GiaVistoManager instance;
	public static GiaVistoManager Instance { get { return instance; } }
	[SerializeField]
	GameObject YesButton;
	[SerializeField]
	GameObject NoButton;
	[SerializeField]
	GameObject ImageToRecognize;
	List<string> imageNames;
	[SerializeField]
	GameObject WaitCover;
	[SerializeField]
	GameObject endgame;
	string currentImageName;
	Color currentColor;
	List<Color> colors;
	bool firstRun;
	UnityEngine.Object[] allAssets;

	public int timeToWait;
	int sameOrNot;
	int answersGiven;
	int correctAnswers;
	public static int maxFigures = 80;
	int iterations = 0;

	// Start is called before the first frame update
	void Start()
	{
		Application.targetFrameRate = 60;
		imageNames = CreateListFromTangram();
		colors = new List<Color>();
		colors.Add(Color.black);
		colors.Add(Color.green);
		colors.Add(Color.red);
		colors.Add(Color.blue);
		colors.Add(Color.magenta);
		colors.Add(Color.yellow);
		firstRun = true;
		timeToWait = 2;
		answersGiven = 0;
		correctAnswers = 0;
		SetupGame();
	}

	// Update is called once per frame
	void Update()
	{

	}
	private void Awake()
	{
		instance = this;
	}

	private void SetupGame()
	{
		sameOrNot = Random.Range(0, 10);
		print(sameOrNot);
		timeToWait = 2;
		WaitCover.GetComponentInChildren<TMP_Text>().text = "Aspetta " + timeToWait + " secondi";
		if (firstRun)
		{
			int rndImg = Random.Range(0, imageNames.Count);
			int rndColor = Random.Range(0, colors.Count);
			currentImageName = imageNames[rndImg];
			currentColor = colors[rndColor];
			ImageToRecognize.GetComponent<UnityEngine.UI.Image>().sprite = Resources.Load<Sprite>("Images/tangram/" + currentImageName);
			ImageToRecognize.GetComponent<UnityEngine.UI.Image>().color = currentColor;
			YesButton.SetActive(false);
			NoButton.SetActive(false);
			//WaitCover.SetActive(true);
			//WaitCover.GetComponentInChildren<TMP_Text>().text = "Aspetta " + timeToWait + " secondi";
			StartCoroutine(WaitForSeconds(timeToWait, false));
		}
		else
		{
			if (sameOrNot > 5)
			{
				int rndImg;
				int rndColor;
				do
				{
					rndImg = Random.Range(0, imageNames.Count);
					rndColor = Random.Range(0, colors.Count);
				} while (imageNames[rndImg] == currentImageName && colors[rndColor] == currentColor);
				currentImageName = imageNames[rndImg];
				currentColor = colors[rndColor];
				ImageToRecognize.GetComponent<UnityEngine.UI.Image>().sprite = Resources.Load<Sprite>("Images/tangram/" + currentImageName);
				ImageToRecognize.GetComponent<UnityEngine.UI.Image>().color = currentColor;
				StartCoroutine(WaitForSeconds(timeToWait, true));
			}
			else{
				StartCoroutine(WaitForSeconds(timeToWait, true));
			}
		}
	}

	IEnumerator WaitForSeconds(int secondsRemaining, bool shouldShowCover)
	{
		if(shouldShowCover){
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
				WaitCover.SetActive(true);
				YesButton.SetActive(true);
				NoButton.SetActive(true);
				firstRun = false;
				SetupGame();
			}
			else
			{
				timeToWait = 2;
				WaitCover.GetComponentInChildren<TMP_Text>().text = "Aspetta " + timeToWait + " secondi";
				WaitCover.SetActive(false);
			}
		}
	}
	public void GetClick(bool yesClicked){
		answersGiven++;
		iterations++;
		if(sameOrNot > 5){
			if(!yesClicked)
				correctAnswers++;
				
		}else{
			if(yesClicked)
				correctAnswers++;
		}
		print(correctAnswers + " di " + answersGiven + " risposte sono corrette");

		if (iterations < maxFigures){
			SetupGame();
		} else {
			endgame.GetComponent<EndGame>().startEndGame();
			print("FINITA SESSIONE");
		}
		
	}

	List<string> CreateListFromTangram(){
		List<string> newList = new List<string>();
		Regex regex = new Regex("^[A-z][1-9]$");
		var objects = Resources.LoadAll("Images/tangram/" ).Where(c=> regex.IsMatch(c.name)).ToList();
		objects.ForEach(delegate(Object obj){
			if (!newList.Contains(obj.name)){
				newList.Add(obj.name);
			}
		});


		return newList;
	}
}
