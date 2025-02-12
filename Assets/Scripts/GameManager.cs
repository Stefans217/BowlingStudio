using TMPro;
using UnityEngine;
using static UnityEngine.InputManagerEntry;

public class GameManager : MonoBehaviour
{
    [SerializeField] private float score = 0;
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private BallController ball;
    [SerializeField] private GameObject pinCollection;
    [SerializeField] private Transform pinAnchor;
    [SerializeField] private InputManager inputManager;

    private FallTrigger[] pins;
    private GameObject pinObjects;


    private void Start()
    {
        // Adding the HandleReset function as a listener to our
        // newly added OnResetPressedEvent
        inputManager.OnResetPressed.AddListener(HandleReset);
        SetPins();
    }

    private void HandleReset()
    {
        ball.ResetBall();
        SetPins();
    }

    private void SetPins()
    {
        // We first make sure that all the previous pins have been destroyed
        // this is so that we don't create a new collection of
        // standing pins on top of already fallen pins

        if (pinObjects)
        {
            foreach (Transform child in pinObjects.transform)
            {
                Destroy(child.gameObject);
            }

            Destroy(pinObjects);
        }

        // We then instantiate a new set of pins to our pin anchor transform
        pinObjects = Instantiate(pinCollection,
            pinAnchor.transform.position,
            Quaternion.identity, transform);

        // We add the Increment Score function as a listener to
        // the OnPinFall event each of new pins
        pins = FindObjectsByType<FallTrigger>(FindObjectsInactive.Include, FindObjectsSortMode.None);
        foreach (FallTrigger pin in pins)
        {
            pin.OnPinFall.AddListener(IncrementScore);
        }

    }

    private void IncrementScore()
    {
        score++;
        scoreText.text = $"Score: {score}";
    }

}
