using System;
using SquareMetric.BL;
using SquareMetric.BO;

namespace SquareMetric.BA {
  /// <summary>Calculations, used for points and vectors in 2-dim space</summary>
  public class CalculateFlat {
    #region... /* CalculateFlat. Instance */
    /// <summary>Class instance. Private</summary>
    static CalculateFlat CalcInstanceIn { get; set;}

    /// <summary>Class instance. Private</summary>
    public static CalculateFlat CalcInstance { get {
      if (CalcInstanceIn == null) { CalcInstanceIn = new CalculateFlat();}
      return CalcInstanceIn;
    }}
    #endregion /* CalculateFlat. Instance */

    #region... /* CalculateFlat. Fields */
    /// <summary>Error text. Private</summary>
    string ErrorTextIn { get; set;}

    /// <summary>Error text. Private</summary>
    public static string ErrorText => CalcInstance.ErrorTextIn;
    #endregion /* CalculateFlat. Fields */

    #region... /* CalculateFlat. Private */
    /// <summary>Get length of finite line. Private</summary>
    /// <param name="pLine">Line (segment) as object</param>
    decimal GetDistanceIn(LineObject.Point pPointA, LineObject.Point pPointB) {
      return MathAccount.Distance(pPointA.X, pPointA.Y, pPointB.X, pPointB.Y);
    }

    /// <summary>Calculate vector production of 2 vectors. Private</summary>
    /// <param name="pLineA">First line (vector)</param>
    /// <param name="pLineB">Second line (vector)</param>
    /// <returns>L1 * L2 * sin(L1^L2); result vector is othogonal to main flat</returns>
    decimal VectorProductIn(LineObject.Line pLineA, LineObject.Line pLineB) {
      return MathAccount.Determinant2(new decimal[] {
          pLineA.PointB.X - pLineA.PointA.X
        , pLineA.PointB.Y - pLineA.PointA.Y
        , pLineB.PointB.X - pLineB.PointA.X
        , pLineB.PointB.Y - pLineB.PointA.Y
      });
    }

    /// <summary>Calculate scalar production of 2 vectors. Private</summary>
    /// <param name="pLineA">First line (vector)</param>
    /// <param name="pLineB">Second line (vector)</param>
    /// <returns>L1 * L2 * cos(L1^L2)</returns>
    decimal ScalarProductIn(LineObject.Line pLineA, LineObject.Line pLineB) {
      return MathAccount.ScalarProduct(
          pLineA.PointB.X - pLineA.PointA.X, pLineA.PointB.Y - pLineA.PointA.Y
        , pLineB.PointB.X - pLineB.PointA.X, pLineB.PointB.Y - pLineB.PointA.Y);
    }    

    /// <summary>Calculate trigonometric sine of angle between 2 vectors. Private</summary>
    /// <param name="pLineA">First vector</param>
    /// <param name="pLineB">Second vector</param>
    /// <returns>Sine value (between -1 and 1)</returns>
    decimal SineOfAngleIn(LineObject.Line pLineA, LineObject.Line pLineB) {
      try {
        return VectorProductIn(pLineA, pLineB) / (pLineA.Length * pLineB.Length);
      } catch(DivideByZeroException ex) {
        ErrorTextIn = ex.Message;
        return decimal.MinValue;
      }
    }

    /// <summary>Calculate trigonometric cosine of angle between 2 vectors. Private</summary>
    /// <param name="pLineA">First vector</param>
    /// <param name="pLineB">Second vector</param>
    /// <returns>Cosine value (between -1 and 1)</returns>
    decimal CosineOfAngleIn(LineObject.Line pLineA, LineObject.Line pLineB) {
      try {
        return ScalarProductIn(pLineA, pLineB) / (pLineA.Length * pLineB.Length);
      } catch(DivideByZeroException ex) {
        ErrorTextIn = ex.Message;
        return decimal.MinValue;
      }
    }
    #endregion /* CalculateFlat. Private */

    #region... /* CalculateFlat. Public */
    /// <summary>Constructor</summary>
    public CalculateFlat() {}

    /// <summary>Get length of finite line</summary>
    /// <param name="pLine">Line (segment) as object</param>
    public static decimal GetDistance(LineObject.Point pPointA, LineObject.Point pPointB) {
      return CalcInstance.GetDistanceIn(pPointA, pPointB);
    }

    /// <summary>Calculate vector production of 2 vectors. Order of lines is important</summary>
    /// <param name="pLineA">First line (vector)</param>
    /// <param name="pLineB">Second line (vector)</param>
    /// <returns>L1 * L2 * sin(angle); result vector is othogonal to main flat</returns>
    public static decimal VectorProduct(LineObject.Line pLineA, LineObject.Line pLineB) {
      return CalcInstance.VectorProductIn(pLineA, pLineB);
    }

    /// <summary>Calculate scalar production of 2 vectors</summary>
    /// <param name="pLineA">First line (vector)</param>
    /// <param name="pLineB">Second line (vector)</param>
    /// <returns>L1 * L2 * cos(L1^L2)</returns>
    public static decimal ScalarProduct(LineObject.Line pLineA, LineObject.Line pLineB) {
      return CalcInstance.ScalarProductIn(pLineA, pLineB);
    }    

    /// <summary>Calculate trigonometric sine of angle between 2 vectors</summary>
    /// <param name="pLineA">First vector</param>
    /// <param name="pLineB">Second vector</param>
    /// <returns>Sine value (between -1 and 1); -BIG on error</returns>
    public static decimal SineOfAngle(LineObject.Line pLineA, LineObject.Line pLineB) {
      Console.WriteLine("SineOfAngle");
      return CalcInstance.SineOfAngleIn(pLineA, pLineB);
    }

    /// <summary>Calculate trigonometric cosine of angle between 2 vectors</summary>
    /// <param name="pLineA">First vector</param>
    /// <param name="pLineB">Second vector</param>
    /// <returns>Cosine value (between -1 and 1)</returns>
    public static decimal CosineOfAngle(LineObject.Line pLineA, LineObject.Line pLineB) {
      return CalcInstance.CosineOfAngleIn(pLineA, pLineB);
    }
    #endregion /* CalculateFlat. Public */
  }
}
