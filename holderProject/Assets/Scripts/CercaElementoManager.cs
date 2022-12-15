using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Text.RegularExpressions;
using System.Linq;

public class CercaElementoManager : MonoBehaviour
{
	private static CercaElementoManager instance;
	public static CercaElementoManager Instance { get { return instance; } }
	[SerializeField]
	GameObject spawnArea;
	[SerializeField]
	GameObject CercaElementoPrefab;
	[SerializeField]
	GameObject endgame;
	List<string> possibleImages;
	public int imagesShown;
	public int correctAnswers;
	int rndImageIndexToShow;
	int amountOfElementToShow;
	int destroyObjectAfterTapDelay;
	// Start is called before the first frame update
	void Start()
	{
		possibleImages = CreateListFromTangram();
		DebugPrint(possibleImages);
		amountOfElementToShow = 90;
		destroyObjectAfterTapDelay = 2;
		StartGame();
	}

	void DebugPrint(List<string> list)
	{
		foreach (string item in list)
		{
			print(item);
		}
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
		GameObject tmp = null;
		rndImageIndexToShow = Random.Range(0, possibleImages.Count);
		StartCoroutine(WaitForSeconds(tmp));
	}
	public void GetClick()
	{
		print(correctAnswers + " di " + amountOfElementToShow + " sono corrette");
	}
	IEnumerator WaitForSeconds(GameObject obj)
	{
		for(int i = 0; i < amountOfElementToShow; i++){
			SpawnElement(obj);
			yield return new WaitForSeconds(2f);
		}
		
		endgame.GetComponent<EndGame>().startEndGame();
		print("FINITA SESSIONE");
	}
	void SpawnElement(GameObject tmp){
		float rndX = Random.Range(-768.0f, 768.0f);
		float rndY = Random.Range(-600.0f, 600.0f);
		tmp = Instantiate(CercaElementoPrefab, spawnArea.transform);
		tmp.transform.localPosition = new Vector2(rndX, rndY);
		tmp.GetComponent<UnityEngine.UI.Image>().sprite = Resources.Load<Sprite>("Images/tangram/" + possibleImages[rndImageIndexToShow]);
		tmp.GetComponent<CercaElementoScript>().destroyDelay = destroyObjectAfterTapDelay;
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
		
		

		/* foreach (var obj in objects.Where(c=> regex.IsMatch(c.name)))
		{
			newList.Add(obj.name);
		} */

		

		return newList;
	}
}
