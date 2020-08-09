namespace Core.Primitive
{
    public class Line
    {
        public Line(OpenTK.Vector3 vStart, OpenTK.Vector3 vEnd)
        {
            StartPoint = vStart;
            EndPoint = vEnd;
        }
        public OpenTK.Vector3 StartPoint;
        public OpenTK.Vector3 EndPoint;
    }
}
