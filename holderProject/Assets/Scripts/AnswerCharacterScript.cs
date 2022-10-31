using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnswerCharacterScript : MonoBehaviour
{
	private int characterPosition;
	public int CharacterPosition { get { return characterPosition; } set { characterPosition = value; } }
	// Start is called before the first frame update
	void Start()
	{

	}

	// Update is called once per frame
	void Update()
	{

	}

	public void DeleteCharacter()
	{
		print("Deleted Character");
		AnswerHandlerCatena type = this.GetComponentInParent<AnswerHandlerCatena>();
		switch (type.keyboard)
		{
			case TypeOfKeyboard.ParolaGiusta:
				ParolaGiustaManager.Instance.DeleteKeystroke(characterPosition);
				break;
			case TypeOfKeyboard.AnelloCatena:
				AnelloDellaCatenaManager.Instance.DeleteKeystroke(characterPosition);
				break;
			case TypeOfKeyboard.QuattroImmaginiUnaCategoria:
				QuattroImmaginiUnaCategoriaManager.Instance.DeleteKeystroke(characterPosition);
				break;
		}
	}
}
