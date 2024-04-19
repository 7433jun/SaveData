using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;

public class CSVManager : MonoBehaviour
{
    [SerializeField] GameObject csvPanel;
    [SerializeField] GameObject textPanel;
    [SerializeField] TMP_InputField saveFileName;
    [SerializeField] GameObject saveContent;
    [SerializeField] TMP_InputField loadFileName;
    [SerializeField] GameObject loadContent;

    StreamWriter sw;
    StreamReader sr;

    public void AddLineButton()
    {
        Instantiate(csvPanel, saveContent.transform);
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

        sw = new StreamWriter(Application.dataPath + "/" +  saveFileName.text + ".csv", false);

        foreach (Transform Panel in saveContent.transform)
        {
            string row = "";
            foreach(TMP_InputField inputField in Panel.GetComponentsInChildren<TMP_InputField>())
            {
                row += inputField.text + ",";
            }
            row = row.TrimEnd(',');

            if (row == ",,,")
            {
                continue;
            }
            else
            {
                sw.WriteLine(row);
            }
        }

        sw.Close();
        Debug.Log("저장 완료");
    }

    public void LoadFileButton()
    {
        if (loadContent.transform.childCount != 0)
        {
            foreach(Transform child in loadContent.transform)
            {
                Destroy(child.gameObject);
            }
        }

        if (File.Exists(Application.dataPath + "/" + loadFileName.text + ".csv"))
        {
            sr = new StreamReader(Application.dataPath + "/" + loadFileName.text + ".csv");

            while (true)
            {
                string line = sr.ReadLine();
                if (line == null)
                    break;

                Instantiate(textPanel, loadContent.transform).GetComponentInChildren<TextMeshProUGUI>().text = line;
            }

            sr.Close();
        }
        else
        {
            Debug.Log(loadFileName.text + ".csv 파일이 존재하지 않습니다.");
        }
    }
}
