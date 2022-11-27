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
}
