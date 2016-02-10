using UnityEngine;
using System.Collections;
using System.IO;
using UnityEngine.SceneManagement;


public class TitleScreenScript : MonoBehaviour {
	public GUISkin guiSkin;
	public Texture backgroundTexture;
	string firstName;
	string lastName;
	string playerName;
	bool isLoggedIn;
	bool deleteButtonClicked;
	bool isDataDeleted;
	bool statsAreShowing;
	public string drivingScene;
	string retrievedLaneDeps;
	bool showLaneDeps;
	bool showNoSignalsLeftError;
	bool showOnlyLaneDeps;
	bool printToFileButtonPressed;
	string rotDeg;
	string maxSteeringAngle;
	string maxSpeed;
	string autoSteeringAngle;
	string msgThresholdDeg;
	string respThresholdDeg;
	private string fileNameForLaneDeps;
	string fileNameForSurvey;
	public AudioSource loginSound;
	public AudioSource buttonSound;
	public AudioSource sound0;
	public AudioSource sound1;
	public AudioSource sound2;
	public AudioSource sound3;
	public Font questionFont;
	public Font defaultFont;
	string urg0 = "";
	string urg1 = "";
	string urg2 = "";
	string urg3 = "";
	string ann0 = "";
	string ann1 = "";
	string ann2 = "";
	string ann3 = "";
	string acc0 = "";
	string acc1 = "";
	string acc2 = "";
	string acc3 = "";
	public Vector2 scrollPosition = Vector2.zero;
	bool printSurveyButtonPressed;
	bool surveyIsShowing;

	void Start () {
		firstName ="";
		lastName = "";
		playerName = DataKeeper.dk.currentPlayerName;
		isLoggedIn = DataKeeper.dk.isLoggedIn;
		fileNameForLaneDeps = "";
		fileNameForSurvey = "";
	}

	void Update () {
		isLoggedIn = DataKeeper.dk.isLoggedIn;
	}

