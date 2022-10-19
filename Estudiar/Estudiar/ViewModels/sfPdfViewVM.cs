using Estudiar.Models;
using Syncfusion.SfPdfViewer.XForms;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Estudiar.ViewModels
{

    public interface IDownloadService
    {
        Task DownloadFileAsync(string url, IProgress<double> progress, CancellationToken token);
    }
    public class PdfPageViewType
    {
        public PageViewMode Mode { get; set; }
        public string Name { get; set; }
    }
    public class sfPdfViewVM:INotifyPropertyChanged, IDownloadService
    {
        

        private ObservableCollection<PdfPageViewType> _myPageViewModels;
        public ObservableCollection<PdfPageViewType> MyPageViewModels
        {
            get { return _myPageViewModels; }
            set { _myPageViewModels = value; OnPropertyChanged("MyPageViewModels"); }
        }

        private Cours _selectedCourse;

        public Cours SelectedCourse
        {
            get { return _selectedCourse; }
            set { _selectedCourse = value; OnPropertyChanged("SelectedCourse"); }
        }

        private MyFile _selectedMaterial;

        public MyFile SelectedMaterial
        {
            get { return _selectedMaterial; }
            set { _selectedMaterial = value; OnPropertyChanged("SelectedMaterial"); }
        }


        private ObservableCollection<int> myVar;

        public ObservableCollection<int> MyProperty
        {
            get { return myVar; }
            set { myVar = value; OnPropertyChanged("MyProperty"); }
        }

        private Stream m_pdfDocumentStream;
        /// <summary>
        /// The PDF document stream that is loaded into the instance of the PDF viewer. 
        /// </summary>
        public Stream PdfDocumentStream
        {
            get
            {
                
                return m_pdfDocumentStream;
            }
            set
            {
                m_pdfDocumentStream = value;
                OnPropertyChanged("PdfDocumentStream");
            }
        }

        private double zoom1Percent;

        public double Zoom1Percent
        {
            get { return zoom1Percent; }
            set { zoom1Percent = value;OnPropertyChanged("PdfDocumentStream"); }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        

        public sfPdfViewVM()
        {
            //Accessing the PDF document that is added as embedded resource as stream.

        }
        public async Task GetStream()
        {
            MyPageViewModels = new ObservableCollection<PdfPageViewType> {
                new PdfPageViewType { Mode = PageViewMode.PageByPage, Name = "Page by Page" },
                new PdfPageViewType { Mode = PageViewMode.Continuous, Name = "Continuous" }
            };
            SelectedMaterial = AppState<MyFile>.DeserializeAndRestore("SelectedMaterial");
            SelectedCourse = AppState<Cours>.DeserializeAndRestore("SelectedCourse");
            //HttpClient httpClient = new HttpClient();
            //HttpResponseMessage response = await httpClient.GetAsync(SelectedMaterial.url);
            //PdfDocumentStream = await response.Content.ReadAsStreamAsync();

            //var cts = new CancellationTokenSource();
            //var progressIndicator = new Progress<double>(ReportProgress);
            //await DownloadFileAsync(SelectedMaterial.url, progressIndicator, cts.Token);
            WebClient client = new WebClient();
            client.DownloadProgressChanged += Client_DownloadProgressChanged;
            byte[] x = await client.DownloadDataTaskAsync(new Uri(SelectedMaterial.url));

            PdfDocumentStream = new MemoryStream(x);
        }

        private void Client_DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            ProgressValue = e.ProgressPercentage;
        }

        private double _progressValue;
        public double ProgressValue
        {
            get { return _progressValue; }
            set { _progressValue= value;OnPropertyChanged(); }
        }
        internal void ReportProgress(double value)
        {
            ProgressValue = value;
        }
        private int bufferSize = 4095;
        private int BufferSize = 4095;
        public async Task DownloadFileAsync(string url, IProgress<double> progress, CancellationToken token)
        {
            try
            {
                HttpClient _client = new HttpClient();

                // Step 1 : Get call
                var response = await _client.GetAsync(url, HttpCompletionOption.ResponseHeadersRead, token);

                if (!response.IsSuccessStatusCode)
                {
                    throw new Exception(string.Format("The request returned with HTTP status code {0}", response.StatusCode));
                }

                // Step 2 : Filename
                var fileName = response.Content.Headers?.ContentDisposition?.FileName ?? "tmp.zip";

                // Step 3 : Get total of data
                var totalData = response.Content.Headers.ContentLength.GetValueOrDefault(-1L);
                var canSendProgress = totalData != -1L && progress != null;

                // Step 4 : Get total of data
                //var filePath = Path.Combine(_fileService.GetStorageFolderPath(), fileName);

                // Step 5 : Download data
                using (var fileStream = new MemoryStream())
                {
                    using (var stream = await response.Content.ReadAsStreamAsync())
                    {
                        var totalRead = 0L;
                        var buffer = new byte[bufferSize];
                        var isMoreDataToRead = true;

                        do
                        {
                            token.ThrowIfCancellationRequested();

                            var read = await stream.ReadAsync(buffer, 0, buffer.Length, token);

                            if (read == 0)
                            {
                                isMoreDataToRead = false;
                            }
                            else
                            {
                                // Write data on disk.
                                await fileStream.WriteAsync(buffer, 0, read);

                                totalRead += read;

                                if (canSendProgress)
                                {
                                    progress.Report((totalRead * 1d) / (totalData * 1d) * 100);
                                }
                            }
                        } while (isMoreDataToRead);
                    }
                    PdfDocumentStream = fileStream;
                }

            }
            catch (Exception e)
            {
                // Manage the exception as you need here.
                System.Diagnostics.Debug.WriteLine(e.ToString());
            }
        }
    }
}
