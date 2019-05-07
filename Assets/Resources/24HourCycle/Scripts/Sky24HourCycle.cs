using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class Sky24HourCycle : MonoBehaviour
{
    public enum CloudType
    {
        None = 0,
        Xen = 1,
        Agile = 2,
        Both = 3
    }
    public enum LightDiskType
    {
        None = 0,
        Simple,
        HighQuality
    }

    // 控制时间循环
    [Header("Control Panel")]
    public Vector4 timeSlot = new Vector4(5, 12, 17, 21);
    public bool isUseCurve = true;
    public AnimationCurve cycleCurve;

    public Vector3 currentTime;
    public float deltaTime = 60.0f;
    public bool isCycle = false;
    public bool isPause = false;
    public bool isCameraRotate = false;

    [Header("Morning Settings")]
    [Header("Sky Morning Settings")]
    public Gradient skyMorningGradient;
    public Color agileMainCloudMorningColor;
    public Color agileBrightCloudMorningColor;
    public Color agileDarkCloudMorningColor;
    public Color agileSecondCloudMorningColor;
    public Color agileRimCloudMorningColor;
    public Color xenBrightCloudMorningColor;
    public Color xenDarkCloudMorningColor;

    [Header("Noon Settings")]
    public Gradient skyNoonGradient;
    public Color agileMainCloudNoonColor;
    public Color agileBrightCloudNoonColor;
    public Color agileDarkCloudNoonColor;
    public Color agileSecondCloudNoonColor;
    public Color agileRimCloudNoonColor;
    public Color xenBrightCloudNoonColor;
    public Color xenDarkCloudNoonColor;

    [Header("Dusk Settings")]
    public Gradient skyDuskGradient;
    public Color agileMainCloudDuskColor;
    public Color agileBrightCloudDuskColor;
    public Color agileDarkCloudDuskColor;
    public Color agileSecondCloudDuskColor;
    public Color agileRimCloudDuskColor;
    public Color xenBrightCloudDuskColor;
    public Color xenDarkCloudDuskColor;

    [Header("Night Settings")]
    public Gradient skyNightGradient;
    public Color agileMainCloudNightColor;
    public Color agileBrightCloudNightColor;
    public Color agileDarkCloudNightColor;
    public Color agileSecondCloudNightColor;
    public Color agileRimCloudNightColor;
    public Color xenBrightCloudNightColor;
    public Color xenDarkCloudNightColor;

    [Header("Sky Settings")]
    [Range(0.0f, 360.0f)]
    public float skyRotation = 0.0f;
    // skyMask目前主要用来控制调整月亮星星的亮度
    [Range(0.0f, 1.75f)]
    public float skyMaskIntenstiy = 0.5f;
    public Color groudColor = new Color(0.369f, 0.349f, 0.341f, 1);

    [Header("Sun Settings")]
    public LightDiskType sunDiskType = LightDiskType.None;
    [Range(0.0f, 1.0f)]
    public float sunSize = 0.04f;
    [Range(1.0f, 10.0f)]
    public float sunSizeConvergence = 5.0f;
    [Range(0.0f, 5.0f)]
    public float sunDiskIntensity = 1.0f;
    [Range(0.0f, 2.0f)]
    public float sunLightCloudIntensity = 0.3f;
    [Range(0.0f, 1.0f)]
    public float sunLightSkyIntensity = 0.2f;

    [Header("Moon Settings")]
    public LightDiskType moonDiskType = LightDiskType.None;
    [Range(0.0f, 1.0f)]
    public float moonSize = 0.088f;
    [Range(1.0f, 10.0f)]
    public float moonSizeConvergence = 6.57f;
    [Range(0.0f, 5.0f)]
    public float moonDiskIntensity = 0.6f;
    [Range(0.0f, 2.0f)]
    public float moonLightCloudIntensity = 0.78f;
    [Range(0.0f, 1.0f)]
    public float moonLightSkyIntensity = 0.036f;

    [Header("General Cloud Settings")]
    public CloudType cloudType = CloudType.Agile;

    [Header("Xen Cloud Settings")]
    [Range(0.0f, 1.0f)]
    public float proceduralCloudAltitude = 0.4f;
    [Range(-180.0f, 180.0f)]
    public float wispyCloudDirection = 0.0f;
    public Texture2D xenCloudMainNoiseTex;
    public Vector2 xenCloudMainNoiseTexScale = new Vector2(2.0f, 2.0f);
    public Texture2D xenCloudDetailTex;
    public Vector2 xenCloudDetailTexScale = new Vector2(2.0f, 2.0f);
    public Vector4 xenCloudMotionSpeed = new Vector4(1.0f, 1.0f, 1.0f, 1.0f);
    [Range(0.0f, 2.0f)]
    public float _XenCloudBrightColorCoe = 0.7f;
    [Range(0.0f, 2.0f)]
    public float xenCloudDarkColorCoe = 0.674f;
    [Range(0.0f, 2.0f)]
    public float xenCloudHDRCoe = 1.0f;
    [Range(0.0f, 1.0f)]
    public float xenCloudAlphaIntensity = 0.5f;
    [Range(-3.0f, 1.0f)]
    public float xenCloudAlphaThreshold = 0.55f;
    [Range(-5.0f, 5.0f)]
    public float xenCloudDetailIntensity = 1.6f;
    [Range(-2.0f, 2.0f)]
    public float xenCloudBlendIntensity = 0.75f;
    [Range(0.0f, 2.0f)]
    public float xenCloudBlendRange = 0.254f;
    [Range(0.0f, 1.75f)]
    public float xenCloudMaskIntensity = 0.5f;

    [Header("Agile Cloud Settings")]
    public Texture agileCloudMainTex;
    [Range(0.0f, 2.0f)]
    public float agileCloudRimColorCoe = 1.87f;
    [Range(0.0f, 1.0f)]
    public float agileCloudLuminance = 0.1f;
    [Range(0.0f, 10.0f)]
    public float agileCloudHeight = 1.0f;
    [Range(0.0f, 2.0f)]
    public float agileCloudPower = 0.2f;

    [Header("Star Settings")]
    [Range(0.0f, 2.0f)]
    public float starIntensity = 1.0f;

    // Record last settings value
    private bool lastIsCycle;
    private float currentTimeInSecond = 0.0f;

    private Color lastBottomColor0;
    private Color lastMiddleColor0;
    private Color lastTopColor0;
    private float lastMiddleTime0 = -1.0f;
    private float lastTopTime0 = -1.0f;
    private Color lastBottomColor1;
    private Color lastMiddleColor1;
    private Color lastTopColor1;
    private float lastMiddleTime1 = -1.0f;
    private float lastTopTime1 = -1.0f;
    private float lastSkyBlendLerpFactor = -1.0f;
    private Vector3 lastTime = new Vector3(-1, -1, -1);
    private bool lastShowStar = false;

    private Material skyboxMat;

    private Vector3 sunRiseDir = new Vector3(-1.0f, 0.0f, 0.0f);
    private Vector3 sunHighDir = new Vector3(0.0f, 1.0f, 0.0f);
    private Vector3 sunSetDir = new Vector3(1.0f, 0.0f, 0.0f);

    private GameObject testCamTarget = null;

    // Start is called before the first frame update
    void Start()
    {
        lastIsCycle = !isCycle;
        skyboxMat = RenderSettings.skybox;
        skyboxMat.SetVector("_SunDir", sunRiseDir);
        sunRiseDir = Vector3.Normalize(sunRiseDir);
        sunHighDir = Vector3.Normalize(sunHighDir);
        sunSetDir = Vector3.Normalize(sunSetDir);
        // To Test
        testCamTarget = GameObject.Find("TestCamTarget");
        testCamTarget.transform.position = Camera.main.transform.position;
    }

    void setGradient(Gradient gradient0, Gradient gradient1, float skyBlendLerpFactor)
    {
        GradientColorKey[] colorKey0 = gradient0.colorKeys;
        GradientColorKey[] colorKey1 = gradient1.colorKeys;

        // unity在设置材质参数的时候会另外复制拷贝一份， 所以检查材质参数如果未变则不再设置
        if (colorKey0.Length == 3 && colorKey1.Length == 3)
        {
            skyboxMat.EnableKeyword("_MIDDLEPOINT_ON");
            List<Color> colorList = new List<Color>();
            List<float> timeList = new List<float>();
            //if (!lastBottomColor0.Equals(colorKey0[0].color) || 
            //    !lastBottomColor1.Equals(colorKey1[0].color))
            {
                colorList.Add(colorKey0[0].color);
                colorList.Add(colorKey1[0].color);
                skyboxMat.SetColorArray("_BottomColor", colorList);
                lastBottomColor0 = colorKey0[0].color;
                lastBottomColor1 = colorKey1[0].color;
            }
            //if (!lastMiddleColor0.Equals(colorKey0[0].color) ||
            //    !lastMiddleColor1.Equals(colorKey1[0].color))
            {
                colorList.Clear();
                colorList.Add(colorKey0[1].color);
                colorList.Add(colorKey1[1].color);
                skyboxMat.SetColorArray("_MiddleColor", colorList);
                lastMiddleColor0 = colorKey0[1].color;
                lastMiddleColor1 = colorKey1[1].color;
            }
            //if (!lastTopColor0.Equals(colorKey0[0].color) ||
            //    !lastTopColor1.Equals(colorKey1[0].color))
            {
                colorList.Clear();
                colorList.Add(colorKey0[2].color);
                colorList.Add(colorKey1[2].color);
                skyboxMat.SetColorArray("_TopColor", colorList);
                lastTopColor0 = colorKey0[2].color;
                lastTopColor1 = colorKey1[2].color;
            }
            //if (!lastMiddleTime0.Equals(colorKey0[1].time) ||
            //    !lastMiddleTime1.Equals(colorKey1[1].time))
            {
                timeList.Add(colorKey0[1].time);
                timeList.Add(colorKey1[1].time);
                skyboxMat.SetFloatArray("_MiddleTime", timeList);
                lastMiddleTime0 = colorKey0[1].time;
                lastMiddleTime1 = colorKey1[1].time;
            }
            //if (!lastTopTime0.Equals(colorKey0[2].time) ||
            //    !lastTopTime1.Equals(colorKey1[2].time))
            {
                timeList.Clear();
                timeList.Add(colorKey0[2].time);
                timeList.Add(colorKey1[2].time);
                skyboxMat.SetFloatArray("_TopTime", timeList);
                lastTopTime0 = colorKey0[2].time;
                lastTopTime1 = colorKey1[2].time;
            }
        }
        else
        {
            skyboxMat.DisableKeyword("_MIDDLEPOINT_ON");
            List<Color> colorList = new List<Color>();
            List<float> timeList = new List<float>();
            //if (!lastBottomColor0.Equals(colorKey0[0].color) ||
            //    !lastBottomColor1.Equals(colorKey1[0].color))
            {
                colorList.Add(colorKey0[0].color);
                colorList.Add(colorKey1[0].color);
                skyboxMat.SetColorArray("_BottomColor", colorList);
                lastBottomColor0 = colorKey0[0].color;
                lastBottomColor1 = colorKey1[0].color;
            }
            //if (!lastTopColor0.Equals(colorKey0[0].color) ||
            //    !lastTopColor1.Equals(colorKey1[0].color))
            {
                colorList.Add(colorKey0[1].color);
                colorList.Add(colorKey1[1].color);
                skyboxMat.SetColorArray("_TopColor", colorList);
                lastTopColor0 = colorKey0[0].color;
                lastTopColor1 = colorKey1[0].color;
            }
        }
        skyboxMat.SetFloat("_SkyBlendLerpFactor", skyBlendLerpFactor);
    }

    void setSkyShifting()
    {
        Gradient gradient0;
        Gradient gradient1;
        float skyBlendLerpFactor = 0.0f;
        if (currentTime.x >= timeSlot.x && currentTime.x < timeSlot.y)
        {
            gradient0 = skyMorningGradient;
            gradient1 = skyNoonGradient;
            skyBlendLerpFactor = (currentTime.x + currentTime.y / 60.0f - timeSlot.x) / (timeSlot.y - timeSlot.x);
        }
        else if (currentTime.x >= timeSlot.y && currentTime.x < timeSlot.z)
        {
            gradient0 = skyNoonGradient;
            gradient1 = skyDuskGradient;
            skyBlendLerpFactor = (currentTime.x + currentTime.y / 60.0f - timeSlot.y) / (timeSlot.z - timeSlot.y);
        }
        else if (currentTime.x >= timeSlot.z && currentTime.x < timeSlot.w)
        {
            gradient0 = skyDuskGradient;
            gradient1 = skyNightGradient;
            skyBlendLerpFactor = (currentTime.x + currentTime.y / 60.0f - timeSlot.z) / (timeSlot.w - timeSlot.z);
        }
        else
        {
            gradient0 = skyNightGradient;
            gradient1 = skyMorningGradient;
            if (currentTime.x >= timeSlot.w)
            {
                skyBlendLerpFactor = (currentTime.x + currentTime.y / 60.0f - timeSlot.w) / (timeSlot.x + 24 - timeSlot.w);
            }
            else
            {
                skyBlendLerpFactor = (currentTime.x + currentTime.y / 60.0f + 24 - timeSlot.w) / (timeSlot.x + 24 - timeSlot.w);
            }
        }

        if(isUseCurve)
        {
            float time = currentTime.x + currentTime.y / 60.0f;
            skyBlendLerpFactor = cycleCurve.Evaluate(time);
        }

        setGradient(gradient0, gradient1, skyBlendLerpFactor);
    }

    void setSunShifting()
    {
        Vector3 sunDir = new Vector3(0.0f, 0.0f, 0.0f);
        float sunDirLerpFactor = 0.0f;
        if (currentTime.x >= timeSlot.x && currentTime.x < timeSlot.y)
        {
            sunDirLerpFactor = (currentTime.x + currentTime.y / 60.0f - timeSlot.x) / (timeSlot.y - timeSlot.x);
            sunDir = Vector3.Slerp(sunRiseDir, sunHighDir, sunDirLerpFactor);
        }
        else if (currentTime.x >= timeSlot.y && currentTime.x < timeSlot.z)
        {
            sunDirLerpFactor = (currentTime.x + currentTime.y / 60.0f - timeSlot.y) / (timeSlot.z - timeSlot.y);
            sunDir = Vector3.Slerp(sunHighDir, sunSetDir, sunDirLerpFactor);
        }
        else if (currentTime.x >= timeSlot.z && currentTime.x < timeSlot.w)
        {
            sunDirLerpFactor = (currentTime.x + currentTime.y / 60.0f - timeSlot.z) / (timeSlot.w - timeSlot.z);
            sunDir = Vector3.Slerp(sunSetDir, -sunHighDir, sunDirLerpFactor);
        }
        else
        {
            if (currentTime.x >= timeSlot.w)
            {
                sunDirLerpFactor = (currentTime.x + currentTime.y / 60.0f - timeSlot.w) / (timeSlot.x + 24 - timeSlot.w);
            }
            else
            {
                sunDirLerpFactor = (currentTime.x + currentTime.y / 60.0f + 24 - timeSlot.w) / (timeSlot.x + 24 - timeSlot.w);
            }
            sunDir = Vector3.Slerp(-sunHighDir, sunRiseDir, sunDirLerpFactor);
        }

        sunDir = Vector3.Normalize(sunDir);
        skyboxMat.SetVector("_SunDir", sunDir);

        // Test
        if (testCamTarget && isCameraRotate && currentTime.x >= timeSlot.x && currentTime.x < timeSlot.z)
        {
            Camera mainCamera = Camera.main;
            testCamTarget.transform.position = mainCamera.transform.position + sunDir * 1000f;
            mainCamera.transform.LookAt(testCamTarget.transform, Vector3.Cross(sunDir, Vector3.forward));
        }
    }

    void setMoonShifting()
    {
        Vector3 moonDir = new Vector3(0.0f, 0.0f, 0.0f);
        float moonDirLerpFactor = 0.0f;
        if (currentTime.x >= timeSlot.x && currentTime.x < timeSlot.y)
        {
            moonDirLerpFactor = (currentTime.x + currentTime.y / 60.0f - timeSlot.x) / (timeSlot.y - timeSlot.x);
            moonDir = Vector3.Slerp(sunRiseDir, -sunHighDir, moonDirLerpFactor);
        }
        else if (currentTime.x >= timeSlot.y && currentTime.x < timeSlot.z)
        {
            moonDirLerpFactor = (currentTime.x + currentTime.y / 60.0f - timeSlot.y) / (timeSlot.z - timeSlot.y);
            moonDir = Vector3.Slerp(-sunHighDir, sunSetDir, moonDirLerpFactor);
        }
        else if (currentTime.x >= timeSlot.z && currentTime.x < timeSlot.w)
        {
            moonDirLerpFactor = (currentTime.x + currentTime.y / 60.0f - timeSlot.z) / (timeSlot.w - timeSlot.z);
            moonDir = Vector3.Slerp(sunSetDir, sunHighDir, moonDirLerpFactor);
        }
        else
        {
            if (currentTime.x >= timeSlot.w)
            {
                moonDirLerpFactor = (currentTime.x + currentTime.y / 60.0f - timeSlot.w) / (timeSlot.x + 24 - timeSlot.w);
            }
            else
            {
                moonDirLerpFactor = (currentTime.x + currentTime.y / 60.0f + 24 - timeSlot.w) / (timeSlot.x + 24 - timeSlot.w);
            }
            moonDir = Vector3.Slerp(sunHighDir, sunRiseDir, moonDirLerpFactor);
        }

        moonDir = Vector3.Normalize(moonDir);
        skyboxMat.SetVector("_MoonDir", moonDir);

        // Test
        if (testCamTarget && isCameraRotate && (currentTime.x >= timeSlot.z || currentTime.x < timeSlot.x))
        {
            Camera mainCamera = Camera.main;
            testCamTarget.transform.position = mainCamera.transform.position + moonDir * 1000f;
            mainCamera.transform.LookAt(testCamTarget.transform, Vector3.Cross(moonDir, Vector3.forward));
        }
    }

    void setStarShifting()
    {
        if (currentTime.x >= timeSlot.z || currentTime.x < timeSlot.x)
        {
            if (lastShowStar == false)
            {
                skyboxMat.EnableKeyword("_STAR_ON");
                lastShowStar = true;
            }
        }
        else
        {
            if (lastShowStar == true)
            {
                skyboxMat.DisableKeyword("_STAR_ON");
                lastShowStar = false;
            }
        }
    }

    void setCloudShifting()
    {
        if (cloudType == CloudType.Agile || cloudType == CloudType.Both)
        {
            float cloudLuminance0 = 0.0f;
            float cloudLuminance1 = 1.0f;
            float morningCloudLuminance = 0.25f;
            float noonCloudLuminance = 1.0f;
            float duskCloudLuminance = 0.25f;
            float nightCloudLuminance = 0.0f;
            float cloudLuminance = 0.0f;
            float cloudLuminanceBlendLerpFactor = 0.0f;
            Color mainCloudColor, mainCloudColor0, mainCloudColor1;
            Color brightCloudColor, brightCloudColor0, brightCloudColor1;
            Color darkCloudColor, darkCloudColor0, darkCloudColor1;
            Color secondCloudColor, secondCloudColor0, secondCloudColor1;
            Color rimCloudColor, rimCloudColor0, rimCloudColor1;
            if (currentTime.x >= timeSlot.x && currentTime.x < timeSlot.y)
            {
                cloudLuminance0 = morningCloudLuminance;
                cloudLuminance1 = noonCloudLuminance;
                mainCloudColor0 = agileMainCloudMorningColor;
                mainCloudColor1 = agileMainCloudNoonColor;
                brightCloudColor0 = agileBrightCloudMorningColor;
                brightCloudColor1 = agileBrightCloudNoonColor;
                darkCloudColor0 = agileDarkCloudMorningColor;
                darkCloudColor1 = agileDarkCloudNoonColor;
                secondCloudColor0 = agileSecondCloudMorningColor;
                secondCloudColor1 = agileSecondCloudNoonColor;
                rimCloudColor0 = agileRimCloudMorningColor;
                rimCloudColor1 = agileRimCloudNoonColor;
                cloudLuminanceBlendLerpFactor = (currentTime.x + currentTime.y / 60.0f - timeSlot.x) / (timeSlot.y - timeSlot.x);
            }
            else if (currentTime.x >= timeSlot.y && currentTime.x < timeSlot.z)
            {
                cloudLuminance0 = noonCloudLuminance;
                cloudLuminance1 = duskCloudLuminance;
                mainCloudColor0 = agileMainCloudNoonColor;
                mainCloudColor1 = agileMainCloudDuskColor;
                brightCloudColor0 = agileBrightCloudNoonColor;
                brightCloudColor1 = agileBrightCloudDuskColor;
                darkCloudColor0 = agileDarkCloudNoonColor;
                darkCloudColor1 = agileDarkCloudDuskColor;
                secondCloudColor0 = agileSecondCloudNoonColor;
                secondCloudColor1 = agileSecondCloudDuskColor;
                rimCloudColor0 = agileRimCloudNoonColor;
                rimCloudColor1 = agileRimCloudDuskColor;
                cloudLuminanceBlendLerpFactor = (currentTime.x + currentTime.y / 60.0f - timeSlot.y) / (timeSlot.z - timeSlot.y);
            }
            else if (currentTime.x >= timeSlot.z && currentTime.x < timeSlot.w)
            {
                cloudLuminance0 = duskCloudLuminance;
                cloudLuminance1 = nightCloudLuminance;
                mainCloudColor0 = agileMainCloudDuskColor;
                mainCloudColor1 = agileMainCloudNightColor;
                brightCloudColor0 = agileBrightCloudDuskColor;
                brightCloudColor1 = agileBrightCloudNightColor;
                darkCloudColor0 = agileDarkCloudDuskColor;
                darkCloudColor1 = agileDarkCloudNightColor;
                secondCloudColor0 = agileSecondCloudDuskColor;
                secondCloudColor1 = agileSecondCloudNightColor;
                rimCloudColor0 = agileRimCloudDuskColor;
                rimCloudColor1 = agileRimCloudNightColor;
                cloudLuminanceBlendLerpFactor = (currentTime.x + currentTime.y / 60.0f - timeSlot.z) / (timeSlot.w - timeSlot.z);
            }
            else
            {
                cloudLuminance0 = nightCloudLuminance;
                cloudLuminance1 = morningCloudLuminance;
                mainCloudColor0 = agileMainCloudNightColor;
                mainCloudColor1 = agileMainCloudMorningColor;
                brightCloudColor0 = agileBrightCloudNightColor;
                brightCloudColor1 = agileBrightCloudMorningColor;
                darkCloudColor0 = agileDarkCloudNightColor;
                darkCloudColor1 = agileDarkCloudMorningColor;
                secondCloudColor0 = agileSecondCloudNightColor;
                secondCloudColor1 = agileSecondCloudMorningColor;
                rimCloudColor0 = agileRimCloudNightColor;
                rimCloudColor1 = agileRimCloudMorningColor;

                if (currentTime.x >= timeSlot.w)
                {
                    cloudLuminanceBlendLerpFactor = (currentTime.x + currentTime.y / 60.0f - timeSlot.w) / (timeSlot.x + 24 - timeSlot.w);
                }
                else
                {
                    cloudLuminanceBlendLerpFactor = (currentTime.x + currentTime.y / 60.0f + 24 - timeSlot.w) / (timeSlot.x + 24 - timeSlot.w);
                }
            }
            if (isUseCurve)
            {
                float time = currentTime.x + currentTime.y / 60.0f;
                cloudLuminanceBlendLerpFactor = cycleCurve.Evaluate(time);
            }
            cloudLuminance = cloudLuminance0 * (1.0f - cloudLuminanceBlendLerpFactor) + cloudLuminance1 * cloudLuminanceBlendLerpFactor;
            skyboxMat.SetFloat("_AgileCloudLuminance", cloudLuminance);
            mainCloudColor = mainCloudColor0 * (1.0f - cloudLuminanceBlendLerpFactor) + mainCloudColor1 * cloudLuminanceBlendLerpFactor;
            brightCloudColor = brightCloudColor0 * (1.0f - cloudLuminanceBlendLerpFactor) + brightCloudColor1 * cloudLuminanceBlendLerpFactor;
            darkCloudColor = darkCloudColor0 * (1.0f - cloudLuminanceBlendLerpFactor) + darkCloudColor1 * cloudLuminanceBlendLerpFactor;
            secondCloudColor = secondCloudColor0 * (1.0f - cloudLuminanceBlendLerpFactor) + secondCloudColor1 * cloudLuminanceBlendLerpFactor;
            rimCloudColor = rimCloudColor0 * (1.0f - cloudLuminanceBlendLerpFactor) + rimCloudColor1 * cloudLuminanceBlendLerpFactor;
            skyboxMat.SetColor("_AgileCloudMainColor", mainCloudColor);
            skyboxMat.SetColor("_AgileCloudBrightColor", brightCloudColor);
            skyboxMat.SetColor("_AgileCloudDarkColor", darkCloudColor);
            skyboxMat.SetColor("_AgileCloudSecondColor", secondCloudColor);
            skyboxMat.SetColor("_AgileCloudRimColor", rimCloudColor);
        }
        if(cloudType == CloudType.Xen || cloudType == CloudType.Both)
        {
            float cloudLuminanceBlendLerpFactor = 0.0f;
            Color brightCloudColor, brightCloudColor0, brightCloudColor1;
            Color darkCloudColor, darkCloudColor0, darkCloudColor1;
            if (currentTime.x >= timeSlot.x && currentTime.x < timeSlot.y)
            {
                brightCloudColor0 = xenBrightCloudMorningColor;
                brightCloudColor1 = xenBrightCloudNoonColor;
                darkCloudColor0 = xenDarkCloudMorningColor;
                darkCloudColor1 = xenDarkCloudNoonColor;
                cloudLuminanceBlendLerpFactor = (currentTime.x + currentTime.y / 60.0f - timeSlot.x) / (timeSlot.y - timeSlot.x);
            }
            else if (currentTime.x >= timeSlot.y && currentTime.x < timeSlot.z)
            {
                brightCloudColor0 = xenBrightCloudNoonColor;
                brightCloudColor1 = xenBrightCloudDuskColor;
                darkCloudColor0 = xenDarkCloudNoonColor;
                darkCloudColor1 = xenDarkCloudDuskColor;
                cloudLuminanceBlendLerpFactor = (currentTime.x + currentTime.y / 60.0f - timeSlot.y) / (timeSlot.z - timeSlot.y);
            }
            else if (currentTime.x >= timeSlot.z && currentTime.x < timeSlot.w)
            {
                brightCloudColor0 = xenBrightCloudDuskColor;
                brightCloudColor1 = xenBrightCloudNightColor;
                darkCloudColor0 = xenDarkCloudDuskColor;
                darkCloudColor1 = xenDarkCloudNightColor;
                cloudLuminanceBlendLerpFactor = (currentTime.x + currentTime.y / 60.0f - timeSlot.z) / (timeSlot.w - timeSlot.z);
            }
            else
            {
                brightCloudColor0 = xenBrightCloudNightColor;
                brightCloudColor1 = xenBrightCloudMorningColor;
                darkCloudColor0 = xenDarkCloudNightColor;
                darkCloudColor1 = xenDarkCloudMorningColor;

                if (currentTime.x >= timeSlot.w)
                {
                    cloudLuminanceBlendLerpFactor = (currentTime.x + currentTime.y / 60.0f - timeSlot.w) / (timeSlot.x + 24 - timeSlot.w);
                }
                else
                {
                    cloudLuminanceBlendLerpFactor = (currentTime.x + currentTime.y / 60.0f + 24 - timeSlot.w) / (timeSlot.x + 24 - timeSlot.w);
                }
            }
            if (isUseCurve)
            {
                float time = currentTime.x + currentTime.y / 60.0f;
                cloudLuminanceBlendLerpFactor = cycleCurve.Evaluate(time);
            }
            brightCloudColor = brightCloudColor0 * (1.0f - cloudLuminanceBlendLerpFactor) + brightCloudColor1 * cloudLuminanceBlendLerpFactor;
            darkCloudColor = darkCloudColor0 * (1.0f - cloudLuminanceBlendLerpFactor) + darkCloudColor1 * cloudLuminanceBlendLerpFactor;
            skyboxMat.SetColor("_XenCloudBrightColor", brightCloudColor);
            skyboxMat.SetColor("_XenCloudDarkColor", darkCloudColor);
        }
    }

    // Update is called once per frame
    void Update()
    {
        // 勾选循环或者取消勾选循环的时候的逻辑
        if(lastIsCycle != isCycle)
        {
            if(isCycle)
            {
                currentTimeInSecond = 0;
                currentTime.x = 0;
                currentTime.y = 0;
                currentTime.z = 0;
            }
            lastIsCycle = isCycle;
        }
        if(isCycle)
        {
            System.TimeSpan t = System.TimeSpan.FromSeconds(currentTimeInSecond);
            currentTime.x = t.Hours;
            currentTime.y = t.Minutes;
            currentTime.z = t.Seconds;
        }

        // 根据当前时间设置变幻
        //if ((lastTime.x != currentTime.x) || (lastTime.y != currentTime.y))
        {
            // 天空
            setSkyShifting();

            // 太阳
            setSunShifting();

            // 月亮
            setMoonShifting();

            // 星星
            setStarShifting();

            // 云
            setCloudShifting();

            lastTime = currentTime;
        }

        updateSkySettings();

        updateSunSettings();

        updateMoonSettings();

        updateCloudSettings();

        updateStarSettings();

        // time elapse
        if (isCycle && !isPause)
        {
            currentTimeInSecond += deltaTime;
        }
    }

    void updateSkySettings()
    {
        skyboxMat.SetFloat("_SkyRotation", skyRotation);
        skyboxMat.SetFloat("_SkyMaskIntensity", skyMaskIntenstiy);
        skyboxMat.SetColor("_GroundColor", groudColor);
    }

    void updateSunSettings()
    {
        if(sunDiskType == LightDiskType.None)
        {
            skyboxMat.EnableKeyword("_SUNDISK_NONE");
            skyboxMat.DisableKeyword("_SUNDISK_SIMPLE");
            skyboxMat.DisableKeyword("_SUNDISK_HIGH_QUALITY");
        }
        else if(sunDiskType == LightDiskType.Simple)
        {
            skyboxMat.DisableKeyword("_SUNDISK_NONE");
            skyboxMat.EnableKeyword("_SUNDISK_SIMPLE");
            skyboxMat.DisableKeyword("_SUNDISK_HIGH_QUALITY");
        }
        else
        {
            skyboxMat.DisableKeyword("_SUNDISK_NONE");
            skyboxMat.DisableKeyword("_SUNDISK_SIMPLE");
            skyboxMat.EnableKeyword("_SUNDISK_HIGH_QUALITY");
        }

        skyboxMat.SetFloat("_SunSize", sunSize);
        skyboxMat.SetFloat("_SunSizeConvergence", sunSizeConvergence);
        skyboxMat.SetFloat("_SunDiskIntensity", sunDiskIntensity);
        skyboxMat.SetFloat("_SunLightCloudIntensity", sunLightCloudIntensity);
        skyboxMat.SetFloat("_SunLightSkyIntensity", sunLightSkyIntensity);
    }

    void updateMoonSettings()
    {
        if (moonDiskType == LightDiskType.None)
        {
            skyboxMat.EnableKeyword("_MOONDISK_NONE");
            skyboxMat.DisableKeyword("_MOONDISK_SIMPLE");
            skyboxMat.DisableKeyword("_MOONDISK_HIGH_QUALITY");
        }
        else if (moonDiskType == LightDiskType.Simple)
        {
            skyboxMat.DisableKeyword("_MOONDISK_NONE");
            skyboxMat.EnableKeyword("_MOONDISK_SIMPLE");
            skyboxMat.DisableKeyword("_MOONDISK_HIGH_QUALITY");
        }
        else
        {
            skyboxMat.DisableKeyword("_MOONDISK_NONE");
            skyboxMat.DisableKeyword("_MOONDISK_SIMPLE");
            skyboxMat.EnableKeyword("_MOONDISK_HIGH_QUALITY");
        }

        skyboxMat.SetFloat("_MoonSize", moonSize);
        skyboxMat.SetFloat("_MoonSizeConvergence", moonSizeConvergence);
        skyboxMat.SetFloat("_MoonDiskIntensity", moonDiskIntensity);
        skyboxMat.SetFloat("_MoonLightCloudIntensity", moonLightCloudIntensity);
        skyboxMat.SetFloat("_MoonLightSkyIntensity", moonLightSkyIntensity);
    }

    void updateCloudSettings()
    {
        if (cloudType == CloudType.None)
        {
            skyboxMat.DisableKeyword("_AGILECLOUD_ON");
            skyboxMat.DisableKeyword("_XENCLOUD_ON");
            return;
        }
        else if (cloudType == CloudType.Agile)
        {
            skyboxMat.EnableKeyword("_AGILECLOUD_ON");
            skyboxMat.DisableKeyword("_XENCLOUD_ON");

            skyboxMat.SetTexture("_AgileCloudMainTex", agileCloudMainTex);
            skyboxMat.SetFloat("_AgileCloudRimColorCoe", agileCloudRimColorCoe);
            // 在shift循环中设置的
            //skyboxMat.SetFloat("_AgileCloudRimColorCoe", agileCloudLuminance);
            skyboxMat.SetFloat("_AgileCloudHeight", agileCloudHeight);
            skyboxMat.SetFloat("_AgileCloudPower", agileCloudPower);
        }
        else if (cloudType == CloudType.Xen)
        {
            skyboxMat.DisableKeyword("_AGILECLOUD_ON");
            skyboxMat.EnableKeyword("_XENCLOUD_ON");

            skyboxMat.SetTexture("_XenCloudMainNoiseTex", xenCloudMainNoiseTex);
            skyboxMat.SetTextureScale("_XenCloudMainNoiseTex", xenCloudMainNoiseTexScale);
            skyboxMat.SetTexture("_XenCloudDetailTex", xenCloudDetailTex);
            skyboxMat.SetTextureScale("_XenCloudDetailTex", xenCloudDetailTexScale);
            skyboxMat.SetFloat("_ProceduralCloudAltitude", proceduralCloudAltitude);
            skyboxMat.SetFloat("_WispyCloudDirection", wispyCloudDirection);
            skyboxMat.SetFloat("_XenCloudBrightColorCoe", _XenCloudBrightColorCoe);
            skyboxMat.SetFloat("_XenCloudDarkColorCoe", xenCloudDarkColorCoe);
            skyboxMat.SetFloat("_XenCloudHDRCoe", xenCloudHDRCoe);
            skyboxMat.SetFloat("_XenCloudAlphaIntensity", xenCloudAlphaIntensity);
            skyboxMat.SetFloat("_XenCloudAlphaThreshold", xenCloudAlphaThreshold);
            skyboxMat.SetFloat("_XenCloudDetailIntensity", xenCloudDetailIntensity);
            skyboxMat.SetFloat("_XenCloudBlendIntensity", xenCloudBlendIntensity);
            skyboxMat.SetFloat("_XenCloudBlendRange", xenCloudBlendRange);
            skyboxMat.SetFloat("_XenCloudMaskIntensity", xenCloudMaskIntensity);
            skyboxMat.SetVector("_XenCloudMotionSpeed", xenCloudMotionSpeed);
        }
        else // Both
        {
            skyboxMat.EnableKeyword("_AGILECLOUD_ON");
            skyboxMat.EnableKeyword("_XENCLOUD_ON");

            skyboxMat.SetTexture("_AgileCloudMainTex", agileCloudMainTex);
            skyboxMat.SetFloat("_AgileCloudRimColorCoe", agileCloudRimColorCoe);
            // 在shift循环中设置的
            //skyboxMat.SetFloat("_AgileCloudRimColorCoe", agileCloudLuminance);
            skyboxMat.SetFloat("_AgileCloudHeight", agileCloudHeight);
            skyboxMat.SetFloat("_AgileCloudPower", agileCloudPower);

            skyboxMat.SetTexture("_XenCloudMainNoiseTex", xenCloudMainNoiseTex);
            skyboxMat.SetTextureScale("_XenCloudMainNoiseTex", xenCloudMainNoiseTexScale);
            skyboxMat.SetTexture("_XenCloudDetailTex", xenCloudDetailTex);
            skyboxMat.SetTextureScale("_XenCloudDetailTex", xenCloudDetailTexScale);
            skyboxMat.SetFloat("_ProceduralCloudAltitude", proceduralCloudAltitude);
            skyboxMat.SetFloat("_WispyCloudDirection", wispyCloudDirection);
            skyboxMat.SetFloat("_XenCloudBrightColorCoe", _XenCloudBrightColorCoe);
            skyboxMat.SetFloat("_XenCloudDarkColorCoe", xenCloudDarkColorCoe);
            skyboxMat.SetFloat("_XenCloudHDRCoe", xenCloudHDRCoe);
            skyboxMat.SetFloat("_XenCloudAlphaIntensity", xenCloudAlphaIntensity);
            skyboxMat.SetFloat("_XenCloudAlphaThreshold", xenCloudAlphaThreshold);
            skyboxMat.SetFloat("_XenCloudDetailIntensity", xenCloudDetailIntensity);
            skyboxMat.SetFloat("_XenCloudBlendIntensity", xenCloudBlendIntensity);
            skyboxMat.SetFloat("_XenCloudBlendRange", xenCloudBlendRange);
            skyboxMat.SetFloat("_XenCloudMaskIntensity", xenCloudMaskIntensity);
            skyboxMat.SetVector("_XenCloudMotionSpeed", xenCloudMotionSpeed);
        }
    }

    void updateStarSettings()
    {
        skyboxMat.SetFloat("_StarIntensity", starIntensity);
    }
}
