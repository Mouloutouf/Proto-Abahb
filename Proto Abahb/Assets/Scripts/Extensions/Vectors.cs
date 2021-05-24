using System.Collections.Generic;
using UnityEngine;

namespace Extensions
{
    public static class Vectors
    {
	    public static Vector3 To3D(this Vector2Int vector2, float yValue = 0)
	    {
		    return new Vector3(vector2.x, yValue, vector2.y);
	    }
        public static Vector3 To3D(this Vector2 vector2, float yValue = 0)
        {
            return new Vector3(vector2.x, yValue, vector2.y);
        }
        
        public static Vector2 To2D(this Vector3 vector3)
        {
	        return new Vector2(vector3.x, vector3.z);
        }
        
        public static Vector3 RandomArea(this Vector3 v)
			=> new Vector3(Random.Range(-v.x, v.x) * .5f, Random.Range(-v.y, v.y) * .5f, Random.Range(-v.z, v.z) * .5f);

		#region Set X/Y/Z

		// Set X

		public static Vector3 WithX(this Vector3 vector, float x)
		{
			return new Vector3(x, vector.y, vector.z);
		}

		public static Vector2 WithX(this Vector2 vector, float x)
		{
			return new Vector2(x, vector.y);
		}

		public static void SetX(this Transform transform, float x)
		{
			transform.position = transform.position.WithX(x);
		}

		// Set Y

		public static Vector3 WithY(this Vector3 vector, float y)
		{
			return new Vector3(vector.x, y, vector.z);
		}

		public static Vector2 WithY(this Vector2 vector, float y)
		{
			return new Vector2(vector.x, y);
		}

		public static void SetY(this Transform transform, float y)
		{
			transform.position = transform.position.WithY(y);
		}

		// Set Z

		public static Vector3 WithZ(this Vector3 vector, float z)
		{
			return new Vector3(vector.x, vector.y, z);
		}

		public static void SetZ(this Transform transform, float z)
		{
			transform.position = transform.position.WithZ(z);
		}

		// Set XY

		public static Vector3 WithXY(this Vector3 vector, float x, float y)
		{
			return new Vector3(x, y, vector.z);
		}

		public static void SetXY(this Transform transform, float x, float y)
		{
			transform.position = transform.position.WithXY(x, y);
		}

		// Set XZ

		public static Vector3 WithXZ(this Vector3 vector, float x, float z)
		{
			return new Vector3(x, vector.y, z);
		}

		public static void SetXZ(this Transform transform, float x, float z)
		{
			transform.position = transform.position.WithXZ(x, z);
		}

		// Set YZ

		public static Vector3 WithYZ(this Vector3 vector, float y, float z)
		{
			return new Vector3(vector.x, y, z);
		}

		public static void SetYZ(this Transform transform, float y, float z)
		{
			transform.position = transform.position.WithYZ(y, z);
		}

		//Reset

		/// <summary>
		/// Set position to Vector3.zero.
		/// </summary>
		public static void ResetPosition(this Transform transform)
		{
			transform.position = Vector3.zero;
		}


		// RectTransform 

		public static void SetPositionX(this RectTransform transform, float x)
		{
			transform.anchoredPosition = transform.anchoredPosition.WithX(x);
		}

		public static void SetPositionY(this RectTransform transform, float y)
		{
			transform.anchoredPosition = transform.anchoredPosition.WithY(y);
		}

		public static void OffsetPositionX(this RectTransform transform, float x)
		{
			transform.anchoredPosition = transform.anchoredPosition.OffsetX(x);
		}

		public static void OffsetPositionY(this RectTransform transform, float y)
		{
			transform.anchoredPosition = transform.anchoredPosition.OffsetY(y);
		}

		#endregion


		#region Offset X/Y/Z

		public static Vector3 Offset(this Vector3 vector, Vector2 offset)
		{
			return new Vector3(vector.x + offset.x, vector.y + offset.y, vector.z);
		}


		public static Vector3 OffsetX(this Vector3 vector, float x)
		{
			return new Vector3(vector.x + x, vector.y, vector.z);
		}

