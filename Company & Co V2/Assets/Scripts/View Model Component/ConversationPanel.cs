using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ConversationPanel : MonoBehaviour
{
    public Text message;
    public Image speaker;
    public GameObject arrow;
    public Panel panel;

    void Start()
    {
        Vector3 pos = arrow.transform.localPosition; //Position actuelle de la flèche
        arrow.transform.localPosition = new Vector3(pos.x, pos.y + 5, pos.z); //La bouge vers le haut de 5
        Tweener t = arrow.transform.MoveToLocal(new Vector3(pos.x, pos.y - 5, pos.z), 0.5f, EasingEquations.EaseInQuad); //Animation en reverse
        t.easingControl.loopType = EasingControl.LoopType.PingPong;
        t.easingControl.loopCount = -1; //Loop d'animation pour la flèche infini
    }

    public IEnumerator Display(SpeakerData sd)
    {
        speaker.sprite = sd.speaker;
        speaker.SetNativeSize();
        for (int i = 0; i < sd.messages.Count; ++i)
        {
            message.text = sd.messages[i];
            arrow.SetActive(i + 1 < sd.messages.Count);
            yield return null;
        }
    }
}
