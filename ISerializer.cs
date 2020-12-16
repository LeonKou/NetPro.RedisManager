﻿using System;
using System.Collections.Generic;
using System.Text;

namespace NetPro.RedisManager
{
    /// <summary>
    /// 序列化
    /// </summary>
    public interface ISerializer
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        byte[] Serialize(object item);

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="serializedObject"></param>
        /// <returns></returns>
        T Deserialize<T>(byte[] serializedObject);
    }
}
