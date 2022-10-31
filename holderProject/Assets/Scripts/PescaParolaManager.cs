using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

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
	List<string> possibleWords;
	List<string> possibleWordsCopy;
	public int wordsShown;
	int correctAnswers;
	// Start is called before the first frame update
	void Start()
	{
		possibleWords = new List<string>();
		possibleWords.Add("CONIGLIO");
		possibleWords.Add("ELICOTTERO");
		possibleWords.Add("PASTA");
		possibleWords.Add("ACQUA");
		possibleWords.Add("MARE");
		possibleWords.Add("CONSIGLIO");
		possibleWordsCopy = new List<string>(possibleWords);
		int rnd = Random.Range(0, possibleWords.Count);
		CriteriaOfExercise.GetComponent<TMP_Text>().text = possibleWords[rnd];
		possibleWordsCopy.RemoveAt(rnd);
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
		print(correctAnswers + " di " + wordsShown + " sono corrette");
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
			StartGame();
		}
	}
}
