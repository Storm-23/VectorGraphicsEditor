using System;
using System.Drawing;
using System.IO;
using System.IO.Compression;
using System.Runtime.Serialization.Formatters.Binary;

namespace SimpleEditor.Common
{
    static class Helper
    {
// ReSharper disable InconsistentNaming
        public const float EPSILON = 0.01f;
// ReSharper restore InconsistentNaming

        /// <summary>
        /// ������ ������������ ���������������
        /// </summary>
        /// <param name="marker">����� �������</param>
        /// <param name="anchor">����� �����</param>
        /// <param name="mouse">��������� �����</param>
        /// <returns></returns>
        public static float GetScale(PointF marker, PointF anchor, PointF mouse)
        {
            var a = marker.Sub(anchor); // ������ ������ Anchor-Marker
            var m = mouse.Sub(anchor);  // ������ ������ Anchor-Mouse(position)
            // ������� �����������
            var scale = m.DotScalar(a) / a.LengthSqr();
            // ������ ���������� �� "�������" ������� �������
            if (Math.Abs(scale) < EPSILON) scale = EPSILON;
            return scale;
        }

        /// <summary>
        /// ������ ���� �������� (� ��������)
        /// </summary>
        /// <param name="marker">����� �������</param>
        /// <param name="anchor">����� �����</param>
        /// <param name="mouse">��������� �����</param>
        /// <returns></returns>
        public static float GetAngle(PointF marker, PointF anchor, PointF mouse)
        {
            var a = marker.Sub(anchor);
            var m = mouse.Sub(anchor);
            return m.Angle(a) * PointFExtension.TO_DEGREES;
        }

        public static void Compress(Stream sourceStream, string compressedFile)
        {
            // ����� ��� ������ ������� �����
            using (FileStream targetStream = File.Create(compressedFile))
            {
                // ����� ���������
                using (GZipStream compressionStream = new GZipStream(targetStream, CompressionMode.Compress))
                {
                    sourceStream.CopyTo(compressionStream); // �������� ����� �� ������ ������ � ������
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="compressedFile"></param>
        /// <param name="targetStream">����� ��� ������ ���������������� �����</param>
        public static void Decompress(string compressedFile, Stream targetStream)
        {
            // ����� ��� ������ �� ������� �����
            using (FileStream sourceStream = new FileStream(compressedFile, FileMode.OpenOrCreate))
            {
                // ����� ������������
                using (GZipStream decompressionStream = new GZipStream(sourceStream, CompressionMode.Decompress))
                {
                    decompressionStream.CopyTo(targetStream);
                }
            }
        }

        /// <summary>
        /// ��������� ��� ������ � �����
        /// </summary>
        /// <param name="stream">����� � ������</param>
        /// <param name="listToSave">������ ��� ����������</param>
        public static void SaveToStream(Stream stream, object obj)
        {
            var formatter = new BinaryFormatter();
            formatter.Serialize(stream, obj);
        }

        /// <summary>
        /// ������������ ������ �� ������ � ������
        /// </summary>
        /// <param name="stream"></param>
        /// <returns></returns>
        public static object LoadFromStream(Stream stream)
        {
            var formatter = new BinaryFormatter();
            return formatter.Deserialize(stream);
        }

        public static VersionInfo GetVersionInfo()
        {
            return new VersionInfo { Version = 2 };
        }
    }
}