	void OnGUI(){
		guiSkin.label.font = defaultFont;
		GUI.skin = guiSkin;
		GUI.DrawTexture(new Rect(0,0,Screen.width, Screen.height), backgroundTexture);


		if(!surveyIsShowing){
			guiSkin.label.alignment = TextAnchor.MiddleCenter;
			guiSkin.label.fontSize = 40;
			GUI.Label(new Rect(Screen.width*0.3f, 0, 600, 100),  "***************");
			GUI.Label(new Rect(Screen.width*0.3f, 50, 600, 100), "LET'S DRIVE!");
			GUI.Label(new Rect(Screen.width*0.3f, 90, 600, 100), "***************");
		}

		//IF STATS BUTTON NOT PRESSED
		if(!statsAreShowing && !surveyIsShowing){
			if(GUI.Button (new Rect(0,Screen.height-100f,150,100),"EXIT")){
				Application.Quit();
			}
				
			//HANDLE DELETE BUTTON
			guiSkin.button.fontSize = 20;
			if (!deleteButtonClicked){
				if(GUI.Button (new Rect(Screen.width-150,Screen.height-100f,150,100),"DELETE\nDATA")){
					deleteButtonClicked = true;
				}
			}else{
				GUI.Button (new Rect(Screen.width-300,Screen.height-100f,150,100),PlayerPrefs.GetString("PlayerName"));
				if(GUI.Button (new Rect(Screen.width-150,Screen.height-100f,150,100),"SURE?")){
					PlayerPrefs.DeleteAll();
					deleteButtonClicked = false;
					isDataDeleted = true;
				}
				if (Input.GetKeyDown(KeyCode.Escape))
					deleteButtonClicked = false;
			}

			if(isDataDeleted){
				if(GUI.Button (new Rect(Screen.width-300,Screen.height-100f,150,100),PlayerPrefs.GetString("PlayerName"))){
					isDataDeleted = false;
				}
			}
				
			guiSkin.button.fontSize = 30;
			if(!isLoggedIn){
				guiSkin.label.fontSize = 30;
				GUI.Label(new Rect(Screen.width*0.3f,200,600,70), "PLEASE ENTER YOUR NAME");
				guiSkin.label.fontSize = 20;
				GUI.Label(new Rect(Screen.width*0.3f,300,100,70), "FIRST:");
				GUI.Label(new Rect(Screen.width*0.3f,400,100,70), "LAST:");
				firstName = GUI.TextField(new Rect(Screen.width*0.3f+145f,295,300,50), firstName).ToUpper();
				lastName = GUI.TextField(new Rect(Screen.width*0.3f+145f,395,300,50), lastName).ToUpper();
				if (GUI.Button(new Rect(Screen.width*0.3f+170f,500,250,70),"DONE")){
					loginSound.Play();
					if(!firstName.Equals("") && !lastName.Equals("")){
						playerName = firstName + " " + lastName;
						PlayerPrefs.SetString("PlayerName", playerName);
						SetDataToDataKeeper(); 
					}
				}
			}

			else if(isLoggedIn){
				guiSkin.label.fontSize = 30;
				GUI.Label(new Rect(Screen.width*0.3f,150,600,70), "WELCOME "+DataKeeper.dk.currentPlayerName+"!");
				if(GUI.Button(new Rect(Screen.width*0.3f+170f,215,250,70),"PLAY")){
					buttonSound.Play();
					DataKeeper.dk.laneDepartures.Clear();
					DataKeeper.dk.numberOfForcedLaneDepartures = 0;
					DataKeeper.dk.totalElapsedAutoSteeringTime = 0;
					DataKeeper.dk.randomMsgsEnabled = true;
					DataKeeper.dk.isPractice = false;
					int signal = SetRandomSignalToTest();
					if(signal == -1){
						showNoSignalsLeftError = true;
					}
					else{
						showNoSignalsLeftError = false;
						StartCoroutine("StartDriving");
					}
				}

				if(showNoSignalsLeftError){
					GUI.Label(new Rect(Screen.width*0.3f+420f,235,250,70),"NO SIGNALS LEFT");
					if(Input.GetKeyDown(KeyCode.Escape)){
						showNoSignalsLeftError = false;
					}
				}

				if(GUI.Button(new Rect(Screen.width*0.3f+170f,315,250,70),"PRACTICE")){
					DataKeeper.dk.laneDepartures.Clear();
					DataKeeper.dk.numberOfForcedLaneDepartures = 0;
					DataKeeper.dk.totalElapsedAutoSteeringTime = 0;
					DataKeeper.dk.randomMsgsEnabled = false;
					DataKeeper.dk.isPractice = true;
					DataKeeper.dk.currentSignal = DataKeeper.dk.practiceSignal;
					StartCoroutine("StartDriving");
				}

				if(GUI.Button(new Rect(Screen.width*0.3f+170f,415,250,70),"STATS")){
					SetDataToDataKeeper();
					statsAreShowing = true;
				}

				if(GUI.Button(new Rect(Screen.width*0.3f+170f,515,250,70),"SURVEY")){
					surveyIsShowing = true;
				}
			}
				
		}else if (statsAreShowing){
			if (!showOnlyLaneDeps){
				guiSkin.label.fontSize = 30;
				GUI.Label(new Rect(Screen.width*0.3f,200,600,70), "STATS FOR "+playerName);
				guiSkin.label.fontSize = 20;
				guiSkin.label.alignment = TextAnchor.MiddleLeft;
				GUI.Label(new Rect(Screen.width*0.3f,300,500,70), "RUNS: "+DataKeeper.dk.numberOfRuns);
				if(DataKeeper.dk.testedSignals.Count == 0){
					GUI.Label(new Rect(Screen.width*0.3f,350,500,70), "TESTED SIGNALS: NONE");
				} else{
					string tested = "";
					foreach(int element in DataKeeper.dk.testedSignals){
						tested += " "+element;
					}
					GUI.Label(new Rect(Screen.width*0.3f,350,500,70), "TESTED SIGNALS: "+tested);
				}

				if(DataKeeper.dk.remainingSignals.Count == 0){
					GUI.Label(new Rect(Screen.width*0.3f,400,500,70), "REMAINING SIGNALS: NONE");
				}
				else{
					string remaining = "";
					foreach(int element in DataKeeper.dk.remainingSignals){
						remaining += " "+element;
					}
					GUI.Label(new Rect(Screen.width*0.3f,400,500,70), "REMAINING SIGNALS: "+remaining);
				}
				GUI.Label(new Rect(Screen.width*0.3f,450,500,70), "LANE DEPARTURES THIS RUN: "+DataKeeper.dk.numberOfLaneDepartures);

				guiSkin.button.fontSize = 20;
				if (GUI.Button(new Rect(Screen.width*0.3f,500,500,70), "VIEW LANE DEPARTURES")){
					showOnlyLaneDeps = true;
					retrievedLaneDeps = "";
					retrievedLaneDeps = GetAllLaneDepartures();
				}
			
			}
			else if(showOnlyLaneDeps){
				guiSkin.textField.fontSize = 10;
				//guiSkin.textField.alignment= TextAnchor.UpperLeft;
				GUI.TextField(new Rect(Screen.width*0.3f-200f,200,1000,300),retrievedLaneDeps); 
				guiSkin.textField.fontSize = 20;
				//guiSkin.textField.alignment= TextAnchor.MiddleCenter;
				guiSkin.button.fontSize = 20;
				if(!printToFileButtonPressed){
					if(GUI.Button(new Rect(Screen.width*0.3f,Screen.height-70,250,70),"PRINT TO FILE")){
						if(DataKeeper.dk.laneDepartures.Count !=0){
							printToFileButtonPressed = true;
						}
					}
				}
				if(printToFileButtonPressed){
						if(GUI.Button(new Rect(Screen.width*0.4f,850,250,70),"SURE?")){
							fileNameForLaneDeps = PrintToFile("stats");
						}
						if(fileNameForLaneDeps != "false" || fileNameForLaneDeps != ""){
							guiSkin.textField.fontSize = 15;
							guiSkin.textField.alignment = TextAnchor.MiddleLeft;	
							GUI.TextField(new Rect(Screen.width*0.4f+300,850,500,70),fileNameForLaneDeps);
						}
						if(Input.GetKeyDown(KeyCode.Escape)){
							printToFileButtonPressed = false;
						}
				}
				if(Input.GetKeyDown(KeyCode.Escape)){
					showOnlyLaneDeps = false;
				}
			}
			guiSkin.button.fontSize = 30;
			if(GUI.Button (new Rect(0,Screen.height-100f,150,100),"BACK") || Input.GetKeyDown(KeyCode.Escape)){
				statsAreShowing = false;
			}
		} else if(surveyIsShowing){
			scrollPosition = GUI.BeginScrollView(new Rect(0, 0, Screen.width, Screen.height), scrollPosition, new Rect(0, 0, 1500f, 1000f));
			guiSkin.label.fontSize = 20;
			GUI.Label(new Rect(Screen.width*0.2f,0,1200,70), "PLEASE LISTEN TO THE 4 SOUNDS AGAIN AND ANSWER THE QUESTIONS.");
			GUI.Label(new Rect(Screen.width*0.2f,30,1000,70), "四つの音をもう一度聴いて、次の質問に答えてください。");

			guiSkin.button.fontSize = 20;
			if(GUI.Button(new Rect(Screen.width*0.3f,100,140,70),"SOUND 0")){
				sound0.Play();
			}
			if(GUI.Button(new Rect(Screen.width*0.3f+200,100,140,70),"SOUND 1")){
				sound1.Play();
			}
			if(GUI.Button(new Rect(Screen.width*0.3f+400,100,140,70),"SOUND 2")){
				sound2.Play();
			}
			if(GUI.Button(new Rect(Screen.width*0.3f+600,100,140,70),"SOUND 3")){
				sound3.Play();
			}
			guiSkin.label.font=questionFont;

			GUI.Label(new Rect(Screen.width*0.2f,200,1200,70), "1. How URGENT do you think the sound is?" +
															" Rate each sound from 1 to 5 (1 = Not urgent, 5 = Very urgent)");
		
			GUI.Label(new Rect(Screen.width*0.2f,240,1250,70), "1. この音はどれくらい緊急だと思いますか？それぞれの音を聞いた後、1から5まで格付けしてください。" +
			          "(1=緊急でない、5=非常に緊急)。");

			urg0 = GUI.TextField(new Rect(Screen.width*0.3f,310,140,70), urg0);
			urg1 = GUI.TextField(new Rect(Screen.width*0.3f+200,310,140,70), urg1);
			urg2 = GUI.TextField(new Rect(Screen.width*0.3f+400,310,140,70), urg2);
			urg3 = GUI.TextField(new Rect(Screen.width*0.3f+600,310,140,70), urg3);

			GUI.Label(new Rect(Screen.width*0.2f,400,1200,70), "2. How ANNOYING do you think the sound is?" +
			          " Rate each sound from 1 to 5 (1 = Not annoying, 5 = Very annoying)");
			
			GUI.Label(new Rect(Screen.width*0.2f,440,1250,70), "2.この音はどれくらい迷惑だと思いますか？"
			          +"それぞれの音を聞いた後、1から5まで格付けしてください。(1=迷惑でない、5=非常に迷惑)。");

			ann0 = GUI.TextField(new Rect(Screen.width*0.3f,510,140,70), ann0);
			ann1 = GUI.TextField(new Rect(Screen.width*0.3f+200,510,140,70), ann1);
			ann2 = GUI.TextField(new Rect(Screen.width*0.3f+400,510,140,70), ann2);
			ann3 = GUI.TextField(new Rect(Screen.width*0.3f+600,510,140,70), ann3);

			GUI.Label(new Rect(Screen.width*0.2f,600,1000,70), "3. Would you ACCEPT the sound as a warning signal if it were used in a lane departure situation?" +
			          "\n Rate each sound from 1 to 5 (1 = Would not accept it at all, 5 = Would accept it very much)");
			
			GUI.Label(new Rect(Screen.width*0.2f,660,1050,70), "3.この音はレーン逸脱の警報音とみなすことができますか。" +
			          "それぞれの音を聞いた後、\n1から5まで格付けしてください。(1=全くみなすことができない、5=とてもみなすことができる）");

			acc0 = GUI.TextField(new Rect(Screen.width*0.3f,730,140,70), acc0);
			acc1 = GUI.TextField(new Rect(Screen.width*0.3f+200,730,140,70), acc1);
			acc2 = GUI.TextField(new Rect(Screen.width*0.3f+400,730,140,70), acc2);
			acc3 = GUI.TextField(new Rect(Screen.width*0.3f+600,730,140,70), acc3);

			if(GUI.Button(new Rect(Screen.width*0.3f,840,250,70),"SUBMIT")){
						fileNameForSurvey = PrintToFile("survey");
						printSurveyButtonPressed = true;
			}
			if(printSurveyButtonPressed){
					if(fileNameForSurvey != "false" || fileNameForSurvey != ""){
						guiSkin.textField.fontSize = 15;
						guiSkin.textField.alignment = TextAnchor.MiddleLeft;	
						GUI.TextField(new Rect(Screen.width*0.3f+300,840,500,70),fileNameForSurvey);
						guiSkin.textField.fontSize = 20;
						guiSkin.textField.alignment = TextAnchor.MiddleCenter;	
					}	
			}
			if(Input.GetKeyDown(KeyCode.Escape)){
				surveyIsShowing = false;
				printSurveyButtonPressed = false;
			}
			GUI.EndScrollView();
		}
	}
	
