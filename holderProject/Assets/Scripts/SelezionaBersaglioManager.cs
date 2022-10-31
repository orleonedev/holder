using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

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
	List<string> possibleImages;
	List<string> possibleImagesCopy;
	public int imagesShown;
	int correctAnswers;
	// Start is called before the first frame update
	void Start()
	{
		possibleImages = new List<string>();
		possibleImages.Add("Image 1");
		possibleImages.Add("Image 2");
		possibleImages.Add("Image 3");
		possibleImages.Add("Image 4");
		possibleImages.Add("Image 5");
		possibleImages.Add("Image 6");
		possibleImages.Add("Image 7");
		possibleImages.Add("Image 8");
		possibleImages.Add("Image 9");
		possibleImages.Add("Image 10");
		possibleImagesCopy = new List<string>(possibleImages);
		int rnd = Random.Range(0, possibleImages.Count);
		CriteriaOfExercise.GetComponent<UnityEngine.UI.Image>().sprite = Resources.Load<Sprite>("testImages/" + possibleImages[rnd]);
		possibleImagesCopy.RemoveAt(rnd);
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
			tmp.GetComponent<SelezionaBersaglioScript>().correct = true;
		}
		//shouldBeSame false branch
		else
		{
			int rnd = Random.Range(0, possibleImagesCopy.Count);
			tmp.GetComponent<UnityEngine.UI.Image>().sprite = Resources.Load<Sprite>("testImages/" + possibleImagesCopy[rnd]);
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
			StartGame();
		}
	}
}
