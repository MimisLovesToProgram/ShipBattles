using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Islands : MonoBehaviour
{
    //An instance to the BaseGameLogic Script:
    public static BaseGameLogic BaseGameLogicScript;

    //Determines whether this island is ready to be conquered by another player:
    public bool IslandIsFree = true;

    //Who does this island belong to;
    public bool IslandIsBlue;

    //Is this island neutral;
    public bool IslandIsNeutral = true;

    //the number of ships defending the island:
    public int NumberOfIslandDefs = 0;

    //The number of enemy ships on the same island:
    public int NumberOfIslandEnemies = 0;

    //The enemy ships docked on this island:
    public List<Ship> EnemyShipsDocked;

    //The GameObjects of this island's flags:
    public GameObject IslandFlag;
    public GameObject SecondFlagPart;

    //Their holder:
    public GameObject FlagHolder;

    //The blue and red colors:
    public Color BlueFlagColor;
    public Color RedFlagColor;

    //The highest and lowest points of the flag(the gameObjects):
    public GameObject LowestFlagPosition;
    public GameObject HighestFlagPosition;

    //The curve the flag will follow when going up and down, with a time to get at the Evaluate call:
    public AnimationCurve FlagCurve;
    public float FlagTime;

    //A bool to define if the flag has to go up and down:
    public bool FlagGoUpNdown;

    //The color the GetFlagToMove method will paint the flag at:
    public Color NewFlagColor;

    private void OnTriggerEnter(Collider other)
    {
        var otherObject = other.GetComponent<Ship>();
        if(other.gameObject.layer == 12 && otherObject)
        {
            if(IslandIsFree && IslandIsNeutral)
            {
                other.GetComponent<Ship>().IslandsDefended.Add(this);
                BaseGameLogic.BlueIslands += 1;
                NumberOfIslandDefs += 1;
                FlagGoUpNdown = true;
                NewFlagColor = BlueFlagColor;
                IslandIsBlue = true;
                IslandIsFree = false;
                IslandIsNeutral = false;

            }else if(IslandIsFree && !IslandIsBlue && !IslandIsNeutral)
            {
                other.GetComponent<Ship>().IslandsDefended.Add(this);
                BaseGameLogic.BlueIslands += 1;
                BaseGameLogic.RedIslands -= 1;
                NumberOfIslandDefs += 1;
                FlagGoUpNdown = true;
                NewFlagColor = BlueFlagColor;
                IslandIsBlue = true;
                IslandIsFree = false;
            }else if(IslandIsBlue)
            {
                other.GetComponent<Ship>().IslandsDefended.Add(this);
                NumberOfIslandDefs += 1;
                IslandIsFree = false;
            }else if(!IslandIsFree && !IslandIsBlue && !IslandIsNeutral)
            {
                other.GetComponent<Ship>().EnemyIslandsAssaulted.Add(this);
                EnemyShipsDocked.Add(other.GetComponent<Ship>());
                NumberOfIslandEnemies += 1;
            }
        }else if(other.gameObject.layer == 11 && otherObject)
        {
            if(IslandIsFree && IslandIsNeutral)
            {
                other.GetComponent<Ship>().IslandsDefended.Add(this);
                BaseGameLogic.RedIslands += 1;
                NumberOfIslandDefs += 1;
                FlagGoUpNdown = true;
                NewFlagColor = RedFlagColor;
                IslandIsBlue = false;
                IslandIsFree = false;
                IslandIsNeutral = false;

            }else if(IslandIsFree && IslandIsBlue)
            {
                other.GetComponent<Ship>().IslandsDefended.Add(this);
                BaseGameLogic.BlueIslands -= 1;
                BaseGameLogic.RedIslands += 1;
                NumberOfIslandDefs += 1;
                FlagGoUpNdown = true;
                NewFlagColor = RedFlagColor;
                IslandIsBlue = false;
                IslandIsFree = false;
            }else if(!IslandIsBlue && !IslandIsNeutral)
            {
                other.GetComponent<Ship>().IslandsDefended.Add(this);
                NumberOfIslandDefs += 1;
                IslandIsFree = false;
            }else if(!IslandIsFree && IslandIsBlue)
            {
                other.GetComponent<Ship>().EnemyIslandsAssaulted.Add(this);
                EnemyShipsDocked.Add(other.GetComponent<Ship>());
                NumberOfIslandEnemies += 1;
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        var otherObject = other.GetComponent<Ship>();
        if(((other.gameObject.layer == 12 && IslandIsBlue) || (other.gameObject.layer == 11 && !IslandIsBlue)) && otherObject)
        {
            NumberOfIslandDefs -= 1;
            other.GetComponent<Ship>().IslandsDefended.Remove(this);
            if(NumberOfIslandDefs == 0)
            {
                if(NumberOfIslandEnemies > 0)
                {
                    if(other.gameObject.layer == 12)
                    {
                        BaseGameLogic.BlueIslands -= 1;
                        BaseGameLogic.RedIslands += 1;
                        foreach(Ship ship in EnemyShipsDocked)
                        {
                            ship.IslandsDefended.Add(this);
                        }
                        NumberOfIslandDefs += EnemyShipsDocked.Count;
                        NumberOfIslandEnemies = 0;
                        EnemyShipsDocked = new List<Ship>();
                        FlagGoUpNdown = true;
                        NewFlagColor = RedFlagColor;
                        IslandIsBlue = false;
                        IslandIsFree = false;
                    }else
                    {
                        BaseGameLogic.BlueIslands += 1;
                        BaseGameLogic.RedIslands -= 1;
                        foreach(Ship ship in EnemyShipsDocked)
                        {
                            ship.IslandsDefended.Add(this);
                        }
                        NumberOfIslandDefs += EnemyShipsDocked.Count;
                        NumberOfIslandEnemies = 0;
                        EnemyShipsDocked = new List<Ship>();
                        FlagGoUpNdown = true;
                        NewFlagColor = BlueFlagColor;
                        IslandIsBlue = true;
                        IslandIsFree = false;
                    }
                }else
                {
                    IslandIsFree = true;
                }
            }
        }else if(((other.gameObject.layer == 11 && IslandIsBlue) || (other.gameObject.layer == 12 && !IslandIsBlue)) && otherObject)
        {
            NumberOfIslandEnemies -= 1;
            EnemyShipsDocked.Remove(other.GetComponent<Ship>());
            other.GetComponent<Ship>().EnemyIslandsAssaulted.Remove(this);
        }
    }

    void GetFlagToMove()
    {
        //Get the FlagTime:
        FlagTime += Time.deltaTime;
        float fallSpeed;
        fallSpeed = FlagCurve.Evaluate(FlagTime);
        if(FlagTime < 1)
        {
            //Move the Flag:
            FlagHolder.transform.position = Vector3.MoveTowards(FlagHolder.transform.position, LowestFlagPosition.transform.position, fallSpeed * Time.deltaTime);

        }else if(FlagTime >= 1 && FlagTime < 2)
        {
            //Change the flags' color:
            IslandFlag.GetComponent<Renderer>().material.color = NewFlagColor;
            SecondFlagPart.GetComponent<Renderer>().material.color = NewFlagColor;
            //Move the Flag:
            FlagHolder.transform.position = Vector3.MoveTowards(FlagHolder.transform.position, HighestFlagPosition.transform.position, fallSpeed * Time.deltaTime);

        }else if(FlagTime >= 2)
        {
            FlagTime = 0;
            FlagGoUpNdown = false;
            NewFlagColor = new Color(0,0,0);
        }

    }

    void Update()
    {
        if(FlagGoUpNdown)
        {
            GetFlagToMove();
        }
    }
}
