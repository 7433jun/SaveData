using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;

public class JsonManager : MonoBehaviour
{
    [SerializeField] GameObject jsonPanel;
    [SerializeField] GameObject textPanel;
    [SerializeField] TMP_InputField saveFileName;
    [SerializeField] GameObject saveContent;
    [SerializeField] TMP_InputField loadFileName;
    [SerializeField] GameObject loadContent;

    public void AddLineButton()
    {
        Instantiate(jsonPanel, saveContent.transform);
    }

    public void RemoveLineButton()
    {
        if (saveContent.transform.childCount == 0)
            return;

        Destroy(saveContent.transform.GetChild(saveContent.transform.childCount - 1).gameObject);
    }

    public void SaveFileButton()
    {
        if (saveContent.transform.childCount == 0)
        {
            Debug.Log("저장할 내용이 없습니다.");
            return;
        }

        JObject jData = new JObject();

        foreach (Transform Panel in saveContent.transform)
        {
            string key = Panel.GetComponentsInChildren<TMP_InputField>()[0].text;
            string value = Panel.GetComponentsInChildren<TMP_InputField>()[1].text;

            if (key == "" || value == "")
            {
                Debug.Log("빈 값이 있습니다.");
                return;
            }

            jData.Add(key, value);
        }

        File.WriteAllText(Application.dataPath + "/" + saveFileName.text + ".json", jData.ToString());

        Debug.Log("저장 완료");
    }

    public void LoadFileButton()
    {
        if (loadContent.transform.childCount != 0)
        {
            foreach (Transform child in loadContent.transform)
            {
                Destroy(child.gameObject);
            }
        }

        if (File.Exists(Application.dataPath + "/" + loadFileName.text + ".json"))
        {
            StreamReader sr = File.OpenText(Application.dataPath + "/" + loadFileName.text + ".json");

            JObject jReadData = JObject.Parse(sr.ReadToEnd());
            
            foreach (var data in jReadData)
            {
                Instantiate(textPanel, loadContent.transform).GetComponentInChildren<TextMeshProUGUI>().text = $"{data.Key} : {data.Value}";
            }
        }
        else
        {
            Debug.Log(loadFileName.text + ".json 파일이 존재하지 않습니다.");
        }
    }
}
