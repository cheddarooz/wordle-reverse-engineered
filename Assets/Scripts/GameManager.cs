using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public GameObject center;
    public GameObject containedLetter;
    public GameObject warningUI;

    string guessedWord;
    string wordToGuess;

    char[] actualWordInCharArray;
    bool[] guessedWordCheck = new bool[5];

    int wordGuessRow = 1;
    int wordGuessColumn = 6;

    char emptySpace = ' ';
    
    void Start()
    {
        wordToGuess = "apple";
        actualWordInCharArray = wordToGuess.ToCharArray();

        guessedWordCheck = new bool[wordToGuess.Length];

        center = GameObject.Find("centerReferencePoint");
        warningUI = GameObject.Find("warningUI");

        warningUI.SetActive(false);
       
        initializeLetterSpaces();
    }

    // Update is called once per frame
    void Update()
    {
        checkInput();
    }

    public void initializeLetterSpaces()
    {
        for (int k = 1; k < 7; k++)
        {
            for (int i = 1; i < 6; i++)
            {
                Vector3 letterPosition = new Vector3(center.transform.position.x + ((i - 6/2.0f) * 150), center.transform.position.y + ((k - 3) * 150), center.transform.position.z);
                GameObject letterToSpawn = (GameObject)Instantiate(containedLetter, letterPosition, Quaternion.identity);
                letterToSpawn.name = "letter" + i + "." + k;
                letterToSpawn.transform.SetParent(GameObject.Find("Canvas").transform);
            }
        }
    }

    public void checkInput()
    {
        if (Input.anyKeyDown && !Input.GetMouseButton(0))
        {
            char currentKeyBoardLetterInput = Input.inputString.ToCharArray()[0];
            int currentKeyboardLetterAsAscii = System.Convert.ToInt32(currentKeyBoardLetterInput);

            if (currentKeyboardLetterAsAscii >= 97 && currentKeyboardLetterAsAscii <= 122)
            {
                if(wordGuessRow <= 5)
                {
                    updateOnScreenLetterGuess(currentKeyBoardLetterInput);
                    //GameObject.Find("wordGuessed").GetComponent<Text>().text = Input.inputString.ToString();
                    guessedWord = guessedWord + Input.inputString.ToString();
                    print(guessedWord);
                    wordGuessRow++;
                    print(guessedWord.Length);
                }
            }   
        }
        if (Input.GetKeyDown(KeyCode.Backspace))
        {
            if (wordGuessRow >= 1)
            {
                guessedWord = guessedWord.Remove(guessedWord.Length - 1);
                wordGuessRow--;
                updateOnScreenLetterGuess(emptySpace);
            }
        }
        if(Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter))
        {
            if(guessedWord.Length == 5)
            {
                print("processing word");
                checkFinalWordInput(guessedWord);
                guessedWord = "";
                wordGuessRow = 1;
                wordGuessColumn--;
            }
            if(guessedWord.Length < 5)
            {
                StartCoroutine(displayGUIWarning());   
            }
        }
    }    

    public void updateOnScreenLetterGuess(char c)
    {
        GameObject.Find("letter" + wordGuessRow + "." + wordGuessColumn).GetComponent<Text>().text = c.ToString();
    }    
    public void checkFinalWordInput(string word)
    {
        char[] guessedWordInCharArray = word.ToCharArray();
        int wordGuessRowForColorChange = 0;
        int wordGuessColumnForColorChange = 6;
        int correctlyGuessedLetterCount = 0;

        for(int i = 0; i < word.Length; i++)
        {
            wordGuessRowForColorChange++;
            if (guessedWordInCharArray[i].Equals(actualWordInCharArray[i]))
            {
                guessedWordCheck[i] = true;
                correctlyGuessedLetterCount++;
                GameObject.Find("letter" + wordGuessRowForColorChange + "." + wordGuessColumnForColorChange).GetComponent<Text>().color = Color.yellow;
                print("green at " + guessedWordInCharArray[i]);
                
            }
        }
        if(correctlyGuessedLetterCount == 5)
        {
            print("win");
        }

        for(int i = 0; i < word.Length; i++)
        {
            wordGuessRowForColorChange = 1;

            for (int k = 0; k < wordToGuess.Length; k++) 
            {
                print(guessedWordInCharArray[i]);
                if(wordGuessRowForColorChange == 6)
                {
                    print("value changed");
                    wordGuessRowForColorChange = 5;
                }

                if ((guessedWordInCharArray[i].Equals(actualWordInCharArray[k])) && (guessedWordCheck[k] == false))
                {
                    GameObject.Find("letter" + wordGuessRowForColorChange + "." + wordGuessColumnForColorChange).GetComponent<Text>().color = Color.yellow;
                    print("yellow");
                    print("yellow at " + guessedWordInCharArray[i] + " with " + actualWordInCharArray[k]);
                    print("current boolean in array" + guessedWordCheck[i]);
                }
                wordGuessRowForColorChange++;
               ///print(wordGuessRowForColorChange);
            }
            

        }
       
    }    

    IEnumerator displayGUIWarning()
    {
        warningUI.SetActive(true);
        yield return new WaitForSeconds(3);
        warningUI.SetActive(false);
    }    
}
