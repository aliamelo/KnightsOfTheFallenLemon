using System.Collections;
using System;
using UnityEngine;

public class ConversationController : MonoBehaviour
{
    //Un panel pour la gauche et un pour la droite
    [SerializeField] ConversationPanel leftPanel;
    [SerializeField] ConversationPanel rightPanel;

    public static event EventHandler completeEvent;

    //Pour "allumer/éteindre" le canvas
    Canvas canvas;

    //Pour tous les speakers et messages
    IEnumerator conversation;

    //Pour animer le panel actuel
    Tweener transition;

    //Pour pas réécrire les strings de positions à chaque utilisation
    const string ShowTop = "Show Top";
    const string ShowBottom = "Show Bottom";
    const string HideTop = "Hide Top";
    const string HideBottom = "Hide Bottom";

    void Start()
    {
        canvas = GetComponentInChildren<Canvas>();

        if (leftPanel.panel.CurrentPosition == null)
            leftPanel.panel.SetPosition(HideBottom, false);

        if (rightPanel.panel.CurrentPosition == null)
            rightPanel.panel.SetPosition(HideBottom, false);

        canvas.gameObject.SetActive(false);
    }

    //Pour afficher les panels de conversation
    public void Show(ConversationData data)
    {
        canvas.gameObject.SetActive(true);
        conversation = Sequence(data);
        conversation.MoveNext();
    }

    //Passe au texte suivant
    public void Next()
    {
        if (conversation == null || transition != null)
            return;

        conversation.MoveNext();
    }

    //Passe par tous les speakers et tous les messages
    IEnumerator Sequence(ConversationData data)
    {
        for (int i = 0; i < data.list.Count; ++i)
        {
            SpeakerData sd = data.list[i];

            ConversationPanel currentPanel = (sd.anchor == TextAnchor.UpperLeft || sd.anchor == TextAnchor.MiddleLeft || sd.anchor == TextAnchor.LowerLeft) ? leftPanel : rightPanel;
            IEnumerator presenter = currentPanel.Display(sd);
            presenter.MoveNext();

            string show, hide;
            if (sd.anchor == TextAnchor.UpperLeft || sd.anchor == TextAnchor.UpperCenter || sd.anchor == TextAnchor.UpperRight)
            {
                show = ShowTop;
                hide = HideTop;
            }
            else
            {
                show = ShowBottom;
                hide = HideBottom;
            }

            //Afiche le panel sur l'écran
            currentPanel.panel.SetPosition(hide, false);
            MovePanel(currentPanel, show);

            //Pour faire une "pause" une fois que le panel est à l'écran
            yield return null; 
            while (presenter.MoveNext()) //Passe tous les textes du speaker
                yield return null;

            //Dégage le panel de l'écran
            MovePanel(currentPanel, hide);
            transition.easingControl.completedEvent += delegate (object sender, EventArgs e) {
                conversation.MoveNext();
            };

            yield return null;
        }

        //Quand tous les speakers ont fini de parler, on "éteint" le canvas
        canvas.gameObject.SetActive(false);
        if (completeEvent != null)
            completeEvent(this, EventArgs.Empty);
    }

    //Pour bouger le panel
    void MovePanel(ConversationPanel obj, string pos)
    {
        transition = obj.panel.SetPosition(pos, true);
        transition.easingControl.duration = 0.5f;
        transition.easingControl.equation = EasingEquations.EaseOutQuad;
    }
}