	void SetDataToDataKeeper(){
		DataKeeper.dk.remainingSignals.Clear();
		DataKeeper.dk.testedSignals.Clear();
		DataKeeper.dk.currentPlayerName = playerName;
		DataKeeper.dk.numberOfRuns = PlayerPrefs.GetInt(playerName+"Runs");
		DataKeeper.dk.isLoggedIn = true;
		for(int i = 0; i < DataKeeper.dk.signals.Length; i++){
			int testedOrNot = PlayerPrefs.GetInt(playerName+"Signal"+i);
			DataKeeper.dk.signals[i] = testedOrNot;
			if(testedOrNot == 0)
				DataKeeper.dk.remainingSignals.Add(i);
			else{
				DataKeeper.dk.testedSignals.Add(i);
			}
		}
	}

	int SetRandomSignalToTest(){
		if (DataKeeper.dk.remainingSignals.Count != 0){
			int random = Random.Range(0,DataKeeper.dk.remainingSignals.Count);
			int randomSignal = (int)DataKeeper.dk.remainingSignals[random];
			DataKeeper.dk.currentSignal = randomSignal;
			return randomSignal;
		}else{
			return -1;
		}
	}
		
	string GetAllLaneDepartures(){
		string laneDeps = "";
		ArrayList list = DataKeeper.dk.laneDepartures;
		foreach(LaneDeparture ld in list){
			laneDeps += ld.GetInfo()+"\n";
		}
		return laneDeps;
	}
	
