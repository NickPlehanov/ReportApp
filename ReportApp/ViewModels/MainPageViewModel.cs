using Newtonsoft.Json;
using ReportApp.Helpers;
using ReportApp.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Text;
using Xamarin.Forms;
using Xamarin.Forms.DataGrid;

namespace ReportApp.ViewModels {
    class MainPageViewModel : BaseViewModel {
        private ObservableCollection<ReportAlarmExBase> _reportAlarmExBases = new ObservableCollection<ReportAlarmExBase>();
        public ObservableCollection<ReportAlarmExBase> reportAlarmExBases {
            get => _reportAlarmExBases;
            set {
                _reportAlarmExBases = value;
                OnPropertyChanged("ReportAlarmExBase");
            }
        }

        private string _ErrMessage;
        public string ErrMessage {
            get => _ErrMessage;
            set {
                _ErrMessage = value;
                OnPropertyChanged("ErrMessage");
            }
        }

        private bool _IsBusy;
        public bool IsBusy {
            get => _IsBusy;
            set {
                _IsBusy = value;
                OnPropertyChanged("IsBusy");
            }
        }

        private DateTime _Date;
        public DateTime Dt {
            get => _Date;
            set {
                if (value == DateTime.Parse("01.01.1900 00:00:00"))
                    _Date = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
                else
                    _Date = value;
                //string dt = value.ToString(CultureInfo.InvariantCulture);
                //if (DateTime.TryParse(value, out _))
                //    _Date = DateTime.Parse(value).ToShortDateString();
                //else
                //    _Date = DateTime.Now.ToShortDateString();
            }
        }
        private string _Late;
        public string Late {
            get => _Late;
            set {
                if (int.TryParse(value, out _))
                    _Late = int.Parse(value).ToString();
                else
                    _Late = "0";
            }
        }

        private ReportAlarmExBase _SelectedReportAlarmExBase;
        public ReportAlarmExBase SelectedReportAlarmExBase {
            get => _SelectedReportAlarmExBase;
            set {
                _SelectedReportAlarmExBase = value;
                string msg = " ОС:" + BoolToString(SelectedReportAlarmExBase.new_onc) + " ПС:" + BoolToString(SelectedReportAlarmExBase.new_ps) + " ТРС:" + BoolToString(SelectedReportAlarmExBase.new_tpc) + Environment.NewLine
                    + " Акт:" + BoolToString(SelectedReportAlarmExBase.new_act) + " х/о:" + BoolToString(SelectedReportAlarmExBase.new_owner) + " Полиция:" + BoolToString(SelectedReportAlarmExBase.new_police) + Environment.NewLine
                    + " Группа: " + SelectedReportAlarmExBase.new_group + Environment.NewLine
                    + " Тревога:" + DateToString(SelectedReportAlarmExBase.new_alarm_dt) + Environment.NewLine
                    + " Отправка:" + DateToString(SelectedReportAlarmExBase.new_departure) + Environment.NewLine
                    + " Прибытие:" + DateToString(SelectedReportAlarmExBase.new_arrival) + Environment.NewLine
                    + " Отмена:" + DateToString(SelectedReportAlarmExBase.new_cancel);
                Application.Current.MainPage.DisplayAlert("Информация", msg, "ОК", "Отмена");
                _SelectedReportAlarmExBase = null;
                OnPropertyChanged("SelectedReportAlarmExBase");
            }
        }

        private string BoolToString(bool? value) {
            return (bool)value ? "+" : "-";
        }
        private string DateToString(DateTime? value) {
            if (value.HasValue)
                return value.Value.ToShortTimeString();
            else
                return null;
        }

        private RelayCommand _GetReport;
        public RelayCommand GetReport {
            get => _GetReport ??= new RelayCommand(async obj => {
                IsBusy = true;
                HttpClient client = new HttpClient();
                HttpRequestMessage request = new HttpRequestMessage {
                    //RequestUri = new Uri("http://193.138.130.98:4555/Report/getreport?late=12&date=14.08.2020"),
                    RequestUri = new Uri("http://193.138.130.98:4555/Report/getreport?late=" + Late + "&date=" + Dt + ""),
                    Method = HttpMethod.Get
                };
                request.Headers.Add("Accept", "application/json");

                HttpResponseMessage response = await client.SendAsync(request);
                if (response.StatusCode == HttpStatusCode.OK) {
                    HttpContent responseContent = response.Content;
                    var json = await responseContent.ReadAsStringAsync();
                    string s = json.Replace(@"\", string.Empty);
                    string final = s.Trim().Substring(1, (s.Length) - 2);
                    try {
                        List<ReportAlarmExBase> ser = JsonConvert.DeserializeObject<List<ReportAlarmExBase>>(final);
                        if (ser.Any()) {
                            reportAlarmExBases.Clear();
                            foreach (ReportAlarmExBase item in ser) {
                                reportAlarmExBases.Add(new ReportAlarmExBase() {
                                    new_act = item.new_act,
                                    new_alarmid = item.new_alarmid,
                                    new_alarm_dt = item.new_alarm_dt,
                                    new_andromeda_alarm = item.new_andromeda_alarm,
                                    new_arrival = item.new_arrival,
                                    new_cancel = item.new_cancel,
                                    new_departure = item.new_departure,
                                    new_group = item.new_group,
                                    new_name = item.new_name,
                                    new_onc = item.new_onc,
                                    new_order = item.new_order,
                                    new_owner = item.new_owner,
                                    new_police = item.new_police,
                                    new_ps = item.new_ps,
                                    new_tpc = item.new_tpc,
                                    new_zone = item.new_zone
                                });
                            }
                        }
                        //await PushAsync(new MenuPage());
                    }
                    catch (Exception ex) {
                        ErrMessage = ex.Message;
                    }
                }
                IsBusy = false;
            });
        }
    }
}
