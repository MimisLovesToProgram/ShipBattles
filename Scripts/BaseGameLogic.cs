using UnityEngine.EventSystems;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//using Unity.Services.Core;
//using Unity.Services.Mediation;

public class BaseGameLogic : MonoBehaviour
{

    //The Camera's AudioSource:
    public AudioSource CameraAudioSource;

    //A generic ships' audio source:
    public AudioSource ShipsAudioSource;

    //The Mute button, and a bool to tell us if the game is muted or not:
    public RectTransform MuteButton;
    public bool GameIsMuted = false;

    //The Mute/Unmute images:
    public Sprite EnabledAudioImage;
    public Sprite DisabledAudioImage;

    //Our AudioClips, with their volume values:
    public AudioClip OceanWavesNseagulls;

    [Range(0,1)]
    public float OceanWavesVol = .2f;
    public AudioClip WindSound;

    [Range(0,1)]
    public float WindVol = .7f;
    public AudioClip CannonBoom;

    [Range(0,1)]
    public float CannonVol = .3f;
    public AudioClip ShipHit;

    [Range(0,1)]
    public float ShipHitVol = .35f;

    //Those 2 should only be used on the ShipsAudioSource.
    public AudioClip ShipHornSound;

    [Range(0,1)]
    public float ShipHornVol = 1;

    //for money using:
    public AudioClip MoneyUsageSF;

    [Range(0,1)]
    public float MoneyVol = .9f;

    //A bool determining whether the game as ended:
    public bool GameEnded;

    public enum ThingsLeftToDo
    {
        SpawnShips,
        PlayTime
    }
    public static ThingsLeftToDo currentThing;

    public static Dictionary<Vector3,Ship> ShipPositions = new Dictionary<Vector3, Ship>();

    //A dictionary for both seas and islands:
    public static Dictionary<Vector3, GameObject> IslandAndSeaPositions = new Dictionary<Vector3, GameObject>();

    [Tooltip("LayerMask of everything")]
    public LayerMask EverythingLayerMask;
    
    [Tooltip("Determines whether its blue's turn or not.")]
    public static bool blueTurn = true;

    [Tooltip("The hilighter.")]
    public Transform hilighter;

    [Tooltip("Determines if the hilighter is active:")]
    public bool hilighterIsActive;
    public RectTransform DeployButtonPanel;

    [Tooltip("The Winning Panel, and its text right beneath")]
    public RectTransform WinningPanel;
    public Text WinningText;

    [Tooltip("Determines if the deploy button panel is active:")]
    public bool deployButtonPanelIsActive;

    [Tooltip("Popup prefab.")]
    public GameObject PopupPanelPrefab;

    //The active popup panel:
    public GameObject ActivePopupPanel;

    //A reference to the Canvas in order to create the new popup panel as its child:
    public RectTransform CanvasRectTrans;

    //The Task Panel's Canvas Group in order to make it fade.
    public CanvasGroup PopupPanelCanvasGroup;
    
    //An int that controls the fading of the taskPanel:
    public static int FadingState = 0;

    //A bool to fade out the Mode Selection Panel and its Image:
    public static bool ModeFadingStarted = false;
    public CanvasGroup ModeImageCanvasGroup;
    public CanvasGroup MapSizePanelCanvasGroup;

    //The MapSize Panel:
    public RectTransform MapSizePanel;

    //The Stats Panel, with its texts:
    public RectTransform SelectionStatsPanel;
    public Text ShipName;
    public Text HealthText;
    public Text DamageText;
    public Text UnitsPerTurnText;
    public Text MaxAttackRangeText;

    //The Full, Imaged stats panel, and then references to its images:
    public RectTransform StatsPanel;
    public Image BlueShipImage1;
    public Image BlueShipImage2;
    public Image BlueSpeedyShipImage1;
    public Image BlueSpeedyShipImage2;
    public Image BlueArmedShipImage;
    public Image BlueToughShipImage;
    public Image RedShipImage1;
    public Image RedShipImage2;
    public Image RedSpeedyShipImage1;
    public Image RedSpeedyShipImage2;
    public Image RedArmedShipImage;
    public Image RedToughShipImage;

    //Also, the stats button:
    public RectTransform StatsButton;

    //The time remaining text, and some ints/bool for time counting:
    public Text TimeRemainingText;
    public float lastTimePicked;
    public int secondsLeft = 30;
    public bool TimePickedOnce = false;

    [Header("The ship prefabs:")]
    public Ship RedShipPrefab;
    public Ship RedSpeedyShipPrefab;
    public Ship RedArmedShipPrefab;
    public Ship RedToughShipPrefab;
    public Ship BlueShipPrefab;
    public Ship BlueSpeedyShipPrefab;
    public Ship BlueArmedShipPrefab;
    public Ship BlueToughShipPrefab;

    //Another random prefab:
    public Cannonball CannonballPrefab;

    //The ship buttons:
    public RectTransform AttackButton;
    public RectTransform MovementButton;
    public RectTransform AttackOrMovementXButton;
    public RectTransform RandomShipGenerationButton;
    public RectTransform SurrenderButton;

    [Header("The ships remaining to deploy and their texts:")]
    public int ShipsRemainingAmount = 2;
    public Text ShipButtonText;
    public int SpeedyShipsRemainingAmount = 2;
    public Text SpeedyShipButtonText;
    public int ArmedShipRemainingAmount = 1;
    public Text ArmedShipButtonText;
    public int ToughShipRemainingAmount = 1;
    public Text ToughShipButtonText;

    [Header("The ship references.")]
    public Ship BlueShip;
    public Ship SecondBlueShip;
    public Ship RedShip;
    public Ship SecondRedShip;
    public Ship BlueSpeedyShip;
    public Ship SecondBlueSpeedyShip;
    public Ship RedSpeedyShip;
    public Ship SecondRedSpeedyShip;
    public Ship BlueArmedShip;
    public Ship RedArmedShip;
    public Ship BlueToughShip;
    public Ship RedToughShip;
    public Ship[] Ships;

    //The Red/Blue ship we hit/selected:
    public Ship SelectedShip;

    //The ship the cannonball hit:
    public Ship AttackedShip;

    public Quaternion RedShipsRotation;
    public static bool RandomShipsGenerationButtonClickedOnce = false;

    //A harbour Script:
    public static Harbour HarbourScript;

    //The Blue/Red Harbour layer mask in order to fix this annoying bug:
    public LayerMask BlueHarbourLayerMask;
    public LayerMask RedHarbourLayerMask;

    //The Gamemode Selection panel:
    public RectTransform GamemodeSelectionPanel;

    //References to the harbours/seas that will be disabled when a smaller map is being sellected(if its 8, both arrays are disabled, not just the 8):
    public GameObject[] HarboursAndSeasToGoIn9;
    public GameObject[] HarboursAndSeasToGoIn8;

    //Harbours/Seas to be moved 1/2 units on the Z axis when the map gets smaller:
    public GameObject[] HarboursAndSeasToBeMoved;

    //The number of seas/harbours to be disabled after a button is clicked:
    public int NumOfHarboursToBeDisabled = 0;

    //Defines if the ship images have been moved:
    public bool ImagesMoved = false;

    //The Image behind the panel above, and its Text underneath:
    public Image GamemodeSelectionImage;
    public Text GamemodeSelectionImageText;

    //The Score texts:
    public Text BlueScoreText;
    public Text RedScoreText;

    //Variables measuring the times each side has won:
    public int timesBlueWon;
    public int timesRedWon;
    public bool ScoreUpdated;

    //A bool to tell us whether the player wants to play alone or with a friend, and another bool defining whether this value is picked:
    public static bool SinglePlayerGame = false;
    public static bool GamemodePicked = false;

    //properties that show if the blue fleet is dead or the red:
    public bool BlueFleetIsDead
    {
        get
        {
            return (!Ships[0] && !Ships[1] && !Ships[2] && !Ships[3] && !Ships[4] && !Ships[5] && currentThing == ThingsLeftToDo.PlayTime);
        }
    }

    public bool RedFleetIsDead
    {
        get
        {
            return (!Ships[6] && !Ships[7] && !Ships[8] && !Ships[9] && !Ships[10] && !Ships[11] && currentThing == ThingsLeftToDo.PlayTime);
        }
    }

    //The ship the AI has targeted to attack, and a bool to mark that we have got it:
    public Ship TargetedAIShip;
    public Ship ShipAIattackComesFrom;
    public static bool TargetedAIShipPicked;
    public static string directionTargetedShipIsAt;

    //The Ship the AI wants to move, and a bool to mark that we have got it:
    public Ship AIShipToMove;
    public static bool AIShipToMovePicked;

    //Determines whether the AI did something:
    public static bool AIdidSomething;
    public static bool AIstartedAttacking;
    public static bool RandomSidePicked;

    //0 = not picked, 1 = true, 2 = false.
    public int LeftSidePicked = 0;

    //The Island the AI wants to spawn a ship at (for buying):
    public static bool AIgotBuyingHarbour = false;

    //A bool to tell if we got the random values that the AI will use to decide which ship it should buy:
    public static bool AIgotRandomBuyingValues = false;

    //A bool to tell if we can play the horn sound for the AI again:
    public static bool AIcanHornAgain = true;

    //Bools that tell us which gamemode was active during the prevuous round:
    public static bool PreviousGamemodeWasAI;
    public static bool PreviousGamemodeWas2;

    //Stuff about the game's currency:
    public float BlueTreasury = 0;

    //The difference between the previous and current treasury value(Blue player):
    public float BlueValueDifference;

    //The difference between the previous and current treasury value(Red player):
    public float RedValueDifference;
    public static int BlueIslands = 0;
    public float RedTreasury = 0;
    public static int RedIslands = 0;
    public RectTransform TreasuryPanel;
    public Text TreasuryText;
    public RectTransform ShipBuyingPanel;
    public static bool ShipBuyingPanelIsActive;

    //A bool that starts generating the islands:
    public static bool CanGenerateIslands = false;

    //The Tutorial-related stuff (Continues until the Hilighter positioning):
    public RectTransform TutorialIntroduction;
    public RectTransform TutorialMapSelection;
    public RectTransform TutorialManualDeployment;
    public RectTransform TutorialRandomShipGeneration;
    public RectTransform TutorialStatsTimeSurrendering;
    public RectTransform TutorialShipSelection;
    public RectTransform TutorialMovement;
    public RectTransform TutorialAttacking;
    public RectTransform TutorialIslandFlags;
    public RectTransform TutorialShipBuying;
    public RectTransform TutorialHarbourInvasion;

    //The red/blue ship layer masks:
    public LayerMask RedShipLayerMask;
    public LayerMask BlueShipLayerMask;

    //The 4 different Loading Screens:
    public Sprite Screen1;
    public Sprite Screen2;
    public Sprite Screen3;
    public Sprite Screen4;

    //The 5 different Facts/Tips on the loading screen:
    public string Fact1 = "Did you know there are exactly 7700 different maps in the game?";
    public string Fact2 = "Send your ships back to your harbour in order to repair them.";
    public string Fact3 = "Make sure to claim some islands for your team in every game. They can be very useful.";
    public string Fact4 = "Defend your islands by leaving a ship-guard next to them.";
    public string Fact5 = "Did you know that the game is made of more than 4100 lines of code?";

    //The Left/Right Side Wall:
    public Transform LeftSideWall;
    public Transform RightSideWall;

    //The sand on the back of the map to cover the void:
    public Transform Sand;

    //The animation curve to get the ship's speed:
    public AnimationCurve SpeedAnimation;

    // The speedy ships' curve, so that they don't look like they stop:
    public AnimationCurve SpeedyShipAnimation;

    //The coin increasing curve:
    public AnimationCurve CoinAnimation;

    //The time to pick the value of the CoinAnimation above:
    public float CoinTime = 0;
    
    //The time we need to pass to the Evaluate method to return our value:
    public float CurveTime;

    //An Interstial Ad:
    // IInterstitialAd RoundEndAd;

    void SelectShip()
    {
        if((((!SinglePlayerGame || SinglePlayerGame && !blueTurn) && !SelectedShip) || SelectedShip && !SelectedShip.AttackTileHolderIsActive) && currentThing == ThingsLeftToDo.PlayTime)
        {
            AIdidSomething = false;

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if(Input.GetMouseButtonDown(0))
            {
                if(blueTurn && Ship.numberOfTimesMoved == 0)
                {
                    if(Physics.Raycast(ray,out hit,Mathf.Infinity,BlueShipLayerMask))
                    {
                        //Disable the Selected Ship tiles:
                        if(SelectedShip != null)
                        {
                            SelectedShip.MovementTileHolder.gameObject.SetActive(false);
                            SelectedShip.MovementTileHolderIsActive = false;
                            SelectedShip.AttackTileHolder.gameObject.SetActive(false);
                            SelectedShip.AttackTileHolderIsActive = false;
                            SelectedShip = null;
                        }
                
                        //Get the position of our ship:
                        Vector3 SelectedShipPosition = hit.point;
 
                        //Round the position in order to match the centers of the squares (Same as the hilighter positioning.).
                        SelectedShipPosition.x -= .5f;
                        SelectedShipPosition.z += .5f;

                        SelectedShipPosition.x = Mathf.Round(SelectedShipPosition.x) + .5f;
                        SelectedShipPosition.z = Mathf.Round(SelectedShipPosition.z) - .5f;

                        //Make sure y is always 0:
                        SelectedShipPosition.y = 0;

                        //Loop through the Ship references:
                        for(int i = 0; i < Ships.Length ; i += 1)
                        {
                            if(Ships[i])
                            {
                                if(Ships[i].gameObject.transform.position == SelectedShipPosition)
                                {
                                    SelectedShip = Ships[i];
                                    i = 12;
                                }
                            }
                        }
                        //Activate all the buttons.
                        AttackButton.gameObject.SetActive(true);
                        MovementButton.gameObject.SetActive(true);
                        AttackOrMovementXButton.gameObject.SetActive(true);
                        SurrenderButton.gameObject.SetActive(false);

                        //Activate the stats panel for the selected ship's stats:
                        SelectionStatsPanel.gameObject.SetActive(true);
                        int ShipNameShortcut = SelectedShip.name.IndexOf('(');
                        ShipName.text = "" + SelectedShip.name.Substring(0,ShipNameShortcut);
                        HealthText.text = "Health: " + SelectedShip.lives + " / " + SelectedShip.maxLives;
                        DamageText.text = "Avg. Damage: " + SelectedShip.damage;
                        UnitsPerTurnText.text = "Units Per Turn: " + SelectedShip.unitsPerTurn;
                        MaxAttackRangeText.text = "Attack Range: " + SelectedShip.MaxAttackRange;
                    }
                }else if(!blueTurn && Ship.numberOfTimesMoved == 0)
                {
                    if(Physics.Raycast(ray,out hit,Mathf.Infinity,RedShipLayerMask))
                    {
                        //Empty the Selected Ship variables (including their tiles):
                        if(SelectedShip != null)
                        {
                            SelectedShip.MovementTileHolder.gameObject.SetActive(false);
                            SelectedShip.MovementTileHolderIsActive = false;
                            SelectedShip.AttackTileHolder.gameObject.SetActive(false);
                            SelectedShip.AttackTileHolderIsActive = false;
                            SelectedShip = null;
                        }

                        //Get the position of our ship:
                        Vector3 SelectedShipPosition = hit.point;
 
                        //Round the position in order to match the centers of the squares (Same as the hilighter positioning.).
                        SelectedShipPosition.x -= .5f;
                        SelectedShipPosition.z += .5f;

                        SelectedShipPosition.x = Mathf.Round(SelectedShipPosition.x) + .5f;
                        SelectedShipPosition.z = Mathf.Round(SelectedShipPosition.z) - .5f;

                        //Make sure y is always 0:
                        SelectedShipPosition.y = 0;

                        //Loop through the Ship references:
                        for(int i = 0; i < Ships.Length ; i += 1)
                        {
                            if(Ships[i])
                            {
                                if(Ships[i].gameObject.transform.position == SelectedShipPosition)
                                {
                                    SelectedShip = Ships[i];
                                    i = 12;
                                }
                            }
                        }
                        //Activate all the buttons.
                        AttackButton.gameObject.SetActive(true);
                        MovementButton.gameObject.SetActive(true);
                        AttackOrMovementXButton.gameObject.SetActive(true);
                        SurrenderButton.gameObject.SetActive(false);

                        //Activate the stats panel for the selected ship's stats:
                        SelectionStatsPanel.gameObject.SetActive(true);
                        int ShipNameShortcut = SelectedShip.name.IndexOf('(');
                        ShipName.text = "" + SelectedShip.name.Substring(0,ShipNameShortcut);
                        HealthText.text = "Health: " + SelectedShip.lives + " / " + SelectedShip.maxLives;
                        DamageText.text = "Avg. Damage: " + SelectedShip.damage;
                        UnitsPerTurnText.text = "Units Per Turn: " + SelectedShip.unitsPerTurn;
                        MaxAttackRangeText.text = "Attack Range: " + SelectedShip.MaxAttackRange;
                    }
                }
            }
        }
    }

