using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cannonball : MonoBehaviour
{

    public static BaseGameLogic BaseGameLogicScript;
    public Vector3 TargetPosition;
    public Vector3 SpawnPosition;
    public Transform trans;
    public float distanceTravelled = 0;
    public float damageToDeal = 0;
    public int timesDamageDivided = 0;
    public string directionHeading;
    public bool otherShipHit = false;
    
    // Start is called before the first frame update
    void Start()
    {
        SpawnPosition = trans.position;
    
        if(!BaseGameLogic.SinglePlayerGame || BaseGameLogic.SinglePlayerGame && !BaseGameLogic.blueTurn)
        {
            if(BaseGameLogicScript.hilighter.gameObject.transform.position.z > BaseGameLogicScript.SelectedShip.gameObject.transform.position.z)
            {
                TargetPosition = new Vector3(BaseGameLogicScript.SelectedShip.gameObject.transform.position.x,0,BaseGameLogicScript.SelectedShip.gameObject.transform.position.z + BaseGameLogicScript.SelectedShip.MaxAttackRange);
                directionHeading = "z";

            }else if(BaseGameLogicScript.hilighter.gameObject.transform.position.z < BaseGameLogicScript.SelectedShip.gameObject.transform.position.z)
            {
                TargetPosition = new Vector3(BaseGameLogicScript.SelectedShip.gameObject.transform.position.x,0,BaseGameLogicScript.SelectedShip.gameObject.transform.position.z - BaseGameLogicScript.SelectedShip.MaxAttackRange);
                directionHeading = "-z";

            }else if(BaseGameLogicScript.hilighter.gameObject.transform.position.x > BaseGameLogicScript.SelectedShip.gameObject.transform.position.x)
            {
                TargetPosition = new Vector3(BaseGameLogicScript.SelectedShip.gameObject.transform.position.x + BaseGameLogicScript.SelectedShip.MaxAttackRange,0,BaseGameLogicScript.SelectedShip.gameObject.transform.position.z);
                directionHeading = "x";

            }else if(BaseGameLogicScript.hilighter.gameObject.transform.position.x < BaseGameLogicScript.SelectedShip.gameObject.transform.position.x)
            {
                TargetPosition = new Vector3(BaseGameLogicScript.SelectedShip.gameObject.transform.position.x - BaseGameLogicScript.SelectedShip.MaxAttackRange,0,BaseGameLogicScript.SelectedShip.gameObject.transform.position.z);
                directionHeading = "-x";
            }
        }else if(BaseGameLogic.SinglePlayerGame && BaseGameLogic.blueTurn)
        {
            if(BaseGameLogic.directionTargetedShipIsAt == "z")
            {
                TargetPosition = new Vector3(BaseGameLogicScript.ShipAIattackComesFrom.gameObject.transform.position.x,0,BaseGameLogicScript.ShipAIattackComesFrom.gameObject.transform.position.z + BaseGameLogicScript.ShipAIattackComesFrom.MaxAttackRange);
                directionHeading = "z";

            }else if(BaseGameLogic.directionTargetedShipIsAt == "-z")
            {
                TargetPosition = new Vector3(BaseGameLogicScript.ShipAIattackComesFrom.gameObject.transform.position.x,0,BaseGameLogicScript.ShipAIattackComesFrom.gameObject.transform.position.z - BaseGameLogicScript.ShipAIattackComesFrom.MaxAttackRange);
                directionHeading = "-z";
                
            }else if(BaseGameLogic.directionTargetedShipIsAt == "x")
            {
                TargetPosition = new Vector3(BaseGameLogicScript.ShipAIattackComesFrom.gameObject.transform.position.x + BaseGameLogicScript.ShipAIattackComesFrom.MaxAttackRange,0,BaseGameLogicScript.ShipAIattackComesFrom.gameObject.transform.position.z);
                directionHeading = "x";

            }else
            {
                TargetPosition = new Vector3(BaseGameLogicScript.ShipAIattackComesFrom.gameObject.transform.position.x - BaseGameLogicScript.ShipAIattackComesFrom.MaxAttackRange,0,BaseGameLogicScript.ShipAIattackComesFrom.gameObject.transform.position.z);
                directionHeading = "-x";
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(damageToDeal == 0)
        {
            if(BaseGameLogic.SinglePlayerGame && BaseGameLogic.blueTurn)
            {
                damageToDeal = Random.Range(BaseGameLogicScript.ShipAIattackComesFrom.damage - 50,BaseGameLogicScript.ShipAIattackComesFrom.damage + 51);
            }else
            {
                damageToDeal = Random.Range(BaseGameLogicScript.SelectedShip.damage - 50,BaseGameLogicScript.SelectedShip.damage + 51);
            }
            //Round it so that it doesn't return a weird float.
            damageToDeal = Mathf.Round(damageToDeal);
        }

        //if we haven't reached our target yet:
        if(trans.position != TargetPosition)
        {
            //..go there:
            trans.position = Vector3.MoveTowards(trans.position,TargetPosition, 1.6f * Time.deltaTime);
        }

        //Track the distance travelled till now:
        distanceTravelled = Vector3.Distance(SpawnPosition,trans.position);

        if(distanceTravelled >= 1 && timesDamageDivided == 0)
        {
            damageToDeal /= 2;
            timesDamageDivided = 1;

        }else if(distanceTravelled >= 2 && timesDamageDivided == 1)
        {
            damageToDeal /= 2;
            timesDamageDivided = 2;
        }else if(distanceTravelled >= 3 && timesDamageDivided == 2)
        {
            damageToDeal /= 2;
            timesDamageDivided = 3;
        }

        //if distanceTravelled is >= 3, destroy this gameObject:
        if((directionHeading == "z" && trans.position.z >= TargetPosition.z) || (directionHeading == "-z" && trans.position.z <= TargetPosition.z) || (directionHeading == "x" && trans.position.x >= TargetPosition.x) || (directionHeading == "-x" && trans.position.x <= TargetPosition.x))
        {
            if(!Ship.ShipSinking)
            {
                Ship.attackFinished = true;
                BaseGameLogic.AIstartedAttacking = false;
            }
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(BaseGameLogic.blueTurn)
        {
            if(other.gameObject.layer == 11)
            {
                var otherShip = other.GetComponentInParent<Ship>();

                if(other.gameObject != BaseGameLogicScript.SelectedShip && !BaseGameLogic.SinglePlayerGame)
                {
                    BaseGameLogicScript.AttackedShip = otherShip;
                
                    if(!otherShipHit)
                    {
                        //subtract the otherShip's lives:
                        BaseGameLogicScript.CameraAudioSource.PlayOneShot(BaseGameLogicScript.ShipHit, BaseGameLogicScript.ShipHitVol);
                        otherShip.lives -= damageToDeal;
                        otherShipHit = true;
                    }

                    //if the other ship's hp is 0:
                    if(otherShip.lives <= 0)
                    {
                        Ship.ShipSinking = true;
                        Destroy(gameObject);
                    }else
                    {
                        //Destroy this cannonball, and set attackFinished to true:
                        Ship.attackFinished = true;
                        BaseGameLogic.AIstartedAttacking = false;
                        Destroy(gameObject);
                    }

                }else if(other.gameObject != BaseGameLogicScript.ShipAIattackComesFrom && BaseGameLogic.SinglePlayerGame)
                {
                    BaseGameLogicScript.AttackedShip = otherShip;
                
                    if(!otherShipHit)
                    {
                        //subtract the otherShip's lives:
                        BaseGameLogicScript.CameraAudioSource.PlayOneShot(BaseGameLogicScript.ShipHit, BaseGameLogicScript.ShipHitVol);
                        otherShip.lives -= damageToDeal;
                        otherShipHit = true;
                    }

                    //if the other ship's hp is 0:
                    if(otherShip.lives <= 0)
                    {
                        Ship.ShipSinking = true;
                        Destroy(gameObject);
                    }else
                    {
                        //Destroy this cannonball, and set attackFinished to true:
                        Ship.attackFinished = true;
                        BaseGameLogic.AIstartedAttacking = false;
                        Destroy(gameObject);
                    }
                }
            }else if(other.gameObject.layer == 12)
            {
                var otherShip = other.GetComponentInParent<Ship>();

                if(otherShip != BaseGameLogicScript.SelectedShip && !BaseGameLogic.SinglePlayerGame)
                {
                    //Destroy this cannonball, and set attackFinished to true:
                    BaseGameLogicScript.CameraAudioSource.PlayOneShot(BaseGameLogicScript.ShipHit, BaseGameLogicScript.ShipHitVol);
                    Ship.attackFinished = true;
                    BaseGameLogic.AIstartedAttacking = false;
                    Destroy(gameObject);
                }
            }
        }else
        {
            if(other.gameObject.layer == 12)
            {
                var otherShip = other.GetComponentInParent<Ship>();

                if(otherShip != BaseGameLogicScript.SelectedShip)
                {
                    BaseGameLogicScript.AttackedShip = otherShip;
                
                    if(!otherShipHit)
                    {
                        //subtract the otherShip's lives:
                        BaseGameLogicScript.CameraAudioSource.PlayOneShot(BaseGameLogicScript.ShipHit, BaseGameLogicScript.ShipHitVol);
                        otherShip.lives -= damageToDeal;
                        otherShipHit = true;
                    }

                    //if the other ship's hp is 0:
                    if(otherShip.lives <= 0)
                    {
                        Ship.ShipSinking = true;
                        Destroy(gameObject);
                    }else
                    {
                        //Destroy this cannonball, and set attackFinished to true:
                        Ship.attackFinished = true;
                        Destroy(gameObject);
                    }
                }
            }else if(other.gameObject.layer == 11)
            {
                var otherShip = other.GetComponentInParent<Ship>();

                if(otherShip != BaseGameLogicScript.SelectedShip)
                {
                    //Destroy this cannonball, and set attackFinished to true:
                    BaseGameLogicScript.CameraAudioSource.PlayOneShot(BaseGameLogicScript.ShipHit, BaseGameLogicScript.ShipHitVol);
                    Ship.attackFinished = true;
                    BaseGameLogic.AIstartedAttacking = false;
                    Destroy(gameObject);
                }
            }
        }
        if(other.gameObject.layer == 7)
        {
            //Destroy this cannonball, and set attackFinished to true:
            Ship.attackFinished = true;
            BaseGameLogic.AIstartedAttacking = false;
            Destroy(gameObject);

        }else if(other.gameObject.layer == 15)
        {
            //Destroy this cannonball, and set attackFinished to true:
            Ship.attackFinished = true;
            BaseGameLogic.AIstartedAttacking = false;
            Destroy(gameObject);
        }
    }
}
