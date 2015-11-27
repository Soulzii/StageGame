using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GUIHandler : MonoBehaviour
{

    public Text VKrachtSlider;
    public Text DKrachtSlider;
    public Text MassaSlider;
    public GameObject GUI;
    public GameObject resetGUI;
    public Text CountdownText;
    public GameObject player;
    float VKValue;
    float DKValue;
    float Mass;
    float timer;
    public bool timerBool;

    // Use this for initialization
    void Start()
    {
        timer = 3;
        DKValue = 0;
        VKValue = 0;
    }

    #region Slider-handler

    public void UpdateMassSliderText(float value)
    {
        Mass = value;
        if (MassaSlider != null) MassaSlider.text = value.ToString("n1");
    }

    public void UpdateVKrachtSliderText(float value)
    {
        VKValue = value;
        if (VKrachtSlider != null) VKrachtSlider.text = value.ToString("n1");
    }

    public void UpdateDKrachtSliderText(float value)
    {
        DKValue = value;
        if (DKrachtSlider != null) DKrachtSlider.text = value.ToString("n0");
    }
    #endregion


    /**Start het level en zet alle waardes goed*/
    public void StartLevel()
    {
        GameHandler.Instance.ConstantForce = VKValue;
        GameHandler.Instance.PushForce = DKValue;
        if (Mass != 0) GameHandler.Instance.Mass = Mass;
        GameHandler.Instance.HasStarted = true;
        GameHandler.Instance.MovementSpeed = 0.001f;
        GUI.SetActive(false);
        GameHandler.Instance.dKStart = DKValue;
        GameHandler.Instance.vKStart = VKValue;
        Debug.Log("Start Game");
    }


    public void ResetLevel()
    {
        Application.LoadLevel(Application.loadedLevel);
    }

    #region Reset Position Checkpoint

    public void LastCheckpoint()
    {
        if (GameHandler.Instance.LastCheckpoint != null)
        {
            //Zet movement uit
            GameHandler.Instance.MovementSpeed = 0;

            //Gui laten verdwijnen
            resetGUI.SetActive(false);

            //Zet speler op laatste checkpoint
            player.transform.position = new Vector3((GameHandler.Instance.LastCheckpoint.transform.position.x), 1.05f, GameHandler.Instance.LastCheckpoint.transform.position.z);

            //Zet de timer aan
            timerBool = true;
            CountdownText.transform.position = new Vector2(200, 200);


            //Laat de camera terugschieten naar de speler
            GameObject pCamera = GameObject.FindGameObjectWithTag("MainCamera");
            pCamera.transform.position = player.transform.position;


            GameHandler.Instance.hasFallen = false;
        }
        else
        {
            Application.LoadLevel(Application.loadedLevel);
        }
    }

    void Update()
    {
        if (timerBool)
        {
            player.transform.rotation = Quaternion.Euler(new Vector3(0, 180 + GameHandler.Instance.LastCheckpoint.transform.eulerAngles.y, 0));
            Debug.Log(GameHandler.Instance.LastCheckpoint.transform.eulerAngles.y);
            timer -= Time.deltaTime;
            CountdownText.text = timer.ToString("n0");

            if (timer <= 0)
            {
                //Zet de movementspeed gelijk aan die van wanneer de speler het checkpoint gehaald heeft.
                GameHandler.Instance.MovementSpeed = GameHandler.Instance.CheckpointSpeed;
                timerBool = false;
                timer = 3;
            }
        }
        else
        {
            CountdownText.transform.position = new Vector2(10000, 10000);
        }
    }
    #endregion

}
