using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    
    
    public GameObject[] players;

    public void CheckWinState()
    {
        int liveCount = 0;

        foreach (GameObject player in players)
        {
            if (player.activeInHierarchy)
            {
                liveCount++;
            }
        }

        if (liveCount <= 1)
        {
            Invoke(nameof(OnWinning), 1.25f);
        }
    }

    private void OnWinning()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }


}
