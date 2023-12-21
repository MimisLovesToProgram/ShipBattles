using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hilighter : MonoBehaviour
{
    public bool hilighterIsAtMovementTile = false;
    public bool hilighterIsAtAttackTile = false;
    public LayerMask MovementTileLayerMask;
    public LayerMask AttackTileLayerMask;

    // Start is called before the first frame update
    void Start()
    {
        Ship.HilighterScript = this;
    }

    // Update is called once per frame
    void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if(Physics.Raycast(ray,Mathf.Infinity,MovementTileLayerMask.value))
        {
            hilighterIsAtMovementTile = true;
        }else
        {
            hilighterIsAtMovementTile = false;
        }

        if(Physics.Raycast(ray,Mathf.Infinity,AttackTileLayerMask.value))
        {
            hilighterIsAtAttackTile = true;
        }else
        {
            hilighterIsAtAttackTile = false;
        }
    }
}
