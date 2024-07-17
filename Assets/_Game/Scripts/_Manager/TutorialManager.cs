using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    public List<Plate> tutPlate;
    public List<Transform> tutCakes;
    public List<int> tutCakesId;
    public int currentTutIndex = 0;
    PanelTutorial panelTutorial;
    void Start()
    {
        
    }

    public void PlayTutorial()
    {
        if(currentTutIndex == 2)
        {
            UIManager.instance.ClosePanelTutorial();
            return;
        }
        UIManager.instance.ShowPanelTutorial();
        if(panelTutorial == null)
            panelTutorial = UIManager.instance.GetPanel(UIPanelType.PanelTutorial).GetComponent<PanelTutorial>();
        panelTutorial.PlayTutorial(tutPlate[currentTutIndex].transform, tutCakes[currentTutIndex]);
        currentTutIndex++;
    }

    public bool TutorialCondition()
    {
        return (ProfileManager.Instance.playerData.playerResourseSave.currentLevel == 0
            && ProfileManager.Instance.playerData.playerResourseSave.currentExp == 0);
    }
}
