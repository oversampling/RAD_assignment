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
using Google.Cloud.Firestore;
using Microsoft.Toolkit.Uwp.Notifications;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace RAD_assignment
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class Lecturer_Appointment : Page
    {
        Dictionary<string, string> details = new Dictionary<string, string>();
        

        private FirestoreDb db;
        public Lecturer_Appointment()
        {
            this.InitializeComponent();
            string path = System.AppDomain.CurrentDomain.BaseDirectory + @"booknow.json";
            System.Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS", path);
            db = FirestoreDb.Create("booknow-61e27");
            


        }

        private async void showAppointment_Click(object sender, RoutedEventArgs e)
        {
            
            Query app = db.Collection("Appointment").WhereEqualTo("lecturerID", details["lecturerId"]);
            QuerySnapshot appointmentSnapshot = await app.GetSnapshotAsync();


            txb_appointment.Text = "";

            foreach (DocumentSnapshot Snapshot in appointmentSnapshot.Documents)
            {

                if (Snapshot.Exists)
                {
                    Dictionary<string, object> item = Snapshot.ToDictionary();
                    foreach (var field in item)
                    {
                        txb_appointment.Text += string.Format("{0} - {1}\n", field.Key, field.Value);
                    }
                }
            }
            
                
            
        }
        private async void acceptAppointment_Click(object sender, RoutedEventArgs e)
        {
            DocumentReference update = db.Collection("Lecturer").Document(details["lecturerId"]);
            Dictionary<string, object> updates = new Dictionary<string, object>

            {

                    {"studentID", txb_student.Text},
                    {"status",  "Accept"}

            };
            await update.UpdateAsync(updates);
            
        }
        private async void declineAppointment_Click(object sender, RoutedEventArgs e)
        {
            DocumentReference update = db.Collection("Lecturer").Document(details["lectureId"]);
            Dictionary<string, object> updates = new Dictionary<string, object>
            {
                {"studentID", txb_student.Text},
                {"status",  "Decline"}
                
            };
            await update.UpdateAsync(updates);
        }

        private async void pendingAppointment_Click(object sender, RoutedEventArgs e)
        {
            DocumentReference update = db.Collection("Lecturer").Document(details["lecturerId"]);
            Dictionary<string, object> updates = new Dictionary<string, object>
            {
                {"studentID", txb_student.Text},
                {"status", "Pending" }
                
            };
            await update.UpdateAsync(updates);
        }

        private void link_lecturerDetail_Tapped(object sender, TappedRoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(Lecturer_Details), details);
        }
        private void link_myAppointment_Tapped(object sender, TappedRoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(Lecturer_Appointment), details);
        }
        private void link_mySchedule_Tapped(object sender, TappedRoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(Lecturer_Schedule), details);
        }

        private void link_chat_Tapped(object sender, TappedRoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(Lecturer_Chat), details);
        }

        private void link_logout_Tapped(object sender, TappedRoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(Lecturer_Login), details);
        }
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            details = (Dictionary<string, string>)e.Parameter;
        }

       
    }
}
