
public class DeviceInfo {

#if UNITY_EDITOR
    public static readonly bool IsMobile = false;
#elif UNITY_ANDROID
    public static readonly bool IsMobile = true;
#elif UNITY_STANDALONE_WIN
    public static readonly bool IsMobile = false;
#else
    public static readonly bool IsMobile = false;
#endif

}
