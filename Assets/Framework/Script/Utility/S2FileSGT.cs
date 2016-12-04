/*▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤

★ 설계자 ☞ 이상호
★ 설계일 ☞ 2015년 04월 20일
★ E-Mail ☞ shmhlove@neowiz.com
★ Desc   ☞ 이 클래스는 로컬내에서 텍스트파일을 쓰거나 읽습니다.

▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤*/
using UnityEngine;
using System;
using System.IO;
using System.Text;
using System.Collections;

public class S2FileSGT : S2Singleton<S2FileSGT>
{
    public override void OnInitialize() { }
    public override void OnFinalize() { }

    public void Write (string strWriteBuff, bool bNewWrite, string strPathName)
    {
        FileStream pFile = new FileStream(strPathName, (bNewWrite ? FileMode.Create : FileMode.Append), FileAccess.Write);
        StreamWriter pStream = new StreamWriter(pFile);
        pStream.WriteLine(strWriteBuff);
        pStream.Close();
        pFile.Close();
    }

    public string Read (string strFileName)
    {
        TextAsset pTextAsset = Resources.Load(strFileName, typeof(TextAsset)) as TextAsset;
        if (null == pTextAsset)
            return "";

        TextReader pReader = new StringReader(pTextAsset.text);
        if (null == pReader)
            return "";

        string strBuff = pReader.ReadToEnd();
        pReader.Close();

        return strBuff;
    }
}