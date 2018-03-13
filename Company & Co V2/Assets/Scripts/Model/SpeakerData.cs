using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]

public class SpeakerData
{
    public List<string> messages; //Contient les "pages" de texte
    public Sprite speaker; //Ref au sprite de la personne en train de parler
    public TextAnchor anchor; //Pour afficher le texte dans un écran du panel
}
