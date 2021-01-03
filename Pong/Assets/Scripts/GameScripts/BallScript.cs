using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BallScript : MonoBehaviour
{
    public Rigidbody2D rb;
    public float speed = 5f;

    // These variables will display the string value of the players' scores
    public Text playerRightScore;
    public Text playerLeftScore;

    // These variables will hold the integer values of the players' scores for ease when incrementing score
    private int pRScore;
    private int pLScore;

    private const double paddleRatio = 1 / 1.4;
    private const int maxAngle = 60;

    void OnCollisionEnter2D(Collision2D col)
    {
        // If the ball hits the player's or computer's White Bar, we want the ball to bounce left/right towards the opposing side
        if (col.gameObject.CompareTag("PlayerRight"))
        {
            // We want the ball to change angle after hitting the paddle depending on where the ball hits the paddle.
            // The max angle the ball will make with a horizontal line will be 45 degrees (above or below).

            // In order to do so we take the point where the ball hit, offset by the negative y location of the paddle, and multiply
            // it by the paddleRatio and 45. 

            // This will give us values from -45 to 45 (inclusive) that we will use to set the new angle of the ball.

            double pointCollided = col.contacts[0].point.y;
            double paddleCenter = GameObject.Find("PlayerRight").transform.position.y;
            float newBallAngle = (float) ((pointCollided - paddleCenter) * paddleRatio * maxAngle);

            float newX = Mathf.Cos(newBallAngle * Mathf.Deg2Rad) * 10f;
            float newY = Mathf.Sin(newBallAngle * Mathf.Deg2Rad) * 10f;

            rb.velocity = new Vector3(-newX, newY, 0f);
        }

        // We use almost identical code for the left player, however, we leave newX positive on our final line (as we want the ball to travel right).
        if (col.gameObject.CompareTag("PlayerLeft"))
        {
            double pointCollided = col.contacts[0].point.y;
            double paddleCenter = GameObject.Find("PlayerLeft").transform.position.y;
            float newBallAngle = (float)((pointCollided - paddleCenter) * paddleRatio * maxAngle);

            float newX = Mathf.Cos(newBallAngle * Mathf.Deg2Rad) * 10f;
            float newY = Mathf.Sin(newBallAngle * Mathf.Deg2Rad) * 10f;

            rb.velocity = new Vector3(newX, newY, 0f);
        }

        // If the ball hits the top or bottom of the screen, we want the ball to bounce up/down towards the center of the screen
        if (col.gameObject.CompareTag("TopBounds") || col.gameObject.CompareTag("BottomBounds"))
        {
            rb.velocity = new Vector3( rb.velocity.x * 2f, rb.position.y * -1, 0f);
        }

        // If the ball hits the left side (computer's side), we want to add a point for the Player
        if (col.gameObject.CompareTag("LeftBounds"))
        {
            pRScore++;
            playerRightScore.text = pRScore.ToString();

            if (pRScore == 10)
            {
                EndGame(0);
            }
            else
            {
                ResetBall();
            }
        }

        // If the ball hits the right side (player's side), we want to add a point for the Computer
        if (col.gameObject.CompareTag("RightBounds"))
        {
            pLScore++;
            playerLeftScore.text = pLScore.ToString();

            if (pLScore == 10)
            {
                EndGame(1);
            }
            else
            {
                ResetBall();
            }
        }
    }

    // At the start of each new ball, we give the ball the following tradjectory
    void Start()
    {
        playerRightScore.text = "0";
        playerLeftScore.text = "0";

        pRScore = 0;
        pLScore = 0;

        // This will be used to choose, at random, which direction the ping pong ball will go at instantiation
        List<int> leftOrRightStart = new List<int> { -1, 1 };
        int choice = leftOrRightStart[Random.Range(0, leftOrRightStart.Count)];

        rb.velocity = new Vector3(speed * choice, 0.5f * choice, 0f);
    }

    void Update()
    {
        // This is a makeshift solution to resetting the ball if it gets caught/stops in a location that freezes the gamestate
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ResetBall();
        }
    }

    // Places the ball at the center of the screen and gives it a new initial velocity
    void ResetBall()
    {
        rb.position = new Vector3(0, 0, 0);

        // This will be used to choose, at random, which direction the ping pong ball will go at instantiation
        List<int> leftOrRightStart = new List<int> { -1, 1 };
        int choice = leftOrRightStart[Random.Range(0, leftOrRightStart.Count)];

        rb.velocity = new Vector3(speed * choice + (pLScore + pRScore) * 2, 0.2f * choice, 0f);
    }

    // Declares a winner and ends the game
    void EndGame(int winner)
    {
        // Player reached 10 points first
        if (winner == 0)
        {

        }
        // Computer reached 10 points first
        else
        {

        }
    }
}