		public static Vector2 OffsetX(this Vector2 vector, float x)
		{
			return new Vector2(vector.x + x, vector.y);
		}

		public static void OffsetX(this Transform transform, float x)
		{
			transform.position = transform.position.OffsetX(x);
		}


		public static Vector2 OffsetY(this Vector2 vector, float y)
		{
			return new Vector2(vector.x, vector.y + y);
		}

		public static Vector3 OffsetY(this Vector3 vector, float y)
		{
			return new Vector3(vector.x, vector.y + y, vector.z);
		}

		public static void OffsetY(this Transform transform, float y)
		{
			transform.position = transform.position.OffsetY(y);
		}


		public static Vector3 OffsetZ(this Vector3 vector, float z)
		{
			return new Vector3(vector.x, vector.y, vector.z + z);
		}

		public static void OffsetZ(this Transform transform, float z)
		{
			transform.position = transform.position.OffsetZ(z);
		}


		public static Vector3 OffsetXY(this Vector3 vector, float x, float y)
		{
			return new Vector3(vector.x + x, vector.y + y, vector.z);
		}

		public static void OffsetXY(this Transform transform, float x, float y)
		{
			transform.position = transform.position.OffsetXY(x, y);
		}

		public static Vector2 OffsetXY(this Vector2 vector, float x, float y)
		{
			return new Vector2(vector.x + x, vector.y + y);
		}


		public static Vector3 OffsetXZ(this Vector3 vector, float x, float z)
		{
			return new Vector3(vector.x + x, vector.y, vector.z + z);
		}

		public static void OffsetXZ(this Transform transform, float x, float z)
		{
			transform.position = transform.position.OffsetXZ(x, z);
		}


		public static Vector3 OffsetYZ(this Vector3 vector, float y, float z)
		{
			return new Vector3(vector.x, vector.y + y, vector.z + z);
		}

		public static void OffsetYZ(this Transform transform, float y, float z)
		{
			transform.position = transform.position.OffsetYZ(y, z);
		}

		#endregion


		#region Clamp

		/// <summary>
		/// Clamp a range
		/// </summary>
		public static Vector3 Clamp(this Vector3 vector, Vector3 min, Vector3 max)
		{
			vector.x = Mathf.Clamp(vector.x, min.x, max.x);
			vector.y = Mathf.Clamp(vector.y, min.y, max.y);
			vector.z = Mathf.Clamp(vector.z, min.z, max.z);
			return vector;
		}

		public static Vector3 ClampX(this Vector3 vector, float min, float max)
		{
			return vector.WithX(Mathf.Clamp(vector.x, min, max));
		}

		public static Vector2 ClampX(this Vector2 vector, float min, float max)
		{
			return vector.WithX(Mathf.Clamp(vector.x, min, max));
		}

		public static void ClampX(this Transform transform, float min, float max)
		{
			transform.SetX(Mathf.Clamp(transform.position.x, min, max));
		}


		public static Vector3 ClampY(this Vector3 vector, float min, float max)
		{
			return vector.WithY(Mathf.Clamp(vector.x, min, max));
		}

		public static Vector2 ClampY(this Vector2 vector, float min, float max)
		{
			return vector.WithY(Mathf.Clamp(vector.x, min, max));
		}

		public static void ClampY(this Transform transform, float min, float max)
		{
			transform.SetY(Mathf.Clamp(transform.position.x, min, max));
		}

		#endregion


		#region Invert

		public static Vector2 InvertX(this Vector2 vector)
		{
			return new Vector2(-vector.x, vector.y);
		}

		public static Vector2 InvertY(this Vector2 vector)
		{
			return new Vector2(vector.x, -vector.y);
		}

		#endregion


		#region Convert

		public static Vector2 ToVector2(this Vector3 vector)
		{
			return new Vector2(vector.x, vector.y);
		}

		public static Vector3 ToVector3(this Vector2 vector)
		{
			return new Vector3(vector.x, vector.y);
		}


