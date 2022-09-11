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
    public sealed partial class Lecturer_Login : Page
    {
        private FirestoreDb db;
        public Lecturer_Login()
        {
            this.InitializeComponent();
            db = FirestoreDb.Create("booknow-61e27");
        }
        private void adminPageLogin_Tapped(object sender, Windows.UI.Xaml.Input.TappedRoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(Admin_Login));
        }

        private void studentLogin_Tapped(object sender, Windows.UI.Xaml.Input.TappedRoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(Student_Login));
        }

        private void lecturerLogin_Tapped(object sender, Windows.UI.Xaml.Input.TappedRoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(Lecturer_Login));
        }

        private async void btn_login_Click(object sender, RoutedEventArgs e)
        {
            Query qref = db.Collection("Lecturer").WhereEqualTo("username", txb_username.Text).WhereEqualTo("password", txb_password.Text).WhereEqualTo("university", txb_university.Text);
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
