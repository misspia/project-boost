using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Bird : MonoBehaviour {

    [SerializeField] float thurstPower = 100f;
    [SerializeField] float mainThrust = 100f;
    [SerializeField] float levelLoadDelay = 2f;
    [SerializeField] AudioClip mainEngine;
    [SerializeField] AudioClip success;
    [SerializeField] AudioClip death;
    
    [SerializeField] ParticleSystem successParticles;

    Rigidbody rigidBody;
    AudioSource audioSource;

    enum State { Alive, Dying, Transcending }
    State state = State.Alive;

    bool isCollisionDisabled = false;

    // Use this for initialization
    void Start() {
        rigidBody = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
    }
    // Update is called once per frame
    void Update() {
        if (state == State.Alive)
        {
            RespondToThrustInput();
            RespondToRotate();
        }
        if(Debug.isDebugBuild)
        {
            RespondToDebugKeys();
        }
    }

    private void RespondToDebugKeys()
    {
        if(Input.GetKeyDown(KeyCode.L)) {
            LoadNextLevel();
        }
        if(Input.GetKeyDown(KeyCode.C))
        {
            isCollisionDisabled = !isCollisionDisabled;
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (state != State.Alive || isCollisionDisabled) return;
        switch (collision.gameObject.tag)
        {
            case "Friendly":
                break;
            case "Finish":
                StartSuccessSequence();
                break;
            default:
                StartDeathSequence();
                break;
        }
    }
    private void StartSuccessSequence()
    {
        state = State.Transcending;
        audioSource.Stop();
        audioSource.PlayOneShot(success);
        successParticles.Play();
        Invoke("LoadNextLevel", levelLoadDelay);
    }
    private void StartDeathSequence()
    {
        state = State.Dying;
        audioSource.Stop();
        audioSource.PlayOneShot(death);
        Invoke("LoadFirstLevel", levelLoadDelay);
    }
    private void LoadNextLevel()
    {
        SceneManager.LoadScene(1);
    }
    private void LoadFirstLevel()
    {
        SceneManager.LoadScene(0);
    }
    private void RespondToThrustInput()
    {
        
        if (Input.GetKey(KeyCode.Space))
        {
            rigidBody.AddRelativeForce(Vector3.up * mainThrust);
            if (!audioSource.isPlaying)
            {
                ApplyThrust();
            }
        }
        else
        {
            audioSource.Stop();
        }
    }
    private void ApplyThrust()
    {
        audioSource.PlayOneShot(mainEngine);
    }
    private void RespondToRotate()
    {
        rigidBody.freezeRotation = true; // take manual control of rotation
        
        float rotationThisFrame = thurstPower * Time.deltaTime;

        if (Input.GetKey(KeyCode.A))
        {
            transform.Rotate(Vector3.forward * rotationThisFrame);
        }
        if (Input.GetKey(KeyCode.D))
        {
            transform.Rotate(Vector3.back * rotationThisFrame);
        }
        rigidBody.freezeRotation = false; // resume physics control of rotation
    }    
}
