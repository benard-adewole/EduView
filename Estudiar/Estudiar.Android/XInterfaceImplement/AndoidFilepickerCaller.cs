using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Estudiar.XInterface;

using Android.App;
using Android.Net;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.Provider;
using Xamarin.Forms;
using System.Threading.Tasks;
using Estudiar.Droid;
using Estudiar.Models;
using System.IO;
using Estudiar.Droid.Helpers;
using Estudiar.Droid.XInterfaceImplement;

[assembly: Dependency(typeof(AndoidFilepickerCaller))]
namespace Estudiar.Droid.XInterfaceImplement
{ 
    public class AndoidFilepickerCaller:FilePickCaller
    {
        private string AppDirImgFolder = Xamarin.Essentials.FileSystem.AppDataDirectory;
        public AndoidFilepickerCaller()
        {
            AppDirImgFolder = CreateDirectory(AppDirImgFolder,"Images");
        }
        public string CreateDirectory(string MainPath, string DirectoryName)
        {
            var directoryPath = Path.Combine(MainPath, DirectoryName);
            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }
            return directoryPath;
            //if (Android.OS.Environment.MediaMounted.Equals(Android.OS.Environment.ExternalStorageState))
        }
        public async Task<byte[]> CompressImageAsync(byte[] Mybyte, float Width, float Height)
        {
            Android.Graphics.Bitmap OriginalImage = await Android.Graphics.BitmapFactory.DecodeByteArrayAsync(Mybyte,0, Mybyte.Length);
            Android.Graphics.Bitmap resizedImage = Android.Graphics.Bitmap.CreateScaledBitmap(OriginalImage, (int)Width, (int)Height, false);
            using (MemoryStream ms = new MemoryStream())
            {
                resizedImage.Compress(Android.Graphics.Bitmap.CompressFormat.Png, 100, ms);
                return ms.ToArray();
            }
        }
        public String getFileName(Android.Net.Uri uri)
        {
            String result = null;

            //if scheme is content
            if (uri.Scheme =="content")
            {
                Android.Database.ICursor cursor = MainActivity.GetMainActivity.ContentResolver.Query(uri, null, null, null, null);
                
                try
                {
                    if (cursor != null && cursor.MoveToFirst())
                    {
                        result = cursor.GetString(cursor.GetColumnIndex(OpenableColumns.DisplayName));
                    }
                }
                finally
                {
                    cursor.Close();
                }
            }
            //if scheme is file
            if (result == null)
            {
                result = uri.Path;
                int cut = result.LastIndexOf('/');
                if (cut != -1)
                {
                    result = result.Substring(cut + 1);
                }
            }
            return result;
        }
        public async Task<List<IncomingFile>> GetFileStreams()
        {
            List<Stream> images = new List<Stream>();
            var listener = new ActivityResultListener(MainActivity.GetMainActivity);

            var intent = new Intent();
            //string num = "Can't get";
            intent.SetType("application/pdf");
            intent.PutExtra(Intent.ExtraAllowMultiple, true);
            intent.SetAction(Intent.ActionGetContent);
            MainActivity.GetMainActivity.StartActivityForResult(Intent.CreateChooser(intent, "Select Pdf(s)"), 1);


            await listener.TaskDone;
            if (listener.TaskDone.Result == true)
            {
                if (listener.ActivityMessageObtained.MyIntent != null)
                {
                    List<IncomingFile> streams = new List<IncomingFile>();
                    //If multiple files are picked
                    if (listener.ActivityMessageObtained.MyIntent.ClipData != null)
                    {
                        
                        for (int i = 0; i < listener.ActivityMessageObtained.MyIntent.ClipData.ItemCount; i++)
                        {
                            Android.Net.Uri uri = listener.ActivityMessageObtained.MyIntent.ClipData.GetItemAt(i).Uri;
                            Stream stream = MainActivity.GetMainActivity.ContentResolver.OpenInputStream(uri);
                            streams.Add(new IncomingFile { Stream = stream, fileName= getFileName(uri)});
                            

                        }
                        return streams;
                    }
                    //If single files are picked
                    else if (listener.ActivityMessageObtained.MyIntent.Data != null)
                    {
                        Android.Net.Uri uri = listener.ActivityMessageObtained.MyIntent.Data;
                        Stream stream = MainActivity.GetMainActivity.ContentResolver.OpenInputStream(uri);
                        streams.Add(new IncomingFile { Stream = stream, fileName = uri.LastPathSegment });
                        return streams;
                    }

                    /*for (int i = 0; i < listener.ActivityMessageObtained.MyIntent.ClipData.ItemCount; i++)
                    {
                        Android.Net.Uri uri = listener.ActivityMessageObtained.MyIntent.ClipData.GetItemAt(i).Uri;
                        Stream stream = MainActivity.GetMainActivity.ContentResolver.OpenInputStream(uri);
                        images.Add(stream);
                        return stream;
                    }*/
                }
                return null;
            }
            return null;

        }

