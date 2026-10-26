using System.Collections;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [HideInInspector]
    public PlayerDirection direction;

    [HideInInspector]
    public float step_len = 0.2f; //the length of each step, which is also the width of the snake body
    [HideInInspector]
    public float movement_frequency = 0.1f; //the frequency of movement, which is also the time interval between each step

    private float counter;//a counter to count the time interval between each step, which is used to control the movement frequency of the snake
    private bool move;//a boolean to control whether the snake is moving or not, which is used to control the movement of the snake

    [SerializeField]
    private GameObject tailPrefab;//the prefab of the snake body, which is used to create new nodes when the snake eats a fruit

    private List<Vector3> delta_position;//a list to store the delta position of each node, which is used to control the movement of the snake body
    private List<Rigidbody> nodes;//    a list to store the rigidbodies of each node, which is used to control the movement of the snake body

    private Rigidbody main_Body;//the rigidbody of the snake head, which is used to control the movement of the snake head
    private Rigidbody head_Body;//the rigidbody of the snake head, which is used to control the movement of the snake head
    
    private Transform tr;// the transform of the snake head, which is used to control the movement of the snake head

    private bool create_node_At_tail;//a boolean to control whether to create a new node at the tail of the snake or not, which is used to control the growth of the snake body

    [SerializeField] private AudioClip eatSound;   // Drag Fruit sound here
    [SerializeField] private AudioClip bombSound;  // Drag Bomb sound here
    private AudioSource audioSource;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        audioSource = GetComponent<AudioSource>();

        tr = transform;//get the transform of the snake head, which is used to control the movement of the snake head
        main_Body = GetComponent<Rigidbody>();//get the rigidbody of the snake head, which is used to control the movement of the snake head

        InitSnakeNodes();
        InitPlayer();

        delta_position = new List<Vector3>()
        {
            new Vector3(-step_len,0f), //left
            new Vector3(0f,step_len), //up
            new Vector3(step_len,0f), //right
            new Vector3(0f,-step_len) //down
        };
    }

    // Update is called once per frame
    void Update()
    {
        CheckMovementFrequecy();
    }

    void FixedUpdate()
    {
        if (move)
        {
            move = false;

            Move();
        }
    }
    void InitSnakeNodes()
    {
        nodes = new List<Rigidbody>();//initialize the list of rigidbodies of each node, which is used to control the movement of the snake body

        nodes.Add(tr.GetChild(0).GetComponent<Rigidbody>()); //the head of the snake
        nodes.Add(tr.GetChild(1).GetComponent<Rigidbody>()); //the first node of the snake body
        nodes.Add(tr.GetChild(2).GetComponent<Rigidbody>()); //the second node of the snake body

        head_Body = nodes[0];//the rigidbody of the snake head, which is used to control the movement of the snake head
    }

    void setDirectionRandom()
    {
        int dirRandom = Random.Range(0, (int)PlayerDirection.COUNT);
        direction = (PlayerDirection)dirRandom;
    }

    void InitPlayer()
    {
        setDirectionRandom();
        switch (direction)
        {
            case PlayerDirection.RIGHT:
                nodes[1].position = nodes[0].position - new Vector3(step_len, 0f, 0f);
                nodes[2].position = nodes[0].position - new Vector3(step_len * 2f, 0f, 0f);
                break;

            case PlayerDirection.LEFT:
                nodes[1].position = nodes[0].position + new Vector3(step_len, 0f, 0f);
                nodes[2].position = nodes[0].position + new Vector3(step_len * 2f, 0f, 0f);
                break;

            case PlayerDirection.UP:
                nodes[1].position = nodes[0].position - new Vector3(0f, step_len, 0f);
                nodes[2].position = nodes[0].position - new Vector3(0f, step_len * 2f, 0f);
                break;

            case PlayerDirection.DOWN:
                nodes[1].position = nodes[0].position + new Vector3(0f, step_len, 0f);
                nodes[2].position = nodes[0].position + new Vector3(0f, step_len * 2f, 0f);
                break;
        }
    }

    void Move()
    {
        Vector3 dposition = delta_position[(int)direction];

        Vector3 nextPosition = main_Body.position + dposition;

        // Expanded boundaries specifically for collision to allow touching the wall
        // Adjusting slightly inward to prevent clipping through the wall texture
        float collision_min_X = -4.8f; 
        float collision_max_X = 4.5f; 
        float collision_min_Y = -2.8f; 
        float collision_max_Y = 2.6f; 

        if (nextPosition.x < collision_min_X || nextPosition.x > collision_max_X || 
            nextPosition.y < collision_min_Y || nextPosition.y > collision_max_Y)
        {
            // Do not move if it hits the wall boundary. 
            // You can optionally trigger death/game over here.
            GamePlayController.instance.GameOver();
            return;
        }

        Vector3 parentPos = head_Body.position;
        Vector3 prevPosition;

        main_Body.position = nextPosition;
        head_Body.position = head_Body.position + dposition;

        for (int i = 1; i < nodes.Count; i++)
        {
            prevPosition = nodes[i].position;

            nodes[i].position = parentPos;
            parentPos = prevPosition;

        }

        //check if we need to create a new node
        //bec we ate a fruit
        if (create_node_At_tail)
        {
            create_node_At_tail = false;
            GameObject newNode = Instantiate(tailPrefab , nodes[nodes.Count - 1].position, Quaternion.identity);

            newNode.transform.SetParent(transform, true);
            nodes.Add(newNode.GetComponent<Rigidbody>());

        }

    }
    void CheckMovementFrequecy()
    {
        counter += Time.deltaTime;

        if(counter >= movement_frequency)
        {
            counter = 0f;
            move = true;

        }
    }

    // Add "public" here so Unity can see it in the OnClick menu
    public void SetInputDirection(PlayerDirection dir)
    {
        // ... your existing logic to check directions ...
        if (direction == PlayerDirection.UP && dir == PlayerDirection.DOWN) return;
        if (direction == PlayerDirection.DOWN && dir == PlayerDirection.UP) return;
        if (direction == PlayerDirection.LEFT && dir == PlayerDirection.RIGHT) return;
        if (direction == PlayerDirection.RIGHT && dir == PlayerDirection.LEFT) return;

        direction = dir;
    }

    public void SetInputDirectionMobile(int dirIndex)
    {
        // This converts the number from the button into the Direction the snake understands
        SetInputDirection((PlayerDirection)dirIndex);
    }
    //void ForceMove()
    //{
    //    counter = 0;
    //    move = false;
    //    Move();

    //}

    void OnTriggerEnter(Collider target)
    {
        if(target.tag == Tags.FRUIT)
        {
            audioSource.clip = eatSound;
            audioSource.Play();

            target.gameObject.SetActive(false);
            create_node_At_tail = true;
            GamePlayController.instance.IncreaseScore();
        }

        if (target.tag == Tags.WALL || target.tag == Tags.BOMB || target.tag == Tags.TAIL)
        {
            if (target.tag == Tags.BOMB)
            {
                audioSource.clip = bombSound;
                audioSource.Play();

                target.gameObject.SetActive(false);
                
                // Reduce the score
                GamePlayController.instance.DecreaseScore();
                
                // Check if snake has parts other than the head to remove
                if (nodes.Count > 1)
                {
                    int lastNodeIndex = nodes.Count - 1;
                    
                    // Destroy the game object in the scene
                    Destroy(nodes[lastNodeIndex].gameObject);
                    
                    // Remove it from the list so the script stops moving it
                    nodes.RemoveAt(lastNodeIndex);
                }
            }
            else if (target.tag == Tags.WALL || target.tag == Tags.TAIL)
            {
                // If we hit a Wall or our own Tail, trigger Game Over
                GamePlayController.instance.GameOver();
            }
        }
    }
}
