using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseNPCScript : MonoBehaviour
{

    public GameObject NPCUI;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public virtual void setShopUIActive()
    {
        NPCUI.SetActive(true);
    }

    public virtual void setShopUIInactive()
    {
        NPCUI.SetActive(false);
        Time.timeScale = 1.0f;
    }
}
