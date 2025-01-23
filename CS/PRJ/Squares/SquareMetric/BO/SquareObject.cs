using System;
using System.Collections.Generic;
using SquareMetric.BA;
using SquareMetric.BL;
using Punct = SquareMetric.BO.LineObject.Point;
using Side = SquareMetric.BO.LineObject.Line;

namespace SquareMetric.BO {
  /// <summary>Structures, used for geometry shapes (2 measures)</summary>
  public static class SquareObject {
    /// <summary>C.0. Abstract geometry figure</summary>
    public class Figure {
      /// <summary>Square of figure</summary>
      public decimal Square { get; set;}

      /// <summary>Each geometry shape have (significant) points, even a circle has central point</summary>
      public Punct[] FPoints { get; set;}

      /// <summary>Virtual method. Calculate square</summary>
      protected virtual void GetSquare() { Console.WriteLine("(operation) Figure.GetSquare");}

      /// <summary>Move geometry shape by 2-dim space</summary>
      /// <param name="movX">Movement by coordinate X</param>
      /// <param name="movY">Movement by coordinate Y</param>
      public void Move(decimal movX = 0, decimal movY = 0) {
        // Move each figure point
        Console.WriteLine("(operation) Figure.Move");
        for (var i = 0; i < FPoints.Length; i++) { FPoints[i].Move(movX, movY);}
      }
    }

    /// <summary>C.1. Circle. Set of points, which are on the same distance from centre</summary>
    public class Circle: Figure {
      /// <summary>Center of circle</summary>
      public Punct Center { get { return FPoints[0];}}

      /// <summary>Radius of circle</summary>
      public decimal Radius { get; set;}

      /// <summary>Realize method. Calculate measure of round</summary>
      protected override void GetSquare() {
        Console.WriteLine("(operation) Circle.GetSquare");
        Square = MathAccount.Numeric_Pi * Radius * Radius;
      }

      /// <summary>Constructor</summary>
      /// <param name="pCX">Centre of circle, X</param>
      /// <param name="pCY">Centre of circle, Y</param>
      /// <param name="pRadius">Radius of circle</param>
      public Circle(Punct pCentre, decimal pRadius) {
        // Init main circle features
        Console.WriteLine("(creation) Circle.Constructor");
        FPoints = new Punct[] { pCentre };
        Radius = pRadius;
        GetSquare();
      }

      /// <summary>Determine, does the circle covers a point</summary>
      /// <param name="pPoint">Point on 2-dim flat as object</param>
      /// <returns>-1: circle does not cover point; 0: C. touches P.; 1: P. lies in C. entirely</returns>
      public int Cover(Punct pPoint) {
        // Evaluate distance between point and circle radius
        var pointDist = CalculateFlat.GetDistance(FPoints[0], pPoint);
        return pointDist < Radius ? 1 : pointDist > Radius ? -1 : 0;
      }
    }

    /// <summary>C.2. Polygion. Figure with several sides (and same amount of vertex)</summary>
    public class BasePolygon: Figure {
      /// <summary>Sides (lines between vertexes) in polygon</summary>
      public Side[] FSides { get; set;}

      /// <summary>Perimeter of polygon</summary>
      public decimal Perimeter { get; set;}

      /// <summary>Move polygon by 2-dim space</summary>
      /// <param name="movX">Movement by coordinate X</param>
      /// <param name="movY">Movement by coordinate Y</param>
      public new void Move(decimal movX = 0, decimal movY = 0) {
        // Move each figure point and side
        for (var i = 0; i < FPoints.Length; i++) {
          FPoints[i].Move(movX, movY);
          FSides[i].Move(movX, movY);
        }
      }

      /// <summary>Virtual method. Calculate square</summary>
      protected override void GetSquare() { Console.WriteLine("(operation) BasePolygon.GetSquare");}

      /// <summary>Constructor</summary>
      /// <param name="pCoords">List of figure point coordinates (3 for triangle, 4 for rectangle,...)</param>
      public BasePolygon(List<Punct> pCoords) {
        // Info
        int sideCnt = pCoords.Count;
        Console.WriteLine("(creation) BasePolygon.Constructor. Sides: {0}", sideCnt);

        // Create array for points and sides
        FPoints = new Punct[sideCnt];
        FSides = new Side[sideCnt];
        Perimeter = 0m;

        for (int i = 0; i < sideCnt; i++) {
          // New point of polygon
          FPoints[i] = pCoords[i];

          // New side of polygon
          if (i > 0) {
            FSides[i - 1] = new Side(pCoords[i - 1], pCoords[i]);
            Perimeter += FSides[i - 1].Length;
          }
        }

        // Last side of polygon
        FSides[sideCnt - 1] = new Side(pCoords[sideCnt - 1], pCoords[0]);
        Perimeter += FSides[sideCnt - 1].Length;

        // Calculate square of just-created figure
        GetSquare();
      }
    }

    /// <summary>C.2.1. Geometrical figure. Triangle</summary>
    public class Triangle: BasePolygon {
      /// <summary>Constructor</summary>
      public Triangle(List<Punct> pCoords): base(pCoords) {}

      /// <summary>Triangle point coordinates. x1</summary>
      public decimal X1 => FPoints[0].X;

      /// <summary>Triangle point coordinates. y1</summary>
      public decimal Y1 => FPoints[0].Y;

      /// <summary>Triangle point coordinates. x2</summary>
      public decimal X2 => FPoints[1].X;

      /// <summary>Triangle point coordinates. y2</summary>
      public decimal Y2 => FPoints[1].Y;

      /// <summary>Triangle point coordinates. x3</summary>
      public decimal X3 => FPoints[2].X;

      /// <summary>Triangle point coordinates. y3</summary>
      public decimal Y3 => FPoints[2].Y;

      /// <summary>Calculate square of triangle</summary>
      protected override void GetSquare() {
        // Square by determinant; sine of angle difference
        Console.WriteLine("(operation) Triangle.GetSquare");
        Square = MathAccount.Abs(MathAccount.VectorProduct(
            FPoints[1].X - FPoints[0].X, FPoints[1].Y - FPoints[0].Y
          , FPoints[2].X - FPoints[0].X, FPoints[2].Y - FPoints[0].Y) / 2);
      }

      /// <summary>Calculate triangle square by method of Heron. For illustrative purposes</summary>
      public decimal GetSquareHeron() {
        // Start multiplier p/2
        Console.WriteLine("(operation) Triangle.GetSquareHeron");
        var pHalf = Perimeter / 2;
        var result = pHalf;

        // Get product of differences between p/2 and each triangle side
        for (int i = 0; i < 3; i++) { result *= pHalf - FSides[i].Length;}
        return MathAccount.Root2(result);
      }
    }

    /// <summary>C2.2. Geometrical figure. Polygon with 4 or more sides</summary>
    public class Polygon: BasePolygon {
      /// <summary>Constructor</summary>
      public Polygon(List<Punct> pCoords): base(pCoords) {}

      /// <summary>Calculate square of polygon</summary>
      protected override void GetSquare() {
        // Square by Gauss (land surveyor) formula
        Console.WriteLine("(operation) Polygon.GetSquare");
        Square = 0m;

        // Square by Gauss. Cycle
        for (int i = 0; i < FPoints.Length; i++) {
          var idx0 = i == 0 ? FPoints.Length - 1 : i - 1;
          var idx1 = i == FPoints.Length - 1 ? 0 : i + 1;
          Square += FPoints[i].Y * (FPoints[idx0].X - FPoints[idx1].X);
        }

        // Previous measures were doubled
        Square = MathAccount.Abs(Square / 2.0m); 
      }
    }
  }
}
