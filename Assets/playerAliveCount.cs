using UnityEngine;

public class playerAliveCount : MonoBehaviour
{    
    public float AliveAIs = 99;
    public void updateCounter(){
        AliveAIs -= 1;
        if(AliveAIs ==0){
            
        }
    }

}
