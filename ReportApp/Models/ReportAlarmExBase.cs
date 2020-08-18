using System;
using System.Collections.Generic;
using System.Text;

namespace ReportApp.Models {
    public class ReportAlarmExBase {
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
    }
}
