using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelGenerator : MonoBehaviour
{
    public GameObject layoutRoom;
    private GameObject endRoom, shopRoom, chestRoom;
    public bool includeShop;
    public bool includeChestRoom;
    private List<GameObject> layoutRoomObjects = new List<GameObject>();
    public RoomPrefabs rooms;
    public Transform generatorPoint;

    public Color startRoomColor;
    public Color endRoomColor;
    public Color chestRoomColor;
    public Color shopColor;

    public int distanceToEnd;
    public int minDistanceToShop;
    public int maxDistanceToShop;
    public int minDistanceToChestRoom;
    public int maxDistanceToChestRoom;

    public float xOffset = 18f;
    public float yOffset = 10f;

    public LayerMask whatIsRoom;

    private List<GameObject> generatedOutlines = new List<GameObject>();
    public RoomCenter centerStart;
    public RoomCenter centerEnd;
    public RoomCenter centerShop;
    public RoomCenter centerChestRoom;
    public RoomCenter[] potentialCenters;

    public enum Direction
    {
        up,
        right,
        down,
        left
    }

    public Direction selectedDirection;


    void Start()
    {
        GenerateStartRoom();
        GenerateRooms();
    }

    void Update()
    {

#if !UNITY_EDITOR        
        if(Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
#endif
    }

    public void GenerateStartRoom()
    {
        Instantiate(layoutRoom, generatorPoint.position, generatorPoint.rotation).GetComponent<SpriteRenderer>().color = startRoomColor;
        selectedDirection = (Direction)Random.Range(0, 4);
        MoveGenerationPoint();
    }

    public void GenerateRooms()
    {
        for(int i = 0; i < distanceToEnd; i++)
        {
            GameObject newRoom = Instantiate(layoutRoom, generatorPoint.position, generatorPoint.rotation);

            layoutRoomObjects.Add(newRoom);

            //case to catch once generated enough rooms to make the end room
            if(i + 1 == distanceToEnd)
            {
                newRoom.GetComponent<SpriteRenderer>().color = endRoomColor;
                layoutRoomObjects.RemoveAt(layoutRoomObjects.Count - 1);
                endRoom = newRoom;
            }

            selectedDirection = (Direction)Random.Range(0, 4);
            MoveGenerationPoint();

            while(Physics2D.OverlapCircle(generatorPoint.position, 0.2f, whatIsRoom))
            {
                selectedDirection = (Direction)Random.Range(0, 4);
                MoveGenerationPoint();
            }
        }
        
        GenerateShop();
        GenerateChestRoom();
        GenerateOutlines();
        GenerateCenters();
    }

    public void MoveGenerationPoint()
    {
        switch(selectedDirection)
        {
            case Direction.up:
                generatorPoint.position += new Vector3(0f, yOffset, 0f);
                break;
            
            case Direction.down:
                generatorPoint.position += new Vector3(0f, -yOffset, 0f);
                break;

            case Direction.right:
                generatorPoint.position += new Vector3(xOffset, 0f, 0f);
                break;
            
            case Direction.left:
                generatorPoint.position += new Vector3(-xOffset, 0f, 0f);
                break;
        }
    }

    public void GenerateOutlines()
    {
        //room outlines
        CreateRoomOutline(Vector3.zero);
        foreach (GameObject room in layoutRoomObjects)
        {
            CreateRoomOutline(room.transform.position);
        }

        CreateRoomOutline(endRoom.transform.position);

        if (includeShop)
        {
            CreateRoomOutline(shopRoom.transform.position);
        }
        if(includeChestRoom)
        {
            CreateRoomOutline(chestRoom.transform.position);
        }

    }

    public void GenerateCenters()
    {
        foreach (GameObject outline in generatedOutlines)
        {
            //this bool makes it so that it does not generate the center outline
            bool generateCenter = true;
            //Start position room
            if(outline.transform.position == Vector3.zero)
            {
                Instantiate(centerStart, outline.transform.position, transform.rotation).theRoom = outline.GetComponent<Room>();
                generateCenter = false;
            }
            if(outline.transform.position == endRoom.transform.position)
            {
                Instantiate(centerEnd, outline.transform.position, transform.rotation).theRoom = outline.GetComponent<Room>();
                generateCenter = false;
            }
            if (includeShop)
            {
                if(outline.transform.position == shopRoom.transform.position)
                {
                    Instantiate(centerShop, outline.transform.position, transform.rotation).theRoom = outline.GetComponent<Room>();
                    generateCenter = false;
                }
            }
            if(includeChestRoom)
            {
                if(outline.transform.position == chestRoom.transform.position)
                {
                    Instantiate(centerChestRoom, outline.transform.position, transform.rotation).theRoom = outline.GetComponent<Room>();
                    generateCenter = false;
                }
            }
            if(generateCenter)
            {
                int centerSelect = Random.Range(0, potentialCenters.Length);
                Instantiate(potentialCenters[centerSelect], outline.transform.position, transform.rotation).theRoom = outline.GetComponent<Room>();
            }
        }
    }

    public void GenerateShop()
    {
        if(includeShop)
        {
            int shopSelector = Random.Range(minDistanceToShop, maxDistanceToShop + 1);
            shopRoom = layoutRoomObjects[shopSelector];
            layoutRoomObjects.RemoveAt(shopSelector);
            shopRoom.GetComponent<SpriteRenderer>().color = shopColor;
        }
    }

    public void GenerateChestRoom()
    {
        if (includeChestRoom)
        {
            int chestRoomSelector = Random.Range(minDistanceToChestRoom, maxDistanceToChestRoom + 1);
            chestRoom = layoutRoomObjects[chestRoomSelector];
            layoutRoomObjects.RemoveAt(chestRoomSelector);
            chestRoom.GetComponent<SpriteRenderer>().color = chestRoomColor;
        }
    }


    public void CreateRoomOutline(Vector3 roomPosition)
    {
        bool roomAbove = Physics2D.OverlapCircle(roomPosition + new Vector3(0f, yOffset, 0f), 0.2f, whatIsRoom);
        bool roomBelow = Physics2D.OverlapCircle(roomPosition + new Vector3(0f, -yOffset, 0f), 0.2f, whatIsRoom);
        bool roomLeft = Physics2D.OverlapCircle(roomPosition + new Vector3(-xOffset, 0f, 0f), 0.2f, whatIsRoom);
        bool roomRight = Physics2D.OverlapCircle(roomPosition + new Vector3(xOffset, 0f, 0f), 0.2f, whatIsRoom);

        int directionCount = 0;
        if(roomAbove)
        {
            directionCount++;
        }
        if (roomBelow)
        {
            directionCount++;
        }
        if (roomLeft)
        {
            directionCount++;
        }
        if (roomRight)
        {
            directionCount++;
        }

        switch(directionCount)
        {
            case 0:
                Debug.LogError("Found no room exists!");
                break;
            
            case 1:
                if(roomAbove)
                {
                    generatedOutlines.Add(Instantiate(rooms.singleUp, roomPosition, transform.rotation));
                }
                if(roomBelow)
                {
                    generatedOutlines.Add(Instantiate(rooms.singleDown, roomPosition, transform.rotation));
                }
                if(roomLeft)
                {
                    generatedOutlines.Add(Instantiate(rooms.singleLeft, roomPosition, transform.rotation));
                }
                if(roomRight)
                {
                    generatedOutlines.Add(Instantiate(rooms.singleRight, roomPosition, transform.rotation));
                }

                break;
                
            case 2:
                if(roomAbove && roomBelow)
                {
                    generatedOutlines.Add(Instantiate(rooms.doubleUpDown, roomPosition, transform.rotation));
                }
                if(roomLeft && roomRight)
                {
                    generatedOutlines.Add(Instantiate(rooms.doubleLeftRight, roomPosition, transform.rotation));
                }
                if(roomAbove && roomRight)
                {
                    generatedOutlines.Add(Instantiate(rooms.doubleUpRight, roomPosition, transform.rotation));
                }
                if(roomAbove && roomLeft)
                {
                    generatedOutlines.Add(Instantiate(rooms.doubleUpLeft, roomPosition, transform.rotation));
                }
                if(roomBelow && roomRight)
                {
                    generatedOutlines.Add(Instantiate(rooms.doubleDownRight, roomPosition, transform.rotation));
                }
                if(roomBelow && roomLeft)
                {
                    generatedOutlines.Add(Instantiate(rooms.doubleDownLeft, roomPosition, transform.rotation));
                }
                break;

            case 3:
                if(roomAbove && roomRight && roomBelow)
                {
                    generatedOutlines.Add(Instantiate(rooms.tripleUpRightDown, roomPosition, transform.rotation));
                }
                if(roomLeft && roomBelow && roomRight)
                {
                    generatedOutlines.Add(Instantiate(rooms.tripleLeftDownRight, roomPosition, transform.rotation));
                }
                if(roomAbove && roomLeft && roomBelow)
                {
                    generatedOutlines.Add(Instantiate(rooms.tripleUpLeftDown, roomPosition, transform.rotation));
                }
                if(roomAbove && roomLeft && roomRight)
                {
                    generatedOutlines.Add(Instantiate(rooms.tripleUpLeftRight, roomPosition, transform.rotation));
                }
                break;

            case 4:
                if(roomAbove && roomBelow && roomLeft && roomRight)
                {
                    generatedOutlines.Add(Instantiate(rooms.fourway, roomPosition, transform.rotation));
                }
                break;
        }
    }
}

[System.Serializable]
public class RoomPrefabs
{
    public GameObject singleUp, singleDown, singleRight, singleLeft,
    doubleUpDown, doubleLeftRight, doubleUpRight, doubleUpLeft, doubleDownRight, doubleDownLeft,
    tripleUpRightDown, tripleLeftDownRight, tripleUpLeftDown, tripleUpLeftRight,
    fourway;
}