		public static Vector2 ToVector2(this Vector2Int vector)
		{
			return new Vector2(vector.x, vector.y);
		}

		public static Vector3 ToVector3(this Vector3Int vector)
		{
			return new Vector3(vector.x, vector.y);
		}


		public static Vector2Int ToVector2Int(this Vector2 vector)
		{
			return new Vector2Int(Mathf.RoundToInt(vector.x), Mathf.RoundToInt(vector.y));
		}

		public static Vector3Int ToVector3Int(this Vector3 vector)
		{
			return new Vector3Int(Mathf.RoundToInt(vector.x), Mathf.RoundToInt(vector.y), Mathf.RoundToInt(vector.z));
		}

		#endregion


		#region Snap

		/// <summary>
		/// Snap to grid of snapValue
		/// </summary>
		public static Vector3 SnapValue(this Vector3 val, float snapValue)
		{
			return new Vector3(
				val.x.Snap(snapValue),
				val.y.Snap(snapValue),
				val.z.Snap(snapValue));
		}

		/// <summary>
		/// Snap to grid of snapValue
		/// </summary>
		public static Vector3 SnapValue(this Vector3 val, Vector3 snapValue)
		{
			return new Vector3(
				val.x.Snap(snapValue.x),
				val.y.Snap(snapValue.y),
				val.z.Snap(snapValue.z));
		}

		/// <summary>
		/// Snap to grid of snapValue
		/// </summary>
		public static Vector2 SnapValue(this Vector2 val, float snapValue)
		{
			return new Vector2(
				val.x.Snap(snapValue),
				val.y.Snap(snapValue));
		}

		/// <summary>
		/// Snap position to grid of snapValue
		/// </summary>
		public static void SnapPosition(this Transform transform, float snapValue)
		{
			transform.position = transform.position.SnapValue(snapValue);
		}

		/// <summary>
		/// Snap to one unit grid
		/// </summary>
		public static Vector2 SnapToOne(this Vector2 vector)
		{
			return new Vector2(Mathf.Round(vector.x), Mathf.Round(vector.y));
		}

		/// <summary>
		/// Snap to one unit grid
		/// </summary>
		public static Vector3 SnapToOne(this Vector3 vector)
		{
			return new Vector3(Mathf.Round(vector.x), Mathf.Round(vector.y), Mathf.Round(vector.z));
		}

		#endregion


		#region Average

		public static Vector3 AverageVector(this Vector3[] vectors)
		{
			if (vectors.IsNull()) return Vector3.zero;

			float x = 0f, y = 0f, z = 0f;
			for (var i = 0; i < vectors.Length; i++)
			{
				x += vectors[i].x;
				y += vectors[i].y;
				z += vectors[i].z;
			}

			return new Vector3(x / vectors.Length, y / vectors.Length, z / vectors.Length);
		}

		public static Vector2 AverageVector(this Vector2[] vectors)
		{
			if (vectors.IsNull()) return Vector2.zero;

			float x = 0f, y = 0f;
			for (var i = 0; i < vectors.Length; i++)
			{
				x += vectors[i].x;
				y += vectors[i].y;
			}

			return new Vector2(x / vectors.Length, y / vectors.Length);
		}

		#endregion


		#region Approximately

		public static bool Approximately(this Vector3 vector, Vector3 compared, float threshold = 0.1f)
		{
			var xDiff = Mathf.Abs(vector.x - compared.x);
			var yDiff = Mathf.Abs(vector.y - compared.y);
			var zDiff = Mathf.Abs(vector.z - compared.z);

			return xDiff <= threshold && yDiff <= threshold && zDiff <= threshold;
		}

		public static bool Approximately(this Vector2 vector, Vector2 compared, float threshold = 0.1f)
		{
			var xDiff = Mathf.Abs(vector.x - compared.x);
			var yDiff = Mathf.Abs(vector.y - compared.y);

			return xDiff <= threshold && yDiff <= threshold;
		}

		#endregion


		#region Get Closest 

