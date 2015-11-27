using UnityEngine;
using System.Collections;

public class PlayerMovement : MonoBehaviour
{


    public float offLevel;
    public bool noGround;
    public bool Finished;


    // Use this for initialization
    void Start()
    {
        noGround = false;

    }

    // Update is called once per frame
    void Update()
    {

        if (GameHandler.Instance.HasStarted && !GameHandler.Instance.hasFallen)
        {
            #region Inputs
            /** De algemene movement / rotatie van het object */
            GameHandler.Instance.CalculateMovement(Time.deltaTime);
            if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
            {
                transform.eulerAngles += Vector3.down * GameHandler.Instance.RotationFactor * Time.deltaTime;
            }
            if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
            {
                transform.eulerAngles += Vector3.up * GameHandler.Instance.RotationFactor * Time.deltaTime;
            }

            transform.position += transform.forward * GameHandler.Instance.MovementSpeed * Time.deltaTime;

            if (Input.GetKeyDown(KeyCode.Escape)) { Application.LoadLevel(Application.loadedLevel); }
            #endregion

            #region Raycasting and Resetting

            Vector3 pos = transform.position;
            Vector3 extents = gameObject.GetComponent<Renderer>().bounds.extents;
            
            noGround = true;
            for (int x = -1; x <= 1; x++)
            {
                for (int y = -1; y <= 1; y++)
                {
                    Ray ray = new Ray(pos + new Vector3(x * (extents.x / 1.5f), 0f, y * (extents.y / 1.5f)), Vector3.down);
                    Debug.DrawRay(pos + new Vector3(x * (extents.x / 1.5f), 0f, y * (extents.y / 1.5f)), Vector3.down, Color.red);
                    RaycastHit hit;
                    if (Physics.Raycast(ray, out hit, 2.5f))
                    {
                        noGround = false;

                        switch(hit.collider.tag)
                        {
                            case "Ice":
                                GameHandler.Instance.Friction = 0;
                                break;

                            case "Grass":
                                GameHandler.Instance.Friction = 15;
                                break;

                            case "Boost":
                                PlateauScript script = hit.collider.GetComponent<PlateauScript>();
                                if (!script.wasUsed) GameHandler.Instance.PushForce = script.BoostAmount;
                                break;

                            case "Checkpoint":
                                if (GameHandler.Instance.MovementSpeed != 0)
                                {
                                    GameHandler.Instance.LastCheckpoint = hit.collider.gameObject;
                                    GameHandler.Instance.CheckpointSpeed = GameHandler.Instance.MovementSpeed;
                                }
                                break;

                            case "Finish":
                                Finished = true;
                                GameHandler.Instance.msEind = GameHandler.Instance.MovementSpeed;
                                break;

                            case "Terrain":
                                //Reset Level
                                GameHandler.Instance.hasFallen = true;

                                GameHandler.Instance.WriteData("Terrain");
                                GameObject gO = GameObject.Find("_GameHandler");
                                GUIHandler gH = gO.GetComponent<GUIHandler>();
                                gH.resetGUI.SetActive(true);
                                break;

                        }

                        #region Old hit-register-handling
                        //if (hit.collider.tag == "Boost")
                        //{
                        //    PlateauScript script = hit.collider.GetComponent<PlateauScript>();
                        //    if (!script.wasUsed) GameHandler.Instance.PushForce = script.BoostAmount;
                        //}
                        //if (hit.collider.tag == "Checkpoint" && GameHandler.Instance.MovementSpeed != 0)
                        //{
                        //    GameHandler.Instance.LastCheckpoint = hit.collider.gameObject;
                        //    GameHandler.Instance.CheckpointSpeed = GameHandler.Instance.MovementSpeed;
                        //}
                        //if(hit.collider.tag =="Finish" && GameHandler.Instance.MovementSpeed != 0)
                        //{
                        //    Finished = true;

                        //}
                        //if(hit.collider.tag=="Terrain")
                        //{
                        //    //Reset Level
                        //    GameHandler.Instance.hasFallen = true;


                        //    GameObject gO = GameObject.Find("_GameHandler");
                        //    GUIHandler gH = gO.GetComponent<GUIHandler>();
                        //    gH.resetGUI.SetActive(true);
                        //}
                        #endregion
                    }
                }
            }

            if(noGround)
            {
                offLevel += Time.deltaTime;
                if (offLevel > .75f & !GameHandler.Instance.hasFallen)
                {
                    //Reset Level
                    GameHandler.Instance.hasFallen = true;
                    GameHandler.Instance.WriteData("OffTrack");

                    GameObject gO = GameObject.Find("_GameHandler");
                    GUIHandler gH = gO.GetComponent<GUIHandler>();
                    gH.resetGUI.SetActive(true);

                    Debug.Log("Afgefalluuuhh");
                    offLevel = 0;
                }
            }
            else { offLevel = 0; }

            #endregion
        }


        //Afhandelen wat er gebeurt als het level gehaald is
        if(Finished)
        {
            if (GameHandler.Instance.MovementSpeed > 0) GameHandler.Instance.MovementSpeed -= (30f * Time.deltaTime);
        }

        if(Finished && GameHandler.Instance.MovementSpeed == 0)
        {
            Finished = false;
            GameHandler.Instance.HasStarted = false;
            GameHandler.Instance.GameGUI.SetActive(false);
            GameHandler.Instance.EndGameGUI.SetActive(true);
            GameHandler.Instance.ConstantForce = 0;
            GameHandler.Instance.Friction = 0;
            Debug.Log("Finished");
            GameHandler.Instance.WriteData("Finished");
        }

    }

}

