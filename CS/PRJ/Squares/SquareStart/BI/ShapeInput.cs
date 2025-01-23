using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using SquareMetric.BO;
using NAngle = SquareMetric.BO.SquareObject.BasePolygon;
using Punct = SquareMetric.BO.LineObject.Point;

namespace SquareStart.BI
{
  /// <summary>Input geometry figures</summary>
  internal class ShapeInput {
    #region... /* ShapeInput. Instance */
    /// <summary>Class instance. Private</summary>
    static ShapeInput ShInstanceIn { get; set;}

    /// <summary>Class instance. Private</summary>
    public static ShapeInput ShInstance { get {
      if (ShInstanceIn == null) { ShInstanceIn = new ShapeInput();}
      return ShInstanceIn;
    }}
    #endregion /* ShapeInput. Instance */

    #region... /* ShapeInput. Fields */
    /// <summary>Error text. Private</summary>
    string ErrorTextIn { get; set;}

    /// <summary>Error text. Private</summary>
    public static string ErrorText => ShInstance.ErrorTextIn;
    #endregion /* ShapeInput. Fields */

    #region... /* ShapeInput. Private */
    /// <summary>Input geometry figure coordinates. Private</summary>
    /// <param name="pInputDoc">Input XML document</param>
    /// <returns>Input files in account</returns>
    IEnumerable<NAngle> ListShapesIn(string pInputDoc) {
      // Load XML document
      var xInput = XDocument.Load(pInputDoc);
      Console.WriteLine("List of shapes will be loaded from '{0}'", pInputDoc);

      // Get files
      foreach (XElement xe in xInput.Element("shapes").Elements("shape")
          .Where(xe => xe.Attributes("in").Count() == 0 || xe.Attribute("in").Value == "1")) {
        // Input next figure
        List<Punct> puncts = new List<Punct>();
        foreach (XElement xe1 in xe.Elements("point")
            .OrderBy(xe1 => Convert.ToInt32(xe1.Attribute("num").Value))) {
          // Append next point to list
          puncts.Add(new Punct(
            Convert.ToDecimal(xe1.Attribute("x").Value),  Convert.ToDecimal(xe1.Attribute("y").Value)));
        }

        // Create figure by list of points. Triangle or polyangle
        if (puncts.Count() == 3) {
          yield return new SquareObject.Triangle(puncts);
        } else {
          yield return new SquareObject.Polygon(puncts);
        }
      }
      xInput = null; // Close XML document
    }
    #endregion /* ShapeInput. Private */

    #region... /* ShapeInput. Public */
    /// <summary>Constructor</summary>
    public ShapeInput() {}

    /// <summary>Input geometry figure coordinates</summary>
    /// <param name="pInputDoc">Input XML document</param>
    /// <returns>Input files in account</returns>
    public static IEnumerable<NAngle> ListShapes(string pInputDoc) {
      foreach (NAngle na in ShInstance.ListShapesIn(pInputDoc)) { yield return na;}
    }
    #endregion /* ShapeInput. Public */
  }
}
