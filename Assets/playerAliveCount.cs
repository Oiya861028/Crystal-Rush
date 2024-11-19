using UnityEngine;
using UnityEngine.SceneManagement;
public class playerAliveCount : MonoBehaviour
{    
    public string nextSceneName;
    public float AliveAIs = 99;
    public void updateCounter(){
        AliveAIs -= 1;
        if(AliveAIs == 0){
            SceneManager.LoadScene(nextSceneName);
        }
    }
    
}
