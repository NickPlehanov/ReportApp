using ReportApp.Models;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;
using Xamarin.Forms.DataGrid;

namespace ReportApp.Helpers {
    public class RowBgProvider : IColorProvider {
        //      public Xamarin.Forms.Color IColorProvider.GetColor(int rowIndex, object item) {
        //          switch (((YourType)item)Status)
        //{
        //	case Status.Error:
        //		return Color.Red;
        //	case Status.Warning:
        //		return Color.Yellow;
        //	case Status.OK:
        //		return Color.Green;

        //              default:
        //		return Color.LightGray;
        //          }
        //      }

        Color IColorProvider.GetColor(int rowIndex, object item) {
            ReportAlarmExBase it = item as ReportAlarmExBase;
            if ((it.new_departure - it.new_alarm_dt).Value.TotalMinutes >= 3)
                return Color.Red;
            else
                return Color.WhiteSmoke;
            //throw new NotImplementedException();
        }
    }
}