    public void OnRestartButtonClicked()
    {
        Ship.numberOfTimesMoved = 0;
        GamemodeSelectionImageText.gameObject.SetActive(true);
        GetRandomLoadingScreen();
    /*    if(RoundEndAd != null)
        {
            if(RoundEndAd.AdState == AdState.Loaded)
            {
                ShowAd();
            }
        }
    */
        //Reset literally EVERY SINGLE VARIABLE:
        BlueTreasury = 0;
        RedTreasury = 0;
        BlueIslands = 0;
        RedIslands = 0;

        ShipBuyingPanel.gameObject.SetActive(false);
        ShipBuyingPanelIsActive = false;
        GameEnded = false;
        currentThing = ThingsLeftToDo.SpawnShips;
        SinglePlayerGame = false;
        GamemodePicked = false;
        ModeImageCanvasGroup.alpha = 1;
        MapSizePanelCanvasGroup.alpha = 1;
        LeftSideWall.position = new Vector3(20.5f,.75f,5.25f);
        RightSideWall.position = new Vector3(9.5f,.75f,5.25f);
        Sand.position = new Vector3(15,.03f, -.5f);
        GamemodeSelectionPanel.gameObject.SetActive(true);
        GamemodeSelectionImage.gameObject.SetActive(true);
        Ship.minimumX = 10;
        Ship.minimumZ = 0;
        ShipButtonText.text = "Normal Ship - 2";
        SpeedyShipButtonText.text = "Speedy Ship - 2";
        ArmedShipButtonText.text = "Armed Ship - 1";
        ToughShipButtonText.text = "Tough Ship - 1";
        foreach(GameObject objectToDisable in HarboursAndSeasToGoIn9)
        {
            objectToDisable.SetActive(true);
        }
        foreach(GameObject objectToDisable in HarboursAndSeasToGoIn8)
        {
            objectToDisable.SetActive(true);
        }
        foreach(GameObject objectToMove in HarboursAndSeasToBeMoved)
        {
            objectToMove.transform.position = new Vector3(objectToMove.transform.position.x, objectToMove.transform.position.y, objectToMove.transform.position.z - NumOfHarboursToBeDisabled);
        }
        gameObject.transform.position = new Vector3(15, 6.7f, 10);
        ImagesMoved = false;
        NumOfHarboursToBeDisabled = 0;
        Ship.IslandGenerationScript.linesLeftToGenerate = 6;
        Ship.SurrenderButtonClicked = false;
        blueTurn = true;
        hilighterIsActive = false;
        ShipsRemainingAmount = 2;
        SpeedyShipsRemainingAmount = 2;
        ArmedShipRemainingAmount = 1;
        ToughShipRemainingAmount = 1;
        BlueShip = null;
        SecondBlueShip = null;
        RedShip = null;
        SecondRedShip = null;
        BlueSpeedyShip = null;
        SecondBlueSpeedyShip = null;
        RedSpeedyShip = null;
        SecondRedSpeedyShip = null;
        BlueArmedShip = null;
        RedArmedShip = null;
        BlueToughShip = null;
        RedToughShip = null;
        Ships = new Ship[12];
        SelectedShip = null;
        AttackedShip = null;
        BlueShipImage1.gameObject.SetActive(true);
        BlueShipImage2.gameObject.SetActive(true);
        BlueSpeedyShipImage1.gameObject.SetActive(true);
        BlueSpeedyShipImage2.gameObject.SetActive(true);
        BlueArmedShipImage.gameObject.SetActive(true);
        BlueToughShipImage.gameObject.SetActive(true);
        RedShipImage1.gameObject.SetActive(true);
        RedShipImage2.gameObject.SetActive(true);
        RedSpeedyShipImage1.gameObject.SetActive(true);
        RedSpeedyShipImage2.gameObject.SetActive(true);
        RedArmedShipImage.gameObject.SetActive(true);
        RedToughShipImage.gameObject.SetActive(true);
        Ship.IslandGenerationScript.IslandPositions = new List<Vector3>();
        foreach(GameObject IslandSeaShip in IslandAndSeaPositions.Values)
        {
            Destroy(IslandSeaShip);
        }
        IslandAndSeaPositions = new Dictionary<Vector3, GameObject>();
        foreach(Ship ship in ShipPositions.Values)
        {
            Destroy(ship.gameObject);
        }
        ShipPositions = new Dictionary<Vector3, Ship>();
        Ship.IslandGenerationScript.linesLeftToGenerate = 6;
        Ship.IslandGenerationScript.currentPointToGenerate = new Vector3(19.5f,0,7.5f);
        Ship.IslandGenerationScript.LineStartPoint = new Vector3(19.5f,0,7.5f);
        Harbour.BlueHarbourInvaded = false;
        Harbour.RedHarbourInvaded = false;
        AttackButton.gameObject.SetActive(false);
        MovementButton.gameObject.SetActive(false);
        AttackOrMovementXButton.gameObject.SetActive(false);
        RandomShipGenerationButton.gameObject.SetActive(true);
        TreasuryPanel.gameObject.SetActive(false);
        StatsButton.gameObject.SetActive(false);
        RandomShipsGenerationButtonClickedOnce = false;
        WinningPanel.gameObject.SetActive(false);
        TimeRemainingText.text = "30s Remaining";
        secondsLeft = 30;
        TimePickedOnce = false;
        Ship.ShipMoving = false;
        Ship.ShipAttacking = false;
        Ship.ShipReadyToMove = false;
        Ship.ShipReadyToAttack = false;
        Ship.MovementButtonClicked = false;
        Ship.AttackButtonClicked = false;
        Ship.BaseGameLogicScript.TargetedAIShip = null;
        TargetedAIShipPicked = false;
        AIShipToMove = null;
        AIShipToMovePicked = false;
        Ship.TargetPositionAndRotationPicked = false;
        directionTargetedShipIsAt = "";
        ShipAIattackComesFrom = null;
        AIdidSomething = false;
        AIstartedAttacking = false;
        ScoreUpdated = false;
    }

    void GetRandomLoadingScreen()
    {
        int RandomScreen = Random.Range(1, 4);
        switch(RandomScreen)
        {
            case 1:
                GamemodeSelectionImage.GetComponent<Image>().sprite = Screen1;
                break;
            case 2:
                GamemodeSelectionImage.GetComponent<Image>().sprite = Screen2;
                break;
            case 3:
                GamemodeSelectionImage.GetComponent<Image>().sprite = Screen3;
                break;
            case 4:
                GamemodeSelectionImage.GetComponent<Image>().sprite = Screen4;
                break;
        }
        int RandomTip = Random.Range(1,6);
        switch(RandomTip)
        {
            case 1:
                GamemodeSelectionPanel.GetChild(5).GetComponent<Text>().text = Fact1;
                break;
            case 2:
                GamemodeSelectionPanel.GetChild(5).GetComponent<Text>().text = Fact2;
                break;
            case 3:
                GamemodeSelectionPanel.GetChild(5).GetComponent<Text>().text = Fact3;
                break;
            case 4:
                GamemodeSelectionPanel.GetChild(5).GetComponent<Text>().text = Fact4;
                break;
            case 5:
                GamemodeSelectionPanel.GetChild(5).GetComponent<Text>().text = Fact5;
                break;
        }
        int OneInAHundred = Random.Range(1,101);
        if(OneInAHundred == 51)
        {
            GamemodeSelectionPanel.GetChild(5).GetComponent<Text>().text = "Did you know this has an one in a hundred chances of being shown?";
        }
    }

    public void ToIntroduction()
    {
        TutorialIntroduction.gameObject.SetActive(true);
    }

    public void ToMapSelection()
    {
        TutorialIntroduction.gameObject.SetActive(false);
        TutorialMapSelection.gameObject.SetActive(true);
    }

    public void ToManualDeployment()
    {
        TutorialMapSelection.gameObject.SetActive(false);
        TutorialManualDeployment.gameObject.SetActive(true);
    }

    public void ToRandomShipGeneration()
    {
        TutorialManualDeployment.gameObject.SetActive(false);
        TutorialRandomShipGeneration.gameObject.SetActive(true);
    }

    public void ToStatsTimeSurrendering()
    {
        TutorialRandomShipGeneration.gameObject.SetActive(false);
        TutorialStatsTimeSurrendering.gameObject.SetActive(true);
    }

    public void ToShipSelection()
    {
        TutorialStatsTimeSurrendering.gameObject.SetActive(false);
        TutorialShipSelection.gameObject.SetActive(true);
    }

    public void ToMovement()
    {
        TutorialShipSelection.gameObject.SetActive(false);
        TutorialMovement.gameObject.SetActive(true);
    }

    public void ToAttacking()
    {
        TutorialMovement.gameObject.SetActive(false);
        TutorialAttacking.gameObject.SetActive(true);
    }

    public void ToIslandFlags()
    {
        TutorialAttacking.gameObject.SetActive(false);
        TutorialIslandFlags.gameObject.SetActive(true);
    }

    public void ToShipBuying()
    {
        TutorialIslandFlags.gameObject.SetActive(false);
        TutorialShipBuying.gameObject.SetActive(true);
    }

    public void ToHarbourInvasion()
    {
        TutorialShipBuying.gameObject.SetActive(false);
        TutorialHarbourInvasion.gameObject.SetActive(true);
    }

    public void ExitTutorial()
    {
        TutorialHarbourInvasion.gameObject.SetActive(false);
    }

    void PositionHilighter()
    {
        //Create a ray and a variable to store the results of the raycast.
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if(Physics.Raycast(ray,out hit,Mathf.Infinity,EverythingLayerMask.value))
        {
            //If it hit something,
            Vector3 point = hit.point;

            //Make sure y is always 0:
            point.y = 0;

            //Make some neseccary changes (tested, because it rounds to an int and we have to position it correctly to a float):
            point.x -= .5f;
            point.z += .5f;

            //Round the X and Z axes
            point.x = Mathf.Round(point.x) + .5f;
            point.z = Mathf.Round(point.z) - .5f;

            if(!deployButtonPanelIsActive && !ShipBuyingPanelIsActive)
            {
                //Position the hilighter:
                hilighter.position = point;
                hilighter.gameObject.SetActive(true);
                hilighterIsActive = true;
            }
        }else if(Physics.Raycast(ray,out hit,Mathf.Infinity,BlueHarbourLayerMask.value))
        {
            //Get the gameObject of its collider to get the position afterwards, and create a new Vector3 for the position:
            GameObject gameObjectHit = hit.collider.gameObject;
            Vector3 desiredPosition = gameObjectHit.transform.position;

            //Make sure the Y position is always 0,  and the Z +.25:
            desiredPosition.y = 0;
            desiredPosition.z += .25f;

            if(!deployButtonPanelIsActive && !ShipBuyingPanelIsActive)
            {
                //Position the hilighter:
                hilighter.position = desiredPosition;
                hilighter.gameObject.SetActive(true);
                hilighterIsActive = true;
            }
        }else
        {
            hilighter.position = new Vector3(0,0,0);
            hilighter.gameObject.SetActive(false);
            hilighterIsActive = false;
        }

        if(hilighter.position.x > Ship.maximumX - .25f || hilighter.position.x < Ship.minimumX + .25f || hilighter.position.z > Ship.maximumZ - .25f || hilighter.position.z < Ship.minimumZ + .25f)
        {
            hilighter.gameObject.SetActive(false);
            hilighterIsActive = false;
        }
    }

    public void On2PalyerButtonClicked()
    {
        if(!ModeFadingStarted)
        {
            SinglePlayerGame = false;
            GamemodePicked = true;
            GamemodeSelectionPanel.gameObject.SetActive(false);
            MapSizePanel.gameObject.SetActive(true);
            PreviousGamemodeWas2 = true;
            if(PreviousGamemodeWasAI)
            {
                timesBlueWon = 0;
                timesRedWon = 0;
                PreviousGamemodeWasAI = false;
            }
        }
    }