		/// <summary>
		/// Finds the position closest to the given one.
		/// </summary>
		/// <param name="position">World position.</param>
		/// <param name="otherPositions">Other world positions.</param>
		/// <returns>Closest position.</returns>
		public static Vector3 GetClosest(this Vector3 position, IEnumerable<Vector3> otherPositions)
		{
			var closest = Vector3.zero;
			var shortestDistance = Mathf.Infinity;

			foreach (var otherPosition in otherPositions)
			{
				var distance = (position - otherPosition).sqrMagnitude;

				if (distance < shortestDistance)
				{
					closest = otherPosition;
					shortestDistance = distance;
				}
			}

			return closest;
		}

		public static Vector3 GetClosest(this IEnumerable<Vector3> positions, Vector3 position)
		{
			return position.GetClosest(positions);
		}

		#endregion

		public static float InverseLerp(Vector3 a, Vector3 b, Vector3 value)
		{
			Vector3 AB = b - a;
			Vector3 AV = value - a;
			return Vector3.Dot(AV, AB) / Vector3.Dot(AB, AB);
		}

		public static Vector3 GetTransformDirection(this Transform transform, Vector3Direction vector3Direction)
		{
			switch (vector3Direction)
			{
				case Vector3Direction.Up:
					return transform.up;
				case Vector3Direction.Down:
					return -transform.up;
				case Vector3Direction.Right:
					return transform.right;
				case Vector3Direction.Left:
					return -transform.right;
				case Vector3Direction.Forward:
					return transform.forward;
				case Vector3Direction.Back:
					return -transform.forward;
				default:
					return Vector3.zero;
			}
		}

		public static Vector3 GetDirection(Vector3Direction vector3Direction)
		{
			switch (vector3Direction)
			{
				case Vector3Direction.Up:
					return Vector3.up;
				case Vector3Direction.Down:
					return Vector3.down;
				case Vector3Direction.Right:
					return Vector3.right;
				case Vector3Direction.Left:
					return Vector3.left;
				case Vector3Direction.Forward:
					return Vector3.forward;
				case Vector3Direction.Back:
					return Vector3.back;
				default:
					return Vector3.zero;
			}
		}
		
		public static Vector2 Rotate(this Vector2 v, float degrees) {
			float sin = Mathf.Sin(degrees * Mathf.Deg2Rad);
			float cos = Mathf.Cos(degrees * Mathf.Deg2Rad);
         
			float tx = v.x;
			float ty = v.y;
			v.x = (cos * tx) - (sin * ty);
			v.y = (sin * tx) + (cos * ty);
			return v;
		}
    
		public static List<Vector2Int> Line(Vector2Int origin , Vector2Int destination)
		{
			List<Vector2Int> output = new List<Vector2Int>(); 
			int x = origin.x;
			int y = origin.y;
			int x2 = destination.x;
			int y2 = destination.y;
			int w = x2 - x ;
			int h = y2 - y ;
			int dx1 = 0, dy1 = 0, dx2 = 0, dy2 = 0 ;
			if (w<0) dx1 = -1 ; else if (w>0) dx1 = 1 ;
			if (h<0) dy1 = -1 ; else if (h>0) dy1 = 1 ;
			if (w<0) dx2 = -1 ; else if (w>0) dx2 = 1 ;
			int longest = Mathf.Abs(w) ;
			int shortest = Mathf.Abs(h) ;
			if (!(longest>shortest)) {
				longest = Mathf.Abs(h) ;
				shortest = Mathf.Abs(w) ;
				if (h<0) dy2 = -1 ; else if (h>0) dy2 = 1 ;
				dx2 = 0 ;            
			}
			int numerator = longest >> 1 ;
			for (int i=0;i<=longest;i++) {
				output.Add(new Vector2Int(x, y));
				numerator += shortest ;
				if (!(numerator<longest)) {
					numerator -= longest ;
					x += dx1 ;
					y += dy1 ;
				} else {
					x += dx2 ;
					y += dy2 ;
				}
			}
			return output;
		}
    }
    public enum Vector3Direction
    {
	    Up,
	    Down,
	    Right,
	    Left,
	    Forward,
	    Back
    }
}