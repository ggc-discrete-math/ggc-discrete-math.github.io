using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HelpPanel : MonoBehaviour
{
    GUIMenu guiMenu;
    
    // Start is called before the first frame update
    void Start()
    {
        guiMenu = Camera.main.GetComponent<GUIMenu>();
        gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void show()
    {
        gameObject.SetActive(!gameObject.activeSelf);
        guiMenu.enabled = !guiMenu.enabled;
    }
}
