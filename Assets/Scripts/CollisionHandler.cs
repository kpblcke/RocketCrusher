using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CollisionHandler : MonoBehaviour
{
    [SerializeField] private RandomSound explosionSound;
    [SerializeField] private RandomSound finishSound;

    [SerializeField] ParticleSystem successParticles;
    [SerializeField] ParticleSystem crashParticles;

    [SerializeField] private float delay = 1.5f;

    private bool _isTransitioning = false;
    bool collisionDisabled = false;
    
    void Update() 
    {
        RespondToDebugKeys();    
    }

    void RespondToDebugKeys()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            LoadNextLevel();
        }
        else if (Input.GetKeyDown(KeyCode.C))
        {
            collisionDisabled = !collisionDisabled;  // toggle collision
        } 
    }

    
    void OnCollisionEnter(Collision other) 
    {
        if (_isTransitioning || collisionDisabled)
        {
            return;
        }
        switch (other.gameObject.tag)
        {
            case "Friendly":
                Debug.Log("This thing is friendly");
                break;
            case "Finish":
                FinishSequence();
                break;
            default:
                CrashSequence();
                break;
        }
    }

    private void FinishSequence()
    {
        _isTransitioning = true;
        successParticles.Play();
        GetComponent<Movement>().enabled = false;
        AudioSource.PlayClipAtPoint(finishSound.GetSound(), Camera.main.transform.position);
        StartCoroutine(LoadNextLevel());
    }

    private void CrashSequence()
    {
        _isTransitioning = true;
        crashParticles.Play();
        GetComponent<Movement>().enabled = false;
        AudioSource.PlayClipAtPoint(explosionSound.GetSound(), Camera.main.transform.position);
        StartCoroutine(ReloadLevel());
    }

    IEnumerator ReloadLevel()
    {
        yield return new WaitForSecondsRealtime(1.5f);
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex);
    }
    
    IEnumerator LoadNextLevel()
    {
        yield return new WaitForSecondsRealtime(1.5f);
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        int nextSceneIndex = currentSceneIndex + 1;
        if (nextSceneIndex == SceneManager.sceneCountInBuildSettings)
        {
            nextSceneIndex = 0;
        }
        SceneManager.LoadScene(nextSceneIndex);
    }
}