using System.Collections;
using UnityEngine;

public class ElectricHeater : Heater
{
    [SerializeField] GameObject progressBar;
    [SerializeField] [HideInInspector] float defaultScale;

    private string[] dialogue = {"Energy is running out, maybe I can find some solar panels."};
    private string[] dialogue_warm = {"Finally warm again..."};
    private string[] dialogue_last = {"That should last a while."};
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
            this.clips = new AudioClip[] {Resources.Load<AudioClip>("Audio/test1")};
            DialogueSystem.the().StartDialogue(dialogue, clips);
        } else {
            if (Mathf.Approximately(ElectricHeaterProvider.the().Energy, 0)) {
                this.clips = new AudioClip[] {Resources.Load<AudioClip>("Audio/test1")};
                DialogueSystem.the().StartDialogue(dialogue_warm, clips);
            } else {
                this.clips = new AudioClip[] {Resources.Load<AudioClip>("Audio/test1")};
                DialogueSystem.the().StartDialogue(dialogue_last, clips);
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
