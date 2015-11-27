using System.Collections;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Serialization;

public class Level {

    [XmlAttribute("lastLevel")]
    public int lastLevel;

    public List<Poging> pogingen;


    public Level(int lastLevel)
    {
        this.lastLevel = lastLevel;
        pogingen = new List<Poging>();
    }

    public Level()
    {
        pogingen = new List<Poging>();
    }
    
    public void AddPoging(float msAtEnd, float vKStart, float dKStart, float mStart, float score, string Reason)
    {
        pogingen.Add(new Poging(msAtEnd, vKStart, dKStart, mStart, score, Reason));
    }
	
}