        public async Task<IncomingFile> GetFileStream()
        {
            List<Stream> images = new List<Stream>();
            var listener = new ActivityResultListener(MainActivity.GetMainActivity);

            var intent = new Intent();
            //string num = "Can't get";
            intent.SetType("application/pdf");
            intent.PutExtra(Intent.ExtraAllowMultiple, false);
            intent.SetAction(Intent.ActionGetContent);
            MainActivity.GetMainActivity.StartActivityForResult(Intent.CreateChooser(intent, "Select Pdf(s)"), 1);


            await listener.TaskDone;
            if (listener.TaskDone.Result == true)
            {
                if (listener.ActivityMessageObtained.MyIntent != null)
                {
                    //If single files are picked
                    Android.Net.Uri uri = listener.ActivityMessageObtained.MyIntent.Data;
                    Stream stream = MainActivity.GetMainActivity.ContentResolver.OpenInputStream(uri);
                    //streams.Add(new IncomingFile { Stream = stream, filePath = uri.LastPathSegment });
                    return new IncomingFile { Stream = stream};
                }
                return null;
            }
            return null;

        }
        public async Task<object[]> PickCopyStreamAsync()
        {

            var listener = new ActivityResultListener(MainActivity.GetMainActivity);
            
            var intent = new Intent();
            intent.SetType("text/*");
            string[]  mimeTypes = { "text/html", "text/plain"};
            intent.PutExtra(Intent.ExtraMimeTypes, mimeTypes);
            intent.PutExtra(Intent.ExtraAllowMultiple, false);
            intent.SetAction(Intent.ActionGetContent);
            MainActivity.GetMainActivity.StartActivityForResult(Intent.CreateChooser(intent, "Select File(\".txt\", \".html\")"),1);

            await listener.TaskDone;
            if (listener.TaskDone.Result == true)
            {
                if (listener.ActivityMessageObtained.MyIntent != null)
                {
                    Android.Net.Uri uri = listener.ActivityMessageObtained.MyIntent.Data;
                    return new object[]{ MainActivity.GetMainActivity.ContentResolver.OpenInputStream(uri), uri.Path};
                }
                return null;
            }
            return null;
        }
        private string SavableName(string Dir ,string Extension = "")
        {
            int i = 0;
            while (File.Exists(Path.Combine(Dir,i.ToString()+"."+Extension)))
            {
                i++;
            }
            return Path.Combine(Dir, i.ToString() + "." + Extension);
        }
        public async Task<List<string>> GetFiles()
        {
            List<string> images = new List<string>();
            var listener = new ActivityResultListener(MainActivity.GetMainActivity);

            var intent = new Intent();
            //string num = "Can't get";
            intent.SetType("image/*");
            intent.PutExtra(Intent.ExtraAllowMultiple, true);
            intent.SetAction(Intent.ActionGetContent);
            MainActivity.GetMainActivity.StartActivityForResult(Intent.CreateChooser(intent, "Select Picture"), 1);


            await listener.TaskDone;
            if (listener.TaskDone.Result == true)
            {
                if (listener.ActivityMessageObtained.MyIntent != null)
                {
                    for (int i = 0; i < listener.ActivityMessageObtained.MyIntent.ClipData.ItemCount; i++)
                    {
                        Android.Net.Uri uri = listener.ActivityMessageObtained.MyIntent.ClipData.GetItemAt(i).Uri;
                        images.Add(uri.LastPathSegment);
                    }
                }
                return images;
            }
            return images;
        }

        public async Task<string> GetFilePath()
        {
            var listener = new ActivityResultListener(MainActivity.GetMainActivity);

            var intent = new Intent();
            intent.SetType("application/pdf");
            //string[] mimeTypes = { "application/pdf", "text/plain" };
            //intent.PutExtra(Intent.ExtraMimeTypes, mimeTypes);
            intent.PutExtra(Intent.ExtraAllowMultiple, false);
            intent.SetAction(Intent.ActionGetContent);
            //intent.SetAction(Intent.ActionPickActivity);
            MainActivity.GetMainActivity.StartActivityForResult(Intent.CreateChooser(intent, "Select PDF File"), 1);

            await listener.TaskDone;
            if (listener.TaskDone.Result == true)
            {
                if (listener.ActivityMessageObtained.MyIntent != null)
                {
                    Android.Net.Uri uri = listener.ActivityMessageObtained.MyIntent.Data;
                    string filePath = uri.LastPathSegment;
                    return filePath;
                    //return new object[] { MainActivity.GetMainActivity.ContentResolver.OpenInputStream(uri), uri.Path };
                }
                return null;
            }
            return null;
        }

        public Task<string> PickCopyAsync()
        {
            throw new NotImplementedException();
        }

        /*public async Task<List<FilePickedModel>> GetFilesStream()
        {
            List<FilePickedModel> images = new List<FilePickedModel>();
            var listener = new ActivityResultListener(MainActivity.GetMainActivity);

            var intent = new Intent();
            string num = "Can't get";
            intent.SetType("image/*");
            intent.PutExtra(Intent.ExtraAllowMultiple, true);
            intent.SetAction(Intent.ActionGetContent);
            MainActivity.GetMainActivity.StartActivityForResult(Intent.CreateChooser(intent, "Select Picture"), 1);

            await listener.TaskDone;
            if (listener.TaskDone.Result == true)
            {
                if (listener.ActivityMessageObtained.MyIntent != null)
                {
                    await Task.Run(() => 
                    {
                        for (int i = 0; i < listener.ActivityMessageObtained.MyIntent.ClipData.ItemCount; i++)
                        {
                            Android.Net.Uri uri = listener.ActivityMessageObtained.MyIntent.ClipData.GetItemAt(i).Uri;
                            listener.ActivityMessageObtained.MyIntent.ClipData.GetItemAt(i);
                            images.Add(new FilePickedModel { FileStream = MainActivity.GetMainActivity.ContentResolver.OpenInputStream(uri) });
                        }
                    });
                    
                }
                return images;
            }
            return images;
        }*/
    }

}