using Google.Cloud.Firestore;
using Microsoft.Toolkit.Uwp.Notifications;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace RAD_assignment
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class make_appointment : Page
    {
        FirestoreDb db;
        Dictionary<string, string> details = new Dictionary<string, string>();


        private void link_studentDetail_Tapped(object sender, TappedRoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(Student_Main_Page), details);
        }

        private void link_SearchScheduleDetails_Tapped(object sender, TappedRoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(Student_Search_Schedules), details);
        }
        private void link_chat_Tapped(object sender, TappedRoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(Student_Chat), details);
        }
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            details = (Dictionary<string, string>)e.Parameter;
        }
        private void Make_Appointment_Tapped(object sender, TappedRoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(make_appointment), details);
        }

        private void my_appointment_Tapped(object sender, TappedRoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(my_appointment), details);
        }
        public make_appointment()
        {

            this.InitializeComponent();
            string path = System.AppDomain.CurrentDomain.BaseDirectory + @"booknow.json";
            System.Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS", path);
            db = FirestoreDb.Create("booknow-61e27");
            if (db != null)
            {
                txb_title.Text = "Success";

            }
        }
        private void TextBlock_SelectionChanged(object sender, RoutedEventArgs e)
        {

        }

        async private void Btn_MakeAppointment_Click(object sender, RoutedEventArgs e)
        {
            Dictionary<string, object> updates = new Dictionary<string, object>
            {
                {"studentID",details["studentId"]},
                { "lecturerID", lecturer_id.Text },
                 {"title", txb_title.Text },
                 {"schedule", "7 July 2022, 0600" },
                 {"status", "pending" },
            };
            DocumentReference update = await db.Collection("Appointment").AddAsync(updates);
        }

    }
}