    public void OnSinglePlayerButtonClicked()
    {
        if(!ModeFadingStarted)
        {
            SinglePlayerGame = true;
            GamemodePicked = true;
            GamemodeSelectionPanel.gameObject.SetActive(false);
            MapSizePanel.gameObject.SetActive(true);
            PreviousGamemodeWasAI = true;
            if(PreviousGamemodeWas2)
            {
                timesBlueWon = 0;
                timesRedWon = 0;
                PreviousGamemodeWas2 = false;
            }
        }
    }

    public void On10x10MapButtonClicked()
    {
        ModeFadingStarted = true;
        CanGenerateIslands = true;
        gameObject.GetComponent<IslandGeneration>().enabled = true;
    }

    public void On9x9MapButtonClicked()
    {
        if(MapSizePanelCanvasGroup.alpha == 1)
        {
            ModeFadingStarted = true;
            NumOfHarboursToBeDisabled = 1;
            gameObject.GetComponent<IslandGeneration>().enabled = true;
            foreach(GameObject objectToDisable in HarboursAndSeasToGoIn9)
            {
                objectToDisable.SetActive(false);
            }
            foreach(GameObject objectToMove in HarboursAndSeasToBeMoved)
            {
                objectToMove.transform.position = new Vector3(objectToMove.transform.position.x, objectToMove.transform.position.y, objectToMove.transform.position.z + 1);
            }
            gameObject.transform.position = new Vector3(15.4f,6.05f, 9.95f);
            Ship.IslandGenerationScript.linesLeftToGenerate -= NumOfHarboursToBeDisabled;
            CanGenerateIslands = true;
            ShipsRemainingAmount -= 1;
            Ship.minimumX = 11;
            Ship.minimumZ = 1;
            ShipButtonText.text = "Normal Ship - 1";
            LeftSideWall.position = new Vector3(20.5f,.75f,6.25f);
            RightSideWall.position = new Vector3(10.5f,.75f,6.25f);
            Sand.position = new Vector3(15,.03f,.5f);
        }
    }

    public void On8x8MapButtonClicked()
    {
        if(MapSizePanelCanvasGroup.alpha == 1)
        {
            ModeFadingStarted = true;
            NumOfHarboursToBeDisabled = 2;
            gameObject.GetComponent<IslandGeneration>().enabled = true;
            foreach(GameObject objectToDisable in HarboursAndSeasToGoIn9)
            {
                objectToDisable.SetActive(false);
            }
            foreach(GameObject objectToDisable in HarboursAndSeasToGoIn8)
            {
                objectToDisable.SetActive(false);
            }
            foreach(GameObject objectToMove in HarboursAndSeasToBeMoved)
            {
                objectToMove.transform.position = new Vector3(objectToMove.transform.position.x, objectToMove.transform.position.y, objectToMove.transform.position.z + 2);
            }
            gameObject.transform.position = new Vector3(16, 5.47f, 9.9f);
            Ship.IslandGenerationScript.linesLeftToGenerate -= NumOfHarboursToBeDisabled;
            CanGenerateIslands = true;
            SpeedyShipsRemainingAmount -= 1;
            Ship.minimumX = 12;
            Ship.minimumZ = 2;
            SpeedyShipButtonText.text = "Speedy Ship - 1";
            LeftSideWall.position = new Vector3(20.5f,.75f,7);
            RightSideWall.position = new Vector3(11.5f,.75f,7);
            Sand.position = new Vector3(15,.03f,1.5f);
        }
    }

    public void OnShipBuyButtonClicked()
    {
        if(blueTurn)
        {
            if((!Ships[0] || !Ships[0] && !Ships[1]) && BlueTreasury >= 45)
            {
                Ships[0] = BlueShip = Instantiate(BlueShipPrefab,hilighter.position,Quaternion.identity);
                Ship.RebuildShipPositions();
                BlueValueDifference = -45;
                ShipBuyingPanel.gameObject.SetActive(false);
                ShipBuyingPanelIsActive = false;
                BlueShipImage1.gameObject.SetActive(true);
                CameraAudioSource.PlayOneShot(MoneyUsageSF,MoneyVol);

            }else if(!Ships[1] && BlueTreasury >= 45 && NumOfHarboursToBeDisabled != 1)
            {
                Ships[1] = SecondBlueShip = Instantiate(BlueShipPrefab,hilighter.position,Quaternion.identity);
                Ship.RebuildShipPositions();
                BlueValueDifference = -45;
                ShipBuyingPanel.gameObject.SetActive(false);
                ShipBuyingPanelIsActive = false;
                BlueShipImage2.gameObject.SetActive(true);
                CameraAudioSource.PlayOneShot(MoneyUsageSF,MoneyVol);
            }
        }else
        {
            if((!Ships[6] || !Ships[6] && !Ships[7]) && RedTreasury >= 45)
            {
                Ships[6] = RedShip = Instantiate(RedShipPrefab,hilighter.position,RedShipsRotation);
                Ship.RebuildShipPositions();
                RedValueDifference = -45;
                ShipBuyingPanel.gameObject.SetActive(false);
                ShipBuyingPanelIsActive = false;
                RedShipImage1.gameObject.SetActive(true);
                CameraAudioSource.PlayOneShot(MoneyUsageSF,MoneyVol);

            }else if(!Ships[7] && RedTreasury >= 45 && NumOfHarboursToBeDisabled != 1)
            {
                Ships[7] = SecondRedShip = Instantiate(RedShipPrefab,hilighter.position,RedShipsRotation);
                Ship.RebuildShipPositions();
                RedValueDifference = -45;
                ShipBuyingPanel.gameObject.SetActive(false);
                ShipBuyingPanelIsActive = false;
                RedShipImage2.gameObject.SetActive(true);
                CameraAudioSource.PlayOneShot(MoneyUsageSF,MoneyVol);
            }
        }
    }

    public void OnSpeedyShipBuyButtonClicked()
    {
        if(blueTurn)
        {
            if((!Ships[2] || !Ships[2] && !Ships[3]) && BlueTreasury >= 55)
            {
                Ships[2] = BlueSpeedyShip = Instantiate(BlueSpeedyShipPrefab,hilighter.position,Quaternion.identity);
                Ship.RebuildShipPositions();
                BlueValueDifference = -55;
                ShipBuyingPanel.gameObject.SetActive(false);
                ShipBuyingPanelIsActive = false;
                BlueSpeedyShipImage1.gameObject.SetActive(true);
                CameraAudioSource.PlayOneShot(MoneyUsageSF,MoneyVol);

            }else if(!Ships[3] && BlueTreasury >= 55 && NumOfHarboursToBeDisabled != 2)
            {
                Ships[3] = SecondBlueSpeedyShip = Instantiate(BlueSpeedyShipPrefab,hilighter.position,Quaternion.identity);
                Ship.RebuildShipPositions();
                BlueValueDifference = -55;
                ShipBuyingPanel.gameObject.SetActive(false);
                ShipBuyingPanelIsActive = false;
                BlueSpeedyShipImage2.gameObject.SetActive(true);
                CameraAudioSource.PlayOneShot(MoneyUsageSF,MoneyVol);
            }
        }else
        {
            if((!Ships[8] || !Ships[8] && !Ships[9]) && RedTreasury >= 55)
            {
                Ships[8] = RedSpeedyShip = Instantiate(RedSpeedyShipPrefab,hilighter.position,RedShipsRotation);
                Ship.RebuildShipPositions();
                RedValueDifference = -55;
                ShipBuyingPanel.gameObject.SetActive(false);
                ShipBuyingPanelIsActive = false;
                RedSpeedyShipImage1.gameObject.SetActive(true);
                CameraAudioSource.PlayOneShot(MoneyUsageSF,MoneyVol);

            }else if(!Ships[9] && RedTreasury >= 55 && NumOfHarboursToBeDisabled != 2)
            {
                Ships[9] = SecondRedSpeedyShip = Instantiate(RedSpeedyShipPrefab,hilighter.position,RedShipsRotation);
                Ship.RebuildShipPositions();
                RedValueDifference = -55;
                ShipBuyingPanel.gameObject.SetActive(false);
                ShipBuyingPanelIsActive = false;
                RedSpeedyShipImage2.gameObject.SetActive(true);
                CameraAudioSource.PlayOneShot(MoneyUsageSF,MoneyVol);
            }
        }
    }

    public void OnArmedShipBuyButtonClicked()
    {
        if(blueTurn)
        {
            if(!Ships[4] && BlueTreasury >= 90)
            {
                Ships[4] = BlueArmedShip = Instantiate(BlueArmedShipPrefab,hilighter.position,Quaternion.identity);
                Ship.RebuildShipPositions();
                BlueValueDifference = -90;
                ShipBuyingPanel.gameObject.SetActive(false);
                ShipBuyingPanelIsActive = false;
                BlueArmedShipImage.gameObject.SetActive(true);
                CameraAudioSource.PlayOneShot(MoneyUsageSF,MoneyVol);
            }
        }else
        {
            if(!Ships[10] && RedTreasury >= 90)
            {
                Ships[10] = RedArmedShip = Instantiate(RedArmedShipPrefab,hilighter.position,RedShipsRotation);
                Ship.RebuildShipPositions();
                RedValueDifference = -90;
                ShipBuyingPanel.gameObject.SetActive(false);
                ShipBuyingPanelIsActive = false;
                RedArmedShipImage.gameObject.SetActive(true);
                CameraAudioSource.PlayOneShot(MoneyUsageSF,MoneyVol);
            }
        }
    }

    public void OnToughShipBuyButtonClicked()
    {
        if(blueTurn)
        {
            if(!Ships[5] && BlueTreasury >= 85)
            {
                Ships[5] = BlueToughShip = Instantiate(BlueToughShipPrefab,hilighter.position,Quaternion.identity);
                Ship.RebuildShipPositions();
                BlueValueDifference = -85;
                ShipBuyingPanel.gameObject.SetActive(false);
                ShipBuyingPanelIsActive = false;
                BlueToughShipImage.gameObject.SetActive(true);
                CameraAudioSource.PlayOneShot(MoneyUsageSF,MoneyVol);
            }
        }else
        {
            if(!Ships[11] && RedTreasury >= 85)
            {
                Ships[11] = RedToughShip = Instantiate(RedToughShipPrefab,hilighter.position,RedShipsRotation);
                Ship.RebuildShipPositions();
                RedValueDifference = -85;
                ShipBuyingPanel.gameObject.SetActive(false);
                ShipBuyingPanelIsActive = false;
                RedToughShipImage.gameObject.SetActive(true);
                CameraAudioSource.PlayOneShot(MoneyUsageSF,MoneyVol);
            }
        }
    }

    public void OnShipExchangeXButtonClicked()
    {
        ShipBuyingPanel.gameObject.SetActive(false);
        ShipBuyingPanelIsActive = false;
    }

    public void OnStatsButtonClicked()
    {
        for(int i = 0; i < Ships.Length; i++)
        {
            if(Ships[i])
            {
                if(i == 0)
                {
                    Text healthText = BlueShipImage1.rectTransform.GetChild(3).GetComponent<Text>();
                    RectTransform healthPanel = BlueShipImage1.rectTransform.GetChild(2).GetComponent<RectTransform>();

                    healthPanel.sizeDelta = new Vector2((BlueShip.lives / BlueShip.maxLives) * 150, 20);
                    healthText.text = "" + BlueShip.lives;
                    
                }else if(i == 1)
                {
                    Text healthText = BlueShipImage2.rectTransform.GetChild(3).GetComponent<Text>();
                    RectTransform healthPanel = BlueShipImage2.rectTransform.GetChild(2).GetComponent<RectTransform>();

                    healthPanel.sizeDelta = new Vector2((SecondBlueShip.lives / SecondBlueShip.maxLives) * 150, 20);
                    healthText.text = "" + SecondBlueShip.lives;
                    
                }else if(i == 2)
                {
                    Text healthText = BlueSpeedyShipImage1.rectTransform.GetChild(3).GetComponent<Text>();
                    RectTransform healthPanel = BlueSpeedyShipImage1.rectTransform.GetChild(2).GetComponent<RectTransform>();

                    healthPanel.sizeDelta = new Vector2((BlueSpeedyShip.lives / BlueSpeedyShip.maxLives) * 150, 20);
                    healthText.text = "" + BlueSpeedyShip.lives;
                    
                }else if(i == 3)
                {
                    Text healthText = BlueSpeedyShipImage2.rectTransform.GetChild(3).GetComponent<Text>();
                    RectTransform healthPanel = BlueSpeedyShipImage2.rectTransform.GetChild(2).GetComponent<RectTransform>();

                    healthPanel.sizeDelta = new Vector2((SecondBlueSpeedyShip.lives / SecondBlueSpeedyShip.maxLives) * 150, 20);
                    healthText.text = "" + SecondBlueSpeedyShip.lives;
                    
                }else if(i == 4)
                {
                    Text healthText = BlueArmedShipImage.rectTransform.GetChild(3).GetComponent<Text>();
                    RectTransform healthPanel = BlueArmedShipImage.rectTransform.GetChild(2).GetComponent<RectTransform>();

                    healthPanel.sizeDelta = new Vector2((BlueArmedShip.lives / BlueArmedShip.maxLives) * 150, 20);
                    healthText.text = "" + BlueArmedShip.lives;
                    
                }else if(i == 5)
                {
                    Text healthText = BlueToughShipImage.rectTransform.GetChild(3).GetComponent<Text>();
                    RectTransform healthPanel = BlueToughShipImage.rectTransform.GetChild(2).GetComponent<RectTransform>();

                    healthPanel.sizeDelta = new Vector2((BlueToughShip.lives / BlueToughShip.maxLives) * 150, 20);
                    healthText.text = "" + BlueToughShip.lives;
                    
                }else if(i == 6)
                {
                    Text healthText = RedShipImage1.rectTransform.GetChild(3).GetComponent<Text>();
                    RectTransform healthPanel = RedShipImage1.rectTransform.GetChild(2).GetComponent<RectTransform>();

                    healthPanel.sizeDelta = new Vector2((RedShip.lives / RedShip.maxLives) * 150, 20);
                    healthText.text = "" + RedShip.lives;
                    
                }else if(i == 7)
                {
                    Text healthText = RedShipImage2.rectTransform.GetChild(3).GetComponent<Text>();
                    RectTransform healthPanel = RedShipImage2.rectTransform.GetChild(2).GetComponent<RectTransform>();

                    healthPanel.sizeDelta = new Vector2((SecondRedShip.lives / SecondRedShip.maxLives) * 150, 20);
                    healthText.text = "" + SecondRedShip.lives;
                    
                }else if(i == 8)
                {
                    Text healthText = RedSpeedyShipImage1.rectTransform.GetChild(3).GetComponent<Text>();
                    RectTransform healthPanel = RedSpeedyShipImage1.rectTransform.GetChild(2).GetComponent<RectTransform>();

                    healthPanel.sizeDelta = new Vector2((RedSpeedyShip.lives / RedSpeedyShip.maxLives) * 150, 20);
                    healthText.text = "" + RedSpeedyShip.lives;
                    
                }else if(i == 9)
                {
                    Text healthText = RedSpeedyShipImage2.rectTransform.GetChild(3).GetComponent<Text>();
                    RectTransform healthPanel = RedSpeedyShipImage2.rectTransform.GetChild(2).GetComponent<RectTransform>();

                    healthPanel.sizeDelta = new Vector2((SecondRedSpeedyShip.lives / SecondRedSpeedyShip.maxLives) * 150, 20);
                    healthText.text = "" + SecondRedSpeedyShip.lives;
                    
                }else if(i == 10)
                {
                    Text healthText = RedArmedShipImage.rectTransform.GetChild(3).GetComponent<Text>();
                    RectTransform healthPanel = RedArmedShipImage.rectTransform.GetChild(2).GetComponent<RectTransform>();

                    healthPanel.sizeDelta = new Vector2((RedArmedShip.lives / RedArmedShip.maxLives) * 150, 20);
                    healthText.text = "" + RedArmedShip.lives;
                    
                }else if(i == 11)
                {
                    Text healthText = RedToughShipImage.rectTransform.GetChild(3).GetComponent<Text>();
                    RectTransform healthPanel = RedToughShipImage.rectTransform.GetChild(2).GetComponent<RectTransform>();

                    healthPanel.sizeDelta = new Vector2((RedToughShip.lives / RedToughShip.maxLives) * 150, 20);
                    healthText.text = "" + RedToughShip.lives;
                }
            }else
            {
                if(i == 0)
                {
                    BlueShipImage1.gameObject.SetActive(false);
                    
                }else if(i == 1)
                {
                    BlueShipImage2.gameObject.SetActive(false);

                }else if(i == 2)
                {
                    BlueSpeedyShipImage1.gameObject.SetActive(false);
                    
                }else if(i == 3)
                {
                    BlueSpeedyShipImage2.gameObject.SetActive(false);
                       
                }else if(i == 4)
                {
                    BlueArmedShipImage.gameObject.SetActive(false);
                    
                }else if(i == 5)
                {
                    BlueToughShipImage.gameObject.SetActive(false);
                    
                }else if(i == 6)
                {
                    RedShipImage1.gameObject.SetActive(false);
                    
                }else if(i == 7)
                {
                    RedShipImage2.gameObject.SetActive(false);
                    
                }else if(i == 8)
                {
                    RedSpeedyShipImage1.gameObject.SetActive(false);
                    
                }else if(i == 9)
                {
                    RedSpeedyShipImage2.gameObject.SetActive(false);
                    
                }else if(i == 10)
                {
                    RedArmedShipImage.gameObject.SetActive(false);
                    
                }else if(i == 11)
                {
                    RedToughShipImage.gameObject.SetActive(false);
                }
            }
        }
        StatsPanel.gameObject.SetActive(true);
    }

