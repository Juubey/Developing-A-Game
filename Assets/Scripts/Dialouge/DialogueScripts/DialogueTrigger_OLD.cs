using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger_OLD : MonoBehaviour
{
  public Dialogue dialogue;
  public void TriggerDialogue()
  {
      FindObjectOfType<DialogueManager_OLD>().StartDialogue(dialogue);
  }
}
