using System;
using System.Collections.Generic;
using Newtonsoft.Json;
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
using Google.Cloud.Firestore;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace RAD_assignment
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>

    public sealed partial class my_appointment : Page
    {

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
        private void Make_Appointment_Tapped(object sender, TappedRoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(make_appointment), details);
        }

        private void my_appointment_Tapped(object sender, TappedRoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(my_appointment), details);
        }
        private FirestoreDb db;
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            details = (Dictionary<string, string>)e.Parameter;
        }

        public my_appointment()
        {
            this.InitializeComponent();
            db = FirestoreDb.Create("booknow-61e27");
        }
       
        async private void Btn_CheckAppointment_Click(object sender, RoutedEventArgs e)
        {
            Query app = db.Collection("Appointment").WhereEqualTo("lecturerID", "OwlOlzd9TjV7Q3MPoDWq");
            QuerySnapshot appointmentSnapshot = await app.GetSnapshotAsync();


            textBox.Text = "";

            foreach (DocumentSnapshot Snapshot in appointmentSnapshot.Documents)
            {

                if (Snapshot.Exists)
                {
                    Dictionary<string, object> item = Snapshot.ToDictionary();
                    foreach (var field in item)
                    {
                        textBox.Text += string.Format("{0} - {1}\n", field.Key, field.Value);
                    }
                }
            }


        }
    }
}
