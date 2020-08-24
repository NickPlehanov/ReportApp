using Newtonsoft.Json;
using Plugin.Connectivity;
using ReportApp.Helpers;
using ReportApp.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Net.Http;
using Xamarin.Forms;

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

        private DateTime _StartDate;
        public DateTime StartDate {
            get => _StartDate;
            set {
                if (value == DateTime.Parse("01.01.2010 00:00:00"))
                    _StartDate = DateTime.Parse(DateTime.Now.ToString("dd-MM-yyyy")).AddDays(-1);
                else
                    _StartDate = value;
                OnPropertyChanged("StartDate");
            }
        }
        private DateTime _EndDate;
        public DateTime EndDate {
            get => _EndDate;
            set {
                if (value == DateTime.Parse("01.01.2010 00:00:00"))
                    _EndDate = DateTime.Parse(DateTime.Now.ToString("dd-MM-yyyy"));
                else
                    _EndDate = value;
                OnPropertyChanged("EndDate");
            }
        }
        private TimeSpan _StartTime;
        public TimeSpan StartTime {
            get => _StartTime;
            set {
                _StartTime = value;
                OnPropertyChanged("StartTime");
            }
        }
        private TimeSpan _EndTime;
        public TimeSpan EndTime {
            get => _EndTime;
            set {
                _EndTime = value;
                OnPropertyChanged("EndTime");
            }
        }

        private string _Late;
        public string Late {
            get => _Late;
            set {
                if (int.TryParse(value, out _))
                    _Late = int.Parse(value).ToString();
                else
                    _Late =null;
            }
        }
        private string _Msg;
        public string Msg {
            get => _Msg;
            set {
                _Msg = "Проведите вниз для обновления данных";
            }
        }

        private ReportAlarmExBase _SelectedReportAlarmExBase;
        public ReportAlarmExBase SelectedReportAlarmExBase {
            get => _SelectedReportAlarmExBase;
            set {
                _SelectedReportAlarmExBase = value;
                string msg =
                    SelectedReportAlarmExBase.new_address + Environment.NewLine
                    +" Опоздание: "+SelectedReportAlarmExBase.delta+Environment.NewLine
                    + " ОС:" + BoolToString(SelectedReportAlarmExBase.new_onc) + " ПС:" + BoolToString(SelectedReportAlarmExBase.new_ps) + " ТРС:" + BoolToString(SelectedReportAlarmExBase.new_tpc) + Environment.NewLine
                    + " Акт:" + BoolToString(SelectedReportAlarmExBase.new_act) + " х/о:" + BoolToString(SelectedReportAlarmExBase.new_owner) + " Полиция:" + BoolToString(SelectedReportAlarmExBase.new_police) + Environment.NewLine
                    + " Группа: " + SelectedReportAlarmExBase.new_group + Environment.NewLine
                    + " Тревога:" + DateToString(SelectedReportAlarmExBase.new_alarm_dt) + Environment.NewLine
                    + " Отправка:" + DateToString(SelectedReportAlarmExBase.new_departure) + Environment.NewLine
                    + " Прибытие:" + DateToString(SelectedReportAlarmExBase.new_arrival) + Environment.NewLine
                    + " Отмена:" + DateToString(SelectedReportAlarmExBase.new_cancel) + Environment.NewLine
                    + " Описание " + SelectedReportAlarmExBase.new_name;
                string header = " №:" + SelectedReportAlarmExBase.new_number + " " + SelectedReportAlarmExBase.new_objname + Environment.NewLine;
                Application.Current.MainPage.DisplayAlert(header, msg, "ОК");
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
                DateTime StartDT = new DateTime(StartDate.Year, StartDate.Month, StartDate.Day, StartTime.Hours, StartTime.Minutes, StartTime.Seconds);
                DateTime EndDT = new DateTime(EndDate.Year, EndDate.Month, EndDate.Day, EndTime.Hours, EndTime.Minutes, EndTime.Seconds);
                if (StartDT>=EndDT) {
                    await Application.Current.MainPage.DisplayAlert("Ошибка", "Дата и время начала и окончания не могут совпадать. Либо дата начала больше даты окончания", "ОК");
                    IsBusy = false;
                }                
                else if (StartDT>DateTime.Now) {
                    await Application.Current.MainPage.DisplayAlert("Ошибка", "Дата и время начала не могут быть больше текущего времени", "ОК");
                    IsBusy = false;
                }
                else {
                    HttpRequestMessage request = new HttpRequestMessage {
                        //RequestUri = new Uri("http://193.138.130.98:4555/Report/getreport?late=12&date=14.08.2020"),
                        //RequestUri = new Uri("http://193.138.130.98:4555/Report/getreport?late=" + Late + "&date=" + Dt + ""),
                        RequestUri = new Uri("http://193.138.130.98:4555/Report/getreport?late=" + Late + "&start=" + StartDT + "&end=" + EndDT + ""),
                        Method = HttpMethod.Get
                    };
                    request.Headers.Add("Accept", "application/json");
                    if (string.IsNullOrEmpty(Late)) {
                        await Application.Current.MainPage.DisplayAlert("Ошибка", "Укажите время опоздания", "ОК");
                        //Msg = "Укажите время опоздания";
                        IsBusy = false;
                    }
                    else {
                        if (CrossConnectivity.Current.IsConnected) {
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
                                        foreach (ReportAlarmExBase item in ser.OrderBy(x => x.new_alarm_dt)) {
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
                                                new_zone = item.new_zone,
                                                new_address = item.new_address,
                                                new_number = item.new_number,
                                                new_objname = item.new_objname,
                                                new_obj = item.new_number + " " + item.new_objname,
                                                delta = TimeSpan.FromMinutes(Convert.ToDouble(item.delta)).ToString(@"hh\:mm\:ss")
                                            });
                                            //reportAlarmExBases.Add(new ReportAlarmExBase(item.new_alarmid, item.new_name, item.new_onc, item.new_tpc, item.new_group,
                                            //    item.new_arrival, item.new_cancel, item.new_departure, item.new_andromeda_alarm, item.new_owner,
                                            //    item.new_police, item.new_order, item.new_act, item.new_alarm_dt, item.new_zone, item.new_ps,
                                            //    item.new_address, item.new_number, item.new_objname, item.new_obj
                                            //    , TimeSpan.FromMinutes(Convert.ToDouble(item.delta)).ToString(@"hh\:mm\:ss")
                                            //    ));
                                        }
                                    }
                                    else {
                                        await Application.Current.MainPage.DisplayAlert("Информация", "Объектов не найдено", "ОК");
                                        reportAlarmExBases.Clear();
                                        IsBusy = false;
                                        Msg = null;
                                    }
                                }
                                catch (Exception ex) {
                                    await Application.Current.MainPage.DisplayAlert("Ошибка", ex.Message, "ОК");
                                    IsBusy = false;
                                }
                            }
                            IsBusy = false;
                            Msg = null;
                        }
                        else {
                            await Application.Current.MainPage.DisplayAlert("Ошибка", "Проверьте подключение к интернету", "ОК");
                            IsBusy = false;
                            Msg = null;
                        }
                    }
                }
            });
        }
    }
}
