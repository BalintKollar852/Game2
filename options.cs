using System.Media;
public class ConfigBody
{
    private static bool fullscreenvalue;
    public bool fullscreen 
    {
        get { return fullscreenvalue; }
        set { fullscreenvalue = value; }
    }
    private static bool fpsvalue;
    public bool fps
    {
        get { return fpsvalue; }
        set { fpsvalue = value; }
    }
    private static bool crosshairvalue;
    public bool crosshair
    {
        get { return crosshairvalue; }
        set { crosshairvalue = value; }
    }
    private static float besttimevalue;
    public float besttime
    {
        get { return besttimevalue; }
        set { besttimevalue = value; }
    }
    private static int bestpointsvalue;
    public int bestpoints
    {
        get { return bestpointsvalue; }
        set { bestpointsvalue = value; }
    }
}
