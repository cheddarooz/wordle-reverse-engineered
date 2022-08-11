using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
    bool[] guessedWordCheck;

    int wordGuessRow = 1;
    int wordGuessColumn = 6;
    int wordGuessColumnForColorChange = 6;

    char emptySpace = ' ';
    
    void Start()
    {
        wordToGuess = getWordFromFile();
        print(wordToGuess);
        actualWordInCharArray = wordToGuess.ToCharArray();
        actualWordInCharArray = shiftArrayIndexPlusOne(actualWordInCharArray);

        guessedWordCheck = new bool[actualWordInCharArray.Length];
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
                if(wordGuessRow < 6)
                {
                    updateOnScreenLetterGuess(currentKeyBoardLetterInput);
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
                wordGuessColumnForColorChange--;
            }
            else
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

        guessedWordInCharArray = shiftArrayIndexPlusOne(guessedWordInCharArray);

        int wordGuessRowForColorChange = 1;
        int correctlyGuessedLetterCount = 0;

        for (int i = 1; i < guessedWordInCharArray.Length; i++)
        {
            if (guessedWordInCharArray[i].Equals(actualWordInCharArray[i]))
            {
                guessedWordCheck[i] = true;
                correctlyGuessedLetterCount++;
                GameObject.Find("letter" + wordGuessRowForColorChange + "." + wordGuessColumnForColorChange).GetComponent<Text>().color = Color.green;
                print("green at " + guessedWordInCharArray[i]);
                
            }
          wordGuessRowForColorChange++;
        }
        
        if (correctlyGuessedLetterCount == 5)
        {
            print("win");
        }

        for(int i = 1; i < actualWordInCharArray.Length; i++)
        {
            wordGuessRowForColorChange = 1;
           
            int currentCharFrequency = wordToGuess.Count(x => (x == actualWordInCharArray[i]));
            int charFrequencyComparison = 0;

            for (int k = 1; k < guessedWordInCharArray.Length; k++) 
            {
                if ((actualWordInCharArray[i].Equals(guessedWordInCharArray[k])) && (guessedWordCheck[k].Equals(false)))
                {
                    charFrequencyComparison++;
                    if(charFrequencyComparison <= currentCharFrequency)
                    {
                        GameObject.Find("letter" + wordGuessRowForColorChange + "." + wordGuessColumnForColorChange).GetComponent<Text>().color = Color.yellow;
                    }
                    //print("yellow at " + guessedWordInCharArray[i] + " with " + actualWordInCharArray[k]);
                    //print("current boolean in array" + guessedWordCheck[k]); 
                }
                wordGuessRowForColorChange++;
            }
        }
        correctlyGuessedLetterCount = 0;
    }
    
    public char[] shiftArrayIndexPlusOne(char[] arr)
    {
        char[] shiftedArray = new char[arr.Length + 1];
        
        for(int i = 1; i < 6; i++)
        {
            shiftedArray[i] = arr[i - 1];
        }
        return shiftedArray;
    }
    
    public string getWordFromFile()
    {
        TextAsset word = (TextAsset)Resources.Load("fiveletterwords", typeof(TextAsset));
        string wordsFromText = word.text;
        string[] splitWords = wordsFromText.Split("\n"[0]);
        int randomWord = Random.Range(0, splitWords.Length);

        string finalWord = splitWords[randomWord];
        finalWord = finalWord.ToLower();

        return finalWord;
    }
    IEnumerator displayGUIWarning()
    {
        warningUI.SetActive(true);
        yield return new WaitForSeconds(3);
        warningUI.SetActive(false);
    }    

}
