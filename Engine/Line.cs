
using OpenTK.Mathematics;
namespace Core.Primitive
{
    public class Line
    {
        public Line(Vector3 vStart, Vector3 vEnd)
        {
            StartPoint = vStart;
            EndPoint = vEnd;
        }
        public Vector3 StartPoint;
        public Vector3 EndPoint;
    }
}
