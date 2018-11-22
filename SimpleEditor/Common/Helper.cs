using System;
using System.Drawing;
using SimpleEditor.Common;

namespace SimpleEditor
{
    static class Helper
    {
        private const float EPSILON = 0.01f;

        /// <summary>
        /// ������ ������������ ���������������
        /// </summary>
        /// <param name="marker">����� �������</param>
        /// <param name="anchor">����� �����</param>
        /// <param name="mouse">��������� �����</param>
        /// <returns></returns>
        public static float GetScale(PointF marker, PointF anchor, PointF mouse)
        {
            var a = marker.Sub(anchor);
            var m = mouse.Sub(anchor);
            var scale = m.DotScalar(a) / a.LengthSqr();
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
    }
}