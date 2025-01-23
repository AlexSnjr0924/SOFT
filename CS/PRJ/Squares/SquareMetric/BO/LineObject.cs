using SquareMetric.BL;

namespace SquareMetric.BO {
  /// <summary>Structures, used for geometry shapes (1 measure)</summary>
  public static class LineObject {
    #region... /* ShapeStruct. Objects */
    /// <summary>Point on infinity 2-dim space</summary>
    public class Point {
      /// <summary>Coordinate X</summary>
      public decimal X { get; set;}

      /// <summary>Coordinate Y</summary>
      public decimal Y { get; set;}

      /// <summary>Coordinate quarter for point</summary>
      public int Quarter { get; set;}

      /// <summary>Move point on infinity square</summary>
      /// <param name="movX">Movement by coordinate X</param>
      /// <param name="movY">Movement by coordinate Y</param>
      public void Move(decimal movX = 0, decimal movY = 0) {
        // Move point
        if (movX != 0) { X += movX;}
        if (movY != 0) { Y += movY;}

        // Determine coordinate quarter
        Quarter = X < 0 ? Y < 0 ? 3 : 2 : Y < 0 ? 4 : 1;
      }

      /// <summary>Constructor</summary>
      /// <param name="pX">Start X coordinate</param>
      /// <param name="pY">Start Y coordinate</param>
      public Point(decimal pX = 0, decimal pY = 0) {
        // Init coordinates, quarter
        X = pX;
        Y = pY;
        Move();
      }
    }

    /// <summary>Section (finite line) between 2 points</summary>
    public class Line {
      /// <summary>First end of segment (can be skipped on input)</summary>
      public Point PointA { get; set;}

      /// <summary>Second end of segment (can be skipped on input)</summary>
      public Point PointB { get; set;}

      /// <summary>Length of segment (can be input directly or calculated as distance between points)</summary>
      public decimal Length { get; set;}

      /// <summary>Move line on infinity square</summary>
      /// <param name="movX">Movement by coordinate X</param>
      /// <param name="movY">Movement by coordinate Y</param>
      public void Move(decimal movX = 0, decimal movY = 0) {
        // Move points
        if (movX != 0 || movY != 0) { 
          PointA.Move(movX, movY);
          PointB.Move(movX, movY);
        }
      }

      /// <summary>Init line features</summary>
      /// <param name="pX1">Point A. Coordinate X</param>
      /// <param name="pY1">Point A. Coordinate Y</param>
      /// <param name="pX2">Point B. Coordinate X</param>
      /// <param name="pY2">Point B. Coordinate Y</param>
      public void Init(decimal pX1, decimal pY1, decimal pX2, decimal pY2) {
        // Init edge points in line
        PointA = new Point(pX1, pY1);
        PointB = new Point(pX2, pY2);

        // Length of finite line
        Length = MathAccount.Distance(pX1, pY1, pX2, pY2);
      }

      /// <summary>Constructor</summary>
      /// <param name="pX1">Point A. Coordinate X</param>
      /// <param name="pY1">Point A. Coordinate Y</param>
      /// <param name="pX2">Point B. Coordinate X</param>
      /// <param name="pY2">Point B. Coordinate Y</param>
      public Line(decimal pX1, decimal pY1, decimal pX2, decimal pY2) { Init(pX1, pY1, pX2, pY2);}

      /// <summary>Constructor (II)</summary>
      /// <param name="pPointA">Point A as object</param>
      /// <param name="pPointB">Point B as object</param>
      public Line(Point pPointA, Point pPointB) { Init(pPointA.X, pPointA.Y, pPointB.X, pPointB.Y);}
    }

    /// <summary>Plane angle</summary>
    public struct StrAngle {
      /// <summary>Angle value in radians, will be calculated after finding trihonometric (co)sine value</summary>
      public decimal ValueRad { get; set;}

      /// <summary>Angle value in degrees (value-in-radians * pi / 2)</summary>
      public decimal ValueDeg { get; set;}

      /// <summary>Sine of angle (vector-product divided by side-length-product)</summary>
      public decimal ASine { get; set;}

      /// <summary>Cosine of angle (scalar-product divided by side-length-product)</summary>
      public decimal ACosine { get; set;}
    }
    #endregion /* ShapeStruct. Objects */
  }
}
