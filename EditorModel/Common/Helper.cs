﻿using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.IO.Compression;
using System.Runtime.Serialization.Formatters.Binary;
using System.Windows;

namespace EditorModel.Common
{
    public static class Helper
    {
        public static float GetAngle(Matrix matrix)
        {
            // взято из ответов https://stackoverflow.com/questions/14125771/calculate-angle-from-matrix-transform
            var x = new Vector(1, 0);
            var matr = new System.Windows.Media.Matrix(matrix.Elements[0], matrix.Elements[1], matrix.Elements[2],
                                                       matrix.Elements[3], matrix.Elements[4], matrix.Elements[5]);
            var rotated = Vector.Multiply(x, matr);
            var angleBetween = Vector.AngleBetween(x, rotated);
            return (float)angleBetween;
        }

        public static SizeF GetSize(Matrix matrix)
        {
            var x = new Vector(1, 0);
            var y = new Vector(0, 1);
            var matr = new System.Windows.Media.Matrix(matrix.Elements[0], matrix.Elements[1], matrix.Elements[2],
                                                       matrix.Elements[3], matrix.Elements[4], matrix.Elements[5]);
            var scaledX = Vector.Multiply(x, matr);
            var scaledY = Vector.Multiply(y, matr);
            return new SizeF((float)scaledX.Length, (float)scaledY.Length);
        }

        public static float GetSkewAngle(Matrix matrix)
        {
            var x = new Vector(1, 0);
            var y = new Vector(0, 1);
            var matr = new System.Windows.Media.Matrix(matrix.Elements[0], matrix.Elements[1], matrix.Elements[2],
                                                       matrix.Elements[3], matrix.Elements[4], matrix.Elements[5]);
            var skewX = Vector.Multiply(x, matr);
            var skewY = Vector.Multiply(y, matr);
            var angleBetween = Vector.AngleBetween(skewX, skewY);
            return (float)angleBetween;
        }

        /// <summary>
        /// Преобразование ContentAlignment в StringFormat.Alignment и StringFormat.LineAlignment
        /// </summary>
        /// <param name="stringFormat">Ссылка на объект StringFormat</param>
        /// <param name="alignment">Значения для установки</param>
        public static void UpdateStringFormat(StringFormat stringFormat, ContentAlignment alignment)
        {
            switch (alignment)
            {
                case ContentAlignment.TopLeft:
                    stringFormat.Alignment = StringAlignment.Near;
                    stringFormat.LineAlignment = StringAlignment.Near;
                    break;
                case ContentAlignment.TopCenter:
                    stringFormat.Alignment = StringAlignment.Center;
                    stringFormat.LineAlignment = StringAlignment.Near;
                    break;
                case ContentAlignment.TopRight:
                    stringFormat.Alignment = StringAlignment.Far;
                    stringFormat.LineAlignment = StringAlignment.Near;
                    break;
                case ContentAlignment.MiddleLeft:
                    stringFormat.Alignment = StringAlignment.Near;
                    stringFormat.LineAlignment = StringAlignment.Center;
                    break;
                case ContentAlignment.MiddleCenter:
                    stringFormat.Alignment = StringAlignment.Center;
                    stringFormat.LineAlignment = StringAlignment.Center;
                    break;
                case ContentAlignment.MiddleRight:
                    stringFormat.Alignment = StringAlignment.Far;
                    stringFormat.LineAlignment = StringAlignment.Center;
                    break;
                case ContentAlignment.BottomLeft:
                    stringFormat.Alignment = StringAlignment.Near;
                    stringFormat.LineAlignment = StringAlignment.Far;
                    break;
                case ContentAlignment.BottomCenter:
                    stringFormat.Alignment = StringAlignment.Center;
                    stringFormat.LineAlignment = StringAlignment.Far;
                    break;
                case ContentAlignment.BottomRight:
                    stringFormat.Alignment = StringAlignment.Far;
                    stringFormat.LineAlignment = StringAlignment.Far;
                    break;
            }
        }
        public static VersionInfo GetVersionInfo()
        {
            return new VersionInfo { Version = 2 };
        }

        // ReSharper disable InconsistentNaming
        public const float EPSILON = 0.01f;
        // ReSharper restore InconsistentNaming

        /// <summary>
        /// Расчёт коэффициента масштабирования
        /// </summary>
        /// <param name="marker">Точка маркера</param>
        /// <param name="anchor">Точка якоря</param>
        /// <param name="mouse">Положение мышки</param>
        /// <returns></returns>
        public static float GetScale(PointF marker, PointF anchor, PointF mouse)
        {
            var a = marker.Sub(anchor); // строим вектор Anchor-Marker
            var m = mouse.Sub(anchor);  // строим вектор Anchor-Mouse(position)
            // считаем коэффициент
            var scale = m.DotScalar(a) / a.LengthSqr();
            // защита результата от "крайних" случаев расчёта
            if (System.Math.Abs(scale) < EPSILON) scale = EPSILON;
            return scale;
        }

        /// <summary>
        /// Расчет угла вращения (в градусах)
        /// </summary>
        /// <param name="marker">Точка маркера</param>
        /// <param name="anchor">Точка якоря</param>
        /// <param name="mouse">Положение мышки</param>
        /// <returns></returns>
        public static float GetAngle(PointF marker, PointF anchor, PointF mouse)
        {
            var a = marker.Sub(anchor);
            var m = mouse.Sub(anchor);
            return m.Angle(a) * PointFExtension.TO_DEGREES;
        }

        public static void Compress(Stream sourceStream, string compressedFile)
        {
            // поток для записи сжатого файла
            using (FileStream targetStream = File.Create(compressedFile))
            {
                // поток архивации
                using (GZipStream compressionStream = new GZipStream(targetStream, CompressionMode.Compress))
                {
                    sourceStream.CopyTo(compressionStream); // копируем байты из одного потока в другой
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="compressedFile"></param>
        /// <param name="targetStream">поток для записи восстановленного файла</param>
        public static void Decompress(string compressedFile, Stream targetStream)
        {
            // поток для чтения из сжатого файла
            using (FileStream sourceStream = new FileStream(compressedFile, FileMode.OpenOrCreate))
            {
                // поток разархивации
                using (GZipStream decompressionStream = new GZipStream(sourceStream, CompressionMode.Decompress))
                {
                    decompressionStream.CopyTo(targetStream);
                }
            }
        }

        /// <summary>
        /// Сохранить все фигуры в поток
        /// </summary>
        /// <param name="stream">поток в памяти</param>
        /// <param name="listToSave">список для сохранения</param>
        public static void SaveToStream(Stream stream, object obj)
        {
            var formatter = new BinaryFormatter();
            formatter.Serialize(stream, obj);
        }

        /// <summary>
        /// Восстановить фигуры из потока в памяти
        /// </summary>
        /// <param name="stream"></param>
        /// <returns></returns>
        public static object LoadFromStream(Stream stream)
        {
            var formatter = new BinaryFormatter();
            return formatter.Deserialize(stream);
        }

    }
}
