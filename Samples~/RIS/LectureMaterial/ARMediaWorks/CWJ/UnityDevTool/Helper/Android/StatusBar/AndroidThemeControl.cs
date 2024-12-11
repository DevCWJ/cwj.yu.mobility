
/// <summary>
/// Hide Navigation Bar는 게임일때만/ Render outside safe area 끄는거 국룰
/// </summary>
public static class AndroidThemeControl
{

    public static void SetStatusBarEnabled(ApplicationChrome.States state)
    {
        ApplicationChrome.statusBarState = state;
    }

    public static void SetStatusBarColor(uint _colorValue)
    {
        ApplicationChrome.statusBarColor = _colorValue;
    }
    
    public static void SetNavigationBarEnabled(ApplicationChrome.States state)
    {
        ApplicationChrome.navigationBarState = state;
    }

    public static void SetNavigationBarColor(uint _colorValue)
    {
        ApplicationChrome.navigationBarColor = _colorValue;
    }
}
