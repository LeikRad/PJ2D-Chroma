using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Narrative : MonoBehaviour
{
    public List<Image> firstCutscene = new();
    public List<Image> secondCutscene = new();
    public List<Image> thirdCutscene = new();
    private MainMenu mainmenu;


    private int first_current_count = 1;
    private int second_current_count = 0;
    private int third_current_count = 0;

    public void NextPanel()
    {
        if (first_current_count < firstCutscene.Count)
        {
            firstCutscene[first_current_count].enabled = true;
            first_current_count++;
        } 
        else if (second_current_count < secondCutscene.Count)
        {
            if (second_current_count == 0) 
            {
                foreach (Image image in firstCutscene)
                {
                    image.enabled = false;
                }
            }
            secondCutscene[second_current_count].enabled = true;
            second_current_count++;
        }
        else
        {
            if (third_current_count == 0)
            {
                foreach (Image image in secondCutscene)
                {
                    image.enabled = false;
                }
            }

            thirdCutscene[third_current_count].enabled = true;
            third_current_count++;
        }
        if (third_current_count == thirdCutscene.Count)
        {
            //mainmenu.GoToScene("Room_1.1");

            SceneChanger.Instance.ChangeScene("StartingRoom", new Vector3(0f, 0f, 0f));

        }
    }
}
