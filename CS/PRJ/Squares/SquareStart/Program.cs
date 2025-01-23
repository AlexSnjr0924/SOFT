using System;
using SquareStart.BI;
using CM = System.Configuration.ConfigurationManager;
using NAngle = SquareMetric.BO.SquareObject.BasePolygon;

namespace SquareStart
{
  /// <summary>Unit test for geometry figures</summary>
  internal class Program {
     #region... /* Program. Fields */
    /// <summary>Error text. Private</summary>
    static string ErrorTextIn { get; set;}

    /// <summary>Error text</summary>
    public static string ErrorText => ErrorTextIn;
    #endregion /* Program. Fields */

    #region... /* Program. Private */
    /// <summary>Input set of geometric figures. Account squares</summary>
    static bool LoadFigures() {
      bool result = true;
      try {
        // Input values. Vars
        string docInput = string.Format(@"{0}\{1}", CM.AppSettings["pathInput"], CM.AppSettings["docInput"]);
        int counter = 0;

        // Input values
        foreach (NAngle na in ShapeInput.ListShapes(docInput)) {
          // Info about next figure
          counter++;
          Console.WriteLine("-- Figure {0}. Sides = {1}", counter, na.FPoints.Length);

          // Info. Points
          for (int i = 0; i < na.FPoints.Length; i++) {
            Console.WriteLine("  Line {0}. ({1}, {2}) - ({3}, {4}). |{5}|"
              , i + 1, na.FSides[i].PointA.X, na.FSides[i].PointA.Y, na.FSides[i].PointB.X, na.FSides[i].PointB.Y
              , na.FSides[i].Length);
          }

          // Info. Figure properties
          Console.WriteLine("  Square = {0} (Perimeter = {1})\r\n", na.Square, na.Perimeter);
        }
      } catch (Exception ex) {
        ErrorTextIn = ex.Message;
        result = false;
      }
      return result;
    }

    /// <summary>Program entry point</summary>
    /// <param name="args">Command line arguments</param>
    [STAThread]
    static void Main(string[] args) {
      try {
        // Start
        Console.Title = "Calculate squares of sharp";
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine("{0} START_ {0}", new string('_', 48));

        // Main action
        if (!LoadFigures()) { throw new Exception(ErrorText);}

        // Finish
        Console.WriteLine("{0} FINISH {0}", new string('_', 48));
      } catch (Exception ex) {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine("PROGRAM ERROR:\r\n{0}", ex.Message);
      } finally {
        // Wait user reaction
        Console.ForegroundColor = ConsoleColor.White;
        if (CM.AppSettings["prgWait"] == "1") {
          Console.WriteLine("Press <Enter>");
          Console.ReadLine();
        }
      }
    }
    #endregion /* Program. Private */
  }
}
