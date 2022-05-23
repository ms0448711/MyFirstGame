using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class LevelGenerator : MonoBehaviour
{

    public GameObject layoutRoom;

    public Color startColor, endColor,shopColor;

    public int distanceToEnd;

    public bool includeShop;
    public int minDistanceToShop, maxDistanceToShop;

    public Transform generatorPoint;

    public enum Direction { Up, Right, Down, Left}

    private Direction selectedDirection;

    public float xOffset = 18f, yOffset = 10f;

    public LayerMask whatIsRoom;

    private GameObject endRoom, shopRoom;

    private List<GameObject> layoutRoomObjects = new List<GameObject>();

    public RoomPrefabs rooms;

    private List<GameObject> generatedOutlines = new List<GameObject>();

    public RoomCenter centerStart, centerEnd, centerShop;

    public RoomCenter[] potentialCenters;
    // Start is called before the first frame update
    void Start()
    {
        Instantiate(layoutRoom, generatorPoint.position, generatorPoint.rotation).GetComponent<SpriteRenderer>().color=startColor;

        selectedDirection = (Direction)Random.Range(0, 4);
        MoveGenerationPoint();

        for(int i = 0; i< distanceToEnd; i++)
        {
            GameObject newRoom = Instantiate(layoutRoom, generatorPoint.position, generatorPoint.rotation);

            layoutRoomObjects.Add(newRoom);

            if (i + 1 == distanceToEnd)
            {
                newRoom.GetComponent<SpriteRenderer>().color = endColor;
                layoutRoomObjects.RemoveAt(layoutRoomObjects.Count-1);
                endRoom = newRoom;
            }

            selectedDirection = (Direction)Random.Range(0, 4);
            MoveGenerationPoint();

            while(Physics2D.OverlapCircle(generatorPoint.position, .2f, whatIsRoom))
            {
                MoveGenerationPoint();
            }
        }

        if (includeShop)
        {
            int shopSelector = Random.Range(minDistanceToShop, maxDistanceToShop + 1);
            shopRoom = layoutRoomObjects[shopSelector];
            layoutRoomObjects.RemoveAt(shopSelector);
            shopRoom.GetComponent<SpriteRenderer>().color = shopColor;
        }

        //create room outline
        CreateRoomOutline(Vector3.zero);
        foreach(GameObject room in layoutRoomObjects)
        {
            CreateRoomOutline(room.transform.position);
        }
        CreateRoomOutline(endRoom.transform.position);

        if (includeShop)
        {
            CreateRoomOutline(shopRoom.transform.position);
        }

        foreach(GameObject outline in generatedOutlines)
        {
            bool generateCenter = true;

            if (outline.transform.position == Vector3.zero)
            {
                Instantiate(centerStart, outline.transform.position, transform.rotation).theRoom = outline.GetComponent<Room>();
                generateCenter = false;
            }
            else if (outline.transform.position==endRoom.transform.position)
            {
                Instantiate(centerEnd, outline.transform.position, transform.rotation).theRoom = outline.GetComponent<Room>();
                generateCenter = false;
            }
            if (includeShop)
            {
                if (outline.transform.position == shopRoom.transform.position)
                {
                    Instantiate(centerShop, outline.transform.position, transform.rotation).theRoom = outline.GetComponent<Room>();
                    generateCenter = false;
                }
            }

            if (generateCenter) 
            {
                int centerSelect = Random.Range(0, potentialCenters.Length);

                Instantiate(potentialCenters[centerSelect], outline.transform.position, transform.rotation).theRoom = outline.GetComponent<Room>();
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
#if UNITY_EDITOR
        if (Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
#endif
    }

    public void MoveGenerationPoint()
    {
        switch (selectedDirection)
        {
            case Direction.Up:
                generatorPoint.position += new Vector3(0f, yOffset, 0f);
                break;
            case Direction.Down:
                generatorPoint.position += new Vector3(0f, -yOffset, 0f);
                break;
            case Direction.Right:
                generatorPoint.position += new Vector3(xOffset, 0f, 0f);
                break;
            case Direction.Left:
                generatorPoint.position += new Vector3(-xOffset, 0f, 0f);
                break;
            default:
                break;
        }
    }

    public void CreateRoomOutline(Vector3 roomPosition)
    {
        bool roomAbove = Physics2D.OverlapCircle(roomPosition+new Vector3(0f,yOffset,0f),.2f,whatIsRoom);
        bool roomBelow = Physics2D.OverlapCircle(roomPosition + new Vector3(0f, -yOffset, 0f), .2f, whatIsRoom);
        bool roomLeft = Physics2D.OverlapCircle(roomPosition + new Vector3(-xOffset, 0f, 0f), .2f, whatIsRoom);
        bool roomRight = Physics2D.OverlapCircle(roomPosition + new Vector3(xOffset, 0f, 0f), .2f, whatIsRoom);

        int directionCount = 0;
        if (roomAbove)
            directionCount++;
        if (roomBelow)
            directionCount++;
        if (roomLeft)
            directionCount++;
        if (roomRight)
            directionCount++;

        switch (directionCount)
        {
            case 0:
                Debug.LogError("Found no room exists!!");
                break;
            case 1:
                if (roomAbove)
                    generatedOutlines.Add(Instantiate(rooms.singleUp, roomPosition, transform.rotation));
                else if(roomBelow)
                    generatedOutlines.Add(Instantiate(rooms.singleDown, roomPosition, transform.rotation));
                else if(roomLeft)
                    generatedOutlines.Add(Instantiate(rooms.singleLeft, roomPosition, transform.rotation));
                else if(roomRight)
                    generatedOutlines.Add(Instantiate(rooms.singleRight, roomPosition, transform.rotation));
                break;
            case 2:
                if (roomAbove&&roomBelow)
                    generatedOutlines.Add(Instantiate(rooms.doubleUpDown, roomPosition, transform.rotation));
                else if (roomLeft&&roomRight)
                    generatedOutlines.Add(Instantiate(rooms.doubleLeftRight, roomPosition, transform.rotation));
                else if (roomAbove&&roomRight)
                    generatedOutlines.Add(Instantiate(rooms.doubleUpRight, roomPosition, transform.rotation));
                else if (roomRight&&roomBelow)
                    generatedOutlines.Add(Instantiate(rooms.doubleRightDown, roomPosition, transform.rotation));
                else if (roomBelow&&roomLeft)
                    generatedOutlines.Add(Instantiate(rooms.doubleDownLeft, roomPosition, transform.rotation));
                else if (roomLeft&&roomAbove)
                    generatedOutlines.Add(Instantiate(rooms.doubleLeftUp, roomPosition, transform.rotation));
                break;
            case 3:
                if(!roomAbove)
                    generatedOutlines.Add(Instantiate(rooms.trippleRightDownLeft, roomPosition, transform.rotation));
                else if (!roomRight)
                    generatedOutlines.Add(Instantiate(rooms.trippleDownLeftUp, roomPosition, transform.rotation));
                else if (!roomBelow)
                    generatedOutlines.Add(Instantiate(rooms.trippleLeftUpRight, roomPosition, transform.rotation));
                else if (!roomLeft)
                    generatedOutlines.Add(Instantiate(rooms.trippleUpRightDown, roomPosition, transform.rotation));
                break;
            case 4:
                generatedOutlines.Add(Instantiate(rooms.fourway, roomPosition, transform.rotation));
                break;
        }

    }


}

[System.Serializable]
public class RoomPrefabs
{
    public GameObject singleUp, singleDown, singleLeft, singleRight,
        doubleUpDown, doubleLeftRight, doubleUpRight, doubleRightDown, doubleDownLeft, doubleLeftUp,
        trippleUpRightDown, trippleRightDownLeft, trippleDownLeftUp, trippleLeftUpRight,
        fourway;
}
