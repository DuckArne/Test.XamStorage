using Plugin.Permissions;
using Plugin.Permissions.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using XamStorage;

namespace Test.XamStorage
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        async private void LocalstorageClicked(object sender, EventArgs e)
        {
            var localStorage = FileSystem.Current.LocalStorage;

            var tester = new FolderTester(localStorage);

            if(await tester.RunTest())
            {
                await Alert("LocalStorage test Passed",String.Empty);
            }
            else
            {
                await Alert("LocalStorage test Failed", tester.ErrorMessage);
            }
        }

       async private void RoamingStorageClicked(object sender, EventArgs e)
        {
            if(Device.RuntimePlatform== Device.UWP)
            {
                var roamingStorage = FileSystem.Current.RoamingStorage;

                var tester = new FolderTester(roamingStorage);

                if (await tester.RunTest())
                {
                    await Alert("RoamingStorage test Passed", String.Empty);
                }
                else
                {
                    await Alert("RoamingStorage Failed", tester.ErrorMessage);
                }
            }
            else
            {
                await DisplayAlert(null, "RoamingStorage is only used in UWP.", "OK");
            }
        }

        async private void PersonalStorageClicked(object sender, EventArgs e)
        {
            var personalStorage = FileSystem.Current.PersonalStorage;
            var tester = new FolderTester(personalStorage);

            if (await tester.RunTest())
            {
                await Alert("PersonalStorage test Passed", String.Empty);
            }
            else
            {
                await Alert("PersonalStorage Failed", tester.ErrorMessage);
            }
        }

        async private void DocumentsFolderStorageClicked(object sender, EventArgs e)
        {

            if (!await CheckPermissions(Permission.Storage))
                return;
            
            var documentsStorage = await FileSystem.Current.DocumentsFolderAsync();

            var tester = new FolderTester(documentsStorage);

            if (await tester.RunTest())
            {
                await Alert("DocumentsStorage test Passed", String.Empty);
            }
            else
            {
                await Alert("DocumentsStorage Failed", tester.ErrorMessage);
            }
        }

      

        async private void MusicFolderStorageClicked(object sender, EventArgs e)
        {
            if (!await CheckPermissions(Permission.Storage))
                return;

            var musicStorage = await FileSystem.Current.MusicFolderAsync();
            var tester = new FolderTester(musicStorage);
            if (await tester.RunTest())
            {
                await Alert("MusicStorage test Passed", String.Empty);
            }
            else
            {
                await Alert("MusicStorage Failed", tester.ErrorMessage);
            }
        }

        async private void PicturesFolderStorageClicked(object sender, EventArgs e)
        {
            if (!await CheckPermissions(Permission.Storage))
                return;

            var picturesStorage = await FileSystem.Current.PicturesFolderAsync();
            var tester = new FolderTester(picturesStorage);

            if (await tester.RunTest())
            {
                await Alert("PicturesStorage test Passed", String.Empty);
            }
            else
            {
                await Alert("PicturesStorage Failed", tester.ErrorMessage);
            }

        }

       async private void VideosFolderStorageClicked(object sender, EventArgs e)
        {
            if (!await CheckPermissions(Permission.Storage))
                return;


            var videosStorage = await FileSystem.Current.VideosFolderAsync();
            var tester = new FolderTester(videosStorage);

            if (await tester.RunTest())
            {
                await Alert("VideosStorage test Passed", String.Empty);
            }
            else
            {
                await Alert("VideosStorage Failed", tester.ErrorMessage);
            }
        }

        async private Task Alert(string message,string errorMessage)
        {
            await DisplayAlert(null, message + "\n\n" + errorMessage, "OK");
        }


        async private Task<bool> CheckPermissions(Permission permission)
        {
            var status = await CrossPermissions.Current.CheckPermissionStatusAsync(permission);

            if (status != PermissionStatus.Granted)
            {
                if (!await CrossPermissions.Current.ShouldShowRequestPermissionRationaleAsync(permission))
                {
                    await DisplayAlert("Needs permission", "This app needs permission " + permission.ToString() + " to properly function", "OK");
                }

                var results = await CrossPermissions.Current.RequestPermissionsAsync(permission);
                //Best practice to always check that the key exists
                if (results.ContainsKey(permission))
                    status = results[permission];
            }

            if (status == PermissionStatus.Granted)
            {
                return true;
            }
            else if (status != PermissionStatus.Unknown)
            {
                await DisplayAlert("Error with the Permission", "Can not continue, try again.", "OK");
                return false;
            }
            return false;
        }
    }
}
