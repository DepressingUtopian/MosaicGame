using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using System;
using SFB;

#if UNITY_EDITOR
using UnityEditor;
#endif
public class UIEvents : MonoBehaviour
{
    
    // Start is called before the first frame update
    private int slider_Gorizontal = 0;
    private int slider_Vertical = 0;

    public GameObject Panel;

    public Slider slider1;
    public Slider slider2;

    public InputField slider1_Text;
    public InputField slider2_Text;
    public InputField imageURL_Field;
    public Button ShowButton;

    public Button buttonExprorer;
    public Button buttonReloadedLevel;

    public GameObject MosaicInitializator;

    
    public Button GameOverButton;

    private string path;
    private bool isShow = false;

    void Start()
    {
        Panel.SetActive(false);
        slider1.onValueChanged.AddListener(delegate { ValueChangeCheck(); });
        slider2.onValueChanged.AddListener(delegate { ValueChangeCheck2(); });
        buttonExprorer.onClick.AddListener(delegate { ExportNewImage(); });
        buttonReloadedLevel.onClick.AddListener(delegate { ReloadedLevel(); });
        ShowButton.onClick.AddListener(delegate { ShowSettings(); });
        slider1_Text.text = CropImage.gorizontalBlockCount.ToString();
        slider2_Text.text = CropImage.verticalBlockCount.ToString();
        GameOverButton.onClick.AddListener(delegate { ReloadedLevel2(); });
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey("escape"))
        {
            Application.Quit();
        }

    }
    public void ValueChangeCheck()
    {
        Debug.Log(slider1.value);
        slider_Gorizontal = (int)slider1.value;
        slider1_Text.text = slider1.value.ToString();
    }
    public void ValueChangeCheck2()
    {
        Debug.Log(slider2.value);
        slider_Vertical = (int)slider2.value;
        slider2_Text.text = slider2.value.ToString();
    }
    public void ExportNewImage()
    {
        var extensions = new[] {
            new ExtensionFilter("Image Files", "png", "jpg", "jpeg" ),
            new ExtensionFilter("All Files", "*" ),
        };
        var paths = StandaloneFileBrowser.OpenFilePanel("Выберите изображение для импорта...","", extensions, false);
        path = paths[0];
        imageURL_Field.text = path;
    }
    public void ReloadedLevel()
    {
        if (path == null)
            return;
        if (path.Length == 0)
            return;
        foreach (var obj in GameEvents.GameObjectMosaicDictionaty)
        {
            Destroy(obj.Value.obj);            
        }
        foreach (var obj in GameEvents.MosaicTileDictionaty)
        {
            Destroy(obj.Value);
        }
        Array.Clear(GameEvents.collectedMosaicBlock,0, GameEvents.collectedMosaicBlock.Length);
        CropImage.gorizontalBlockCount = int.Parse(slider1_Text.text);
        CropImage.verticalBlockCount = int.Parse(slider2_Text.text);
        CropImage.imgPath = path;
        var scriptName = MosaicInitializator.GetComponent<CropImage>();
        scriptName.runCropProcess();
        //MosaicInitializator.GetComponent<CropImage>().gameObject.
        // Backplate.
    }
    public void ReloadedLevel2()
    {
       
        foreach (var obj in GameEvents.GameObjectMosaicDictionaty)
        {
            Destroy(obj.Value.obj);
        }
        foreach (var obj in GameEvents.MosaicTileDictionaty)
        {
            Destroy(obj.Value);
        }
        Array.Clear(GameEvents.collectedMosaicBlock, 0, GameEvents.collectedMosaicBlock.Length);
     
       
        var scriptName = MosaicInitializator.GetComponent<CropImage>();
        scriptName.runCropProcess();
        //MosaicInitializator.GetComponent<CropImage>().gameObject.
        // Backplate.
    }
    private void ShowSettings()
    {
        isShow = !isShow;
        Panel.SetActive(isShow);
    }

}
