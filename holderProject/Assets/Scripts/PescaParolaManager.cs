using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Linq;
using System.Text.RegularExpressions;

public class PescaParolaManager : MonoBehaviour
{
	private static PescaParolaManager instance;
	public static PescaParolaManager Instance { get { return instance; } }
	[SerializeField]
	GameObject description;
	[SerializeField]
	GameObject CriteriaOfExercise;
	[SerializeField]
	GameObject spawnArea;
	[SerializeField]
	GameObject PescaParolaPrefab;
	[SerializeField]
	GameObject endgame;
	List<string> possibleWords;
	List<string> possibleWordsCopy;
	public static int maxNumberWords = 20;
	public int wordsShown;
	int correctAnswers;
	// Start is called before the first frame update
	void Start()
	{
		possibleWords = CreateList(CSVData.Instance.holderWordList.wordobject);
		possibleWordsCopy = new List<string>(possibleWords);
		int rnd = Random.Range(0, possibleWords.Count);
		CriteriaOfExercise.GetComponent<TMP_Text>().text = possibleWords[rnd];
		possibleWordsCopy.RemoveAt(rnd);
		if (possibleWordsCopy.Count > maxNumberWords) {
			possibleWordsCopy = possibleWordsCopy.GetRange(0,maxNumberWords);
		}
		print(possibleWordsCopy.Count);
		StartGame();
	}

	// Update is called once per frame
	void Update()
	{

	}
	private void Awake()
	{
		instance = this;
	}
	public void StartGame()
	{
		
		float rndX = Random.Range(-768.0f, 768.0f);
		float rndY = Random.Range(-600.0f, 600.0f);
		var tmp = Instantiate(PescaParolaPrefab, spawnArea.transform);
		tmp.transform.localPosition = new Vector2(rndX, rndY);
		int shouldBeSame = Random.Range(0, 3);
		//shouldBeSame true branch
		if (shouldBeSame < 2)
		{
			tmp.GetComponentInChildren<TMP_Text>().text = CriteriaOfExercise.GetComponent<TMP_Text>().text;
			tmp.GetComponent<PescaParolaScript>().correct = true;
		}
		//shouldBeSame false branch
		else
		{
			int rnd = Random.Range(0, possibleWordsCopy.Count);
			tmp.GetComponentInChildren<TMP_Text>().text = possibleWordsCopy[rnd];
			possibleWordsCopy.RemoveAt(rnd);
		}
		StartCoroutine(WaitForSeconds(tmp));
	}
	public void GetClick(bool correct)
	{
		if (correct)
		{
			correctAnswers++;
		}
	}
	IEnumerator WaitForSeconds(GameObject obj)
	{
		yield return new WaitForSeconds(2);
		if (obj != null)
		{
			if (obj.GetComponent<PescaParolaScript>().correct)
				PescaParolaManager.Instance.wordsShown++;
			Destroy(obj);
			if (possibleWordsCopy.Count > 0){
				StartGame();
			} else {
				endgame.GetComponent<EndGame>().startEndGame();
				print("FINITA SESSIONE");
			}
			
		}
	}

	List<string> CreateList(CSVData.WordObject[] Data){
		List<string> newList = new List<string>();
		CSVData.WordObject[] CopyData = Data;
		CSVData.WordObject[] filteredData = CopyData;
		CSVData.WordObject random = new CSVData.WordObject();
		do
		{
			random = CopyData.ElementAt(Random.Range(0,CopyData.Count()));
			CopyData = CopyData.Where(c=> c!=random).ToArray();
			newList.Add(random.Word);
			var threeChar = random.Word.Substring(0,3);
			Regex regex = new Regex("^"+threeChar);
			filteredData = CopyData.Where(c=> regex.IsMatch(c.Word)).ToArray();
		} while (filteredData.Count()<5);

		
		foreach (var item in filteredData)
		{
			newList.Add(item.Word);

		}
		return newList;
	}
}
