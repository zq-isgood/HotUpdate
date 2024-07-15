﻿using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class FileUtil
{
    /// <summary>
    /// 检测这个路径的文件是否存在
    /// </summary>
    /// <param name="path"></param>
    /// <returns></returns>
    public static bool IsExists(string path)
    {
        FileInfo file = new FileInfo(path);
        return file.Exists;
    }

    /// <summary>
    /// 写入文件
    /// </summary>
    /// <param name="path"></param>
    /// <param name="data"></param>
    public static void WriteFile(string path, byte[] data)
    {
        //获取标准路径，把\\换成/
        path = PathUtil.GetStandardPath(path);
        //文件夹的路径
        string dir = path.Substring(0, path.LastIndexOf("/"));
        if(!Directory.Exists(dir))
        {
            Directory.CreateDirectory(dir);
        }
        FileInfo file = new FileInfo(path);
        if (file.Exists)
        {
            file.Delete();
        }
        try
        {
            using (FileStream fs = new FileStream(path, FileMode.Create, FileAccess.Write))
            {
                fs.Write(data, 0, data.Length); //写文件
                fs.Close();
            }
        }
        catch(IOException e)
        {
            Debug.LogError(e.Message);
        }
    }
}
