using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;

public class TimerScript : MonoBehaviour
{
   public float timeRemaining = 10;
   public bool timerIsRunning = false;
   public TextMeshProUGUI timeText;

   private void Start()
   {
       // Starts the timer automatically
       timerIsRunning = true;
   }


   void Update()
   {
       if (timerIsRunning)
       {
           if (timeRemaining > 0)
           {
               timeRemaining -= Time.deltaTime;
               DisplayTime(timeRemaining);
           }
           else
           {
               timeRemaining = 0;
               timerIsRunning = false;
           }
       }
   }

   void DisplayTime(float timeToDisplay)
   {
       timeToDisplay += 1;

       float minutes = Mathf.FloorToInt(timeToDisplay / 60);
       float seconds = Mathf.FloorToInt(timeToDisplay % 60);

       timeText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
   }
}
