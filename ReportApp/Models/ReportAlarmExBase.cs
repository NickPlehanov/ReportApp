using Newtonsoft.Json.Schema;
using System;
using System.Collections.Generic;
using System.Text;

namespace ReportApp.Models {
    public class ReportAlarmExBase {
        //public ReportAlarmExBase(Guid? new_alarmid, string new_name, bool? new_onc, bool? new_tpc, int? new_group, DateTime? new_arrival, DateTime? new_cancel, DateTime? new_departure, string new_andromeda_alarm, bool? new_owner, bool? new_police, bool? new_order, bool? new_act, DateTime? new_alarm_dt, string new_zone, bool? new_ps, string new_address, string new_number, string new_objname, string new_obj, string delta) {
        //    this.new_alarmid = new_alarmid;
        //    this.new_name = new_name;
        //    this.new_onc = new_onc;
        //    this.new_tpc = new_tpc;
        //    this.new_group = new_group;
        //    this.new_arrival = new_arrival;
        //    this.new_cancel = new_cancel;
        //    this.new_departure = new_departure;
        //    this.new_andromeda_alarm = new_andromeda_alarm;
        //    this.new_owner = new_owner;
        //    this.new_police = new_police;
        //    this.new_order = new_order;
        //    this.new_act = new_act;
        //    this.new_alarm_dt = new_alarm_dt;
        //    this.new_zone = new_zone;
        //    this.new_ps = new_ps;
        //    this.new_address = new_address;
        //    this.new_number = new_number;
        //    this.new_objname = new_objname;
        //    this.new_obj = new_obj;
        //    this.delta = delta;
        //}

        public Guid? new_alarmid { get; set; }
        public string new_name { get; set; }
        public bool? new_onc { get; set; }
        public bool? new_tpc { get; set; }
        public int? new_group { get; set; }
        public DateTime? new_arrival { get; set; }
        public DateTime? new_cancel { get; set; }
        public DateTime? new_departure { get; set; }
        public string new_andromeda_alarm { get; set; }        
        public bool? new_owner { get; set; }
        public bool? new_police { get; set; }
        public bool? new_order { get; set; }
        public bool? new_act { get; set; }
        public DateTime? new_alarm_dt { get; set; }
        public string new_zone { get; set; }
        public bool? new_ps { get; set; }
        public string new_address { get; set; }
        public string new_number { get; set; }
        public string new_objname { get; set; }
        public string new_obj { get; set; }
        public string delta { get; set; }
        //private string _delta { get; set; }
        //public string delta { 
        //    get => _delta;
        //    set {
        //        if (Double.TryParse(value, out _)) {
        //            TimeSpan span = TimeSpan.FromMinutes(Convert.ToDouble(value));
        //            _delta = span.ToString(@"hh\:mm\:ss");
        //        }
        //        else
        //            _delta = null;
        //    }
        //}
    }
}
