﻿using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Google.Cloud.Firestore;
using System.Collections.Generic;
using System;
using Microsoft.Toolkit.Uwp.Notifications;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace RAD_assignment
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class Admin_Login : Page
    {
        FirestoreDb db;
        public Admin_Login()
        {
            this.InitializeComponent();
            string path = System.AppDomain.CurrentDomain.BaseDirectory + @"booknow.json";
            System.Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS", path);
            db = FirestoreDb.Create("booknow-61e27");
            txb_password.Text = "Success";
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
            Query qref = db.Collection("Admin").WhereEqualTo("username", txb_username.Text).WhereEqualTo("password", txb_password.Text).WhereEqualTo("university", txb_university.Text);
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
                        loginId.Add("adminId", documentID);
                        Frame.Navigate(typeof(Admin_Main_Page), loginId);
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
    }
}
