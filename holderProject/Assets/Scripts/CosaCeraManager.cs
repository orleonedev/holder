using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Linq;

public class CosaCeraManager : MonoBehaviour
{
	private static CosaCeraManager instance;
	public static CosaCeraManager Instance { get { return instance; } }
	[SerializeField]
	GameObject VerticalLayoutExercise;
	[SerializeField]
	GameObject VerticalLayoutSolution;
	[SerializeField]
	GameObject CosaCeraInputPrefab;
	[SerializeField]
	GameObject CheckListButton;
	List<string> possibleWords;
	List<string> possibleWordsCopy;
	List<GameObject> listofSolutions;
	List<GameObject> listofAnswers;
	[SerializeField]
	GameObject endgame;
	
	public static int nRows = 5;
	public static int nSchede = 10;
	public static int maximumTries = 3;
	int WordsFound;
	int tries;
	
	// Start is called before the first frame update
	void Start()
	{
		VerticalLayoutSolution.transform.localPosition = new Vector3(0f, 0f, 0f);
		possibleWords = CreateList(CSVData.Instance.holderWordList.wordobject);
		listofSolutions = new List<GameObject>();
		listofAnswers = new List<GameObject>();
		
		WordsFound = 0;
		tries = 0;
		
		possibleWordsCopy = new List<string>(possibleWords);
		
		for (int i = 0; i < nRows; i++)
		{
			int rndWord = Random.Range(0, possibleWordsCopy.Count);
			var tmpSol = Instantiate(CosaCeraInputPrefab, VerticalLayoutExercise.transform);
			tmpSol.GetComponent<TMP_InputField>().text = possibleWordsCopy[rndWord];
			tmpSol.GetComponent<TMP_InputField>().interactable = false;
			//debug
			print(possibleWordsCopy[rndWord]);
			possibleWordsCopy.RemoveAt(rndWord);
			listofSolutions.Add(tmpSol);
			var tmpAns = Instantiate(CosaCeraInputPrefab, VerticalLayoutSolution.transform);
			listofAnswers.Add(tmpAns);
			tmpAns.SetActive(false);
		}
		StartCoroutine(WaitForSeconds());
	}

	// Update is called once per frame
	void Update()
	{

	}
	private void Awake()
	{
		instance = this;
	}
	IEnumerator WaitForSeconds()
	{
		yield return new WaitForSeconds(2f);
		ToggleLayoutVisibility(false, true);
		VerticalLayoutSolution.transform.localPosition = new Vector3(-512.0f, 0f, 0f);
		CheckListButton.SetActive(true);
	}

	IEnumerator WaitForSecondsAndNewScheda()
	{
		yield return new WaitForSeconds(2f);
		VerticalLayoutSolution.transform.localPosition = new Vector3(-512.0f, 0f, 0f);
		EnablesObjectsAtNewGame();
		NewScheda();
		
	}
	void ToggleLayoutVisibility(bool solutionsVisible, bool answersVisible)
	{
		foreach (GameObject obj in listofSolutions)
		{
			obj.SetActive(solutionsVisible);
		}
		foreach (GameObject obj in listofAnswers)
		{
			obj.SetActive(answersVisible);
		}
	}
	public void CheckList(){
		CheckListButton.SetActive(false);
		tries++;
		WordsFound = 0;
		for(int i = 0; i < listofAnswers.Count; i++){
			if(listofAnswers[i].GetComponent<TMP_InputField>().text.ToUpper() == listofSolutions[i].GetComponent<TMP_InputField>().text){
				listofSolutions[i].GetComponentInChildren<TMP_Text>().alpha = 0.0f;
				WordsFound++;
			}
			else{
				listofAnswers[i].GetComponent<TMP_InputField>().text = "";
			}
		}
		ToggleLayoutVisibility(true, true);
		if(WordsFound == listofSolutions.Count && tries <= maximumTries){
			print("Bravo");
			DisableObjectsAtEndGame();
			if (possibleWordsCopy.Count >= nRows) {
				StartCoroutine(WaitForSecondsAndNewScheda());
			} else {
				endgame.GetComponent<EndGame>().startEndGame();
				print("FINITA SESSIONE");
			}
				
		}else if(tries >= maximumTries){
			print("Sbagliato");
			DisableObjectsAtEndGame();
			if (possibleWordsCopy.Count >= nRows) {
				StartCoroutine(WaitForSecondsAndNewScheda());
			} else {
				endgame.GetComponent<EndGame>().startEndGame();
				print("FINITA SESSIONE");
			}
		}else{
			StartCoroutine(WaitForSeconds());
		}
	}
	void DisableObjectsAtEndGame(){
		foreach (GameObject obj in listofSolutions)
		{
			obj.GetComponent<TMP_InputField>().interactable = false;
		}
		foreach (GameObject obj in listofAnswers)
		{
			obj.GetComponent<TMP_InputField>().interactable = false;
		}
		CheckListButton.GetComponent<UnityEngine.UI.Button>().interactable = false;
	}

	void EnablesObjectsAtNewGame(){
		/* foreach (GameObject obj in listofSolutions)
		{
			obj.GetComponent<TMP_InputField>().interactable = true;
		} */
		foreach (GameObject obj in listofAnswers)
		{
			obj.GetComponent<TMP_InputField>().interactable = true;
		}
		CheckListButton.GetComponent<UnityEngine.UI.Button>().interactable = true;
	}

	void NewScheda(){
		ClearEverything();

		listofSolutions = new List<GameObject>();
		listofAnswers = new List<GameObject>();
		
		WordsFound = 0;
		tries = 0;
		
		for (int i = 0; i < nRows; i++)
		{
			int rndWord = Random.Range(0, possibleWordsCopy.Count);
			var tmpSol = Instantiate(CosaCeraInputPrefab, VerticalLayoutExercise.transform);
			tmpSol.GetComponent<TMP_InputField>().text = possibleWordsCopy[rndWord];
			tmpSol.GetComponent<TMP_InputField>().interactable = false;
			//debug
			print(possibleWordsCopy[rndWord]);
			possibleWordsCopy.RemoveAt(rndWord);
			listofSolutions.Add(tmpSol);
			var tmpAns = Instantiate(CosaCeraInputPrefab, VerticalLayoutSolution.transform);
			listofAnswers.Add(tmpAns);
			tmpAns.SetActive(false);
		}
		StartCoroutine(WaitForSeconds());
	}

	void ClearEverything(){

		for (var i = VerticalLayoutExercise.transform.childCount - 1; i >= 0; i--){
  			Object.Destroy(VerticalLayoutExercise.transform.GetChild(i).gameObject);
		}

		for (var i = VerticalLayoutSolution.transform.childCount - 1; i >= 0; i--){
  			Object.Destroy(VerticalLayoutSolution.transform.GetChild(i).gameObject);
		}
		

	}

	public List<string> CreateList(CSVData.WordObject[] Data){
		List<string> newList = new List<string>();
		CSVData.WordObject[] CopyData = Data;

		for (int i = 0; i < nSchede*nRows; i++)
		{
			CSVData.WordObject random = CopyData.ElementAt(Random.Range(0,CopyData.Count()));
			newList.Add(random.Word);
			CopyData = CopyData.Where(c=> c!= random).ToArray();
		}


		return newList;
	}
}
