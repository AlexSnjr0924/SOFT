using System;
using System.Collections.Generic;
using System.Linq;
using static System.Net.Mime.MediaTypeNames;

namespace SquareMetric.BL {
  /// <summary>Math operations for geometrical figures</summary>
  public static class MathAccount {
    #region... /* SquareMath. Const. Internal */
    /// <summary>Max possible degree of 10, type "decimal"</summary>
    internal const int Max_Dgr = 28;
    #endregion /* SquareMath. Const. Internal */

    #region... /* SquareMath. Const */
    /// <summary>Foundamental math constant Pi</summary>
    public const decimal Numeric_Pi = 3.1415926535897932384626433833m; // 3.14159265358979323846264338327950288419716939937510582097

    /// <summary>Array of Rome figires for quarters</summary>
    public static readonly string[] Quart_Sign = {"O", "I", "II", "III", "IV"};
    #endregion /* SquareMath. Const */

    #region... /* SquareMath. Values */
    /// <summary>Result of previous operation: imagine (complex) or not</summary>
    public static bool LastImagine;
    #endregion /* SquareMath. Values */

    #region... /* SquareMath. Private */
    /// <summary>Static constructor</summary>
    static MathAccount() {}
    #endregion /* SquareMath. Private */

    #region... /* SquareMath. Operations */
    /// <summary>Absolute value of decimal number</summary>
    /// <param name="pValue">Start value</param>
    /// <returns>Non-negative decimal value</returns>
    public static decimal Abs(decimal pValue) { return pValue < 0 ? -pValue : pValue;}

    /// <summary>Extract square root from numeric value (more high precision, than double)</summary>
    /// <param name="pValue">Start value (the square)</param>
    /// <returns>Decimal square root</returns>
    public static decimal Root2(decimal pValue) {
      // Type of result
      LastImagine = pValue < 0;
      decimal startVal = LastImagine ? -pValue : pValue;
      decimal result = 0m;

      // Represent 
      decimal calcVal = startVal;
      string s = Convert.ToString(calcVal);
      int pos = s.IndexOf(".");

      // Remove final zeroes
      int l = s.Length;
      if (pos >= 0 && pos < l) { while (s[l - 1] == '0') { l--;}}

      // Transform start value to integer
      int pcnt = (l - pos) / 2;
      for (int i = 0; i < pcnt; i++) { calcVal *= 100m;}
      int shft = pcnt;

      // Place digits by pairs
      List<int> dgtStack = new List<int>();
      decimal tail = calcVal;
      while (tail >= 1m) {
        int place = (int)(tail % 100.0m);
        dgtStack.Add(place);
        tail = (tail - place) / 100m;
      }

      // Get possible digits of square root
      int idx = dgtStack.Count - 1, firstDgt = 0;
      tail = 0;
      for (int i = 0; i < Max_Dgr; i++) {
        // Digit shift
        if (idx < 0) { shft++;}
        tail = 100 * tail + (idx >= 0 ? dgtStack[idx] : 0);

        // Search next digit
        int crDgt = 0;
        while (20.0m * result * crDgt + crDgt * crDgt <= tail && crDgt <= 9) { crDgt++;}
        crDgt--;

        // Values for next iteration
        tail -= 20.0m * result * crDgt + crDgt * crDgt;
        result = 10.0m * result + crDgt;
        idx--;

        // First digit of result
        if (i == 0) { firstDgt = crDgt;}
        if ((firstDgt >= 5 && i == Max_Dgr - 3) || (firstDgt >= 3 && i == Max_Dgr - 2)) { break;}
      }

      // Move square root to real value
      for (int i = 0; i < shft; i++) { result /= 10m;}
      return result;
    }

    /// <summary>Calculate distance betweem 2 points on 2-dim flat</summary>
    /// <param name="pX1">Point 1. Coordinate X</param>
    /// <param name="pY1">Point 1. Coordinate Y</param>
    /// <param name="pX2">Point 2. Coordinate X</param>
    /// <param name="pY2">Point 2. Coordinate Y</param>
    public static decimal Distance(decimal pX1, decimal pY1, decimal pX2, decimal pY2) {
      // Projections on axes X,Y
      var prjX = pX2 - pX1;
      var prjY = pY2 - pY1;

      // Some partial cases
      if (pX1 == pX2) { return pY2 > pY1 ? prjY : -prjY;}
      if (pY1 == pY2) { return pX2 > pX1 ? prjX : -prjX;}

      // Length (distance between extreme points) of finite line
      return Root2(prjX * prjX + prjY * prjY);
    }

    /// <summary>Calculate determinant of matrix (2x2)</summary>
    /// <param name="pValues">Array of elements: |[a11, a12], [a21, a22]|</param>
    /// <returns>Matrix determinant</returns>
    public static decimal Determinant2(decimal[] pValues) { return pValues[0] * pValues[3] - pValues[1] * pValues[2];}

    /// <summary>Calculate vector product of 2 vectors (L1 * L2 * sin L1^L2)</summary>
    /// <param name="pX1">Vector1. Coordinate X</param>
    /// <param name="pY1">Vector1. Coordinate Y</param>
    /// <param name="pX2">Vector2. Coordinate X</param>
    /// <param name="pY2">Vector2. Coordinate Y</param>
    /// <returns>Vector product, based on determinant |(X1, Y1), (X2, Y2)|</returns>
    public static decimal VectorProduct(decimal pX1, decimal pY1, decimal pX2, decimal pY2) {
      return Determinant2(new decimal[] { pX1, pY1, pX2, pY2 });
    }

    /// <summary>Calculate scalar product of vector parts (L1 * L2 * cos L1^L2)</summary>
    /// <param name="pX1">Vector1. Coordinate X</param>
    /// <param name="pY1">Vector1. Coordinate Y</param>
    /// <param name="pX2">Vector2. Coordinate X</param>
    /// <param name="pY2">Vector2. Coordinate Y</param>
    /// <returns>Scalar product, sum of pair coordinate products = X1 * X2 + Y1 * Y2</returns>
    public static decimal ScalarProduct(decimal pX1, decimal pY1, decimal pX2, decimal pY2) { return pX1 * pX2 + pY1 * pY2;}
    #endregion /* SquareMath. Operations */
  }
}