	string PrintToFile(string dataType){
		string fileName = "";
		if(dataType.Equals("stats")){
			fileName = Application.persistentDataPath + "/" + playerName+"_Run"+DataKeeper.dk.numberOfRuns+"_"+Random.Range(0,10000)+".txt";
			Debug.Log(fileName);
			if (File.Exists(fileName)) 
			{
				return "false";
			}
			StreamWriter sw = File.CreateText(fileName);
			sw.WriteLine("TimeStamp, Player, ReactionTime, SteeringDegree, Signal, "+ 
			             "depSide, straightZoneNumber, isForcedDeparture, isCorrectResponse, testRunNumber");
			foreach (LaneDeparture ld in DataKeeper.dk.laneDepartures){
				sw.WriteLine(ld.GetInfo());
			}
			sw.Close();
		} else if (dataType.Equals("survey")){
			fileName = Application.persistentDataPath + "/" + playerName+"_SURVEY"+"_"+Random.Range(0,10000)+".txt";
			if (File.Exists(fileName)) 
			{
				return "false";
			}
			StreamWriter sw = File.CreateText(fileName);
			sw.WriteLine("TimeStamp, Player, "+ 
			             "URG0, URG1, URG2, URG3, ANN0, ANN1, ANN2, ANN3, ACC0, ACC1, ACC2, ACC3");
			string data = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:ff")+", "+playerName+", "+urg0+", "+urg1+", "+urg2+", "+urg3+", "+ann0+", "+ann1+", "+ann2+", "+ann3+", "+acc0+", "+acc1+", "+acc2+", "+acc3;
			sw.WriteLine(data);
			sw.Close();
		}
		return fileName;	
	}

	IEnumerator StartDriving(){
		yield return new WaitForSeconds(0.5f);
		SceneManager.LoadScene(drivingScene);
	}
}