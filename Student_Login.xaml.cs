using Google.Cloud.Firestore;
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
    public sealed partial class Student_Login : Page
    {
        FirestoreDb db;
        public Student_Login()
        {
            this.InitializeComponent();
        }
        async private void btn_login_Click(object sender, RoutedEventArgs e)
        {
            // Reference: https://cloud.google.com/dotnet/docs/reference/Google.Cloud.Firestore/latest
            //CollectionReference coll = db.Collection("UniversityAdmin");
            //Dictionary<string, object> data1 = new Dictionary<string, object>
            //{
            //    { "Username", txb_username.Text},
            //    { "Password", txb_password.Text},
            //    { "University", txb_university.Text}

            //};
            //coll.AddAsync(data1);
            //txb_university.Text = "succesfully added";
            Query qref = db.Collection("Student").WhereEqualTo("username", txb_username.Text).WhereEqualTo("password", txb_password.Text).WhereEqualTo("university", txb_university.Text);
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
                        loginId.Add("studentId", documentID);
                        Frame.Navigate(typeof(Student_Main_Page), loginId);
                        break;
                    }
                }
            }
            else
            {
                txb_username.Text = "Wrong password or username";
            }

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

        private void btn_login_Click_1(object sender, RoutedEventArgs e)
        {

        }
    }
}