    public void OnStatsPanelCloseButtonClicked()
    {
        StatsPanel.gameObject.SetActive(false);
        hilighter.position = new Vector3(12.5f,0,8.5f);
    }

    public void GetNewPopupPanel()
    {
        if(ActivePopupPanel)
        {
            Destroy(ActivePopupPanel);
        }
        ActivePopupPanel = Instantiate(PopupPanelPrefab,CanvasRectTrans);
        PopupPanelCanvasGroup = ActivePopupPanel.GetComponent<CanvasGroup>();
        FadingState = 1;
    }

    public void FadeOutTaskPanel()
    {
        if(FadingState == 3)
        {
            FadingState = 4;
        }
    }

    public void OnMuteButtonClicked()
    {
        GameIsMuted = !GameIsMuted;
        if(GameIsMuted)
        {
            AudioListener.volume = 0;
            MuteButton.GetComponent<Image>().sprite = EnabledAudioImage;
        }else
        {
            AudioListener.volume = 1;
            MuteButton.GetComponent<Image>().sprite = DisabledAudioImage;
        }
    }

    void UpdateTreasury()
    {
        if(blueTurn)
        {
            //Prepare to slowly increase the money:
            CoinTime += Time.deltaTime;
            float updatingFactor;
            updatingFactor = CoinAnimation.Evaluate(CoinTime);
            //Show the results:
            TreasuryText.text = "" + updatingFactor * (BlueTreasury + BlueValueDifference);
            //Check if its done and reset:
            if(updatingFactor >= 1)
            {
                BlueTreasury += BlueValueDifference;
                CoinTime = 0;
                BlueValueDifference = 0;
            }
        }else
        {
            //Prepare to slowly increase the money:
            CoinTime += Time.deltaTime;
            float updatingFactor;
            updatingFactor = CoinAnimation.Evaluate(CoinTime);
            //Show the results:
            TreasuryText.text = "" + updatingFactor * (RedTreasury + RedValueDifference);
            //Check if its done and reset:
            if(updatingFactor >= 1)
            {
                RedTreasury += RedValueDifference;
                CoinTime = 0;
                RedValueDifference = 0;
            }
        }
    }

