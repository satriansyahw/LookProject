using GenHelper;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Auth;
using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace GenHelper
{


    public class FileUpload
    {
        Settings settings = new Settings();
        public CloudStorageAccount StorageAccount { get; set; }
        public CloudBlobContainer Kontainer { get; set; }
        public CloudBlobClient BlobKlien { get; set; }

        public FileUpload()
        {
            /*Msh blm complete untuk bisa langsung ok integrasi cloud n local folder*/

            if(settings.UploadType ==0)// 0=cloud,1= network share,2=ftp
            {
                var storageCredential = new StorageCredentials(settings.CloudAccountName, settings.CloudAccountKey);
                this.StorageAccount = new CloudStorageAccount(storageCredential, true);
                this.BlobKlien = this.StorageAccount.CreateCloudBlobClient();
                this.Kontainer = this.BlobKlien.GetContainerReference(settings.CloudAccountName);
            }
        }
        //public FileUpload(string accountName, string accountKey, string containerName)
        //{
        //    var storageCredential = new StorageCredentials(accountName, accountKey);
        //    this.StorageAccount = new CloudStorageAccount(storageCredential, true);
        //    this.BlobKlien = this.StorageAccount.CreateCloudBlobClient();
        //    this.Kontainer = this.BlobKlien.GetContainerReference(containerName);
        //}
        //public static FileUpload GetInstance(string accountName, string accountKey, string containerName)
        //{

        //    {
        //        if (instance == null)
        //        {
        //            instance = new FileUpload(accountName, accountKey, containerName);
        //        }
        //        return instance;
        //    }
        //}
        private async Task CreateFolderCloud(string folderName)
        {
            await this.CreateDefaultFileCloud(folderName);
        }
        private async Task<bool> DeleteFileCloud(string PathFileName)
        {
            bool result = true;
            try
            {
                var myblob = this.Kontainer.GetBlobReference(PathFileName);
                await myblob.DeleteIfExistsAsync();
            }
            catch
            {
                result = false;
            }
            return result;
        }
        private async Task<bool> CreateDefaultFileCloud(string folderName)
        {
            bool result = true;
            try
            {
                string localPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                string localFileName = "Default" + ".txt";
                string PathFileName = Path.Combine(folderName, localFileName);
                var sourceFile = Path.Combine(localPath, localFileName);
                await File.WriteAllTextAsync(sourceFile, "Hello World only a default Waktu Creating Folder");
                var myblob = this.Kontainer.GetBlockBlobReference(PathFileName);
                await myblob.DeleteIfExistsAsync();
                await UploadFromFileCloud(sourceFile, PathFileName);
                File.Delete(sourceFile);
            }
            catch
            {
                result = false;
            }
            return result;

        }
        private async Task<bool> UploadFromFileCloud(string sourceFile, string AzurePathFileName)
        {
            bool result = true;
            try
            {
                var myblob = this.Kontainer.GetBlockBlobReference(AzurePathFileName);
                await myblob.UploadFromFileAsync(sourceFile);
                File.Delete(sourceFile);
            }
            catch
            {
                result = false;
            }
            return result;
        }
        private async Task<bool> UploadFromStreamCloud(Stream sourceFile, string AzurePathFileName)
        {
            bool result = true;
            try
            {
                var myblob = this.Kontainer.GetBlockBlobReference(AzurePathFileName);
                await myblob.UploadFromStreamAsync(sourceFile);
            }
            catch
            {
                result = false;
            }
            return result;
        }
        private async Task<bool> UploadFromByteArrayCloud(byte[] sourceFile, string AzurePathFileName)
        {
            bool result = true;
            try
            {
                var myblob = this.Kontainer.GetBlockBlobReference(AzurePathFileName);
                await myblob.UploadFromByteArrayAsync(sourceFile, 0, sourceFile.Length);
            }
            catch
            {
                result = false;
            }
            return result;
        }
        private async Task<string> DownloadToFilePathCloud(string AzurePathFileName)
        {
            string result = string.Empty;
            try
            {
                string localPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                string myGuid = Guid.NewGuid().ToString();
                string myfile = Path.GetFileName(AzurePathFileName);
                string PathFileName = Path.Combine(localPath, myGuid + "." + Path.GetExtension(myfile));
                string ReturnPathFileName = Path.Combine(localPath, myfile);
                var myblob = this.Kontainer.GetBlockBlobReference(AzurePathFileName);
                await myblob.DownloadToFileAsync(PathFileName, FileMode.Create);
                File.Copy(PathFileName, ReturnPathFileName);
                result = ReturnPathFileName;
                File.Delete(PathFileName);
            }
            catch
            {

            }
            return result;
        }
        private async Task<MemoryStream> DownloadToStreamCloud(string AzurePathFileName)
        {
            MemoryStream memStream = new MemoryStream();
            try
            {
                string localPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                string myGuid = Guid.NewGuid().ToString();
                string myfile = Path.GetFileName(AzurePathFileName);
                string PathFileName = Path.Combine(localPath, myGuid + "." + Path.GetExtension(myfile));
                string ReturnPathFileName = Path.Combine(localPath, myfile);
                var myblob = this.Kontainer.GetBlockBlobReference(AzurePathFileName);
                await myblob.DownloadRangeToStreamAsync(memStream, 0, memStream.Length);

            }
            catch
            {

            }
            return memStream;
        }
        private async Task<Byte[]> DownloadToByteArrayCloud(string AzurePathFileName)
        {
            Byte[] mybyte = null;
            try
            {
                string localPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                string myGuid = Guid.NewGuid().ToString();
                string myfile = Path.GetFileName(AzurePathFileName);
                string PathFileName = Path.Combine(localPath, myGuid + "." + Path.GetExtension(myfile));
                string ReturnPathFileName = Path.Combine(localPath, myfile);
                var myblob = this.Kontainer.GetBlockBlobReference(AzurePathFileName);
                await myblob.DownloadToByteArrayAsync(mybyte, 0);

            }
            catch
            {

            }
            return mybyte;
        }

        private void CreateFolderNetwork(string folderName)
        {
            string myfolder = settings.NetworkSharing + @"/" + folderName;
            if(Directory.Exists(myfolder))
            {
                Directory.CreateDirectory(myfolder);
            }
        }
        private bool DeleteFileNetwork(string PathFileName)
        {
            bool result = true;
            if(File.Exists(PathFileName))
            {
                File.Delete(PathFileName);
            }
            
            return result;
        }
        private bool UploadFromByteArrayNetwork(string webFolderUpload, string filenameOriginal, Byte[] data)
        {
            bool bola = false;
            try
            {
                webFolderUpload = settings.NetworkSharing + @"\" + webFolderUpload;
                if (!System.IO.Directory.Exists(webFolderUpload))
                {
                    System.IO.Directory.CreateDirectory(webFolderUpload);
                }
                webFolderUpload =  webFolderUpload + @"\" + filenameOriginal;
                if (System.IO.File.Exists(webFolderUpload))
                    System.IO.File.Delete(webFolderUpload);
                File.WriteAllBytes(webFolderUpload, data);
                bola = true;
            }
            catch  (Exception ex){ }
            return bola;
        }
        private byte[] DownloadToByteArrayNetwork(string webFolderUpload,string PathFileName)
        {
            byte[] result = null;
            try
            {
                webFolderUpload = settings.NetworkSharing + @"\" + webFolderUpload + @"\" + PathFileName;
                if (File.Exists(webFolderUpload))
                {
                    result = System.IO.File.ReadAllBytes(webFolderUpload);
                }
              
            }
            catch
            {

            }
            return result;
        }
        private async Task<MemoryStream> DownloadToStreamNetwork(string foldername, string filename)
        {
            MemoryStream memStream = new MemoryStream();
            try
            {
                //string localPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                //string myGuid = Guid.NewGuid().ToString();
                //string myfile = Path.GetFileName(AzurePathFileName);
                //string PathFileName = Path.Combine(localPath, myGuid + "." + Path.GetExtension(myfile));
                //string ReturnPathFileName = Path.Combine(localPath, myfile);
                //var myblob = this.Kontainer.GetBlockBlobReference(AzurePathFileName);
                //await myblob.DownloadRangeToStreamAsync(memStream, 0, memStream.Length);


            }
            catch
            {

            }
            return memStream;
        }
        public bool UploadFile(string foldername, string filename, string filearray)
        {
            bool result = false;
            if (!string.IsNullOrEmpty(foldername) & !string.IsNullOrEmpty(filename) & !string.IsNullOrEmpty(filearray))
            {
                filearray = filearray.Replace("\"", string.Empty);
                Byte[] bytes = Convert.FromBase64String(filearray);
                if(settings.UploadType == 0)//cloud
                {
                    var myupload = this.UploadFromByteArrayCloud(bytes, foldername + @"\" + filename).GetAwaiter().GetResult();
                    result = true;
                }
                else if (settings.UploadType == 1)//sharing
                {
                    var myupload = this.UploadFromByteArrayNetwork(foldername, filename, bytes);
                    result = true;
                }
                else if (settings.UploadType == 2)//ftp
                {

                }
            }
            return result;
        }
        public bool UploadFile(string foldername, string filename, Byte[] bytes)
        {
            bool result = false;
            if (!string.IsNullOrEmpty(foldername) & !string.IsNullOrEmpty(filename) & bytes != null)
            {
                if (settings.UploadType == 0)//cloud
                {
                    var myupload = this.UploadFromByteArrayCloud(bytes, foldername + @"\" + filename).GetAwaiter().GetResult();
                    result = true;
                }
                else if (settings.UploadType == 1)//sharing
                {
                    var myupload = this.UploadFromByteArrayNetwork(foldername, filename, bytes);
                    result = true;
                }
                else if (settings.UploadType == 2)//ftp
                {

                }
            }
            return result;
        }
        public byte[] DownloadFile(string foldername, string filename)
        {
            byte[] result = null;
            try
            {
                if (!string.IsNullOrEmpty(foldername) & !string.IsNullOrEmpty(filename))
                {
                    if (settings.UploadType==0)//cloud
                    {
                        result = this.DownloadToByteArrayCloud(foldername + @"\" + filename).GetAwaiter().GetResult();
                    }
                    else if (settings.UploadType == 1)//sharing
                    {
                        result= this.DownloadToByteArrayNetwork(foldername, filename);
                    }
                    else if (settings.UploadType == 2)//ftp
                    {

                    }


                }
            }
            catch (Exception ex) { }
            return result;
        }
        public bool FTPUploadFile(string foldername, string filename, Byte[] bytes)
        {
            bool result = false;
            try
            {
                string to_uri = settings.FTPAddress + @"/"+foldername+ @"/" + filename;
                FtpWebRequest request =
                   (FtpWebRequest)WebRequest.Create(to_uri);
                request.KeepAlive = true;
                request.UsePassive = true;
                request.UseBinary = true;
                request.Method = WebRequestMethods.Ftp.UploadFile;
                request.Credentials = new NetworkCredential(settings.FTPUserName, settings.FTPPassword);
                request.ContentLength = bytes.Length;
                using (Stream request_stream = request.GetRequestStream())
                {
                    request_stream.Write(bytes, 0, bytes.Length);
                    request_stream.Close();
                }
                result = true;

            }
            catch (Exception ex) { };
            return result;
        }
        public byte[] FTPUploadFile(string foldername, string filename)
        {
            byte[] result = null;
            try
            {
                string to_uri = settings.FTPAddress + @"/" + foldername + @"/" + filename;
                FtpWebRequest request =
                   (FtpWebRequest)WebRequest.Create(to_uri);
                request.Method = WebRequestMethods.Ftp.DownloadFile;

                string localPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                string myGuid = Guid.NewGuid().ToString();
                string myfile = Path.GetFileName(filename);
                string PathFileName = Path.Combine(localPath, myGuid + "." + Path.GetExtension(myfile));
                string ReturnPathFileName = Path.Combine(localPath, PathFileName);

                request.Credentials = new NetworkCredential(settings.FTPUserName, settings.FTPPassword);
                FileStream outputStream = new FileStream(ReturnPathFileName, FileMode.Create);
                FtpWebResponse response = (FtpWebResponse)request.GetResponse();
                Stream ftpStream = response.GetResponseStream();
                long cl = response.ContentLength;
                int bufferSize = 5120;
                int readCount;
                byte[] buffer = new byte[bufferSize];

                readCount = ftpStream.Read(buffer, 0, bufferSize);
                while (readCount > 0)
                {
                    outputStream.Write(buffer, 0, readCount);
                    readCount = ftpStream.Read(buffer, 0, bufferSize);
                }
             
                ftpStream.Close();
                outputStream.Close();
                response.Close();
                result = System.IO.File.ReadAllBytes(ReturnPathFileName);
                if (File.Exists(PathFileName))
                {
                    File.Delete(PathFileName);
                }
               

            }
            catch (Exception ex) { };
            return result;
        }
    }
    
}

