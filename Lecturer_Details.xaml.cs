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
    public sealed partial class Lecturer_Details : Page
    {
        Dictionary<string, string> details = new Dictionary<string, string>();

        private FirestoreDb db;
        public Lecturer_Details()
        {
            this.InitializeComponent();
            db = FirestoreDb.Create("booknow-61e27");

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
        private async void btn_update_Click(object sender, RoutedEventArgs e)
        {

            Query qref = db.Collection("Lecturer").WhereEqualTo("username", txb_username.Text);
            QuerySnapshot capitalQuerySnapshot = await qref.GetSnapshotAsync();

            if (capitalQuerySnapshot.Count > 0)
            {
                foreach (DocumentSnapshot documentSnapshot in capitalQuerySnapshot.Documents)
                {
                    if (documentSnapshot.Exists)
                    {
                        string documentID = documentSnapshot.Id;
                        Dictionary<string, object> data = documentSnapshot.ToDictionary();
                        foreach (KeyValuePair<string, object> pair in data)
                        {
                            Console.WriteLine("{0}: {1}", pair.Key, pair.Value);
                        }
                        Dictionary<string, string> loginId = new Dictionary<string, string>();
                        loginId.Add("lecturerId", documentID);
                        DocumentReference update = db.Collection("Lecturer").Document("OwlOlzd9TjV7Q3MPoDWq");
                        Dictionary<string, object> updates = new Dictionary<string, object>
                         {

                         {"username", txb_username.Text },
                         {"password", txb_password.Text },
                         {"university",txb_university.Text }
                        };
                        await update.UpdateAsync(updates);
                        Frame.Navigate(typeof(Lecturer_Details), loginId);
                        break;
                    }
                }
            }
            else
            {
                new ToastContentBuilder()
                    .AddText("Wrong Username Or Password", hintMaxLines: 1)
                    .AddText("Please reenter your username and password").Show();
            }
        }
    }
}
