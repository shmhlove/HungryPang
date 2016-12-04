/*
 * Application.LogCallback does work, and will be called when exceptions occur.
 * Perhaps surprisingly, floating point divide by zero does not throw an exception.
 * This is part of the .NET floating point specification, and it makes sense, since floating point numbers have a representation for infinity.
 * Integer divide by zero does throw an exception, as there is no way to represent infinity with an integer.
 * While Unity will occasionally catch a stack overflow exception, you can't rely on this -- usually the Unity editor just dies.
 * Note that the script generates a log file in your Assets folder. You may need to reimport this asset after running in order to see the latest changes from in the Unity editor.
*/

using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net;
using System.Net.Mail;    
using System.Net.Mime; 
using System.Net.Security;
using System.ComponentModel;
using System.Text.RegularExpressions;
using System.Security.Cryptography.X509Certificates;

public enum eCrashWriteType
{
    EWRITEOFF = 0,
    EWRITEFILE,
    EWRITEMAIL,
    EWRITESERVER,
};

[Serializable]
public class S2CrashReporter
{
//    public StreamWriter m_Writer;
//    int m_ExceptionCount = 0;

//    public eCrashWriteType m_WriteMode = eCrashWriteType.EWRITEMAIL;

//    public string projectName;
//    public string mailingList;
//    int m_MaxLogCount = 10;
//    List<string> m_logBuffer = new List<string>();
//    string postURL = "";

//    public void Start()
//    {
//        if (m_WriteMode == eCrashWriteType.EWRITEOFF)
//            return;

//        Application.RegisterLogCallback(HandleException);
//    }

//    IEnumerator SendDebugToServer()
//    {
//        WWW www = new WWW(postURL, System.Text.ASCIIEncoding.ASCII.GetBytes(MakeMassageHeader(BufferToText())));
//        yield return www;

//        FinalWorking();
//    }

//    IEnumerator SendDebugToFile()
//    {
//#if UNITY_ANDROID || UNITY_IPHONE
//        string attachmentPath = "exception.png";
//#else
//        string attachmentPath = Application.dataPath + "/exception.png";
//#endif
//        if (File.Exists(attachmentPath))
//            File.Delete(attachmentPath);

//        Single.Coroutine.Routine(() =>
//        {
//#if UNITY_ANDROID || UNITY_IPHONE
//            m_Writer = new StreamWriter(Path.Combine(Application.persistentDataPath, "unityexceptions.txt"));
//#else
//            m_Writer = new StreamWriter(Path.Combine(Application.dataPath, "unityexceptions.txt"));
//#endif
//            m_Writer.AutoFlush = true;

//            m_Writer.WriteLine(MakeMassageHeader(BufferToText()));
//            m_Writer.Close();

//            FinalWorking();

//        }, ScreenShot(attachmentPath));

//        yield return null;
//    }

//    IEnumerator ScreenShot(string attachmentPath)
//    {
//        Application.CaptureScreenshot(attachmentPath);

//        yield return new WaitForSeconds(1);

//        FileInfo info = new FileInfo(attachmentPath);

//        Debug.Log(info.ToString());

//    }

//    IEnumerator SendDebugToMail(LogType type)
//    {
//        MailMessage mail = new MailMessage();

//        mail.From = new MailAddress("CrashReporter");

//        if (mailingList.Length > 0 && mailingList.IndexOf(";") != -1)
//        {
//            string[] sliceList = mailingList.Split(';');
//            for (int i = 0; i < sliceList.Length; i++)
//            {
//                mail.To.Add(sliceList[i]);
//                Debug.Log(sliceList[i]);
//            }
//        }
//        else if (mailingList.Length > 0)
//        {
//            mail.To.Add(mailingList);
//        }
//        else
//        {
//            yield return null;
//        }
//        mail.Subject = "[" + projectName + " CrashReport - " + type.ToString() + " #" + VersionController.VersionText + " ] " + S2ApiServer.Instance.TeamName + " #" + DateTime.Now;

//        mail.Body = MakeMassageHeader(BufferToText());

//#if UNITY_ANDROID || UNITY_IPHONE
//        string attachmentPath = "exception.png";
//#else
//        string attachmentPath = Application.dataPath + "/exception.png";
//#endif

//        if (File.Exists(attachmentPath))
//            File.Delete(attachmentPath);

//        Single.Coroutine.Routine(() =>
//        {

//#if UNITY_ANDROID || UNITY_IPHONE
//            attachmentPath = Application.persistentDataPath + "/exception.png";
//#endif

//            if (File.Exists(attachmentPath))
//            {
//                Attachment inline = new Attachment(attachmentPath);
//                string contentID = Path.GetFileName(attachmentPath).Replace(".", "") + "@zofm";
//                inline.ContentDisposition.Inline = true;
//                inline.ContentDisposition.DispositionType = DispositionTypeNames.Inline;
//                inline.ContentId = contentID;
//                inline.ContentType.MediaType = "image/png";
//                inline.ContentType.Name = Path.GetFileName(attachmentPath);

//                mail.Attachments.Add(inline);
//            }

//            SmtpClient smtpServer = new SmtpClient("smtp.gmail.com", 587);
//            smtpServer.Credentials = new System.Net.NetworkCredential("yoonhwan.ko@gmail.com", S2Util.Decrypt("U+VKgwhbSxP7bsZ4oo9OXg==")
//                                                                       ) as ICredentialsByHost;
//            smtpServer.EnableSsl = true;
//            ServicePointManager.ServerCertificateValidationCallback =
//            delegate(object s, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
//            {
//                return true;
//            };

//            smtpServer.Send(mail);
//            FinalWorking();
//            Debug.Log("send finish");
//#if UNITY_ANDROID || UNITY_IPHONE
//            if (File.Exists(attachmentPath))
//                File.Delete(attachmentPath);
//#endif
//        }, ScreenShot(attachmentPath));


//        yield return null;
//    }


//    void ResetBufferToLimit()
//    {
//        while (m_logBuffer.Count > m_MaxLogCount)
//        {
//            m_logBuffer.RemoveAt(0);
//        }
//    }

//    void HandleException(string condition, string stackTrace, LogType type)
//    {
//        Debug.Log("CrashReporter Exception Catched");
//        string sep = "------------------------------------------------------------------------------\r\n";
//        m_logBuffer.Add(sep + type.ToString() + " " + Time.realtimeSinceStartup.ToString() + "\r\n" + condition + "\r\n" + stackTrace);
//        Debug.Log("CrashReporter Exception Catched");
//        Debug.Log(sep);

//        ResetBufferToLimit();
//        if (type == LogType.Exception)
//        {

//            m_ExceptionCount++;

//            switch (m_WriteMode)
//            {
//                case eCrashWriteType.EWRITEFILE:
//                    {
//                        Single.Coroutine.Routine(() =>
//                        {

//                        },
//                        SendDebugToFile());
//                    }
//                    break;
//                case eCrashWriteType.EWRITESERVER:
//                    Single.Coroutine.Routine(() =>
//                    {

//                    },
//                    SendDebugToServer());
//                    break;
//                case eCrashWriteType.EWRITEMAIL:
//                    Single.Coroutine.Routine(() =>
//                    {

//                    },
//                    SendDebugToMail(type));
//                    break;
//            }

//        }
//    }

//    public static void FinalWorking()
//    {
//        if (!Application.isEditor)
//        {
//            SomethingReallyBadHappened();
//        }
//#if UNITY_EDITOR
//        Debug.Log("<color=yellow>Assert&Exception Occured, See the console log</color>");
//        S2Util.EditorPauseOfToggle(true);
//#else

//        Application.Quit();
//#endif
//    }

//    string BufferToText()
//    {
//        string stringText = "";

//        for (int i = 0; i < m_logBuffer.Count; i++)
//        {
//            stringText += m_logBuffer[(m_logBuffer.Count - 1) - i];
//        }

//        return stringText;
//    }

//    string MakeMassageHeader(string body)
//    {
//        /*
//        Textures
//            Meshes
//                Materials
//                Animations
//                Audio
//                Object Count 
//                */

//        long textureSize = 0;
//        long meshSize = 0;
//        long materialSize = 0;
//        long animationSize = 0;
//        long audioSize = 0;
//        long totalSize = System.GC.GetTotalMemory(true);

//        UnityEngine.Object[] textures = Resources.FindObjectsOfTypeAll(typeof(Texture));
//        foreach (Texture t in textures)
//            textureSize = Profiler.GetRuntimeMemorySize(t);
//        UnityEngine.Object[] meshs = Resources.FindObjectsOfTypeAll(typeof(Mesh));
//        foreach (Mesh t in meshs)
//            meshSize = Profiler.GetRuntimeMemorySize(t);
//        UnityEngine.Object[] materials = Resources.FindObjectsOfTypeAll(typeof(Material));
//        foreach (Material t in materials)
//            materialSize = Profiler.GetRuntimeMemorySize(t);

//        UnityEngine.Object[] anims = Resources.FindObjectsOfTypeAll(typeof(AnimationClip));
//        foreach (AnimationClip t in anims)
//            animationSize = Profiler.GetRuntimeMemorySize(t);

//        UnityEngine.Object[] audios = Resources.FindObjectsOfTypeAll(typeof(AudioClip));
//        foreach (AudioClip t in audios)
//            audioSize = Profiler.GetRuntimeMemorySize(t);


//        string mS2 = "\r\n\r\n------------------------------------------------------------------------------\r\n";
//        mS2 += string.Format("UserInfomation\r\n\r\nUserID:{0}, UID:{1}, TeamName:{2}, TeamIdent:{3}",
//                              S2ApiServer.Instance.UserID,
//                              S2ApiServer.Instance.UID,
//                              S2ApiServer.Instance.TeamName,
//                              S2ApiServer.Instance.eFavoriteTeam.ToString()
//                              );
//        mS2 += "\r\n\r\n------------------------------------------------------------------------------\r\n";

//        mS2 += string.Format
//            (
//                "System Infomation\r\n\r\nModel:{0}, Name:{1}, Type:{2}, Ident:{3}\nSystem:{4}, Lang:{5}, MemSize:{6}, ProcCount:{7}, ProcType:x {8}\nScreen:{9}x{10}, DPI:{11}dpi, FullScreen:{12}, {13}, {14}, vmem: {15}, Fill: {16} Max Texture: {17}\n\nScene {18}, Unity Version {19}",
//                SystemInfo.deviceModel,
//                SystemInfo.deviceName,
//                SystemInfo.deviceType,
//                SystemInfo.deviceUniqueIdentifier,

//                SystemInfo.operatingSystem,
//                Localization.language,
//                SystemInfo.systemMemorySize,
//                SystemInfo.processorCount,
//                SystemInfo.processorType,

//                Screen.currentResolution.width,
//                Screen.currentResolution.height,
//                Screen.dpi,
//                Screen.fullScreen,
//                SystemInfo.graphicsDeviceName,
//                SystemInfo.graphicsDeviceVendor,
//                SystemInfo.graphicsMemorySize,
//                SystemInfo.graphicsPixelFillrate,
//                SystemInfo.maxTextureSize,

//                Application.loadedLevelName,
//                Application.unityVersion
//                );

//        mS2 += "\r\n\r\n------------------------------------------------------------------------------\r\n";
//        mS2 += string.Format("Memory Status\r\n\r\nTotal:{0}Bytes\r\nTexture:{1}Bytes\r\nMash:{2}Bytes\r\nMaterial:{3}Bytes\r\nAnimation:{4}\r\nAudio:{5}\r\n",
//                             totalSize,
//                             textureSize,
//                             meshSize,
//                             materialSize,
//                             animationSize,
//                             audioSize);
//        mS2 += "\r\n\r\n------------------------------------------------------------------------------\r\n";
//        mS2 += "Log & Stack Trace\r\n\r\n";
//        mS2 += body;

//        return mS2;
//    }

//    static void SomethingReallyBadHappened()
//    {
//        //NB: Try and recover or fail gracefully here.
//    }
}
