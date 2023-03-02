using System.Collections;
using UnityEngine;

public class ElectricHeater : Heater
{
    [SerializeField] GameObject progressBar;
    [SerializeField] [HideInInspector] float defaultScale;

    private string[] dialogue = null;
    private string audio_path = null;
    private AudioClip[] clips = null;

    public override float Heating => Mathf.Approximately(ElectricHeaterProvider.the().Energy, 0) ? TemperatureSystem.the().OutsideTemperature : provideHeat;

    void Start() {
        defaultScale = progressBar.transform.localScale.y;
        SetSprite();
    }

    void Update() {
        var scale = progressBar.transform.localScale;
        scale.y = ElectricHeaterProvider.the().Energy * defaultScale;
        progressBar.transform.localScale = scale;
        var pos = progressBar.transform.localPosition;
        pos.y = -(1 - scale.y - (1 - defaultScale)) / 2;
        progressBar.transform.localPosition = pos;
    }

    public override void UseItem() {
        if (ElectricHeaterProvider.the().HasSolarPanel) return;
        var solar = InventoryManager.the().TakeBagItem("Solar Panel");
        
        if (solar == null) {

            if(!DialogueSystem.the().english){
                this.audio_path = "Audio/DE/";
                this.dialogue = new string[] {"Die Energie ist bald aus, vielleicht kann ich irgendwo Solarpanels finden."};
            } else {
                this.audio_path = "Audio/EN/";
                this.dialogue = new string[] {"Energy is running out; maybe I can find some solar panels."};
            }

            this.clips = new AudioClip[] {Resources.Load<AudioClip>(audio_path + "Electricheater1")};
            DialogueSystem.the().StartDialogue(dialogue, clips);
        } else {
            if (Mathf.Approximately(ElectricHeaterProvider.the().Energy, 0)) {

                if(!DialogueSystem.the().english){
                    this.audio_path = "Audio/DE/";
                    this.dialogue = new string[] {"Endlich ist es wieder warm..."};
                } else {
                    this.audio_path = "Audio/EN/";
                    this.dialogue = new string[] {"Finally warm again..."};
                }

                this.clips = new AudioClip[] {Resources.Load<AudioClip>(audio_path + "Electricheater2")};
                DialogueSystem.the().StartDialogue(dialogue, clips);
            } else {

                if(!DialogueSystem.the().english){
                    this.audio_path = "Audio/DE/";
                    this.dialogue = new string[] {"Das sollte für eine weile reichen."};
                } else {
                    this.audio_path = "Audio/EN/";
                    this.dialogue = new string[] {"That should last a while."};
                }
                
                this.clips = new AudioClip[] {Resources.Load<AudioClip>(audio_path + "Electricheater3")};
                DialogueSystem.the().StartDialogue(dialogue, clips);
            }
            ElectricHeaterProvider.the().AddSolarPanel();
            PlayerController.the().UnregisterCollectItem(this);
        }
    }

    public override string GetItemName() {
        return "Electric Heater";
    }

    public override string GetItemDescription() {
        return "Don't freeze to death. Must be fed with energy before running out.";
    }

    public override bool IsInteractible() {
        return !ElectricHeaterProvider.the().HasSolarPanel;
    }

    public override bool IsCollectible() {
        return false;
    }
}
