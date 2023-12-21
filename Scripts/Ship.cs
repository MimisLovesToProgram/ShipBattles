using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class Ship : MonoBehaviour
{

    //The Movement/Attack tiles, and bools to tell whether they are active or not:
    public Transform AttackTileHolder;
    public bool AttackTileHolderIsActive;
    public Transform MovementTileHolder;
    public bool MovementTileHolderIsActive;

    //determines whether attacking finished:
    public static bool attackFinished;

    //Determines whether the user spawned a cannonball:
    public static bool cannonballFired = false;

    //bools that define whether the ship is ready to move/attack:
    public static bool ShipReadyToMove = false;
    public static bool ShipReadyToAttack = false;

    //3 bools that turn true if the movement or rotation finishes or a ship sunk:
    public static bool movementFinished = false;
    public static bool rotationFinished = false;

    //Determines if the ship is sinking:
    public static bool ShipSinking = false;

    //Same as the upper one, but for movement:
    public static bool TargetPositionAndRotationPicked = false;

    //This ship's desired rotation:
    public Quaternion desiredRotation = new Quaternion(0,0,0,0);

    //The ship's targetPosition:
    public Vector3 TargetPosition = Vector3.zero;

    //The units the ship is able to move per turn.
    public int unitsPerTurn;

    //The number of times the ship moved this round:
    public static int numberOfTimesMoved = 0;

    //The number of lives the ship has.
    public float lives;

    //The maximum lives each ship has:
    public float maxLives;

    //The damage the ship does to another per cannonball
    public float damage;

    //The number of units the cannonball of each ship can get to:
    public int MaxAttackRange;



    //Tells us which direction the ship is looking at (used for AI ships only):
    public string directionAIshipLooking = "z";
    
    //Reference to the baseGameLogic script:
    public static BaseGameLogic BaseGameLogicScript;

    //X and Z bounds (determining if the targetPosition of the ship is outside of the map or not.)
    public static int maximumX = 20;
    public static int minimumX = 10;
    public static int minimumZ = 0;
    public static int maximumZ = 10;

    //The variables which tell us wether we pressed any of the action buttons or not.
    public static bool MovementButtonClicked = false;
    public static bool AttackButtonClicked = false;

    //And the cannonball spawn point:
    public Transform CannonballSpawnPoint;

    //The IslandGeneration script:
    public static IslandGeneration IslandGenerationScript;

    //The hilighter script:
    public static Hilighter HilighterScript;

    //When the ships sink, we need this var to know what was the attacked ship's first position:
    public static float AttackedShipYBeforeSinking;

    //..and a bool to get it only once:
    public static bool AttackedShipYBeforeSinkingPicked = false;

    //Bools to tell the timer to stop when a ship oves or attacks
    public static bool ShipMoving = false;
    public static bool ShipAttacking = false;

    //Just important, dont ask why
    public static bool SurrenderButtonClicked;
    public static bool ShipAllowedToMove;
    public static bool ShipAllowedToAttack;

    //A list of the island this ship is defending:
    public List<Islands> IslandsDefended;

    //A list of the enemy islands this ship is at:
    public List<Islands> EnemyIslandsAssaulted;

    public void OnAttackButtonClicked()
    {
        if(!ShipMoving && !ShipAttacking)
        {
            BaseGameLogicScript.SelectedShip.AttackTileHolder.gameObject.SetActive(true);
            BaseGameLogicScript.SelectedShip.AttackTileHolderIsActive = true;
            BaseGameLogicScript.SelectedShip.MovementTileHolder.gameObject.SetActive(false);
            BaseGameLogicScript.SelectedShip.MovementTileHolderIsActive = false;
            AttackButtonClicked = true;
            MovementButtonClicked = false;
        }
    }

    public void OnMovementButtonClicked()
    {
        if(!ShipMoving && !ShipAttacking)
        {
            BaseGameLogicScript.SelectedShip.MovementTileHolder.gameObject.SetActive(true);
            BaseGameLogicScript.SelectedShip.MovementTileHolderIsActive = true;
            for(int i = 0; i <=3 ; i++)
            {
                BaseGameLogicScript.SelectedShip.MovementTileHolder.GetChild(i).gameObject.SetActive(true);
                for(int a = 0; a < IslandGenerationScript.IslandPositions.Count; a++)
                {
                    if(Mathf.Abs(Vector3.Distance(BaseGameLogicScript.SelectedShip.MovementTileHolder.GetChild(i).gameObject.transform.position, IslandGenerationScript.IslandPositions[a])) < .01)
                    {
                        BaseGameLogicScript.SelectedShip.MovementTileHolder.GetChild(i).gameObject.SetActive(false);
                    }
                }
        
                if(!(BaseGameLogicScript.SelectedShip.MovementTileHolder.GetChild(i).gameObject.transform.position.z > minimumZ && BaseGameLogicScript.SelectedShip.MovementTileHolder.GetChild(i).gameObject.transform.position.z < maximumZ && BaseGameLogicScript.SelectedShip.MovementTileHolder.GetChild(i).gameObject.transform.position.x > minimumX && BaseGameLogicScript.SelectedShip.MovementTileHolder.GetChild(i).gameObject.transform.position.x < maximumX))
                {
                    BaseGameLogicScript.SelectedShip.MovementTileHolder.GetChild(i).gameObject.SetActive(false);
                }
            
                foreach(var shipPos in BaseGameLogic.ShipPositions.Keys)
                {
                    if(Mathf.Abs(Vector3.Distance(BaseGameLogicScript.SelectedShip.MovementTileHolder.GetChild(i).gameObject.transform.position, shipPos)) < .00003)
                    {
                        BaseGameLogicScript.SelectedShip.MovementTileHolder.GetChild(i).gameObject.SetActive(false);
                    }
                }
            }
            BaseGameLogicScript.SelectedShip.AttackTileHolder.gameObject.SetActive(false);
            BaseGameLogicScript.SelectedShip.AttackTileHolderIsActive = false;
            MovementButtonClicked = true;
            AttackButtonClicked = false;
        }
    }

    public void OnMovementOrAttackXButtonClicked()
    {
        if(numberOfTimesMoved == 0 && !ShipMoving && !ShipAttacking)
        {
            BaseGameLogicScript.AttackButton.gameObject.SetActive(false);
            BaseGameLogicScript.MovementButton.gameObject.SetActive(false);
            BaseGameLogicScript.AttackOrMovementXButton.gameObject.SetActive(false);
            BaseGameLogicScript.SurrenderButton.gameObject.SetActive(true);
            BaseGameLogicScript.SelectedShip.AttackTileHolder.gameObject.SetActive(false);
            BaseGameLogicScript.SelectedShip.AttackTileHolderIsActive = false;
            BaseGameLogicScript.SelectedShip.MovementTileHolder.gameObject.SetActive(false);
            BaseGameLogicScript.SelectedShip.MovementTileHolderIsActive = false;
            ShipReadyToAttack = false;
            BaseGameLogicScript.SelectedShip = null;
            MovementButtonClicked = false;
            AttackButtonClicked = false;
            BaseGameLogicScript.SelectionStatsPanel.gameObject.SetActive(false);
        }
    }

    public void OnSurrenderButtonClicked()
    {
        if(((BaseGameLogic.SinglePlayerGame && !BaseGameLogic.blueTurn) || !BaseGameLogic.SinglePlayerGame) && !BaseGameLogicScript.GameEnded)
        {
            SurrenderButtonClicked = true;
            BaseGameLogicScript.WinningPanel.gameObject.SetActive(true);
            BaseGameLogicScript.SelectionStatsPanel.gameObject.SetActive(false);
            BaseGameLogicScript.SurrenderButton.gameObject.SetActive(false);
            if(BaseGameLogic.blueTurn)
            {
                BaseGameLogicScript.WinningText.text = "You won, Red! Blue Surrendered! Press the button below if you would like to play another match!";
                if(!BaseGameLogicScript.ScoreUpdated)
                {
                    BaseGameLogicScript.timesRedWon += 1;
                    BaseGameLogicScript.RedScoreText.text = "" + BaseGameLogicScript.timesRedWon;
                    BaseGameLogicScript.BlueScoreText.text = "" + BaseGameLogicScript.timesBlueWon;
                    BaseGameLogicScript.ScoreUpdated = true;
                }
            }else
            {
                if(!BaseGameLogic.SinglePlayerGame)
                {
                    BaseGameLogicScript.WinningText.text = "You won, Blue! Red Surrendered! Press the button below if you would like to play another match!";
                }else
                {
                    BaseGameLogicScript.WinningText.text = "Well, Blue has won since you have just surrendered. But why did you do that?";
                }

                if(!BaseGameLogicScript.ScoreUpdated)
                {
                    BaseGameLogicScript.timesBlueWon += 1;
                    BaseGameLogicScript.BlueScoreText.text = "" + BaseGameLogicScript.timesBlueWon;
                    BaseGameLogicScript.RedScoreText.text = "" + BaseGameLogicScript.timesRedWon;
                    BaseGameLogicScript.ScoreUpdated = true;
                }
            }
            BaseGameLogicScript.CameraAudioSource.Play();
            BaseGameLogicScript.GameEnded = true;
            BaseGameLogicScript.hilighter.position = new Vector3(0,0,0);
        }
    }
    
    void PreMovement()
    {
        //Run only if the user has not used all their moves:
        if(numberOfTimesMoved < BaseGameLogicScript.SelectedShip.unitsPerTurn)
        {
            if(Input.GetMouseButton(0) && HilighterScript.hilighterIsAtMovementTile)
            {
                ShipReadyToMove = true;
            }
        }
    }

    void Movement()
    {
        if(!TargetPositionAndRotationPicked)
        {
            TargetPosition = BaseGameLogicScript.hilighter.transform.position;
            desiredRotation = Quaternion.LookRotation(TargetPosition - gameObject.transform.position, Vector3.up);
            TargetPositionAndRotationPicked = true;
            MovementTileHolder.gameObject.SetActive(false);
            MovementTileHolderIsActive = false;
        }
        if(TargetPosition == new Vector3(gameObject.transform.position.x,0,gameObject.transform.position.z - 1) || TargetPosition == new Vector3(gameObject.transform.position.x,0,gameObject.transform.position.z + 1) || TargetPosition == new Vector3(gameObject.transform.position.x - 1,0,gameObject.transform.position.z) || TargetPosition == new Vector3(gameObject.transform.position.x + 1,0,gameObject.transform.position.z))
        {
            ShipAllowedToMove = true;
            BaseGameLogicScript.CurveTime = -0.9f;
            
        }else if(!ShipAllowedToMove)
        {
            ShipReadyToMove = false;
            TargetPosition = Vector3.zero;
            desiredRotation = new Quaternion(0,0,0,0);
            TargetPositionAndRotationPicked = false;
        }
        if(ShipAllowedToMove)
        {
            //Rotate the ship
            gameObject.transform.rotation = Quaternion.Lerp(gameObject.transform.rotation, desiredRotation, 4.8f * Time.deltaTime);

            if(Mathf.Abs(desiredRotation.y) - Mathf.Abs(gameObject.transform.rotation.y) < 0.00002 && !rotationFinished)
            {
                rotationFinished = true;
            }

            if(!ShipMoving)
            {
                ShipMoving = true;
                BaseGameLogicScript.ShipsAudioSource.PlayOneShot(BaseGameLogicScript.ShipHornSound, BaseGameLogicScript.ShipHornVol);
            }
            BaseGameLogicScript.CurveTime += Time.deltaTime;

            float speed = .56f;
            speed *= BaseGameLogicScript.SpeedAnimation.Evaluate(BaseGameLogicScript.CurveTime);

            //Move the ship
            gameObject.transform.position = Vector3.MoveTowards(gameObject.transform.position, TargetPosition, speed * Time.deltaTime);
 
            if(gameObject.transform.position == TargetPosition && !movementFinished)
            {
                movementFinished = true;
            }
                
            if(movementFinished && rotationFinished)
            {
                //..and then rotate and move the ship at the point it should exactly be looking at:
                gameObject.transform.rotation = desiredRotation;
                gameObject.transform.position = TargetPosition;

                numberOfTimesMoved += 1;
                BaseGameLogicScript.AttackButton.gameObject.SetActive(true);
                BaseGameLogicScript.MovementButton.gameObject.SetActive(true);
                movementFinished = false;
                rotationFinished = false;
                TargetPositionAndRotationPicked = false;
                ShipReadyToMove = false;
                ShipMoving = false;
                ShipAllowedToMove = false;

                //finally, rebuild the dictionary 'ShipPositions':
                RebuildShipPositions();
            }

            if(numberOfTimesMoved == unitsPerTurn)
            {
                BaseGameLogicScript.TreasuryText.text = "0";
                MovementButtonClicked = false; 
                if(BaseGameLogic.blueTurn)
                {
                    if(!Harbour.RedHarbourInvaded)
                    {
                        BaseGameLogicScript.GetNewPopupPanel();
                        BaseGameLogicScript.ActivePopupPanel.transform.GetChild(0).GetComponent<Text>().text = "Now, you can select a ship, attack or move, Red";
                    }
                    BaseGameLogicScript.RedValueDifference = BaseGameLogic.RedIslands;
                    BaseGameLogic.blueTurn = false;

                }else
                {
                    if(!BaseGameLogic.SinglePlayerGame)
                    {
                        if(!Harbour.BlueHarbourInvaded)
                        {
                            BaseGameLogicScript.GetNewPopupPanel();
                            BaseGameLogicScript.ActivePopupPanel.transform.GetChild(0).GetComponent<Text>().text = "Now, you can select a ship, attack or move, Blue";
                        }
                    }
                    BaseGameLogicScript.BlueValueDifference = BaseGameLogic.BlueIslands;
                    BaseGameLogic.blueTurn = true;
                }
                BaseGameLogicScript.AttackButton.gameObject.SetActive(false);
                BaseGameLogicScript.MovementButton.gameObject.SetActive(false);
                BaseGameLogicScript.SelectionStatsPanel.gameObject.SetActive(false);
                TargetPosition = Vector3.zero;
                desiredRotation = new Quaternion(0,0,0,0);
                TargetPositionAndRotationPicked = false;
                numberOfTimesMoved = 0;
                Harbour.ShipsGotHealedIndicator = false;
                BaseGameLogicScript.lastTimePicked = Time.time;
                BaseGameLogicScript.secondsLeft = 30;
                BaseGameLogicScript.TimeRemainingText.text = "30s Remaining";
                BaseGameLogicScript.TimePickedOnce = false;
                BaseGameLogicScript.MovementButton.gameObject.SetActive(false);
                BaseGameLogicScript.AttackButton.gameObject.SetActive(false);
                BaseGameLogicScript.AttackOrMovementXButton.gameObject.SetActive(false);
                BaseGameLogicScript.SurrenderButton.gameObject.SetActive(true);
                BaseGameLogicScript.SelectedShip = null;
            }
        }
    }
    
    void PreAttack()
    {
        if(Input.GetMouseButton(0) && HilighterScript.hilighterIsAtAttackTile)
        {
            ShipReadyToAttack = true;
        }
    }

    void Attack()
    {
        AttackTileHolder.gameObject.SetActive(false);
        AttackTileHolderIsActive = false;
        //i is for every attack tile within the range
        for(int i = 0; i <= MaxAttackRange; i++)
        {
            if(BaseGameLogic.blueTurn)
            {
                if(BaseGameLogicScript.hilighter.position == new Vector3(gameObject.transform.position.x,0,gameObject.transform.position.z + i) || BaseGameLogicScript.hilighter.position == new Vector3(gameObject.transform.position.x,0,gameObject.transform.position.z - i))
                {
                    ShipAllowedToAttack = true;

                }else if(BaseGameLogicScript.hilighter.position == new Vector3(gameObject.transform.position.x + i,0,gameObject.transform.position.z))
                {
                    ShipAllowedToAttack = true;
                    
                }else if(BaseGameLogicScript.hilighter.position == new Vector3(gameObject.transform.position.x - i,0,gameObject.transform.position.z))
                {
                    ShipAllowedToAttack = true;
                }
            }else
            {
                if(BaseGameLogicScript.hilighter.position == new Vector3(gameObject.transform.position.x,0,gameObject.transform.position.z - i) || BaseGameLogicScript.hilighter.position == new Vector3(gameObject.transform.position.x,0,gameObject.transform.position.z + i))
                {
                    ShipAllowedToAttack = true;

                }else if(BaseGameLogicScript.hilighter.position == new Vector3(gameObject.transform.position.x - i,0,gameObject.transform.position.z))
                {
                    ShipAllowedToAttack = true;
                    
                }else if(BaseGameLogicScript.hilighter.position == new Vector3(gameObject.transform.position.x + i,0,gameObject.transform.position.z))
                {
                    ShipAllowedToAttack = true;
                }
            }
        }
        if(ShipAllowedToAttack)
        {
            if(!cannonballFired)
            {
                Instantiate(BaseGameLogicScript.CannonballPrefab,CannonballSpawnPoint.gameObject.transform.position,gameObject.transform.rotation);
                BaseGameLogicScript.CameraAudioSource.PlayOneShot(BaseGameLogicScript.CannonBoom, BaseGameLogicScript.CannonVol);
                cannonballFired = true;
                ShipAttacking = true;
            }
                
            if(attackFinished)
            {
                BaseGameLogicScript.TreasuryText.text = "0";
                attackFinished = false;
                if(BaseGameLogic.blueTurn)
                {
                    if(!BaseGameLogicScript.RedFleetIsDead)
                    {
                        BaseGameLogicScript.GetNewPopupPanel();
                        BaseGameLogicScript.ActivePopupPanel.transform.GetChild(0).GetComponent<Text>().text = "Now, you can select a ship, attack or move, Red";
                    }
                    BaseGameLogicScript.RedValueDifference = BaseGameLogic.RedIslands;
                    BaseGameLogic.blueTurn = false;
                }else
                {
                    if(!BaseGameLogic.SinglePlayerGame)
                    {
                        if(!BaseGameLogicScript.BlueFleetIsDead)
                        {
                            BaseGameLogicScript.GetNewPopupPanel();
                            BaseGameLogicScript.ActivePopupPanel.transform.GetChild(0).GetComponent<Text>().text = "Now, you can select a ship, attack or move, Blue";
                        }
                    }
                    BaseGameLogicScript.BlueValueDifference = BaseGameLogic.BlueIslands;
                    BaseGameLogic.blueTurn = true;
                }
                BaseGameLogicScript.SelectionStatsPanel.gameObject.SetActive(false);
                ShipAttacking = false;
                ShipReadyToAttack = false;
                AttackButtonClicked = false;
                ShipAllowedToAttack = false;
                AttackTileHolder.gameObject.SetActive(false);
                AttackTileHolderIsActive = false;
                BaseGameLogicScript.MovementButton.gameObject.SetActive(false);
                BaseGameLogicScript.AttackButton.gameObject.SetActive(false);
                BaseGameLogicScript.AttackOrMovementXButton.gameObject.SetActive(false);
                BaseGameLogicScript.SurrenderButton.gameObject.SetActive(true);
                BaseGameLogicScript.lastTimePicked = Time.time;
                BaseGameLogicScript.secondsLeft = 30;
                BaseGameLogicScript.TimeRemainingText.text = "30s Remaining";
                BaseGameLogicScript.TimePickedOnce = false;
                ShipSinking = false;
                Harbour.ShipsGotHealedIndicator = false;
                cannonballFired = false;
                BaseGameLogicScript.SelectedShip = null;
                BaseGameLogicScript.AttackedShip = null;
                numberOfTimesMoved = 0;
            }
        }else
        {
            ShipReadyToAttack = false;
        }
    }

    void Sink()
    {
        //Get required values:
        if(!AttackedShipYBeforeSinkingPicked)
        {
            AttackedShipYBeforeSinking = gameObject.transform.position.y;
            BaseGameLogicScript.ShipsAudioSource.Play();
            AttackedShipYBeforeSinkingPicked = true;
        }
        
        if(unitsPerTurn == 2)
        {
            //Move towards the "Sunken Ship Position"
            gameObject.transform.position = Vector3.MoveTowards(gameObject.transform.position, new Vector3(gameObject.transform.position.x,AttackedShipYBeforeSinking - .7f, gameObject.transform.position.z), .16f * Time.deltaTime);

        }else
        {
            //Move towards the new "Sunken Ship Position"
            gameObject.transform.position = Vector3.MoveTowards(gameObject.transform.position, new Vector3(gameObject.transform.position.x,AttackedShipYBeforeSinking - .9f, gameObject.transform.position.z), .16f * Time.deltaTime);
        }

        if((gameObject.transform.position.y <= (AttackedShipYBeforeSinking - .7f) && unitsPerTurn == 2) || (gameObject.transform.position.y <= (AttackedShipYBeforeSinking - .9f) && unitsPerTurn == 1))
        {
            movementFinished = true;
        }

        if(movementFinished)
        {
            ShipSinking = false;

            string nameCutout = gameObject.name.Substring(4);
            if(nameCutout == " Armed Ship(Clone)")
            {
                BaseGameLogicScript.Ships[4] = null;
                BaseGameLogicScript.BlueArmedShip = null;
                BaseGameLogicScript.BlueArmedShipImage.gameObject.SetActive(false);
        
            }else if(nameCutout == " Tough Ship(Clone)")
            {
                BaseGameLogicScript.Ships[5] = null;
                BaseGameLogicScript.BlueToughShip = null;
                BaseGameLogicScript.BlueToughShipImage.gameObject.SetActive(false);
                
            }else if(nameCutout == " ship(Clone)")
            {
                if(BaseGameLogicScript.BlueShip == this)
                {
                    BaseGameLogicScript.Ships[0] = null;
                    BaseGameLogicScript.BlueShip = null;
                    BaseGameLogicScript.BlueShipImage1.gameObject.SetActive(false);

                }else if(BaseGameLogicScript.SecondBlueShip == this)
                {
                    BaseGameLogicScript.Ships[1] = null;
                    BaseGameLogicScript.SecondBlueShip = null;
                    BaseGameLogicScript.BlueShipImage2.gameObject.SetActive(false);
                }
                
            }else if(nameCutout == " Speedy Ship(Clone)")
            {
                if(BaseGameLogicScript.BlueSpeedyShip == this)
                {
                    BaseGameLogicScript.Ships[2] = null;
                    BaseGameLogicScript.BlueSpeedyShip = null;
                    BaseGameLogicScript.BlueSpeedyShipImage1.gameObject.SetActive(false);

                }else if(BaseGameLogicScript.SecondBlueSpeedyShip == this)
                {
                    BaseGameLogicScript.Ships[3] = null;
                    BaseGameLogicScript.SecondBlueSpeedyShip = null;
                    BaseGameLogicScript.BlueSpeedyShipImage2.gameObject.SetActive(false);
                }
        
            }else if(nameCutout == "Armed Ship(Clone)")
            {
                BaseGameLogicScript.Ships[10] = null;
                BaseGameLogicScript.RedArmedShip = null;
                BaseGameLogicScript.RedArmedShipImage.gameObject.SetActive(false);

            }else if(nameCutout == "Tough Ship(Clone)")
            {
                BaseGameLogicScript.Ships[11] = null;
                BaseGameLogicScript.RedToughShip = null;
                BaseGameLogicScript.RedToughShipImage.gameObject.SetActive(false);

            }else if(nameCutout == "ship(Clone)")
            {
                if(BaseGameLogicScript.RedShip == this)
                {
                    BaseGameLogicScript.Ships[6] = null;
                    BaseGameLogicScript.RedShip = null;
                    BaseGameLogicScript.RedShipImage1.gameObject.SetActive(false);

                }else if(BaseGameLogicScript.SecondRedShip == this)
                {
                    BaseGameLogicScript.Ships[7] = null;
                    BaseGameLogicScript.SecondRedShip = null;
                    BaseGameLogicScript.RedShipImage2.gameObject.SetActive(false);
                }

            }else if(nameCutout == "Speedy Ship(Clone)")
            {
                if(BaseGameLogicScript.RedSpeedyShip == this)
                {
                    BaseGameLogicScript.Ships[8] = null;
                    BaseGameLogicScript.RedSpeedyShip = null;
                    BaseGameLogicScript.RedSpeedyShipImage1.gameObject.SetActive(false);

                }else if(BaseGameLogicScript.SecondRedSpeedyShip == this)
                {
                    BaseGameLogicScript.Ships[9] = null;
                    BaseGameLogicScript.SecondRedSpeedyShip = null;
                    BaseGameLogicScript.RedSpeedyShipImage2.gameObject.SetActive(false);
                }
            }
            foreach(Islands island in IslandsDefended)
            {
                island.NumberOfIslandDefs -= 1;
                if(island.NumberOfIslandDefs == 0 && island.NumberOfIslandEnemies > 0)
                {
                    if(gameObject.layer == 12)
                    {
                        BaseGameLogic.BlueIslands -= 1;
                        BaseGameLogic.RedIslands += 1;
                        foreach(Ship ship in island.EnemyShipsDocked)
                        {
                            ship.IslandsDefended.Add(island);
                        }
                        island.NumberOfIslandDefs += island.EnemyShipsDocked.Count;
                        island.NumberOfIslandEnemies = 0;
                        island.EnemyShipsDocked = new List<Ship>();
                        island.IslandFlag.GetComponent<Renderer>().material.color = island.RedFlagColor;
                        island.IslandIsBlue = false;
                        island.IslandIsFree = false;
                    }else
                    {
                        BaseGameLogic.BlueIslands += 1;
                        BaseGameLogic.RedIslands -= 1;
                        foreach(Ship ship in island.EnemyShipsDocked)
                        {
                            ship.IslandsDefended.Add(island);
                        }
                        island.NumberOfIslandDefs += island.EnemyShipsDocked.Count;
                        island.NumberOfIslandEnemies = 0;
                        island.EnemyShipsDocked = new List<Ship>();
                        island.IslandFlag.GetComponent<Renderer>().material.color = island.BlueFlagColor;
                        island.IslandIsBlue = true;
                        island.IslandIsFree = false;
                    }
                }
            }
            AttackedShipYBeforeSinking = 0;
            AttackedShipYBeforeSinkingPicked = false;
            movementFinished = false;
            attackFinished = true;
            BaseGameLogic.AIstartedAttacking = false;
            Destroy(gameObject);
            RebuildShipPositions();
        }
    }

    public static Dictionary<Vector3,Ship> RebuildShipPositions()
    {
        BaseGameLogic.ShipPositions = new Dictionary<Vector3, Ship>();

        //After that, add the ships with their positions one by one in a "for" loop:
        foreach(Ship ship in BaseGameLogicScript.Ships)
        {
            if(ship)
            {
                BaseGameLogic.ShipPositions[ship.gameObject.transform.position] = ship;
            }
        }
        return BaseGameLogic.ShipPositions;
    }

    // Update is called once per frame
    void Update()
    {
        if(BaseGameLogic.currentThing == BaseGameLogic.ThingsLeftToDo.PlayTime)
        {
            if(MovementButtonClicked)
            {
                PreMovement();
            }

            if(ShipReadyToMove)
            {
                if(this == BaseGameLogicScript.SelectedShip)
                {
                    Movement();
                }
            }
           
            if(AttackButtonClicked)
            {
                PreAttack();
            }

            if(ShipReadyToAttack)
            {
                if(this == BaseGameLogicScript.SelectedShip)
                {
                    Attack();
                }
            }

            if(ShipSinking)
            {
                if(this == BaseGameLogicScript.AttackedShip)
                {
                    Sink();
                }
            }
        }
    }
}
