using System.Collections;
using System.Xml;
using System.Xml.Serialization;

public class Poging{

    public float msAtEnd;
    public float vKrachtStart;
    public float dKrachtStart;
    public float massaStart;
    public float Score;
    public string Reason;

    public Poging(float msAtEnd, float vKStart, float dKStart, float mStart, float score, string Reason)
    {
        this.msAtEnd = msAtEnd;
        this.vKrachtStart = vKStart;
        this.dKrachtStart = dKStart;
        this.massaStart = mStart;
        this.Score = score;
        this.Reason = Reason;
    }

    public Poging()
    {

    }
}
