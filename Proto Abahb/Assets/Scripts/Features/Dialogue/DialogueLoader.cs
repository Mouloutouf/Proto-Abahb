using System.Collections;
using System.Collections.Generic;
using Features.Dialogue;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Utilities.Runtime.NeUI;
using Utilities.Singletons;

public class DialogueLoader : Singleton<DialogueLoader>
{
    [SerializeField] private DialogueData serializedDialogue;
    #region References
    [FoldoutGroup("References"), SerializeField]
    private CanvasGroupFade fadeGroup;
    [FoldoutGroup("References"), SerializeField]
    private SuperTextMesh titleText;
    [FoldoutGroup("References"), SerializeField]
    private SuperTextMesh dialogueText;
    [FoldoutGroup("References"), SerializeField]
    private Image characterImage;

    [Space] [FoldoutGroup("References"), SerializeField]
    private CharacterDB characterDatabase;
    #endregion

    #region Events
    [FoldoutGroup("Events"), SerializeField]
    private UnityEvent OnPlay;
    [FoldoutGroup("Events"), SerializeField]
    private UnityEvent OnNext;

    #endregion
    
    private DialogueData dialogue;
    private int dialogueIndex;
    private CharacterName character;

    [Button]
    public void PlayDialogue(DialogueData dialogue)
    {
        if (dialogue == null)
        {
            if (this.dialogue == null) dialogue = serializedDialogue;
            else
            {
                NextFrame();
                return;
            }
        }
        
        OnPlay.Invoke();
        this.dialogue = dialogue;
        if(!fadeGroup.gameObject.activeSelf) fadeGroup.Fade(true);
        NextFrame();
    }

    [Button]
    public void NextFrame()
    {
        if(dialogue == null) return;
        if (dialogueText.reading) { dialogueText.SkipToEnd(); return; }
        if(dialogue.dialogueFrames.Count <= dialogueIndex || (dialogueIndex > 0 && !dialogueText.gameObject.activeSelf)) {EndDialogue(); return;}
        
        OnNext.Invoke();
        var frame = dialogue[dialogueIndex];
        if(characterImage) characterImage.sprite = frame.Sprite;
        if (frame.Character != character)
        {
            character = frame.Character;
            if(titleText) titleText.text = $"• {characterDatabase[character].DisplayName} •";
        }
        if(dialogueText) dialogueText.text = frame.Text;
        dialogueIndex++;
    }

    public void EndDialogue()
    {
        fadeGroup.Fade(false);
    }
}
