using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PathGenerator : MonoBehaviour
{
    public Transform plane;
    public Transform tilePrefab;
    public Transform prefabStraight;
    public Transform prefabUpLeft; //0>upleft 90>rightup 180>leftup 270>upright
    public Transform arrowTowerPrefab;
    public Transform bombTowerPrefab;
    public Transform groundBarricadePrefab;
    public Transform blockBarricadePrefab;
    public Transform kingPrefab;
    public Transform spawnPoint;
    public Transform gate;

    int mapSize;
    int turn = 6;
    int arrowTowerCount;
    int bombTowerCount;
    int groundBarricadeCount;
    int blockBarricadeCount;
    int percentageDone = 0;
    int percentIncrement;

    [HideInInspector]
    public Vector3 spawnPointPosition;
    Vector3 gatePosition;

    public delegate void PercentChange(int change);
    public event PercentChange OnPercentChange;

    public int PercentDone
    {
        get
        {
            return percentageDone;
        }
        set
        {
            if (value >= percentageDone)
            {
                OnPercentChange(value);
                //Debug.Log("The value of percentDone : " + value);
                percentageDone = Mathf.Clamp(value, 0, 100);
            }
        }
    }

    int length;
    [Range(0, 1)]
    int side;
    Coord currentCoord;
    Coord startCoord;
    [HideInInspector]
    public Coord endCoord;
    Coord allCoord;
    //public int seed = 10
    List<Coord> path;
    int[,] isAvailable;//0>Nothing 1>Straight 2> side 3>upleft 4>upright 5>leftup 6>rightup 7>tower 8>Barricade 9>Don't use

    public void SetValuesAndGenerate(ScriptableRankProperties props)
    {
        mapSize = props.mapSize;
        turn = props.turns;
        arrowTowerCount = props.numOfArrowTowers;
        bombTowerCount = props.numOfBombTowers;
        groundBarricadeCount = props.numOfGroundBarricade;
        blockBarricadeCount = props.numOfBlockBarricade;
        percentIncrement = 80 / (turn/2);
        Generate();
    }

    void Generate()
    {
        Instantiate(plane, Vector3.zero, Quaternion.identity);
        plane.localScale = new Vector3(mapSize / 10.0f, 1, mapSize / 10.0f);
        length = (mapSize / 3) - 1;
        currentCoord = new Coord();
        path = new List<Coord>();
        isAvailable = new int[length + 1, length + 1];
        for (int i = 0; i <= length; i++)
        {
            for (int j = 0; j <= length; j++)
            {
                isAvailable[i, j] = 0;
            }
        }
        //InstantiateTile(PrefabStraight, 0, 0);
        //InstantiateTile(9, 0);
        //InstantiateTile(0, 9);
        //InstantiateTile(9, 9);
        GeneratePath();
        if (arrowTowerCount + bombTowerCount < (int)(path.Count * 1.5))
        {
            GenerateDefense(arrowTowerCount, arrowTowerPrefab);
            GenerateDefense(bombTowerCount, bombTowerPrefab);
            PercentDone = percentageDone + 10;
        }
        else
        {
            Debug.LogError("Too Many Towers For This Path length is " + length);
        }
        if ((groundBarricadeCount + blockBarricadeCount) < (path.Count - 4))
        {
            GenerateBarricade(blockBarricadeCount, blockBarricadePrefab);
            GenerateBarricade(groundBarricadeCount, groundBarricadePrefab);
            PercentDone = percentageDone + 10;
        }
        else
        {
            Debug.LogError("Too Many barricades for this path lenght is" + length);
        }

        for (int i = 0; i <= length; i++)
        {
            for (int j = 0; j <= length; j++)
            {
                //Debug.Log("[x] = " + i + " [y] = " + j + " value = " + isAvailable[i, j]);
            }
        }
    }

    public void GeneratePath()
    {
        System.Random rand = new System.Random();
        currentCoord.x = rand.Next(1, length - 1);
        currentCoord.y = 0;
        isAvailable[currentCoord.x, currentCoord.y] = 9;
        InstantiateTile(prefabStraight, currentCoord.x, 0, currentCoord.y);
        path.Add(new Coord(currentCoord));
        startCoord = new Coord(currentCoord);
        for (int i = turn; i > 0; i = i - 2)
        {
            move(i);
            //add the percentIncrement here
            PercentDone = percentIncrement + percentageDone;
        }
        
        for (int i = currentCoord.y + 1; i <= length - 1; i++)
        {
            isAvailable[currentCoord.x, i] = 1;
            InstantiateTile(prefabStraight, currentCoord.x, 0, i);
            path.Add(new Coord(currentCoord.x, i));
        }
        currentCoord.y = length;
        isAvailable[currentCoord.x, currentCoord.y] = 9;
        InstantiateTile(prefabStraight, currentCoord.x, 0, currentCoord.y);
        path.Add(new Coord(currentCoord));
        endCoord = new Coord(currentCoord);
        //isAvailable[startCoord.x, startCoord.y] = true;
        //InstantiateTile(SpawnPoint, startCoord.x, 0, startCoord.y);
        spawnPointPosition = new Vector3((float)((-mapSize / 2) + 1.5 + (3 * startCoord.x)), (float)0.4, (float)((-mapSize / 2) + 1.5 + (3 * startCoord.y)));
        Transform spawnEntity = Instantiate(spawnPoint, spawnPointPosition, Quaternion.identity) as Transform;
        spawnEntity.gameObject.name = "SpawnPoint";
        spawnEntity.parent = this.transform;
        //InstantiateTile(KingPrefab, startCoord.x, 0, startCoord.y);
        //isAvailable[endCoord.x, endCoord.y] = true;
        //InstantiateTile(Gate, endCoord.x, 0, endCoord.y);
        gatePosition = new Vector3((float)((-mapSize / 2) + 1.5 + (3 * endCoord.x)), (float)1, (float)((-mapSize / 2) + 1.5 + (3 * endCoord.y)));
        Quaternion rotation = Quaternion.Euler(0, 0, 0);
        Transform gateEntity = Instantiate(gate, gatePosition, rotation) as Transform;
        gateEntity.gameObject.name = "Gate";
        gateEntity.parent = this.transform;
        //traversing the path
        //foreach(Coord co in path)
        //{
        //    Debug.Log("x = " + co.x + " y = " + co.y + co.availSides[0] + co.availSides[1] + co.availSides[2] + co.availSides[3]);
        //}
        //Debug.Log(path.Count);
        if (percentageDone != 80)
        {
            PercentDone = 80;
        }
    }

    void move(int turnsLeft)
    {
        System.Random rand = new System.Random();
        int p = rand.Next(currentCoord.y + 2, length - turnsLeft + 1);
        for (int i = currentCoord.y + 1; i <= (p - 1); i++)
        {
            isAvailable[currentCoord.x, i] = 1;
            InstantiateTile(prefabStraight, currentCoord.x, 0, i);
            path.Add(new Coord(currentCoord.x, i));
        }
        currentCoord.y = p;

        if (currentCoord.x <= 2)
        {
            side = 1;//right
        }
        else if (currentCoord.x >= length - 2)
        {
            side = 0;//left
        }
        else
        {
            side = rand.Next(0, 2);
        }
        if (side == 0)// left
        {
            isAvailable[currentCoord.x, currentCoord.y] = 3;
            InstantiateTile(prefabUpLeft, currentCoord.x, 0, currentCoord.y);
            path.Add(new Coord(currentCoord.x, currentCoord.y));
            rand = new System.Random();
            p = rand.Next(1, currentCoord.x - 1);
            for (int i = currentCoord.x - 1; i >= (p + 1); i--)
            {
                isAvailable[i, currentCoord.y] = 2;
                InstantiateTile(prefabStraight, i, 0, currentCoord.y, 90);
                path.Add(new Coord(i, currentCoord.y));
            }
            currentCoord.x = p;
            isAvailable[currentCoord.x, currentCoord.y] = 5;
            InstantiateTile(prefabUpLeft, currentCoord.x, 0, currentCoord.y, 180);
            path.Add(new Coord(currentCoord.x, currentCoord.y));
        }
        else//right
        {
            isAvailable[currentCoord.x, currentCoord.y] = 4;
            InstantiateTile(prefabUpLeft, currentCoord.x, 0, currentCoord.y, 270);
            path.Add(new Coord(currentCoord.x, currentCoord.y));
            rand = new System.Random();
            p = rand.Next(currentCoord.x + 2, length - 1);
            for (int i = currentCoord.x + 1; i <= (p - 1); i++)
            {
                isAvailable[i, currentCoord.y] = 2;
                InstantiateTile(prefabStraight, i, 0, currentCoord.y, 90);
                path.Add(new Coord(i, currentCoord.y));
            }
            currentCoord.x = p;
            isAvailable[currentCoord.x, currentCoord.y] = 6;
            InstantiateTile(prefabUpLeft, currentCoord.x, 0, currentCoord.y, 90);
            path.Add(new Coord(currentCoord.x, currentCoord.y));
        }
    }

    void GenerateDefense(int count, Transform prefab)
    {
        for (int i = 0; i < count;)
        {
            bool found = false;
            System.Random rand = new System.Random();
            int p = rand.Next(3, path.Count - 1);
            int[] side = new int[] { 0, 1, 2, 3 };
            side = Utility.ShuffleArray<int>(side);
            //Debug.Log("i = " + i + " Random p is " + p + " sides " + side[0] + side[1] + side[2] + side[3]);
            foreach (int s in side)
            {
                if (!found)
                {
                    //Debug.Log("Random p is " + p + " tower no = " + i + " Coord = " + path[p].x + " and " + path[p].y + " Side = " + s + " is " );
                    if (s == 0 && isAvailable[path[p].x - 1, path[p].y] == 0)
                    {
                        isAvailable[path[p].x - 1, path[p].y] = 7;
                        InstantiateTile(prefab, (float)(path[p].x - 0.5), 0, path[p].y);
                        found = true;
                        i++;
                    }
                    else if (s == 1 && isAvailable[path[p].x + 1, path[p].y] == 0)
                    {
                        isAvailable[path[p].x + 1, path[p].y] = 7;
                        InstantiateTile(prefab, (float)(path[p].x + 0.5), 0, path[p].y);
                        found = true;
                        i++;
                    }
                    else if (s == 2 && isAvailable[path[p].x, path[p].y + 1] == 0)
                    {
                        isAvailable[path[p].x, path[p].y + 1] = 7;
                        InstantiateTile(prefab, path[p].x, 0, (float)(path[p].y + 0.5));
                        found = true;
                        i++;
                    }
                    else if (s == 3 && isAvailable[path[p].x, path[p].y - 1] == 0)
                    {
                        isAvailable[path[p].x, path[p].y - 1] = 7;
                        InstantiateTile(prefab, path[p].x, 0, (float)(path[p].y - 0.5));
                        found = true;
                        i++;
                    }
                }
            }
        }
    }

    void GenerateBarricade(int count, Transform prefab)
    {
        for (int i = 0; i < count;)
        {
            System.Random rand = new System.Random();
            int p = rand.Next(2, path.Count - 1);
            //Debug.Log("i = " + i + " Random p is " + p + " sides " + side[0] + side[1] + side[2] + side[3]);
            if (isAvailable[path[p].x, path[p].y] == 1)
            {
                isAvailable[path[p].x, path[p].y] = 8;
                InstantiateTile(prefab, path[p].x, 0, (float)(path[p].y + 0.5));
                //Debug.Log("[x] = " + path[p].x + " [y] = " + path[p].y + " value = " + isAvailable[path[p].x, path[p].y]);
                i++;
            }
            else if (isAvailable[path[p].x, path[p].y] == 2)
            {
                isAvailable[path[p].x, path[p].y] = 8;
                InstantiateTile(prefab, (float)(path[p].x - 0.5), 0, path[p].y, 90);
                //Debug.Log("[x] = " + path[p].x + " [y] = " + path[p].y + " value = " + isAvailable[path[p].x, path[p].y]);
                i++;
            }
            else if (isAvailable[path[p].x, path[p].y] == 3)
            {
                isAvailable[path[p].x, path[p].y] = 8;
                InstantiateTile(prefab, (float)(path[p].x - 0.5), 0, path[p].y, 90);
                //Debug.Log("[x] = " + path[p].x + " [y] = " + path[p].y + " value = " + isAvailable[path[p].x, path[p].y]);
                i++;
            }
            else if (isAvailable[path[p].x, path[p].y] == 4)
            {
                isAvailable[path[p].x, path[p].y] = 8;
                InstantiateTile(prefab, (float)(path[p].x + 0.5), 0, path[p].y, 90);
                //Debug.Log("[x] = " + path[p].x + " [y] = " + path[p].y + " value = " + isAvailable[path[p].x, path[p].y]);
                i++;
            }
            else if (isAvailable[path[p].x, path[p].y] == 5)
            {
                isAvailable[path[p].x, path[p].y] = 8;
                InstantiateTile(prefab, path[p].x, 0, (float)(path[p].y + 0.5));
                //Debug.Log("[x] = " + path[p].x + " [y] = " + path[p].y + " value = " + isAvailable[path[p].x, path[p].y]);
                i++;
            }
            else if (isAvailable[path[p].x, path[p].y] == 6)
            {
                isAvailable[path[p].x, path[p].y] = 8;
                InstantiateTile(prefab, (float)(path[p].x - 0.5), 0, path[p].y, 90);
                //Debug.Log("[x] = " + path[p].x + " [y] = " + path[p].y + " value = " + isAvailable[path[p].x, path[p].y]);
                i++;
            }
        }
    }

    void InstantiateTile(Transform prefab, float x, float y, float z)
    {
        //Debug.Log("x = " + x + " y = " + y + " prefab is " + prefab.name);
        float xx = (float)((-mapSize / 2) + 1.5 + (3 * x));
        float zz = (float)((-mapSize / 2) + 1.5 + (3 * z));
        Transform newPath = Instantiate(prefab, new Vector3(xx, y, zz), Quaternion.identity) as Transform;
        newPath.parent = this.transform;
        //Debug.Log("[x] = "+ x + " [y] = " + z + " value = " + isAvailable[(int)x, (int)z]);
        //Debug.LogError("");
        //Instantiate(tilePrefab, new Vector3(xx, 0, yy), Quaternion.Euler(Vector3.right * 90));
    }

    void InstantiateTile(Transform prefab, float x, float y, float z, float angleY)
    {
        //Debug.Log("x = " + x + " y = " + y + " prefab is " + prefab.name);
        float xx = (float)((-mapSize / 2) + 1.5 + (3 * x));
        float zz = (float)((-mapSize / 2) + 1.5 + (3 * z));
        Transform newPath = Instantiate(prefab, new Vector3(xx, y, zz), Quaternion.identity) as Transform;
        newPath.eulerAngles = new Vector3(newPath.rotation.x, angleY, newPath.rotation.z);
        newPath.parent = this.transform;
        //Debug.Log("[x] = " + x + " [y] = " + z + " value = " + isAvailable[(int)x, (int)z]);
        //Debug.LogError("");
        //Instantiate(tilePrefab, new Vector3(xx, 0, yy), Quaternion.Euler(Vector3.right * 90));
    }

}


public class Coord
{
    public int x;
    public int y;


    public Coord() { }

    public Coord(Coord co)
    {
        x = co.x;
        y = co.y;
    }

    public Coord(int _x, int _y)
    {
        x = _x;
        y = _y;
    }

}
