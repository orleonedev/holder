using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Text.RegularExpressions;
using System.Linq;

public class SelezionaBersaglioManager : MonoBehaviour
{
	private static SelezionaBersaglioManager instance;
	public static SelezionaBersaglioManager Instance { get { return instance; } }
	[SerializeField]
	GameObject description;
	[SerializeField]
	GameObject CriteriaOfExercise;
	[SerializeField]
	GameObject spawnArea;
	[SerializeField]
	GameObject SelezionaBersaglioPrefab;
	[SerializeField]
	GameObject endgame;
	List<string> possibleImages;
	
	Dictionary<string,Color> dictWithColors;
	public int imagesShown;
	int correctAnswers;
	List<Color> colors = new List<Color>();
	public static int maxDistractors = 30;
	
	// Start is called before the first frame update
	void Start()
	{
		possibleImages = CreateListFromTangram();
		colors.Add(Color.black);
		colors.Add(Color.blue);
		colors.Add(Color.cyan);
		colors.Add(Color.gray);
		colors.Add(Color.green);
		colors.Add(Color.magenta);
		colors.Add(Color.red);
		colors.Add(Color.yellow);
		dictWithColors = CreateDictionaryOfColors(possibleImages);
		int rnd = Random.Range(0, dictWithColors.Count);
		print(dictWithColors.ElementAt(rnd).Key + " " + dictWithColors.ElementAt(rnd).Value);
		CriteriaOfExercise.GetComponent<UnityEngine.UI.Image>().sprite = Resources.Load<Sprite>("Images/tangram/" + dictWithColors.ElementAt(rnd).Key);
		CriteriaOfExercise.GetComponent<UnityEngine.UI.Image>().color = dictWithColors.ElementAt(rnd).Value;
		dictWithColors.Remove(dictWithColors.ElementAt(rnd).Key);
		while(dictWithColors.Count>maxDistractors){
			dictWithColors.Remove(dictWithColors.ElementAt(Random.Range(0,dictWithColors.Count())).Key);
		}
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
		print(correctAnswers + " di " + imagesShown + " sono corrette");
		float rndX = Random.Range(-768.0f, 768.0f);
		float rndY = Random.Range(-600.0f, 600.0f);
		var tmp = Instantiate(SelezionaBersaglioPrefab, spawnArea.transform);
		tmp.transform.localPosition = new Vector2(rndX, rndY);
		int shouldBeSame = Random.Range(0, 3);
		//shouldBeSame true branch
		if (shouldBeSame < 2)
		{
			tmp.GetComponent<UnityEngine.UI.Image>().sprite = CriteriaOfExercise.GetComponent<UnityEngine.UI.Image>().sprite;
			tmp.GetComponent<UnityEngine.UI.Image>().color = CriteriaOfExercise.GetComponent<UnityEngine.UI.Image>().color;
			tmp.GetComponent<SelezionaBersaglioScript>().correct = true;
		}
		//shouldBeSame false branch
		else
		{
			int rnd = Random.Range(0, dictWithColors.Count);
			tmp.GetComponent<UnityEngine.UI.Image>().sprite = Resources.Load<Sprite>("Images/tangram/" + dictWithColors.ElementAt(rnd).Key);
			tmp.GetComponent<UnityEngine.UI.Image>().color = dictWithColors.ElementAt(rnd).Value;
			dictWithColors.Remove(dictWithColors.ElementAt(rnd).Key);
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
			if (obj.GetComponent<SelezionaBersaglioScript>().correct)
				SelezionaBersaglioManager.Instance.imagesShown++;
			Destroy(obj);
			if (dictWithColors.Count > 0)
			{	
				print(dictWithColors.Count);
				StartGame();
			} else {
				endgame.GetComponent<EndGame>().startEndGame();
				print("FINITA SESSIONE");
			}
			
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

	Dictionary<string,Color> CreateDictionaryOfColors(List<string> nameList){
		Dictionary<string,Color> newDict = new Dictionary<string, Color>();
		List<string> nameListCopy = new List<string>(nameList);
		

		nameList.ForEach(delegate(string name){
			if (colors.Count > 0){
				Regex regex = new Regex(name.Substring(0,1));
			List<string> sameInitial = nameListCopy.Where(c=> regex.IsMatch(c)).ToList();
			if (sameInitial.Count > 0){
			Color colorToUse = colors.ElementAt(Random.Range(0,colors.Count));
			sameInitial.ForEach(delegate(string name){
				newDict.Add(name,colorToUse);
			});
			colors.Remove(colorToUse);
			nameListCopy.RemoveAll(c=> sameInitial.Contains(c));
			sameInitial.Clear();
			}
			} 		
		});

		return newDict;
	}

	void DebugPrint(List<string> list)
	{
		foreach (string item in list)
		{
			print(item);
		}
	}
}
