using System;
using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using System.Net;
using Org.Apache.Http.Client;
using System.Net.Sockets;
using System.IO;
using Newtonsoft.Json;
using System.Collections.Generic;
using Java.Lang;
using System.Linq;
using Android.Database;

namespace DroidApiExample
{
    [Activity(Label = "AndroidApiExample", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : Activity
    {
        int count = 1;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);

            // Get our button from the layout resource,
            // and attach an event to it
            Button button = FindViewById<Button>(Resource.Id.MyButton);
            TextView textView = FindViewById<TextView>(Resource.Id.MyTextView);
            Spinner spinner = FindViewById<Spinner>(Resource.Id.MySpinner);
            ImportedData[] dataArray = null;

            button.Click += delegate 
            {
                button.Text = string.Format("{0} clicks!", count++);
                HttpWebRequest req = WebRequest.CreateHttp("http://192.168.10.10:8052/api/start");
                req.Method = "GET";
                req.ContentType = "application/json";
                using(StreamReader sr = new StreamReader(req.GetResponse().GetResponseStream()))
                {
                    string txt = sr.ReadToEnd();
                    var dane = JsonConvert.DeserializeObject<IEnumerable<ImportedData>>(txt);
                    var adapter = new ArrayAdapter<ImportedData>(this, Android.Resource.Layout.SimpleSpinnerItem, dataArray = dane.ToArray());
                    adapter.SetDropDownViewResource(Android.Resource.Layout.SimpleDropDownItem1Line);
                    spinner.Adapter = adapter;
                    
                    System.Text.StringBuilder sb = new System.Text.StringBuilder();
                    foreach(var item in dane)
                    {
                        sb.AppendLine(item.Title);
                    }
                    textView.Text = sb.ToString();
                }
            };

            spinner.ItemSelected += (sender, e) => 
            {
                textView.Text = dataArray[e.Id].Message;
            };
        }
    }

    public class ImportedData
    {
        public string Title { get; set; }
        public string Message { get; set; }
        public DateTime Data { get; set; }

        public override string ToString()
        {
            return Title;
        }
    }


}

