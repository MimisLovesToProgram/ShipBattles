using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IslandGeneration : MonoBehaviour
{

    [Tooltip("Reference to the island prefab.")]
    public GameObject IslandPrefab;

    [Tooltip("Reference to the sea prefab.")]
    public GameObject SeaPrefab;

    [Tooltip("The number of objects to generate per line.")]
    int objectsPerLine = 10;

    [Tooltip("The lines left to generate objects in.")]
    public int linesLeftToGenerate = 6;

    [Tooltip("The islands to generate in each line.")]
    public int islandsPerLine;

    [Tooltip("The place to generate the next island/sea.")]
    public Vector3 currentPointToGenerate = new Vector3(19.5f,0,7.5f);

    [Tooltip("The position at the start of the first line. this will be used to go to the start of the next line.")]
    public Vector3 LineStartPoint = new Vector3(19.5f,0,7.5f);

    [Tooltip("The only island in the line, if islandPerLine is 1")]
    public int island;

    [Tooltip("The first of the two islands in the line, if islandPerLine is 2")]
    public int island1;

    [Tooltip("The second island on the line, if islandPerLine is 2.")]
    public int island2;

    //..And an array to store the island positions:
    public List<Vector3> IslandPositions = new List<Vector3>();

    public void RandomIslandGeneration()
    {
        //Call GenerateObject until all the objects are generated
        if(linesLeftToGenerate != 0)
        {
            InvokeRepeating("GenerateObject",0,.05f);
        }else
        {
            CancelInvoke();
            BaseGameLogic.CanGenerateIslands = false;
            this.enabled = false;
        }
    }

    private void GenerateObject()
    {
        if(linesLeftToGenerate > 0)
        {
            //The Generation Loop:
            for(int i = 1; i <= objectsPerLine - Ship.BaseGameLogicScript.NumOfHarboursToBeDisabled; i += 1)
            {
                //Determines the number of islands per line: 
                if(islandsPerLine == 0)
                {
                    islandsPerLine = Random.Range(1,3);
                }
                //if this is the first time, run the code:
                if(i == 1)
                {
                    //Determines where will the islands generate in the line:
                    for(int a = 0; a == 0; a += 0)
                    {
                        if(islandsPerLine == 1)
                        {
                            island = Random.Range(1,11 - Ship.BaseGameLogicScript.NumOfHarboursToBeDisabled);
                            a = 1;
                        }else
                        {
                            island1 = Random.Range(1,11 - Ship.BaseGameLogicScript.NumOfHarboursToBeDisabled);
                            island2 = Random.Range(1,11 - Ship.BaseGameLogicScript.NumOfHarboursToBeDisabled);
                            if(island1 != island2)
                            {
                                a = 1;
                            }
                        }
                    }  
                }
            
                //Start Generating:
                if(islandsPerLine == 1)
                {
                    if(island == i)
                    {
                        BaseGameLogic.IslandAndSeaPositions[currentPointToGenerate] = Instantiate(IslandPrefab,currentPointToGenerate,Quaternion.identity);
                        IslandPositions.Add(currentPointToGenerate);
                    }else
                    {
                        BaseGameLogic.IslandAndSeaPositions[currentPointToGenerate + new Vector3(0,.1f,0)] = Instantiate(SeaPrefab,currentPointToGenerate,Quaternion.identity);
                    }
                }else
                {
                    if(island1 == i || island2 == i)
                    {
                        BaseGameLogic.IslandAndSeaPositions[currentPointToGenerate] = Instantiate(IslandPrefab,currentPointToGenerate,Quaternion.identity);
                        IslandPositions.Add(currentPointToGenerate);
                    }else
                    {
                        BaseGameLogic.IslandAndSeaPositions[currentPointToGenerate + new Vector3(0,.1f,0)] = Instantiate(SeaPrefab,currentPointToGenerate,Quaternion.identity);
                    }
                }
                currentPointToGenerate.x -= 1;
            }

            //Resetting the values.
            linesLeftToGenerate -= 1;
            LineStartPoint.z -= 1;
            currentPointToGenerate = LineStartPoint;

            islandsPerLine = 0;

            if(island != 0)
            {
                island = 0;
            }
            if(island1 != 0)
            {
                island1 = 0;
            }
            if(island2 != 0)
            {
                island2 = 0;
            }
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        Ship.IslandGenerationScript = this;
    }

    // Update is called once per frame
    void Update()
    {
        if(BaseGameLogic.CanGenerateIslands)
        {
            RandomIslandGeneration();
        }
    }
}
