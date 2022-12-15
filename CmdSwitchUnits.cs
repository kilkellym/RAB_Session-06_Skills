#region Namespaces
using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using System;
using System.Collections.Generic;
using System.Diagnostics;

#endregion

namespace RAB_Session_06_Skills
{
    [Transaction(TransactionMode.Manual)]
    public class CmdSwitchUnits : IExternalCommand
    {
        public Result Execute(
          ExternalCommandData commandData,
          ref string message,
          ElementSet elements)
        {
            UIApplication uiapp = commandData.Application;
            UIDocument uidoc = uiapp.ActiveUIDocument;
            Application app = uiapp.Application;
            Document doc = uidoc.Document;

            // get current units
            Units currentUnits = doc.GetUnits();

            // create new units object
            Units newUnits = new Units(UnitSystem.Metric);

            // set format options
            FormatOptions format = new FormatOptions(UnitTypeId.Millimeters);
            format.Accuracy = 0.01;
            format.UseDigitGrouping = true;

            FormatOptions format2 = new FormatOptions(UnitTypeId.SquareMeters);
            format2.Accuracy = .000001;
            format2.UseDigitGrouping = true;

            newUnits.SetFormatOptions(SpecTypeId.Length, format);
            newUnits.SetFormatOptions(SpecTypeId.Area, format2);

            using(Transaction t = new Transaction(doc))
            {
                t.Start("Change units");


                doc.SetUnits(newUnits);


                t.Commit();
            }

            Autodesk.Revit.DB.FormatValueOptions formatvalueoptions = new Autodesk.Revit.DB.FormatValueOptions();
            formatvalueoptions.AppendUnitSymbol = false;

            Autodesk.Revit.DB.FormatOptions formatoptions = new Autodesk.Revit.DB.FormatOptions(Autodesk.Revit.DB.UnitTypeId.Currency, new Autodesk.Revit.DB.ForgeTypeId());
            formatoptions.UseDefault = false;
            formatoptions.SetUnitTypeId(Autodesk.Revit.DB.UnitTypeId.Currency);
            formatoptions.SetSymbolTypeId(new Autodesk.Revit.DB.ForgeTypeId());
            formatoptions.Accuracy = 0.01;
            //formatoptions.SuppressLeadingZeros = true;
            formatoptions.SuppressSpaces = false;
            formatoptions.SuppressTrailingZeros = false;
            formatoptions.UseDigitGrouping = true;
            formatoptions.UsePlusPrefix = false;

            formatvalueoptions.SetFormatOptions(formatoptions);


            return Result.Succeeded;
        }
    }
}