    void AImovementAndAttacking()
    {
        int ToughOrArmed = 0;
        int NormalOrSpeedy = 0;

        //Check if the AI has lost some ships, and if yes and they can be restored, buy new ones on a random harbour. Also, In order to decide if the AI has to spawn a tough ship or armed ship, randomize it as well:
        if(!AIgotRandomBuyingValues)
        {
            ToughOrArmed = Random.Range(0,2);
            NormalOrSpeedy = Random.Range(0,2);
            AIgotRandomBuyingValues = true;
        }
        
        if(!Ships[4] && BlueTreasury >= 90 && ToughOrArmed == 0)
        {
            Vector3 BuyingHarbourPos = new Vector3();

            while(!AIgotBuyingHarbour)
            {
                int randomHarbour = Random.Range(0,10 - NumOfHarboursToBeDisabled);
                BuyingHarbourPos = new Vector3(19.5f - randomHarbour,0,.5f + NumOfHarboursToBeDisabled);

                if(!ShipPositions.ContainsKey(BuyingHarbourPos))
                {
                    AIgotBuyingHarbour = true;
                }else
                {
                    BuyingHarbourPos = Vector3.zero;
                }
            }

            Ships[4] = BlueArmedShip = Instantiate(BlueArmedShipPrefab, BuyingHarbourPos,Quaternion.identity);
            Ship.RebuildShipPositions();
            BlueValueDifference = -90;
            BlueArmedShipImage.gameObject.SetActive(true);
            CameraAudioSource.PlayOneShot(MoneyUsageSF, MoneyVol);
        }

        if(!Ships[5] && BlueTreasury >= 85 && ToughOrArmed == 1)
        {
            Vector3 BuyingHarbourPos = new Vector3();

            while(!AIgotBuyingHarbour)
            {
                int randomHarbour = Random.Range(0,10 - NumOfHarboursToBeDisabled);
                BuyingHarbourPos = new Vector3(19.5f - randomHarbour,0,.5f + NumOfHarboursToBeDisabled);

                if(!ShipPositions.ContainsKey(BuyingHarbourPos))
                {
                    AIgotBuyingHarbour = true;
                }else
                {
                    BuyingHarbourPos = Vector3.zero;
                }
            }

            Ships[5] = BlueToughShip = Instantiate(BlueToughShipPrefab, BuyingHarbourPos,Quaternion.identity);
            Ship.RebuildShipPositions();
            BlueValueDifference = -85;
            BlueToughShipImage.gameObject.SetActive(true);
            CameraAudioSource.PlayOneShot(MoneyUsageSF, MoneyVol);
        }

        if((!Ships[2] || !Ships[2] && !Ships[3]) && BlueTreasury >= 55 && NormalOrSpeedy == 0)
        {
            Vector3 BuyingHarbourPos = new Vector3();

            while(!AIgotBuyingHarbour)
            {
                int randomHarbour = Random.Range(0,10 - NumOfHarboursToBeDisabled);
                BuyingHarbourPos = new Vector3(19.5f - randomHarbour,0,.5f + NumOfHarboursToBeDisabled);

                if(!ShipPositions.ContainsKey(BuyingHarbourPos))
                {
                    AIgotBuyingHarbour = true;
                }else
                {
                    BuyingHarbourPos = Vector3.zero;
                }
            }

            Ships[2] = BlueSpeedyShip = Instantiate(BlueSpeedyShipPrefab, BuyingHarbourPos,Quaternion.identity);
            Ship.RebuildShipPositions();
            BlueValueDifference = -55;
            BlueSpeedyShipImage1.gameObject.SetActive(true);
            CameraAudioSource.PlayOneShot(MoneyUsageSF, MoneyVol);

        }else if(!Ships[3] && BlueTreasury >= 55 && NormalOrSpeedy == 0 && NumOfHarboursToBeDisabled != 2)
        {
            Vector3 BuyingHarbourPos = new Vector3();

            while(!AIgotBuyingHarbour)
            {
                int randomHarbour = Random.Range(0,10 - NumOfHarboursToBeDisabled);
                BuyingHarbourPos = new Vector3(19.5f - randomHarbour,0,.5f + NumOfHarboursToBeDisabled);

                if(!ShipPositions.ContainsKey(BuyingHarbourPos))
                {
                    AIgotBuyingHarbour = true;
                }else
                {
                    BuyingHarbourPos = Vector3.zero;
                }
            }

            Ships[3] = SecondBlueSpeedyShip = Instantiate(BlueSpeedyShipPrefab, BuyingHarbourPos,Quaternion.identity);
            Ship.RebuildShipPositions();
            BlueValueDifference = -55;
            BlueSpeedyShipImage2.gameObject.SetActive(true);
            CameraAudioSource.PlayOneShot(MoneyUsageSF, MoneyVol);
        }

        if((!Ships[0] || !Ships[0] && !Ships[1]) && BlueTreasury >= 45 && NormalOrSpeedy == 1)
        {
            Vector3 BuyingHarbourPos = new Vector3();

            while(!AIgotBuyingHarbour)
            {
                int randomHarbour = Random.Range(0,10 - NumOfHarboursToBeDisabled);
                BuyingHarbourPos = new Vector3(19.5f - randomHarbour,0,.5f + NumOfHarboursToBeDisabled);

                if(!ShipPositions.ContainsKey(BuyingHarbourPos))
                {
                    AIgotBuyingHarbour = true;
                }else
                {
                    BuyingHarbourPos = Vector3.zero;
                }
            }

            Ships[0] = BlueShip = Instantiate(BlueShipPrefab, BuyingHarbourPos,Quaternion.identity);
            Ship.RebuildShipPositions();
            BlueValueDifference = -45;
            BlueShipImage1.gameObject.SetActive(true);
            CameraAudioSource.PlayOneShot(MoneyUsageSF, MoneyVol);

        }else if(!Ships[1] && BlueTreasury >=  45 && NormalOrSpeedy == 1 && NumOfHarboursToBeDisabled != 1)
        {
            Vector3 BuyingHarbourPos = new Vector3();

            while(!AIgotBuyingHarbour)
            {
                int randomHarbour = Random.Range(0,10 - NumOfHarboursToBeDisabled);
                BuyingHarbourPos = new Vector3(19.5f - randomHarbour,0,.5f + NumOfHarboursToBeDisabled);

                if(!ShipPositions.ContainsKey(BuyingHarbourPos))
                {
                    AIgotBuyingHarbour = true;
                }else
                {
                    BuyingHarbourPos = Vector3.zero;
                }
            }

            Ships[1] = SecondBlueShip = Instantiate(BlueShipPrefab, BuyingHarbourPos,Quaternion.identity);
            Ship.RebuildShipPositions();
            BlueValueDifference = -45;
            BlueShipImage2.gameObject.SetActive(true);
            CameraAudioSource.PlayOneShot(MoneyUsageSF, MoneyVol);
        }
        AIgotBuyingHarbour = false;

        //If the AI hasn't picked a target already,
        if(!TargetedAIShipPicked && Ship.numberOfTimesMoved <= 1 && !Ship.ShipMoving && !GameEnded)
        {
            //Look through all the ships, and check if they can attack:
            for(int i = 0; i < Ships.Length; i++)
            {
                if(!TargetedAIShipPicked && i <= 5)
                {
                    //Quick reference to the current ship:
                    Ship CurrentShip = Ships[i];

                    //if this ship is still alive:
                    if(CurrentShip != null)
                    {
                        //Get every ship and get its position, then compare it to the radius of the CurrentShip
                        foreach(Ship possibleShipToAttack in ShipPositions.Values)
                        {
                            bool AIcanAttackZ = true;
                            bool AIcanAttackNoZ = true;
                            bool AIcanAttackX = true;
                            bool AIcanAttackNoX = true;

                            //Check if the ship is within the CurrentShip's radius:
                            for(int a = 0; a <= CurrentShip.MaxAttackRange; a++)
                            {
                                string nameCutout = possibleShipToAttack.name.Substring(0,4);
                                if(nameCutout != "Blue")
                                {
                                    //Check if there are islands where the ship can attack:
                                    if(IslandAndSeaPositions.ContainsKey(new Vector3(CurrentShip.gameObject.transform.position.x,0,CurrentShip.gameObject.transform.position.z + a)) || ShipPositions.ContainsKey(new Vector3(CurrentShip.gameObject.transform.position.x,0,CurrentShip.gameObject.transform.position.z + a)) && ShipPositions[new Vector3(CurrentShip.gameObject.transform.position.x,0,CurrentShip.gameObject.transform.position.z + a)].gameObject.name.Substring(0,4) == "Blue" && a != 0)
                                    {
                                        AIcanAttackZ = false;

                                    }
                                    if(IslandAndSeaPositions.ContainsKey(new Vector3(CurrentShip.gameObject.transform.position.x,0,CurrentShip.gameObject.transform.position.z - a)) || ShipPositions.ContainsKey(new Vector3(CurrentShip.gameObject.transform.position.x,0,CurrentShip.gameObject.transform.position.z - a)) && ShipPositions[new Vector3(CurrentShip.gameObject.transform.position.x,0,CurrentShip.gameObject.transform.position.z - a)].gameObject.name.Substring(0,4) == "Blue" && a != 0)
                                    {
                                        AIcanAttackNoZ = false;

                                    }
                                    if(IslandAndSeaPositions.ContainsKey(new Vector3(CurrentShip.gameObject.transform.position.x + a,0,CurrentShip.gameObject.transform.position.z)) || ShipPositions.ContainsKey(new Vector3(CurrentShip.gameObject.transform.position.x + a,0,CurrentShip.gameObject.transform.position.z)) && ShipPositions[new Vector3(CurrentShip.gameObject.transform.position.x + a,0,CurrentShip.gameObject.transform.position.z)].gameObject.name.Substring(0,4) == "Blue" && a != 0)
                                    {
                                        AIcanAttackX = false;

                                    }
                                    if(IslandAndSeaPositions.ContainsKey(new Vector3(CurrentShip.gameObject.transform.position.x - a,0,CurrentShip.gameObject.transform.position.z)) || ShipPositions.ContainsKey(new Vector3(CurrentShip.gameObject.transform.position.x - a,0,CurrentShip.gameObject.transform.position.z)) && ShipPositions[new Vector3(CurrentShip.gameObject.transform.position.x - a,0,CurrentShip.gameObject.transform.position.z)].gameObject.name.Substring(0,4) == "Blue" && a != 0)
                                    {
                                        AIcanAttackNoX = false;
                                    }
                                    
                                    //Attack where the ships can:
                                    if(possibleShipToAttack.gameObject.transform.position == new Vector3(CurrentShip.gameObject.transform.position.x,0,CurrentShip.gameObject.transform.position.z + a) && CurrentShip.directionAIshipLooking != "-z" && AIcanAttackZ)
                                    {
                                        TargetedAIShip = possibleShipToAttack;
                                        AIstartedAttacking = true;
                                        ShipAIattackComesFrom = CurrentShip;
                                        TargetedAIShipPicked = true;
                                        directionTargetedShipIsAt = "z";

                                    }else if(possibleShipToAttack.gameObject.transform.position == new Vector3(CurrentShip.gameObject.transform.position.x,0,CurrentShip.gameObject.transform.position.z - a) && CurrentShip.directionAIshipLooking != "z" && AIcanAttackNoZ)
                                    {
                                        TargetedAIShip = possibleShipToAttack;
                                        AIstartedAttacking = true;
                                        ShipAIattackComesFrom = CurrentShip;
                                        TargetedAIShipPicked = true;
                                        directionTargetedShipIsAt = "-z";

                                    }else if(possibleShipToAttack.gameObject.transform.position == new Vector3(CurrentShip.gameObject.transform.position.x + a,0,CurrentShip.gameObject.transform.position.z) && CurrentShip.directionAIshipLooking != "-x" && AIcanAttackX)
                                    {
                                        TargetedAIShip = possibleShipToAttack;
                                        AIstartedAttacking = true;
                                        ShipAIattackComesFrom = CurrentShip;
                                        TargetedAIShipPicked = true;
                                        directionTargetedShipIsAt = "x";

                                    }else if(possibleShipToAttack.gameObject.transform.position == new Vector3(CurrentShip.gameObject.transform.position.x - a,0,CurrentShip.gameObject.transform.position.z) && CurrentShip.directionAIshipLooking != "x" && AIcanAttackNoX)
                                    {
                                        TargetedAIShip = possibleShipToAttack;
                                        AIstartedAttacking = true;
                                        ShipAIattackComesFrom = CurrentShip;
                                        TargetedAIShipPicked = true;
                                        directionTargetedShipIsAt = "-x";
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
        if(ShipAIattackComesFrom != null && !Harbour.BlueHarbourInvaded && !Ship.ShipMoving && !GameEnded)
        {
            if(TargetedAIShipPicked && (ShipAIattackComesFrom.unitsPerTurn == 2 && Ship.numberOfTimesMoved <= 1) || (ShipAIattackComesFrom.unitsPerTurn == 1 && Ship.numberOfTimesMoved == 0))
            {
                if(!Ship.cannonballFired)
                {
                    CameraAudioSource.PlayOneShot(CannonBoom, CannonVol);
                    Instantiate(CannonballPrefab,ShipAIattackComesFrom.CannonballSpawnPoint.transform.position,ShipAIattackComesFrom.gameObject.transform.rotation);
                    Ship.cannonballFired = true;
                }

                if(Ship.attackFinished && !AIstartedAttacking)
                {
                    Ship.RebuildShipPositions();
                    TreasuryText.text = "0";
                    RedValueDifference = RedIslands;
                    if(!RedFleetIsDead)
                    {
                        GetNewPopupPanel();
                        ActivePopupPanel.transform.GetChild(0).GetComponent<Text>().text = "Now, you can select a ship, attack or move, Red";
                    }
                    Ship.cannonballFired = false;
                    Ship.attackFinished = false;
                    Ship.numberOfTimesMoved = 0;
                    blueTurn = false;
                    AIgotRandomBuyingValues = false;
                    ShipAIattackComesFrom = null;
                    TargetedAIShip = null;
                    TargetedAIShipPicked = false;
                    directionTargetedShipIsAt = "";
                    AIdidSomething = true;
                    AIstartedAttacking = false;
                    AIShipToMove = null;
                    AIShipToMovePicked = false;
                    Ship.TargetPositionAndRotationPicked = false;
                    Harbour.ShipsGotHealedIndicator = false;
                    lastTimePicked = Time.time;
                    secondsLeft = 30;
                    TimeRemainingText.text = "30s Remaining";
                    TimePickedOnce = false;
                    Ship.ShipSinking = false;
                }
            }
        }

        //Now, for the movement:
        if(!AIdidSomething && !AIstartedAttacking && !GameEnded)
        {
            //First, get a random number, and get a ship out of it. If it doesnt exist, do it again:
            while(!AIShipToMovePicked)
            {
                int randomShip = Random.Range(0,6);
                if(Ships[randomShip])
                {
                    AIShipToMovePicked = true;
                    AIShipToMove = Ships[randomShip];
                }
            }

            //Get targetPositions/rotations:
            if(!IslandAndSeaPositions.ContainsKey(new Vector3(AIShipToMove.gameObject.transform.position.x,0,AIShipToMove.gameObject.transform.position.z + 1)) && !ShipPositions.ContainsKey(new Vector3(AIShipToMove.gameObject.transform.position.x,0,AIShipToMove.gameObject.transform.position.z + 1)))
            {
                //Get desired position/rotations
                if(!Ship.TargetPositionAndRotationPicked)
                {
                    AIShipToMove.TargetPosition = new Vector3(AIShipToMove.gameObject.transform.position.x,0,AIShipToMove.gameObject.transform.position.z + 1);
                    AIShipToMove.desiredRotation = Quaternion.LookRotation(AIShipToMove.TargetPosition - AIShipToMove.gameObject.transform.position, Vector3.up);
                    AIShipToMove.directionAIshipLooking = "z";
                    Ship.TargetPositionAndRotationPicked = true;
                    if(AIShipToMove.unitsPerTurn == 2 && Ship.numberOfTimesMoved == 0)
                    {
                        CurveTime = -2;
                    }else if(AIShipToMove.unitsPerTurn == 1)
                    {
                        CurveTime = -.9f;
                    }
                }
            }else
            {
                while(!RandomSidePicked)
                {
                    int randomSide = Random.Range(1,3);
                    if(randomSide == 1)
                    {
                        if(AIShipToMove.gameObject.transform.position.x + 1 < Ship.maximumX)
                        {
                            LeftSidePicked = 1;
                            RandomSidePicked = true;
                        }
                    }else
                    {
                        if(AIShipToMove.gameObject.transform.position.x - 1 > Ship.minimumX)
                        {
                            LeftSidePicked = 2;
                            RandomSidePicked = true;
                        }
                    }
                }
            }

            //Check if the ship can turn:
            if(!IslandAndSeaPositions.ContainsKey(new Vector3(AIShipToMove.gameObject.transform.position.x + 1,0,AIShipToMove.gameObject.transform.position.z)) && (LeftSidePicked == 1 || LeftSidePicked == 2 && AIShipToMove.gameObject.transform.position.x + 1 < Ship.maximumX && (IslandAndSeaPositions.ContainsKey(new Vector3(AIShipToMove.gameObject.transform.position.x - 1,0,AIShipToMove.gameObject.transform.position.z)) || ShipPositions.ContainsKey(new Vector3(AIShipToMove.gameObject.transform.position.x - 1,0,AIShipToMove.gameObject.transform.position.z)))) && RandomSidePicked && !Ship.TargetPositionAndRotationPicked && !ShipPositions.ContainsKey(new Vector3(AIShipToMove.gameObject.transform.position.x + 1,0,AIShipToMove.gameObject.transform.position.z)))
            {
                //Get desired position/rotations
                AIShipToMove.TargetPosition = new Vector3(AIShipToMove.gameObject.transform.position.x + 1,0,AIShipToMove.gameObject.transform.position.z);
                AIShipToMove.desiredRotation = Quaternion.LookRotation(AIShipToMove.TargetPosition - AIShipToMove.gameObject.transform.position, Vector3.up);
                AIShipToMove.directionAIshipLooking = "x";
                Ship.TargetPositionAndRotationPicked = true;
                if(AIShipToMove.unitsPerTurn == 2 && Ship.numberOfTimesMoved == 0)
                {
                    CurveTime = -2;
                }else if(AIShipToMove.unitsPerTurn == 1)
                {
                    CurveTime = -.9f;
                }
            }else if(!IslandAndSeaPositions.ContainsKey(new Vector3(AIShipToMove.gameObject.transform.position.x - 1,0,AIShipToMove.gameObject.transform.position.z)) && !Ship.TargetPositionAndRotationPicked && (LeftSidePicked == 2 || LeftSidePicked == 1 && AIShipToMove.gameObject.transform.position.x - 1 > Ship.minimumX && (IslandAndSeaPositions.ContainsKey(new Vector3(AIShipToMove.gameObject.transform.position.x + 1,0,AIShipToMove.gameObject.transform.position.z)) || ShipPositions.ContainsKey(new Vector3(AIShipToMove.gameObject.transform.position.x + 1,0,AIShipToMove.gameObject.transform.position.z)))) && RandomSidePicked && !ShipPositions.ContainsKey(new Vector3(AIShipToMove.gameObject.transform.position.x - 1,0,AIShipToMove.gameObject.transform.position.z)))
            {
                //Get desired position/rotations
                AIShipToMove.TargetPosition = new Vector3(AIShipToMove.gameObject.transform.position.x - 1,0,AIShipToMove.gameObject.transform.position.z);
                AIShipToMove.desiredRotation = Quaternion.LookRotation(AIShipToMove.TargetPosition - AIShipToMove.gameObject.transform.position, Vector3.up);
                AIShipToMove.directionAIshipLooking = "-x";
                Ship.TargetPositionAndRotationPicked = true;
                if(AIShipToMove.unitsPerTurn == 2 && Ship.numberOfTimesMoved == 0)
                {
                    CurveTime = -2;
                }else if(AIShipToMove.unitsPerTurn == 1)
                {
                    CurveTime = -.9f;
                }
            }else if(!IslandAndSeaPositions.ContainsKey(new Vector3(AIShipToMove.gameObject.transform.position.x,0,AIShipToMove.gameObject.transform.position.z - 1)) && RandomSidePicked && !Ship.TargetPositionAndRotationPicked && !ShipPositions.ContainsKey(new Vector3(AIShipToMove.gameObject.transform.position.x,0,AIShipToMove.gameObject.transform.position.z - 1)) && AIShipToMove.gameObject.transform.position.z > Ship.minimumZ && AIShipToMove.gameObject.transform.position.z < Ship.maximumZ)
            {
                //Get desired position/rotations
                AIShipToMove.TargetPosition = new Vector3(AIShipToMove.gameObject.transform.position.x,0,AIShipToMove.gameObject.transform.position.z - 1);
                AIShipToMove.desiredRotation = Quaternion.LookRotation(AIShipToMove.TargetPosition - AIShipToMove.gameObject.transform.position, Vector3.up);
                AIShipToMove.directionAIshipLooking = "-z";
                Ship.TargetPositionAndRotationPicked = true;
                if(AIShipToMove.unitsPerTurn == 2 && Ship.numberOfTimesMoved == 0)
                {
                    CurveTime = -2;
                }else if(AIShipToMove.unitsPerTurn == 1)
                {
                    CurveTime = -.9f;
                }
            }
            
            if(!Ship.TargetPositionAndRotationPicked && Ship.numberOfTimesMoved == 1)
            {
                Ship.RebuildShipPositions();
                TreasuryText.text = "0";
                RedValueDifference = RedIslands;
                if(!Harbour.RedHarbourInvaded)
                {
                    GetNewPopupPanel();
                    ActivePopupPanel.transform.GetChild(0).GetComponent<Text>().text = "Now, you can select a ship, attack or move, Red";
                }
                blueTurn = false;
                AIgotRandomBuyingValues = false;
                SelectionStatsPanel.gameObject.SetActive(false);
                AIShipToMove.TargetPosition = Vector3.zero;
                AIShipToMove.desiredRotation = new Quaternion(0,0,0,0);
                Ship.numberOfTimesMoved = 0;
                Harbour.ShipsGotHealedIndicator = false;
                lastTimePicked = Time.time;
                secondsLeft = 30;
                TimeRemainingText.text = "30s Remaining";
                TimePickedOnce = false;
                MovementButton.gameObject.SetActive(false);
                AttackButton.gameObject.SetActive(false);
                AttackOrMovementXButton.gameObject.SetActive(false);
                SurrenderButton.gameObject.SetActive(true);
                AIShipToMovePicked = false;
                AIShipToMove = null;
                RandomSidePicked = false;
                Ship.movementFinished = false;
                Ship.rotationFinished = false;
                Ship.ShipMoving = false;
                Ship.TargetPositionAndRotationPicked = false;
                AIcanHornAgain = true;
                LeftSidePicked = 0;

            }else if(!Ship.TargetPositionAndRotationPicked && Ship.numberOfTimesMoved == 0)
            {
                Ship.RebuildShipPositions();
                AIShipToMovePicked = false;
                AIShipToMove = null;
                RandomSidePicked = false;
                LeftSidePicked = 0;
            }

            if(AIShipToMovePicked)
            {
                //Rotate the ship
                AIShipToMove.gameObject.transform.rotation = Quaternion.Lerp(AIShipToMove.gameObject.transform.rotation, AIShipToMove.desiredRotation, 5 * Time.deltaTime);

                if(Mathf.Abs(AIShipToMove.desiredRotation.y) - Mathf.Abs(AIShipToMove.gameObject.transform.rotation.y) < 0.00002 && !Ship.rotationFinished)
                {
                    Ship.rotationFinished = true;
                }

                if(!Ship.ShipMoving)
                {
                    Ship.ShipMoving = true;
                    if(AIcanHornAgain)
                    {
                        ShipsAudioSource.PlayOneShot(ShipHornSound,ShipHornVol);
                        AIcanHornAgain = false;
                    }
                }
                CurveTime += Time.deltaTime;
                float speed = .56f;
                if(AIShipToMove.unitsPerTurn == 1)
                {
                    speed *= SpeedAnimation.Evaluate(CurveTime);
                }else
                {
                    speed *= SpeedyShipAnimation.Evaluate(CurveTime);
                }
                //Move the ship
                AIShipToMove.gameObject.transform.position = Vector3.MoveTowards(AIShipToMove.gameObject.transform.position, AIShipToMove.TargetPosition, speed * Time.deltaTime);
 
                if(AIShipToMove.gameObject.transform.position == AIShipToMove.TargetPosition && !Ship.movementFinished)
                {
                    Ship.movementFinished = true;
                }

                if(Ship.movementFinished && Ship.rotationFinished)
                {
                    //..and then rotate and move the ship at the point it should exactly be looking at:
                    AIShipToMove.gameObject.transform.rotation = AIShipToMove.desiredRotation;
                    AIShipToMove.gameObject.transform.position = AIShipToMove.TargetPosition;

                    Ship.numberOfTimesMoved += 1;
                    RandomSidePicked = false;
                    Ship.movementFinished = false;
                    Ship.rotationFinished = false;
                    Ship.ShipMoving = false;
                    LeftSidePicked = 0;
                    Ship.TargetPositionAndRotationPicked = false;
                }

                if(Ship.numberOfTimesMoved == AIShipToMove.unitsPerTurn)
                {
                    Ship.RebuildShipPositions();
                    LeftSidePicked = 0;
                    TreasuryText.text = "0";
                    RedValueDifference = RedIslands;
                    if(!Harbour.RedHarbourInvaded)
                    {
                        GetNewPopupPanel();
                        ActivePopupPanel.transform.GetChild(0).GetComponent<Text>().text = "Now, you can select a ship, attack or move, Red";
                    }
                    blueTurn = false;
                    AIgotRandomBuyingValues = false;
                    SelectionStatsPanel.gameObject.SetActive(false);
                    AIShipToMove.TargetPosition = Vector3.zero;
                    AIShipToMove.desiredRotation = new Quaternion(0,0,0,0);
                    Ship.numberOfTimesMoved = 0;
                    Harbour.ShipsGotHealedIndicator = false;
                    lastTimePicked = Time.time;
                    secondsLeft = 30;
                    TimeRemainingText.text = "30s Remaining";
                    TimePickedOnce = false;
                    MovementButton.gameObject.SetActive(false);
                    AttackButton.gameObject.SetActive(false);
                    AttackOrMovementXButton.gameObject.SetActive(false);
                    SurrenderButton.gameObject.SetActive(true);
                    AIShipToMovePicked = false;
                    AIcanHornAgain = true;
                    AIShipToMove = null;
                }
            }
        }
    }

    void DeployShip()
    {
        if(hilighterIsActive)
        {
            if(blueTurn && !SinglePlayerGame && ((hilighter.position.z == .5f && NumOfHarboursToBeDisabled == 0) || (hilighter.position.z == 1.5f && NumOfHarboursToBeDisabled == 1) || (hilighter.position.z == 2.5f && NumOfHarboursToBeDisabled == 2)))
            {
                if(!ShipPositions.ContainsKey(hilighter.position))
                {
                    DeployButtonPanel.gameObject.SetActive(true);
                    deployButtonPanelIsActive = true;
                    RandomShipGenerationButton.gameObject.SetActive(false);
                }
            }else if(!blueTurn && hilighter.position.z == 9.5f)
            {
                if(!ShipPositions.ContainsKey(hilighter.position))
                {
                    DeployButtonPanel.gameObject.SetActive(true);
                    deployButtonPanelIsActive = true;
                    RandomShipGenerationButton.gameObject.SetActive(false);
                }
            }
        }
    }

    public void OnShipDeployButtonClicked()
    {
        if(ShipsRemainingAmount != 0)
        {           
            if(blueTurn)
            {
                Ship blueShip = null;
                blueShip = ShipPositions[hilighter.position] = Instantiate(BlueShipPrefab,hilighter.position,Quaternion.identity);
                if(ShipsRemainingAmount == 2 || ShipsRemainingAmount == 1 && NumOfHarboursToBeDisabled == 1)
                {
                    BlueShip = blueShip;
                    Ships[0] = BlueShip;
                }else
                {
                    SecondBlueShip = blueShip;
                    Ships[1] = SecondBlueShip;
                }
                ShipsRemainingAmount -= 1;
                ShipButtonText.text = "Normal Ship - " + ShipsRemainingAmount;
                DeployButtonPanel.gameObject.SetActive(false);
                deployButtonPanelIsActive = false;
                RandomShipGenerationButton.gameObject.SetActive(true);
            }else
            {
                Ship redShip = null;
                redShip = ShipPositions[hilighter.position] = Instantiate(RedShipPrefab,hilighter.position,RedShipsRotation);
                if(ShipsRemainingAmount == 2 || ShipsRemainingAmount == 1 && NumOfHarboursToBeDisabled == 1)
                {
                    RedShip = redShip;
                    Ships[6] = RedShip;
                }else
                {
                    SecondRedShip = redShip;
                    Ships[7] = SecondRedShip;
                }
                ShipsRemainingAmount -= 1;
                ShipButtonText.text = "Normal Ship - " + ShipsRemainingAmount;
                DeployButtonPanel.gameObject.SetActive(false);
                deployButtonPanelIsActive = false;
                RandomShipGenerationButton.gameObject.SetActive(true);
            } 
            if(blueTurn && ShipsRemainingAmount == 0 && SpeedyShipsRemainingAmount == 0 && ArmedShipRemainingAmount == 0 && ToughShipRemainingAmount == 0)
            {
                blueTurn = false;
                ShipsRemainingAmount = 2;
                SpeedyShipsRemainingAmount = 2;
                ArmedShipRemainingAmount = 1;
                ToughShipRemainingAmount = 1;
                GetNewPopupPanel();
                ActivePopupPanel.transform.GetChild(0).GetComponent<Text>().text = "Deploy your ships, Red";
                ShipButtonText.text = "Normal Ship - 2";
                SpeedyShipButtonText.text = "Speedy Ship - 2";
                if(NumOfHarboursToBeDisabled == 1)
                {
                    ShipButtonText.text = "Normal Ship - 1";
                    ShipsRemainingAmount = 1;

                }else if(NumOfHarboursToBeDisabled == 2)
                {
                    SpeedyShipButtonText.text = "Speedy Ship - 1";
                    SpeedyShipsRemainingAmount = 1;
                }
                ArmedShipButtonText.text = "Armed Ship - 1";
                ToughShipButtonText.text = "Tough Ship - 1";
                RandomShipsGenerationButtonClickedOnce = true;
            }      
            if(!blueTurn && ShipsRemainingAmount == 0 && SpeedyShipsRemainingAmount == 0 && ArmedShipRemainingAmount == 0 && ToughShipRemainingAmount == 0)
            {
                blueTurn = true;
                if(!SinglePlayerGame)
                {
                    GetNewPopupPanel();
                    ActivePopupPanel.transform.GetChild(0).GetComponent<Text>().text = "Now, you can select a ship, attack or move, Blue";
                }
                currentThing = ThingsLeftToDo.PlayTime;
                lastTimePicked = Time.time;
                RandomShipGenerationButton.gameObject.SetActive(false);
                TreasuryPanel.gameObject.SetActive(true);
                StatsButton.gameObject.SetActive(true);
                SurrenderButton.gameObject.SetActive(true);
            }
        }
    }

    public void OnSpeedyShipDeployButtonClicked()
    {
        if(SpeedyShipsRemainingAmount != 0)
        {
            if(blueTurn)
            {
                Ship blueSpeedyShip = null;
                blueSpeedyShip = ShipPositions[hilighter.position] = Instantiate(BlueSpeedyShipPrefab,hilighter.position,Quaternion.identity);
                if(SpeedyShipsRemainingAmount == 2 || SpeedyShipsRemainingAmount == 1 && NumOfHarboursToBeDisabled == 2)
                {
                    BlueSpeedyShip = blueSpeedyShip;
                    Ships[2] = BlueSpeedyShip;
                }else
                {
                    SecondBlueSpeedyShip = blueSpeedyShip;
                    Ships[3] = SecondBlueSpeedyShip;
                }
                SpeedyShipsRemainingAmount -= 1;
                SpeedyShipButtonText.text = "Speedy Ship - " + SpeedyShipsRemainingAmount;
                DeployButtonPanel.gameObject.SetActive(false);
                deployButtonPanelIsActive = false;
                RandomShipGenerationButton.gameObject.SetActive(true);
            }else
            {
                Ship redSpeedyShip = null;
                redSpeedyShip = ShipPositions[hilighter.position] = Instantiate(RedSpeedyShipPrefab,hilighter.position,RedShipsRotation);
                if(SpeedyShipsRemainingAmount == 2 || SpeedyShipsRemainingAmount == 1 && NumOfHarboursToBeDisabled == 2)
                {
                    RedSpeedyShip = redSpeedyShip;
                    Ships[8] = RedSpeedyShip;
                }else
                {
                    SecondRedSpeedyShip = redSpeedyShip;
                    Ships[9] = SecondRedSpeedyShip;
                }
                SpeedyShipsRemainingAmount -= 1;
                SpeedyShipButtonText.text = "Speedy Ship - " + SpeedyShipsRemainingAmount;
                DeployButtonPanel.gameObject.SetActive(false);
                deployButtonPanelIsActive = false;
                RandomShipGenerationButton.gameObject.SetActive(true);
            } 
            if(blueTurn && ShipsRemainingAmount == 0 && SpeedyShipsRemainingAmount == 0 && ArmedShipRemainingAmount == 0 && ToughShipRemainingAmount == 0)
            {
                blueTurn = false;
                ShipsRemainingAmount = 2;
                SpeedyShipsRemainingAmount = 2;
                ArmedShipRemainingAmount = 1;
                ToughShipRemainingAmount = 1;
                GetNewPopupPanel();
                ActivePopupPanel.transform.GetChild(0).GetComponent<Text>().text = "Deploy your ships, Red";
                ShipButtonText.text = "Normal Ship - 2";
                SpeedyShipButtonText.text = "Speedy Ship - 2";
                if(NumOfHarboursToBeDisabled == 1)
                {
                    ShipButtonText.text = "Normal Ship - 1";
                    ShipsRemainingAmount = 1;

                }else if(NumOfHarboursToBeDisabled == 2)
                {
                    SpeedyShipButtonText.text = "Speedy Ship - 1";
                    SpeedyShipsRemainingAmount = 1;
                }
                ArmedShipButtonText.text = "Armed Ship - 1";
                ToughShipButtonText.text = "Tough Ship - 1";
                RandomShipsGenerationButtonClickedOnce = true;
            }      
            if(!blueTurn && ShipsRemainingAmount == 0 && SpeedyShipsRemainingAmount == 0 && ArmedShipRemainingAmount == 0 && ToughShipRemainingAmount == 0)
            {
                blueTurn = true;
                if(!SinglePlayerGame)
                {
                    GetNewPopupPanel();
                    ActivePopupPanel.transform.GetChild(0).GetComponent<Text>().text = "Now, you can select a ship, attack or move, Blue";
                }
                currentThing = ThingsLeftToDo.PlayTime;
                lastTimePicked = Time.time;
                RandomShipGenerationButton.gameObject.SetActive(false);
                TreasuryPanel.gameObject.SetActive(true);
                StatsButton.gameObject.SetActive(true);
                SurrenderButton.gameObject.SetActive(true);
            }
        }
    }

    public void OnArmedShipDeployButtonClicked()
    {
        if(ArmedShipRemainingAmount != 0)
        {
            if(blueTurn)
            {
                Ship blueArmedShip = null;
                blueArmedShip = ShipPositions[hilighter.position] = Instantiate(BlueArmedShipPrefab,hilighter.position,Quaternion.identity);
                BlueArmedShip = blueArmedShip;
                Ships[4] = BlueArmedShip;
                ArmedShipRemainingAmount -= 1;
                ArmedShipButtonText.text = "Armed Ship - " + ArmedShipRemainingAmount;
                DeployButtonPanel.gameObject.SetActive(false);
                deployButtonPanelIsActive = false;
                RandomShipGenerationButton.gameObject.SetActive(true);
            }else
            {
                Ship redArmedShip = null;
                redArmedShip = ShipPositions[hilighter.position] = Instantiate(RedArmedShipPrefab,hilighter.position,RedShipsRotation);
                RedArmedShip = redArmedShip;
                Ships[10] = RedArmedShip;
                ArmedShipRemainingAmount -= 1;
                ArmedShipButtonText.text = "Armed Ship - " + ArmedShipRemainingAmount;
                DeployButtonPanel.gameObject.SetActive(false);
                deployButtonPanelIsActive = false;
                RandomShipGenerationButton.gameObject.SetActive(true);
            } 
            if(blueTurn && ShipsRemainingAmount == 0 && SpeedyShipsRemainingAmount == 0 && ArmedShipRemainingAmount == 0 && ToughShipRemainingAmount == 0)
            {
                blueTurn = false;
                ShipsRemainingAmount = 2;
                SpeedyShipsRemainingAmount = 2;
                ArmedShipRemainingAmount = 1;
                ToughShipRemainingAmount = 1;
                GetNewPopupPanel();
                ActivePopupPanel.transform.GetChild(0).GetComponent<Text>().text = "Deploy your ships, Red";
                ShipButtonText.text = "Normal Ship - 2";
                SpeedyShipButtonText.text = "Speedy Ship - 2";
                if(NumOfHarboursToBeDisabled == 1)
                {
                    ShipButtonText.text = "Normal Ship - 1";
                    ShipsRemainingAmount = 1;

                }else if(NumOfHarboursToBeDisabled == 2)
                {
                    SpeedyShipButtonText.text = "Speedy Ship - 1";
                    SpeedyShipsRemainingAmount = 1;
                }
                ArmedShipButtonText.text = "Armed Ship - 1";
                ToughShipButtonText.text = "Tough Ship - 1";
                RandomShipsGenerationButtonClickedOnce = true;
            }      
            if(!blueTurn && ShipsRemainingAmount == 0 && SpeedyShipsRemainingAmount == 0 && ArmedShipRemainingAmount == 0 && ToughShipRemainingAmount == 0)
            {
                blueTurn = true;
                if(!SinglePlayerGame)
                {
                    GetNewPopupPanel();
                    ActivePopupPanel.transform.GetChild(0).GetComponent<Text>().text = "Now, you can select a ship, attack or move, Blue";
                }
                currentThing = ThingsLeftToDo.PlayTime;
                lastTimePicked = Time.time;
                RandomShipGenerationButton.gameObject.SetActive(false);
                TreasuryPanel.gameObject.SetActive(true);
                StatsButton.gameObject.SetActive(true);
                SurrenderButton.gameObject.SetActive(true);
            }
        }
    }

    public void OnToughShipDeployButtonClicked()
    {
        if(ToughShipRemainingAmount != 0)
        {
            if(blueTurn)
            {
                Ship blueToughShip = null;
                blueToughShip = ShipPositions[hilighter.position] = Instantiate(BlueToughShipPrefab,hilighter.position,Quaternion.identity);
                BlueToughShip = blueToughShip;
                Ships[5] = BlueToughShip;
                ToughShipRemainingAmount -= 1;
                ToughShipButtonText.text = "Tough Ship - " + ToughShipRemainingAmount;
                DeployButtonPanel.gameObject.SetActive(false);
                deployButtonPanelIsActive = false;
                RandomShipGenerationButton.gameObject.SetActive(true);
            }else
            {
                Ship redToughShip = null;
                redToughShip = ShipPositions[hilighter.position] = Instantiate(RedToughShipPrefab,hilighter.position,RedShipsRotation);
                RedToughShip = redToughShip;
                Ships[11] = RedToughShip;
                ToughShipRemainingAmount -= 1;
                ToughShipButtonText.text = "Tough Ship - " + ToughShipRemainingAmount;
                DeployButtonPanel.gameObject.SetActive(false);
                deployButtonPanelIsActive = false;
                RandomShipGenerationButton.gameObject.SetActive(true);
            } 
            if(blueTurn && ShipsRemainingAmount == 0 && SpeedyShipsRemainingAmount == 0 && ArmedShipRemainingAmount == 0 && ToughShipRemainingAmount == 0)
            {
                blueTurn = false;
                ShipsRemainingAmount = 2;
                SpeedyShipsRemainingAmount = 2;
                ArmedShipRemainingAmount = 1;
                ToughShipRemainingAmount = 1;
                GetNewPopupPanel();
                ActivePopupPanel.transform.GetChild(0).GetComponent<Text>().text = "Deploy your ships, Red";
                ShipButtonText.text = "Normal Ship - 2";
                SpeedyShipButtonText.text = "Speedy Ship - 2";
                if(NumOfHarboursToBeDisabled == 1)
                {
                    ShipButtonText.text = "Normal Ship - 1";
                    ShipsRemainingAmount = 1;

                }else if(NumOfHarboursToBeDisabled == 2)
                {
                    SpeedyShipButtonText.text = "Speedy Ship - 1";
                    SpeedyShipsRemainingAmount = 1;
                }
                ArmedShipButtonText.text = "Armed Ship - 1";
                ToughShipButtonText.text = "Tough Ship - 1";
                RandomShipsGenerationButtonClickedOnce = true;
            }      
            if(!blueTurn && ShipsRemainingAmount == 0 && SpeedyShipsRemainingAmount == 0 && ArmedShipRemainingAmount == 0 && ToughShipRemainingAmount == 0)
            {
                blueTurn = true;
                if(!SinglePlayerGame)
                {
                    GetNewPopupPanel();
                    ActivePopupPanel.transform.GetChild(0).GetComponent<Text>().text = "Now, you can select a ship, attack or move, Blue";
                }
                currentThing = ThingsLeftToDo.PlayTime;
                lastTimePicked = Time.time;
                RandomShipGenerationButton.gameObject.SetActive(false);
                TreasuryPanel.gameObject.SetActive(true);
                StatsButton.gameObject.SetActive(true);
                SurrenderButton.gameObject.SetActive(true);
            }
        }
    }

    public void OnRandomShipGenerationButtonClicked()
    {
        if(blueTurn && currentThing == ThingsLeftToDo.SpawnShips)
        {
            //Get some random values and store them in some random variables:
            if(ShipsRemainingAmount != 0 || SpeedyShipsRemainingAmount != 0 || ArmedShipRemainingAmount != 0 || ToughShipRemainingAmount != 0)
            {
                if(ShipsRemainingAmount != 0)
                {
                    //Get a random value and if there aren't any ships at that position, place the ship there:
                    int suggestedPosition = Random.Range(0,10 - NumOfHarboursToBeDisabled);
            
                    Vector3 ActualPosition = new Vector3(19.5f - suggestedPosition,0,.5f + NumOfHarboursToBeDisabled);

                    while(ShipPositions.ContainsKey(ActualPosition))
                    {
                        suggestedPosition = Random.Range(0,10 - NumOfHarboursToBeDisabled);
                        ActualPosition = new Vector3(19.5f - suggestedPosition,0,.5f + NumOfHarboursToBeDisabled);
                    }

                    if((ShipsRemainingAmount == 2 && !Ships[0]) || ShipsRemainingAmount == 1 && !Ships[0] && NumOfHarboursToBeDisabled == 1)
                    {
                        BlueShip = ShipPositions[ActualPosition] = Instantiate(BlueShipPrefab,ActualPosition,Quaternion.identity);
                        Ships[0] = BlueShip;
                        ShipsRemainingAmount -= 1;
                    }
                    
                    int suggestedPosition2 = Random.Range(0,10 - NumOfHarboursToBeDisabled);

                    Vector3 ActualPosition2 = new Vector3(19.5f - suggestedPosition2,0,.5f + NumOfHarboursToBeDisabled);

                    while(ShipPositions.ContainsKey(ActualPosition2))
                    {
                        suggestedPosition2 = Random.Range(0,10 - NumOfHarboursToBeDisabled);
                        ActualPosition2 = new Vector3(19.5f - suggestedPosition2,0,.5f + NumOfHarboursToBeDisabled);
                    }
                    
                    if(ShipsRemainingAmount == 1 && !Ships[1])
                    {
                        SecondBlueShip = ShipPositions[ActualPosition2] = Instantiate(BlueShipPrefab, ActualPosition2, Quaternion.identity);
                        Ships[1] = SecondBlueShip;
                        ShipsRemainingAmount -= 1;
                    }
                    
                }

                if(SpeedyShipsRemainingAmount != 0)
                {
                    //Get a random value and if there aren't any ships at that position, place the ship there:
                    int suggestedPosition = Random.Range(0,10 - NumOfHarboursToBeDisabled);

                    Vector3 ActualPosition = new Vector3(19.5f - suggestedPosition,0,.5f + NumOfHarboursToBeDisabled);

                    while(ShipPositions.ContainsKey(ActualPosition))
                    {
                        suggestedPosition = Random.Range(0,10 - NumOfHarboursToBeDisabled);
                        ActualPosition = new Vector3(19.5f - suggestedPosition,0,.5f + NumOfHarboursToBeDisabled);
                    }

                    if((SpeedyShipsRemainingAmount == 2 && !Ships[2]) || SpeedyShipsRemainingAmount == 1 && !Ships[2] && NumOfHarboursToBeDisabled == 2)
                    {
                        BlueSpeedyShip = ShipPositions[ActualPosition] = Instantiate(BlueSpeedyShipPrefab,ActualPosition,Quaternion.identity);
                        Ships[2] = BlueSpeedyShip;
                        SpeedyShipsRemainingAmount -= 1;
                    }

                    int suggestedPosition2 = Random.Range(0,10 - NumOfHarboursToBeDisabled);

                    Vector3 ActualPosition2 = new Vector3(19.5f - suggestedPosition2,0,.5f + NumOfHarboursToBeDisabled);

                    while(ShipPositions.ContainsKey(ActualPosition2))
                    {
                        suggestedPosition2 = Random.Range(0,10 - NumOfHarboursToBeDisabled);
                        ActualPosition2 = new Vector3(19.5f - suggestedPosition2,0,.5f + NumOfHarboursToBeDisabled);
                    }
                    
                    if(SpeedyShipsRemainingAmount == 1 && !Ships[3])
                    {
                        SecondBlueSpeedyShip = ShipPositions[ActualPosition2] = Instantiate(BlueSpeedyShipPrefab, ActualPosition2, Quaternion.identity);
                        Ships[3] = SecondBlueSpeedyShip;
                        SpeedyShipsRemainingAmount -= 1;
                    }
                    
                }

                if(ArmedShipRemainingAmount != 0)
                {
                    //Get a random value and if there aren't any ships at that position, place the ship there:
                    int suggestedPosition = Random.Range(0,10 - NumOfHarboursToBeDisabled);

                    Vector3 ActualPosition = new Vector3(19.5f - suggestedPosition,0,.5f + NumOfHarboursToBeDisabled);
                    
                    while(ShipPositions.ContainsKey(ActualPosition))
                    {
                        suggestedPosition = Random.Range(0,10 - NumOfHarboursToBeDisabled);
                        ActualPosition = new Vector3(19.5f - suggestedPosition,0,.5f + NumOfHarboursToBeDisabled);
                    }
                    
                    if(!Ships[4])
                    {
                        BlueArmedShip = ShipPositions[ActualPosition] = Instantiate(BlueArmedShipPrefab,ActualPosition,Quaternion.identity);
                        Ships[4] = BlueArmedShip;

                        ArmedShipRemainingAmount -= 1;
                    }
                    
                }

                if(ToughShipRemainingAmount != 0)
                {
                    //Get a random value and if there aren't any ships at that position, place the ship there:
                    int suggestedPosition = Random.Range(0,10 - NumOfHarboursToBeDisabled);

                    Vector3 ActualPosition = new Vector3(19.5f - suggestedPosition,0,.5f + NumOfHarboursToBeDisabled);

                    while(ShipPositions.ContainsKey(ActualPosition))
                    {
                        suggestedPosition = Random.Range(0,10 - NumOfHarboursToBeDisabled);
                        ActualPosition = new Vector3(19.5f - suggestedPosition,0,.5f + NumOfHarboursToBeDisabled);
                    }

                    if(!Ships[5])
                    {
                        BlueToughShip = ShipPositions[ActualPosition] = Instantiate(BlueToughShipPrefab,ActualPosition,Quaternion.identity);
                        Ships[5] = BlueToughShip;

                        ToughShipRemainingAmount -= 1;
                    }
                }
            }
            
            GetNewPopupPanel();
            ActivePopupPanel.transform.GetChild(0).GetComponent<Text>().text = "Deploy your ships, Red";
            blueTurn = false;
            ShipsRemainingAmount = 2;
            SpeedyShipsRemainingAmount = 2;
            ArmedShipRemainingAmount = 1;
            ToughShipRemainingAmount = 1;
            ShipButtonText.text = "Normal Ship - 2";
            SpeedyShipButtonText.text = "Speedy Ship - 2";
            if(NumOfHarboursToBeDisabled == 1)
            {
                ShipButtonText.text = "Normal Ship - 1";
                ShipsRemainingAmount = 1;

            }else if(NumOfHarboursToBeDisabled == 2)
            {
                SpeedyShipButtonText.text = "Speedy Ship - 1";
                SpeedyShipsRemainingAmount = 1;
            }
            ArmedShipButtonText.text = "Armed Ship - 1";
            ToughShipButtonText.text = "Tough Ship - 1";
            
        }

        if(!blueTurn && currentThing == ThingsLeftToDo.SpawnShips && RandomShipsGenerationButtonClickedOnce)
        {
            //Get some random values and store them in some random variables in a loop:
            if(ShipsRemainingAmount != 0 || SpeedyShipsRemainingAmount != 0 || ArmedShipRemainingAmount != 0 || ToughShipRemainingAmount != 0)
            {
                if(ShipsRemainingAmount != 0)
                {
                    //Get a random value and if there aren't any ships at that position, place the ship there:
                    int suggestedPosition = Random.Range(0,10 - NumOfHarboursToBeDisabled);
            
                    Vector3 ActualPosition = new Vector3(19.5f - suggestedPosition,0,9.5f);

                    while(ShipPositions.ContainsKey(ActualPosition))
                    {
                        suggestedPosition = Random.Range(0,10 - NumOfHarboursToBeDisabled);
                        ActualPosition = new Vector3(19.5f - suggestedPosition,0,9.5f);
                    }

                    if((ShipsRemainingAmount == 2 && !Ships[6]) || ShipsRemainingAmount == 1 && !Ships[6] && NumOfHarboursToBeDisabled == 1)
                    {
                        RedShip = ShipPositions[ActualPosition] = Instantiate(RedShipPrefab,ActualPosition,RedShipsRotation);
                        Ships[6] = RedShip;
                        ShipsRemainingAmount -= 1;
                    }

                    int suggestedPosition2 = Random.Range(0,10 - NumOfHarboursToBeDisabled);

                    Vector3 ActualPosition2 = new Vector3(19.5f - suggestedPosition2,0,9.5f);

                    while(ShipPositions.ContainsKey(ActualPosition2))
                    {
                        suggestedPosition2 = Random.Range(0,10 - NumOfHarboursToBeDisabled);
                        ActualPosition2 = new Vector3(19.5f - suggestedPosition2,0,9.5f);
                    }
                    
                    if(ShipsRemainingAmount == 1 && !Ships[7])
                    {
                        SecondRedShip = ShipPositions[ActualPosition2] = Instantiate(RedShipPrefab, ActualPosition2, RedShipsRotation);
                        Ships[7] = SecondRedShip;
                        ShipsRemainingAmount -= 1;
                    }
                    
                }

                if(SpeedyShipsRemainingAmount != 0)
                {
                    //Get a random value and if there aren't any ships at that position, place the ship there:
                    int suggestedPosition = Random.Range(0,10 - NumOfHarboursToBeDisabled);

                    Vector3 ActualPosition = new Vector3(19.5f - suggestedPosition,0,9.5f);

                    while(ShipPositions.ContainsKey(ActualPosition))
                    {
                        suggestedPosition = Random.Range(0,10 - NumOfHarboursToBeDisabled);
                        ActualPosition = new Vector3(19.5f - suggestedPosition,0,9.5f);
                    }
        
                    if((SpeedyShipsRemainingAmount == 2 && !Ships[8]) || SpeedyShipsRemainingAmount == 1 && !Ships[8] && NumOfHarboursToBeDisabled == 2)
                    {
                        RedSpeedyShip = ShipPositions[ActualPosition] = Instantiate(RedSpeedyShipPrefab,ActualPosition,RedShipsRotation);
                        Ships[8] = RedSpeedyShip;
                        SpeedyShipsRemainingAmount -= 1;
                    }

                    int suggestedPosition2 = Random.Range(0,10 - NumOfHarboursToBeDisabled);

                    Vector3 ActualPosition2 = new Vector3(19.5f - suggestedPosition2,0,9.5f);

                    while(ShipPositions.ContainsKey(ActualPosition2))
                    {
                        suggestedPosition2 = Random.Range(0,10 - NumOfHarboursToBeDisabled);
                        ActualPosition2 = new Vector3(19.5f - suggestedPosition2,0,9.5f);
                    }
                    
                    if(SpeedyShipsRemainingAmount == 1 && !Ships[9])
                    {
                        SecondRedSpeedyShip = ShipPositions[ActualPosition2] = Instantiate(RedSpeedyShipPrefab, ActualPosition2, RedShipsRotation);
                        Ships[9] = SecondRedSpeedyShip;
                        SpeedyShipsRemainingAmount -= 1;
                    }
                    
                }

                if(ArmedShipRemainingAmount != 0)
                {
                    //Get a random value and if there aren't any ships at that position, place the ship there:
                    int suggestedPosition = Random.Range(0,10 - NumOfHarboursToBeDisabled);

                    Vector3 ActualPosition = new Vector3(19.5f - suggestedPosition,0,9.5f);
                    
                    while(ShipPositions.ContainsKey(ActualPosition))
                    {
                        suggestedPosition = Random.Range(0,10 - NumOfHarboursToBeDisabled);
                        ActualPosition = new Vector3(19.5f - suggestedPosition,0,9.5f);
                    }
                    
                    if(!Ships[10])
                    {
                        RedArmedShip = ShipPositions[ActualPosition] = Instantiate(RedArmedShipPrefab,ActualPosition,RedShipsRotation);
                        Ships[10] = RedArmedShip;

                        ArmedShipRemainingAmount -= 1;
                    }
                }

                if(ToughShipRemainingAmount != 0)
                {
                    //Get a random value and if there aren't any ships at that position, place the ship there:
                    int suggestedPosition = Random.Range(0,10 - NumOfHarboursToBeDisabled);

                    Vector3 ActualPosition = new Vector3(19.5f - suggestedPosition,0,9.5f);

                    while(ShipPositions.ContainsKey(ActualPosition))
                    {
                        suggestedPosition = Random.Range(0,10 - NumOfHarboursToBeDisabled);
                        ActualPosition = new Vector3(19.5f - suggestedPosition,0,9.5f);
                    }

                    if(!Ships[11])
                    {
                        RedToughShip = ShipPositions[ActualPosition] = Instantiate(RedToughShipPrefab,ActualPosition,RedShipsRotation);
                        Ships[11] = RedToughShip;

                        ToughShipRemainingAmount -= 1;
                    }
                }
                
                blueTurn = true;
                if(!SinglePlayerGame)
                {
                    GetNewPopupPanel();
                    ActivePopupPanel.transform.GetChild(0).GetComponent<Text>().text = "Now, you can select a ship, attack or move, Blue";
                }

                currentThing = ThingsLeftToDo.PlayTime;
                lastTimePicked = Time.time;
                RandomShipGenerationButton.gameObject.SetActive(false);
                TreasuryPanel.gameObject.SetActive(true);
                StatsButton.gameObject.SetActive(true);
                SurrenderButton.gameObject.SetActive(true);
            }
        }
        RandomShipsGenerationButtonClickedOnce = true;
    }

    public void OnXButtonClicked()
    {
        //Disable the deploy button panel
        DeployButtonPanel.gameObject.SetActive(false);
        deployButtonPanelIsActive = false;
        RandomShipGenerationButton.gameObject.SetActive(true);
    }

/*    async void InitializeAd()
    {
        try
        {
            await UnityServices.InitializeAsync();
            RoundEndAd = MediationService.Instance.CreateInterstitialAd("Interstitial_Android");
            LoadAd();

        }catch(InitializeFailedException exception)
        {
            //Do something
        }
    }
*/

/*    async void LoadAd()
    {
        try
        {
            await RoundEndAd.LoadAsync();

        }catch(LoadFailedException exception)
        {
            Invoke("OnLoadFailed", 3);
        }
    }
*/

/*    void OnLoadFailed()
    {
        Debug.Log("Attempting to reload ad");
        RoundEndAd = null;
        InitializeAd();
    }
*/

/*    async void ShowAd()
    {
        try
        {
            InterstitialAdShowOptions showOptions = new InterstitialAdShowOptions();
            showOptions.AutoReload = true;
            await RoundEndAd.ShowAsync(showOptions);

        }catch(ShowFailedException exception)
        {
            //Do something
        }
    }
*/

    // Start is called before the first frame update
    void Start()
    {
        GetRandomLoadingScreen();
        currentThing = ThingsLeftToDo.SpawnShips;
        Ship.BaseGameLogicScript = this;
        Cannonball.BaseGameLogicScript = this;
        Harbour.BaseGameLogicScript = this;
        Islands.BaseGameLogicScript = this;
/*        if(Application.platform == RuntimePlatform.Android)
        {
            InitializeAd();
        }
*/
    }

    // Update is called once per frame
    void Update()
    {
        if(!GameEnded && !ShipBuyingPanelIsActive && !deployButtonPanelIsActive)
        {
            PositionHilighter();
        }
        SelectShip();

        if(BlueValueDifference != 0 || RedValueDifference != 0)
        {
            UpdateTreasury();
        }

        if(!CameraAudioSource.isPlaying)
        {
            CameraAudioSource.PlayOneShot(OceanWavesNseagulls, OceanWavesVol);
            CameraAudioSource.PlayOneShot(WindSound, WindVol);
        }

        if(ModeFadingStarted)
        {
            ModeImageCanvasGroup.alpha -= Time.deltaTime;
            MapSizePanelCanvasGroup.alpha -= Time.deltaTime;
            if(ModeImageCanvasGroup.alpha <= 0 && MapSizePanelCanvasGroup.alpha <= 0)
            {
                ModeFadingStarted = false;
                GamemodeSelectionImage.gameObject.SetActive(false);
                MapSizePanel.gameObject.SetActive(false);
            }
        }

        if(FadingState != 0)
        {
            if(FadingState == 1)
            {
                PopupPanelCanvasGroup.alpha += Time.deltaTime;
            }
            if(PopupPanelCanvasGroup.alpha >= 1 && FadingState == 1)
            {
                FadingState = 2;
            }
            if(FadingState == 2)
            {
                Invoke("FadeOutTaskPanel",4);
                FadingState = 3;
            }
            if(FadingState == 4)
            {
                PopupPanelCanvasGroup.alpha -= Time.deltaTime;
             }
            if(PopupPanelCanvasGroup.alpha <= 0)
            {
                FadingState = 0;
            }
        }

        //The EventSystem.current.IsPointerOverGameObject() checks if the mouse is over UI (its a built-in function).
        if(currentThing == ThingsLeftToDo.PlayTime && (blueTurn && ((hilighter.position.z == .5f && NumOfHarboursToBeDisabled == 0) || (hilighter.position.z == 1.5f && NumOfHarboursToBeDisabled == 1) || (hilighter.position.z == 2.5f && NumOfHarboursToBeDisabled == 2)) || !blueTurn && hilighter.position.z == 9.5f) && Input.GetMouseButtonDown(0) && ((SinglePlayerGame && !blueTurn) || !SinglePlayerGame) && GamemodePicked && hilighterIsActive && !ShipPositions.ContainsKey(hilighter.position) && !EventSystem.current.IsPointerOverGameObject() && !Ship.ShipMoving)
        {
            if(SelectedShip)
            {
                if(!SelectedShip.MovementTileHolderIsActive && !SelectedShip.AttackTileHolderIsActive)
                {
                    ShipBuyingPanel.gameObject.SetActive(true);
                    ShipBuyingPanelIsActive = true;
                }
            }else
            {
                ShipBuyingPanel.gameObject.SetActive(true);
                ShipBuyingPanelIsActive = true;
            }
        }
        
        if(currentThing == ThingsLeftToDo.SpawnShips && Input.GetMouseButton(0))
        {
            DeployShip();
        }
        if(currentThing == ThingsLeftToDo.PlayTime && !Ship.ShipMoving && !Ship.ShipAttacking && !GameEnded)
        {
            if(secondsLeft == 30 && !TimePickedOnce)
            {
                lastTimePicked = Time.time;
                TimeRemainingText.text = "30s Remaining";
                TimePickedOnce = true;
            }

            if(Time.time - lastTimePicked >= 1)
            {
                lastTimePicked = Time.time;
                secondsLeft -= 1;
                TimeRemainingText.text = secondsLeft + "s Remaining";
            }

            if(secondsLeft == 0)
            {
                TreasuryText.text = "0";
                if(blueTurn)
                {
                    blueTurn = false;
                    RedValueDifference = RedIslands;
                    BlueTreasury += BlueIslands;
                    GetNewPopupPanel();
                    ActivePopupPanel.transform.GetChild(0).GetComponent<Text>().text = "Now, you can select a ship, attack or move, Red";
                }else
                {
                    blueTurn = true;
                    BlueValueDifference = BlueIslands;
                    RedTreasury += RedIslands;
                    if(!SinglePlayerGame)
                    {
                        GetNewPopupPanel();
                        ActivePopupPanel.transform.GetChild(0).GetComponent<Text>().text = "Now, you can select a ship, attack or move, Blue";
                    }
                }
                lastTimePicked = Time.time;
                secondsLeft = 30;
                TimeRemainingText.text = "30s Remaining";
                TimePickedOnce = false;
                Harbour.ShipsGotHealedIndicator = false;
                ShipBuyingPanel.gameObject.SetActive(false);
                ShipBuyingPanelIsActive = false;
                Ship.numberOfTimesMoved = 0;
                if(SelectedShip)
                {
                    SelectedShip.TargetPosition = Vector3.zero;
                    SelectedShip.desiredRotation = new Quaternion(0,0,0,0);
                    SelectedShip.AttackTileHolder.gameObject.SetActive(false);
                    SelectedShip.AttackTileHolderIsActive = false;
                    SelectedShip.MovementTileHolder.gameObject.SetActive(false);
                    SelectedShip.MovementTileHolderIsActive = false;
                }
                Ship.RebuildShipPositions();
                AttackButton.gameObject.SetActive(false);
                MovementButton.gameObject.SetActive(false);
                Ship.movementFinished = false;
                Ship.rotationFinished = false;
                Ship.TargetPositionAndRotationPicked = false;
                Ship.ShipReadyToMove = false;
                Ship.ShipMoving = false;
                Ship.ShipAllowedToMove = false;
                SelectionStatsPanel.gameObject.SetActive(false);
                Ship.ShipAttacking = false;
                Ship.ShipReadyToAttack = false;
                Ship.AttackButtonClicked = false;
                Ship.ShipAllowedToAttack = false;
                AttackOrMovementXButton.gameObject.SetActive(false);
                SurrenderButton.gameObject.SetActive(true);
                Ship.ShipSinking = false;
                Harbour.ShipsGotHealedIndicator = false;
                Ship.cannonballFired = false;
                SelectedShip = null;
                AttackedShip = null;
            }
        }
        if((Harbour.BlueHarbourInvaded || Harbour.RedHarbourInvaded) && !Ship.ShipMoving && !GameEnded)
        {
            //Activate the winning Panel:
            WinningPanel.gameObject.SetActive(true);
            SelectionStatsPanel.gameObject.SetActive(false);
            SurrenderButton.gameObject.SetActive(false);

            if(Harbour.RedHarbourInvaded)
            {
                if(!SinglePlayerGame)
                {
                    WinningText.text = "Congratulations, Blue! Red's Harbour Was Invaded! Press the button below if you would like to play another match!";
                }else
                {
                    WinningText.text = "Well, you lost! Try a different strategy next time!";
                }

                if(!ScoreUpdated)
                {
                    timesBlueWon += 1;
                    BlueScoreText.text = "" + timesBlueWon;
                    RedScoreText.text = "" + timesRedWon;
                    ScoreUpdated = true;
                }
            }else if(Harbour.BlueHarbourInvaded)
            {
                WinningText.text = "Congratulations, Red! Blue's Harbour Was Invaded! Press the button below if you would like to play another match!";
                if(!ScoreUpdated)
                {
                    timesRedWon += 1;
                    RedScoreText.text = "" + timesRedWon;
                    BlueScoreText.text = "" + timesBlueWon;
                    ScoreUpdated = true;
                }
            }
            CameraAudioSource.Play();
            GameEnded = true;
            hilighter.position = new Vector3(0,0,0);
        }
        if((BlueFleetIsDead || RedFleetIsDead) && !GameEnded)
        {
            //Activate the winning Panel:
            WinningPanel.gameObject.SetActive(true);
            SelectionStatsPanel.gameObject.SetActive(false);
            SurrenderButton.gameObject.SetActive(false);

            if(BlueFleetIsDead)
            {
                WinningText.text = "Congratulations, Red! Blue Lost Their Fleet! Press the button below if you would like to play another match!";
                if(!ScoreUpdated)
                {
                    timesRedWon += 1;
                    RedScoreText.text = "" + timesRedWon;
                    BlueScoreText.text = "" + timesBlueWon;
                    ScoreUpdated = true;
                }
            }else if(RedFleetIsDead)
            {
                if(!SinglePlayerGame)
                {
                    WinningText.text = "Congratulations, Blue! Red Lost Their Fleet! Press the button below if you would like to play another match!";
                }else
                {
                    WinningText.text = "Well, you lost! Try a different strategy next time!";
                }

                if(!ScoreUpdated)
                {
                    timesBlueWon += 1;
                    BlueScoreText.text = "" + timesBlueWon;
                    RedScoreText.text = "" + timesRedWon;
                    ScoreUpdated = true;
                }
            }
            CameraAudioSource.Play();
            GameEnded = true;
            hilighter.position = new Vector3(0,0,0);
        }
        if(GamemodePicked && SinglePlayerGame && blueTurn && currentThing == ThingsLeftToDo.PlayTime && !BlueFleetIsDead)
        {
            AImovementAndAttacking();
        }
    }
}
