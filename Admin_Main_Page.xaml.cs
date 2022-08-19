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
    public sealed partial class Admin_Main_Page : Page
    {
        private Dictionary<string, string> details;
        public Admin_Main_Page()
        {
            this.InitializeComponent();
        }
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            details = (Dictionary<string, string>)e.Parameter;
        }

        private void link_chat_Tapped(object sender, TappedRoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(Admin_All_Chat), details);
        }

        private void link_add_lecturer_Tapped(object sender, TappedRoutedEventArgs e)
        {

        }

        private void link_add_student_Tapped(object sender, TappedRoutedEventArgs e)
        {

        }

        private void link_lecturerDetail_Tapped(object sender, TappedRoutedEventArgs e)
        {

        }

        private void link_studentDetail_Tapped(object sender, TappedRoutedEventArgs e)
        {

        }
    }
}
