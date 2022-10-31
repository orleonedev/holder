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
	List<GameObject> listofSolutions;
	List<GameObject> listofAnswers;
	int WordsFound;
	int tries;
	int maximumTries;
	// Start is called before the first frame update
	void Start()
	{
		VerticalLayoutSolution.transform.localPosition = new Vector3(0f, 0f, 0f);
		possibleWords = new List<string>();
		listofSolutions = new List<GameObject>();
		listofAnswers = new List<GameObject>();
		possibleWords.Add("PIZZA");
		possibleWords.Add("MOZZARELLA");
		possibleWords.Add("ACQUA");
		possibleWords.Add("FARINA");
		possibleWords.Add("LETTO");
		possibleWords.Add("CUSCINO");
		possibleWords.Add("ATTREZZO");
		possibleWords.Add("SCALE");
		possibleWords.Add("PIANO");
		possibleWords.Add("CHITARRA");
		WordsFound = 0;
		tries = 0;
		maximumTries = 3;
		var possibleWordsCopy = new List<string>(possibleWords);
		int rnd = Random.Range(3, 6);
		for (int i = 0; i < rnd; i++)
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
		}else if(tries >= maximumTries){
			print("Sbagliato");
			DisableObjectsAtEndGame();
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
}
