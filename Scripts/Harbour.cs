using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Harbour : MonoBehaviour
{

    //2 bools that tell us whether an enemy ship got to our harbour:
    public static bool BlueHarbourInvaded = false;
    public static bool RedHarbourInvaded = false;

    //Another bool that determines whether this Harbour is a red or blue start:
    public bool harbourIsBlue;

    //A bool that determines whether a ship got the health addition to its hp after sitting at the harbour for a turn:
    public bool ShipGotHealed = false;

    //A bool that tells if a ship entered to heal:
    public bool ShipEntered = false;

    //The sip that entered:
    public Ship EnteredShip;
    public static bool BlueShipGotHealedIndicator;
    public static bool RedShipGotHealedIndicator;
    public static bool ShipsGotHealedIndicator;
    public static BaseGameLogic BaseGameLogicScript;
    public static int HarboursUpdated;

    private void OnTriggerEnter(Collider other)
    {
        //Another nameCutout..
        string nameCutout = other.gameObject.name.Substring(0,3);

        if(harbourIsBlue && nameCutout == "red")
        {
            BlueHarbourInvaded = true;
            
        }else if(!harbourIsBlue && nameCutout == "blu")
        {
            RedHarbourInvaded = true;
        }

        //Heal any ships touching the harbour:
        if((nameCutout == "Blu" || nameCutout == "Red") && other.GetComponent<Ship>().lives < other.GetComponent<Ship>().maxLives && !Ship.ShipSinking)
        {
            EnteredShip = other.GetComponent<Ship>();
            ShipEntered = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        //Name cutouts never end
        string nameCutout = other.gameObject.name.Substring(0,3);

        if(nameCutout == "Blu" || nameCutout == "Red")
        {
            EnteredShip = null;
            if(harbourIsBlue)
            {
                BlueShipGotHealedIndicator = true;
            }else
            {
                RedShipGotHealedIndicator = true;
            }
            ShipEntered = false;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        string nameCutout = gameObject.name.Substring(0,4);
        if(nameCutout == "Blue")
        {
            harbourIsBlue = true;
        }else
        {
            harbourIsBlue = false;
        }
        BaseGameLogic.HarbourScript = this;
    }

    // Update is called once per frame
    void Update()
    {
        if(!ShipsGotHealedIndicator)
        {
            RedShipGotHealedIndicator = false;
            BlueShipGotHealedIndicator = false;
            ShipsGotHealedIndicator = true;
        }

        if(EnteredShip != null && ShipEntered && !ShipGotHealed && !Ship.ShipSinking)
        {
            if((EnteredShip.maxLives - EnteredShip.lives) <= 25)
            {
                EnteredShip.lives = EnteredShip.maxLives;
                EnteredShip = null;
                ShipGotHealed = true;
                ShipEntered = false;

            }else if((EnteredShip.maxLives - EnteredShip.lives) > 25)
            {
                EnteredShip.lives += 25;
                ShipGotHealed = true;
            }
        }

        if((BlueHarbourInvaded || RedHarbourInvaded) || (BaseGameLogicScript.BlueFleetIsDead || BaseGameLogicScript.RedFleetIsDead) || Ship.SurrenderButtonClicked)
        {
            EnteredShip = null;
            ShipEntered = false;
            ShipGotHealed = false;
            Ship.SurrenderButtonClicked = false;
        }

        if(!BlueShipGotHealedIndicator && harbourIsBlue)
        {
            ShipGotHealed = false;
            HarboursUpdated += 1;

        }else if(!RedShipGotHealedIndicator && !harbourIsBlue)
        {
            ShipGotHealed = false;
            HarboursUpdated += 1;
        }

        if(HarboursUpdated >= 20)
        {
            BlueShipGotHealedIndicator = true;
            RedShipGotHealedIndicator = true;
            HarboursUpdated = 0;
        }
    }
}
