using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Quiz_Manager : MonoBehaviour {

    // The following script is the core of the "Stotter-Quiz".
    // It randomly selects questions for the player, 
    // saves the answers and manages what the UI displays.     
    

    public Text QuizExpText; // Reference to UI
    public GameObject QuizExpBtn; // Reference to UI
    public Text QuizExpBtnTxt; // Reference to UI

    public GameObject QuizTrueBtn; // Reference to UI
    public GameObject QuizFalseBtn; // Reference to UI
    public GameObject QuizContBtn; // Reference to UI
    public Text QuizContBtnTxt; // Reference to UI

    public Text titleText; // Reference to UI

    public string startTitleString = "Das Stotter-Quiz"; // Reference to UI

    public int questionCounter; // Number of the current question
    public int questionAmount; // The maximum amount of questions given
    public int questperpool = 2; // questions chosen per pool
    private int questppCount; // support variable in a function
    public int points; // Points collected
    [TextArea]
    public string InitText; // explanation text at the beginning
    private Question foundQuest; // Quest chosen to come up next. Randomly chosen out of a pool
    public int questionResult;  // The right answer to the current question is an int and can either be 0 or 1
    public bool answerRight; // Is set to true when the answer is correct
    

    public List<Question> questList = new List<Question>(); // The list of questions which will be asked
    public List<Question> answerList = new List<Question>(); // The list of answers given
    public List<Question> searchList = new List<Question>(); // When the quiz starts this list gets filled with all the potential questions which go into the questList


    // This function (re-)starts the Quiz. Here out of a prepared pool of questions the "searchlist" gets formed
    // and after that a specific number of questions get selected which then get asked one after the other.

    public void StartQuiz()
    {
        QuizExpBtn.SetActive(false);        
      
        questionCounter = 0;
        points = 0;

        questList.Clear();
        answerList.Clear();

        PoolPool pp = GameObject.Find("PoolPool").gameObject.GetComponent<PoolPool>();

        for (int i = 0; i < pp.poolList.Count; i++)
        {
            QuestionPool qp = pp.poolList[i];
            questppCount = questperpool;
            searchList.Clear();

            for(int z = 0; z < qp.questList.Count; z++)
            {
                Question q = qp.questList[z];

                if(q.result == 0)
                {
                    searchList.Add(qp.questList[z]);
                    searchList.Add(qp.questList[z]);
                }
                searchList.Add(qp.questList[z]);
            }

            

            for (int u = 0; u < questppCount; u++) 
            {
                int r = Random.Range(0, searchList.Count-1);

                questList.Add(searchList[r]);

                searchList.Remove(searchList[r]);

                

            }

            
        }

        QuizExpBtnTxt.text = "Erneut starten";        

        Question(questionCounter);        
    }

    // This sets up the next question in the UI and the variables behind the scene

    public void Question(int Nr)
    {
        QuizContBtn.SetActive(false);

        QuizTrueBtn.SetActive(true);
        QuizFalseBtn.SetActive(true);

        Nr += 1;
        questionCounter = Nr;
       
        foundQuest = questList[Mathf.RoundToInt(Random.Range(0, (questList.Count-1)))];

        titleText.text = "Frage " + Nr + " von " + questionAmount;

        QuizExpText.text = foundQuest.GetComponent<Question>().question;

        questionResult = foundQuest.GetComponent<Question>().result;
    }
      
    // This gets activated via a button, registers the answer and triggers the next question.

    public void Answer(int answerInt)
    {
        
        if (questionResult == answerInt)
        {
            points += 1;
            answerRight = true;
            
        }
        else
        {
            answerRight = false;
            answerList.Add(foundQuest);

        }

        
        questList.Remove(foundQuest);

        if (questionCounter < questionAmount)
        {
            Question(questionCounter);
        }
        else
        {
            QuizTrueBtn.SetActive(false);
            QuizFalseBtn.SetActive(false);
            QuizContBtn.SetActive(true);
            QuizExpBtn.SetActive(false);

            titleText.text = "Endergebnis";

            questionCounter = 0;


            if (points < questionAmount)
            {
                QuizExpText.text = points + " Punkt(e)! Wenn Sie auf 'Weiter' klicken werden Ihnen zum Abschluss noch die Erklärungen zu den Behauptungen angezeigt, die Sie falsch eingeschätzt haben.";
            }
            else
            {
                QuizExpText.text = "Sehr gut! Sie haben " + points + " Punkte und damit alle Fragen richtig beantwortet!";
            }
        }
    }

    // This triggers when the quiz is finished.
    // Here the player gets an explanation for each question she or he gave a false answer to.

    public void EndResult()
    {
        questionCounter += 1;
        if(questionCounter > answerList.Count)
        {
            QuizTrueBtn.SetActive(false);
            QuizFalseBtn.SetActive(false);
            QuizContBtn.SetActive(false);
            QuizExpBtn.SetActive(true);

            titleText.text = startTitleString;
          

            QuizExpText.text = "Vielen Dank für die Teilnahme. Versuchen Sie es doch erneut. Das Quiz wählt für Sie aktuell " + questionAmount + " Behauptungen aus einem Pool von " + 50 + " Behauptungen übers Stottern insgesamt. Vielleich sieht Ihr Quiz dann beim nächsten Mal anders aus.";

            QuizExpBtnTxt.text = "Erneut versuchen";
        }
        else
        {          

            titleText.text = questionCounter + ". falsch eingeschätzte Behauptung";

            QuizExpText.text = "Behauptung: " + answerList[questionCounter - 1].question + "\n" + "\n" + "Auflösung: " + answerList[questionCounter - 1].answer;
        }

    }

    // The "Reserve"-function simply exists to store script-lines which are currently not in use but maybe get used in later versions again.

    public void Reserve()
    {
        string addString = "";

        if (questionResult == 0)
        {
            addString = "Wahrheit: ";
        }
        else
        {
            addString = "Mythos: ";
        }


        QuizExpText.text = addString + foundQuest.GetComponent<Question>().answer;
    }
}

