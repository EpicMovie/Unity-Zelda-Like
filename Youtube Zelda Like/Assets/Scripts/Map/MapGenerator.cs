using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    public GameObject tilePrefab;
    public GameObject obstaclePrefab;

    public Vector2 mapSize;

    [Range(0, 1)]
    public float outlinePercent;

    [Range(0, 1)]
    public float obstaclePercent;

    public int seed = 10;

    List<Coord> allTileCoords;
    Queue<Coord> shuffledTileCoords;
    Coord mapCenter;

    void Start()
    {
        GenerateMap();
    }

    public void GenerateMap()
    {
        allTileCoords = new List<Coord>();

        for (int x = 0; x < mapSize.x; x++)
        {
            for (int y = 0; y < mapSize.y; y++)
            {
                allTileCoords.Add(new Coord(x, y));
            }
        }

        shuffledTileCoords = new Queue<Coord>(UtilLibrary.Shuffle(allTileCoords.ToArray(), seed));

        mapCenter = new Coord((int)(mapSize.x / 2), (int)(mapSize.y / 2));

        string holderName = "Generated Map";

        // Remove Prev Map
        if (transform.Find(holderName))
        {
            DestroyImmediate(transform.Find(holderName).gameObject);
        }

        Transform mapHolder = new GameObject(holderName).transform;
        mapHolder.parent = transform;

        // Create new Tiles 
        for (int x = 0; x < mapSize.x; x++)
        {
            for (int y = 0; y < mapSize.y; y++)
            {
                Vector3 tilePosition = CoordToPosition(x, y);
                GameObject newTile = Instantiate(tilePrefab, tilePosition, Quaternion.Euler(Vector3.right * 90)) as GameObject;

                newTile.transform.localScale = Vector3.one * (1 - outlinePercent);
                newTile.transform.parent = mapHolder;
            }
        }

        // Create new Obstacles
        {
            bool[,] obstacleMap = new bool[(int)mapSize.x, (int)mapSize.y];

            int obstacleCount = (int)(mapSize.x * mapSize.y * obstaclePercent);
            int curObstacleCount = 0;

            for (int i = 0; i < obstacleCount; i++)
            {
                Coord randCoord = GetRandCoord();

                if (randCoord != mapCenter && IsMapFullyAccessible(in obstacleMap, curObstacleCount))
                {
                    curObstacleCount++;

                    obstacleMap[randCoord.x, randCoord.y] = true;

                    Vector3 obstaclePosition = CoordToPosition(randCoord.x, randCoord.y);

                    GameObject newObstacle = Instantiate(obstaclePrefab, obstaclePosition + Vector3.up * 0.5f, Quaternion.identity) as GameObject;

                    newObstacle.transform.localScale = Vector3.one * (1 - outlinePercent);
                    newObstacle.transform.parent = mapHolder;
                }
            }
        }
    }

    bool IsMapFullyAccessible(in bool[,] obstacleMap, int curObstacleCount)
    {
        int width = obstacleMap.GetLength(0);
        int height = obstacleMap.GetLength(1);

        bool[,] mapFlags = new bool[width, height];

        Queue<Coord> queue = new Queue<Coord>();
        queue.Enqueue(mapCenter);

        mapFlags[mapCenter.x, mapCenter.y] = true;

        int accessibleTileCount = 1;

        while (queue.Count > 0)
        {
            Coord tile = queue.Dequeue();

            for (int x = -1; x <= 1; x++)
            {
                for (int y = -1; y <= 1; y++)
                {
                    int neighbourX = tile.x + x;
                    int neighbourY = tile.y + y;

                    // Only Judged by cross direction 
                    if (x != 0 && y != 0)
                    {
                        continue;
                    }
                
                    if (neighbourX >= 0 && neighbourX < width && neighbourY >= 0 && neighbourY < height)
                    {
                        if (mapFlags[neighbourX, neighbourY] == false && obstacleMap[neighbourX, neighbourY] == false)
                        {
                            mapFlags[neighbourX, neighbourY] = true;
                            queue.Enqueue(new Coord(neighbourX, neighbourY));
                            accessibleTileCount++;
                        }
                    }
                }
            }
        }

        int targetAccessibleTileCount = (int)(mapSize.x * mapSize.y - curObstacleCount);
        return targetAccessibleTileCount == accessibleTileCount;
    }

    Vector3 CoordToPosition(int x, int y)
    {
        return new Vector3(-mapSize.x / 2 + 0.5f + x, 0, -mapSize.y + 0.5f + y);
    }

    public Coord GetRandCoord()
    {
        Coord randCoord = shuffledTileCoords.Dequeue();
        shuffledTileCoords.Enqueue(randCoord);

        return randCoord;
    }

    public struct Coord
    {
        public int x;
        public int y;

        public Coord(int _x, int _y)
        {
            x = _x;
            y = _y;
        }

        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }

        public bool Equals(Coord coord) => x == coord.x && y == coord.y;

        public override int GetHashCode() => (x, y).GetHashCode();

        public static bool operator ==(Coord lhs, Coord rhs) => lhs.Equals(rhs);

        public static bool operator !=(Coord lhs, Coord rhs) => lhs.Equals(rhs) == false;
    }

    [System.Serializable]
    public class Map
    {
        public Coord mapSize;

        [Range(0, 1)]
        public float obstaclePercent;

        public int seed;

        public Color foregroundColour;
        public Color backgroundColour;

        public Coord mapCenter
        {
            get
            {
                return new Coord(mapSize.x / 2, mapSize.y / 2);
            }
        }
    }
}

