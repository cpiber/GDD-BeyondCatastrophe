using System.Collections;
using System.Collections.Generic;
using Items.BluePrints;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ShipRepair : PermanentItem
{
    string[] dialogue = null;
    AudioClip[] clips = null;

    string audio_path = null;
    
    private bool firstUsage = true;
    private int requiredWoodLogs = 10;
    
    public const string SHIP_FIRST_DIALOG = "shipFirstDialog";
    public const string SHIP_REPAIRED = "shipRepaired";

    public override void UseItem()
    {
        //Ship already repaired
        if (ProgressSystem.the().getProgress(SHIP_REPAIRED))
        {
            dialoguRepairOrEnd();
            return; 
        }
        
        //First usage of the ship
        if (!ProgressSystem.the().getProgress(SHIP_FIRST_DIALOG))
        {
            dialogFirstUse();
            return;
        }
        
        //BluePrints found and enough WoodLogs
        if (ProgressSystem.the().getProgress(ShipBluePrint.SHIPBLUEPRINT_COLLECTED_PORT) && 
            ProgressSystem.the().getProgress(ShipBluePrint.SHIPBLUEPRINT_COLLECTED_DI) && 
            (InventoryManager.the().TakeBagItem("WoodLogs", requiredWoodLogs) != null))
        {
            ProgressSystem.the().setProgress(SHIP_REPAIRED);
            dialoguRepairOrEnd();
            return;
        }
        
        //PORT BluePrint found - ISLAND missing
        if (ProgressSystem.the().getProgress(ShipBluePrint.SHIPBLUEPRINT_COLLECTED_PORT) &&
            !ProgressSystem.the().getProgress(ShipBluePrint.SHIPBLUEPRINT_COLLECTED_DI))
        {
            dialogMissingIsland();
            return;
        }
        
        //BluePrints found, but WoodLogs missing
        if (ProgressSystem.the().getProgress(ShipBluePrint.SHIPBLUEPRINT_COLLECTED_PORT) &&
        ProgressSystem.the().getProgress(ShipBluePrint.SHIPBLUEPRINT_COLLECTED_DI))
        {
            dialogBothFound();
            return;
        }
        
        //BluePrints are still missing
        dialogShipBluePrintsMissing();
    }

    public override string GetItemName() {
        return "Ship";
    }

    public override string GetItemDescription() {
        return "This is a broken ship, which can be repaired.";
    }

    public override bool IsInteractible() {
        return true;
    }

    public override bool IsCollectible() {
        return false;
    }


    private void dialogFirstUse()
    {
        Debug.Log("IN dialogFirstUse");
        
        if(!DialogueSystem.the().english){
            audio_path = "Audio/DE/";
            dialogue = new string[] {"Mit diesem Schiff könnte ich von der Insel flüchten!",
                                     "Zuerst müsste ich es aber reparieren...",
                                     "Mit etwas Holz könnte das klappen!"};
        } else {
            audio_path = "Audio/EN/";
            dialogue = new string[] {"With this ship, I can leave this island behind.",
                                     "But first, I need to repair it...",
                                     "That should work with some wooden logs!"};
        }

        clips = new AudioClip[]
        {
            Resources.Load<AudioClip>(audio_path + "ShipRepair1"),
            Resources.Load<AudioClip>(audio_path + "ShipRepair2"),
            Resources.Load<AudioClip>(audio_path + "ShipRepair3")  
        };
        DialogueSystem.the().StartDialogue(dialogue, clips);
        
        ProgressSystem.the().setProgress(SHIP_FIRST_DIALOG);
    }

    private void dialogBothFound()
    {
        
        if(!DialogueSystem.the().english){
            audio_path = "Audio/DE/";
            dialogue = new string[] {"10 Holzstämme sollten reichen, um das Schiff wieder intakt zu bringen!"};
        } else {
            audio_path = "Audio/EN/";
            dialogue = new string[] {"10 wood logs should be enough to fix this ship!"};
        }

        clips = new AudioClip[] {Resources.Load<AudioClip>(audio_path + "ShipRepair4")};

        DialogueSystem.the().StartDialogue(dialogue, clips);
    }

    private void dialogMissingIsland()
    {
        
        if(!DialogueSystem.the().english){
            audio_path = "Audio/DE/";
            
            dialogue = new string[] {"Etwas auf diesem Plan fehlt...!",
                                     "Vielleicht sollte ich mich umsehen und die zweite Hälfte finden!"};
        } else {
            audio_path = "Audio/EN/";
            dialogue = new string[] {"Something is missing on this plan...",
                                     "Maybe I should take a look around and try to find the second half!"};
        }

        clips = new AudioClip[]
        {
            Resources.Load<AudioClip>(audio_path + "ShipRepair5"),
            Resources.Load<AudioClip>(audio_path + "ShipRepair6") 
        };
        DialogueSystem.the().StartDialogue(dialogue, clips);
    }
    
    private void dialogShipBluePrintsMissing()
    {
        
        if(!DialogueSystem.the().english){
            audio_path = "Audio/DE/";
            
            dialogue = new string[] {"Ich muss die Baupläne des Schiffes finden!",
                                     "Die sollten doch hier irgendwo sein..." };
        } else {
            audio_path = "Audio/EN/";
            dialogue = new string[] {"I need to find the ship blueprints!",
                                     "They should be around here somewhere..."};
        }

        clips = new AudioClip[]
        {
            Resources.Load<AudioClip>(audio_path + "ShipRepair7"),
            Resources.Load<AudioClip>(audio_path + "ShipRepair8") 
        };
        DialogueSystem.the().StartDialogue(dialogue, clips);
    }

    private void dialoguRepairOrEnd() {
        if (!ProgressSystem.the().getProgress(FollowPlayer.CHILD_FOLLOWING)) {
            dialogRepairShip();
        } else {
            dialogEnd();
        }
    }

    private void dialogRepairShip()
    {
        
        if(!DialogueSystem.the().english){
            audio_path = "Audio/DE/";
            
            dialogue = new string[] {"Wow! Ich konnte das Schiff reparieren!",
                                     "Jetzt muss ich nur noch meine Tochter finden!"};
        } else {
            audio_path = "Audio/EN/";
            dialogue = new string[] {"Wow! I managed to repair the ship!",
                                     "Now I only have to find my daughter!"};
        }

        clips = new AudioClip[]
        {
            Resources.Load<AudioClip>(audio_path + "ShipRepair9"),
            Resources.Load<AudioClip>(audio_path + "ShipRepair10") 
        };
        DialogueSystem.the().StartDialogue(dialogue, clips);
    }
    
    [ContextMenu("Start End Dialogue")]
    private void dialogEnd()
    {
        
        if(!DialogueSystem.the().english){
            audio_path = "Audio/DE/";
            
            dialogue = new string[] {"Lass uns von hier abhauen!"};
        } else {
            audio_path = "Audio/EN/";
            dialogue = new string[] {"Lets get away from here!"};
        }

        clips = new AudioClip[]
        {
            Resources.Load<AudioClip>(audio_path + "ShipRepair11"),
        };
        StartCoroutine(EndCutScene());
    }

    private IEnumerator EndCutScene() {
        yield return DialogueSystem.the().StartDialogueRoutine(dialogue, clips);
        // Start cut-scene
        PlayerController.the().cameraFollowPlayer = false;
        SceneLoader.the().preventAllUnloading = true;
        var raft = GetComponent<Raft>();
        raft.enabled = true;
        yield return raft.StartSequence();
        SceneManager.LoadScene("GameEnded");
    }
        
}
