using UnityEngine;

namespace Utilities.Geometry
{
    public class Ellipse
    {
        public Ellipse(float width, float height, Vector3 position, UnityEngine.Animations.Axis upAxis)
        {
            Width = width;
            Height = height;
            Position = position;
            UpAxis = upAxis;
        }
        // Values
        public float Width { get; set; }
        public float Height { get; set; }
        public Vector3 Position { get; set; }
        public UnityEngine.Animations.Axis UpAxis { get; set; }
        
        public Vector3[] GetPoints(Vector3 direction, float pointsAngle, int pointsNbr)
        {
            Vector3[] points = new Vector3[pointsNbr];
            // If the direction isn't zero, then proceed... Be careful, it'll return an empty list if direction == 0.
            if (direction == Vector3.zero)
                direction = UpAxis == UnityEngine.Animations.Axis.Z ? Vector3.up : UpAxis == UnityEngine.Animations.Axis.Y ? Vector3.forward : Vector3.right;
            
            pointsAngle = pointsAngle == 360 ? Mathf.Deg2Rad * ((pointsAngle - 360 / pointsNbr) / 2) : pointsNbr == 1 ? Mathf.Deg2Rad * pointsAngle : Mathf.Deg2Rad * (pointsAngle / 2);
            float stepAngle = UpAxis == UnityEngine.Animations.Axis.Z ? (Mathf.Atan2(direction.y, direction.x)) : UpAxis == UnityEngine.Animations.Axis.Y ? Mathf.Atan2(direction.z, direction.x)  : Mathf.Atan2(direction.z, direction.y);
            stepAngle += pointsAngle;
            
            float step;
            if (pointsNbr < 2) step = 0;
            else step = (pointsAngle * 2) / (pointsNbr - 1);
 
            for (int i = 0; i <= (pointsNbr - 1); i++)
            {
                float X = Position.x;
                float Y = Position.y;
                float Z = Position.z;
                if (UpAxis == UnityEngine.Animations.Axis.Z)
                {
                    X += Width * Mathf.Cos(stepAngle - step * i);
                    Y += Height * Mathf.Sin(stepAngle - step * i);
                }
                else if (UpAxis == UnityEngine.Animations.Axis.Y)
                {
                    X += Width * Mathf.Cos(stepAngle - step * i);
                    Z += Height * Mathf.Sin(stepAngle - step * i);
                }
                else
                {
                    Y += Width * Mathf.Cos(stepAngle - step * i);
                    Z += Height * Mathf.Sin(stepAngle - step * i);
                }
                
                points.SetValue(new Vector3(X, Y, Z), i);
            }
            return points;
        }

        public Vector3 GetPoint(float angle)
        {
            angle = Mathf.Deg2Rad * angle;

            float X = Position.x;
            float Y = Position.y;
            float Z = Position.z;
            if (UpAxis == UnityEngine.Animations.Axis.Z)
            {
                X += Width * Mathf.Cos(angle);
                Y += Height * Mathf.Sin(angle);
            }
            else if (UpAxis == UnityEngine.Animations.Axis.Y)
            {
                X += Width * Mathf.Cos(angle);
                Z += Height * Mathf.Sin(angle);
            }
            else
            {
                Y += Width * Mathf.Cos(angle);
                Z += Height * Mathf.Sin(angle);
            }
            
            return new Vector3(X, Y, Z);
        }
    }
}