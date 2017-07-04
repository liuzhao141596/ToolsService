using System;
using System.IO;
using System.IO.Compression;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace ToolsServices
{
    /// <summary>
    /// 压缩解压缩
    /// Create BY Lzz In 20170704
    /// </summary>
    public class GZipCompress
    {
        /// <summary>
        /// 将对象序列化化成字节数组byte[]，再用gzip压缩
        /// </summary>
        /// <param name="obj">对象</param>
        /// <returns>字节数组</returns>
        public static byte[] GetBinaryFormatDataCompress(object obj)
        {
            byte[] binaryDataResult = null;
            MemoryStream memStream = new MemoryStream();
            IFormatter brFormatter = new BinaryFormatter();
            //dsOriginal.RemotingFormat = SerializationFormat.Binary;
            brFormatter.Serialize(memStream, obj);
            binaryDataResult = memStream.ToArray();
            memStream.Close();
            memStream.Dispose();
            return Compress(binaryDataResult);
        }


        /// <summary>
        /// Gzip压缩数据
        /// </summary>
        /// <param name="data">要压缩的数据</param>
        /// <returns></returns>
        public static byte[] Compress(byte[] data)
        {
            byte[] bData;
            MemoryStream ms = new MemoryStream();
            GZipStream stream = new GZipStream(ms, CompressionMode.Compress, true);
            stream.Write(data, 0, data.Length);
            stream.Close();
            stream.Dispose();
            //必须把stream流关闭才能返回ms流数据,不然数据会不完整
            //并且解压缩方法stream.Read(buffer, 0, buffer.Length)时会返回0
            bData = ms.ToArray();
            ms.Close();
            ms.Dispose();
            return bData;
        }

        /// <summary>
        /// 将字节数组反解压后序列化成对象
        /// </summary>
        /// <param name="binaryData">字节数组</param>
        /// <returns>对象</returns>
        public static object GetBinaryFormatDataDecompress(byte[] binaryData)
        {
            MemoryStream memStream = new MemoryStream(Decompress(binaryData));
            IFormatter brFormatter = new BinaryFormatter();
            Object obj = brFormatter.Deserialize(memStream);
            return obj;
        }

        /// <summary>
        /// Gzip解压数据
        /// </summary>
        /// <param name="data">被GZip压缩过的数据</param>
        /// <returns></returns>
        public static byte[] Decompress(byte[] data)
        {
            byte[] bData;
            MemoryStream ms = new MemoryStream();
            ms.Write(data, 0, data.Length);
            ms.Position = 0;
            GZipStream stream = new GZipStream(ms, CompressionMode.Decompress, true);
            byte[] buffer = new byte[1024];
            MemoryStream temp = new MemoryStream();
            int read = stream.Read(buffer, 0, buffer.Length);
            while (read > 0)
            {
                temp.Write(buffer, 0, read);
                read = stream.Read(buffer, 0, buffer.Length);
            }
            //必须把stream流关闭才能返回ms流数据,不然数据会不完整
            stream.Close();
            stream.Dispose();
            ms.Close();
            ms.Dispose();
            bData = temp.ToArray();
            temp.Close();
            temp.Dispose();
            return bData;
        }
    }
}
