using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.IO;
using System;
using System.Collections.Generic;

public class GameHandler : MonoBehaviour
{

    public static GameHandler Instance { get; private set; }

    /** Snelheid van het object*/
    public float MovementSpeed;
    public Text MSText;
    public GameObject Pointer;
    /** Rotatie waarmee het object roteerd*/
    public float RotationFactor;
    /** De Constante kracht die op het object wordt uitgeoefend*/
    public float ConstantForce;
    /** De wrijving die aanwezig is*/
    public float Friction;
    /** De massa van het object*/
    public float Mass;
    /** De kracht van de eerste duw*/
    public float PushForce;
    public Text AccText;
    public Slider AccSlider;
    public float PushTime = 0;

    /** GUI tijdens en na de game*/
    public GameObject GameGUI;
    public GameObject EndGameGUI;

    /** */
    public float Score;
    public Text ScoreText;

    public bool HasStarted;
    public bool hasFallen;

    public GameObject LastCheckpoint;
    public float CheckpointSpeed;

    //Negatieve and positieve acceleration position voor de pijl
    public Vector3 pAPos;
    public Vector3 nAPos;


    /* Waardes die opgeslagen worden in XML**/
    public float dKStart;
    public float vKStart;
    public float msEind;


    //Movement calculation en snelheidsmeters etc. veranderen
    public void CalculateMovement(float dT)
    {
        float accel = (PushForce + ConstantForce - Friction) / Mass;

        if (MovementSpeed == 0 && PushForce == 0)
        {
            accel = 0;
        }


        {
            AccText.text = accel.ToString("n2") + "m/s²";
            if (AccSlider.value > accel) { AccSlider.value -= Time.deltaTime * 25; }
            if (AccSlider.value < accel) { AccSlider.value += Time.deltaTime * 25; }
        }


        if (PushForce != 0)
        {
            PushTime += dT;
            if (PushTime >= .75f)
            {
                PushForce = 0;
                PushTime = 0;
            }
        }
        if (MovementSpeed >= 0.001f)
        {
            MovementSpeed += accel * dT;

        }
        if (PushForce == 0 && MovementSpeed <= 0.001f)
        {
            MovementSpeed = 0; if (!GameObject.Find("_GameHandler").GetComponent<GUIHandler>().resetGUI.activeInHierarchy && !GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>().Finished && !GameObject.Find("_GameHandler").GetComponent<GUIHandler>().timerBool)
            {
                GameObject.Find("_GameHandler").GetComponent<GUIHandler>().resetGUI.SetActive(true); WriteData("Stilstand");
            }
        }
       


        MSText.text = MovementSpeed.ToString("n1");
        Pointer.transform.rotation = Quaternion.Euler(0, 0, 120 - (MovementSpeed * 6));


    }

    //Coin pickup
    public void PickUp(float val)
    {
        Score += val;
        ScoreText.text = "Score : " + Score.ToString();
    }

    //Singleton
    void Awake()
    {
        Instance = this;
    }

    public void ToMainMenu()
    {
        Application.LoadLevel("MainMenu");
    }


    #region WriteLevelToXML
    /**Haalt alle levels in een file op, mits de file bestaat en voegt er een nieuwe entry aan toe*/
    public void WriteData(string reason)
    {

        LevelContainer levelCollection;
        List<Level> levels;

        /**Als file bestaat haal alle bestaaned data op en voeg een entry toe, anders maak een bestand */
        if (File.Exists(Path.Combine(Application.persistentDataPath, "gamedata.xml")))
        {
            Debug.Log("File Found");
            levelCollection = LevelContainer.Load(Path.Combine(Application.persistentDataPath, "gamedata.xml"));
            levels = levelCollection.Levels;
            bool hasLevel = false;
            foreach (Level l in levels)
            {
                if (l.lastLevel == Application.loadedLevel - 1)
                    hasLevel = true;
            }
            if (!hasLevel)
                levels.Add(new Level(Application.loadedLevel - 1));


            foreach (Level l in levels)
            {
                if(l.lastLevel == Application.loadedLevel - 1)
                {
                    l.AddPoging(msEind, vKStart, dKStart, Mass, Score, reason, l.pogingen.Count + 1);

                }
            }
            Debug.Log(Path.Combine(Application.persistentDataPath, "gamedata.xml"));
            levelCollection.Save(Path.Combine(Application.persistentDataPath, "gamedata.xml"));
            
        }
        else 
        {
            levels = new List<Level>();
            levelCollection = new LevelContainer(levels);
            levels.Add(new Level(Application.loadedLevel - 1));
            foreach (Level l in levels)
            {
                if (l.lastLevel == Application.loadedLevel - 1)
                {
                    l.AddPoging(msEind, vKStart, dKStart, Mass, Score, reason, 1);
                }
            }
            Debug.Log(Path.Combine(Application.persistentDataPath, "gamedata.xml"));
            levelCollection.Save(Path.Combine(Application.persistentDataPath, "gamedata.xml"));
        }

    }

    #endregion



}
