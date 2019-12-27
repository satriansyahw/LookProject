using Jil;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;

namespace GenHelper
{
    public class Helper
    {
        public Helper()
        {

        }
        public byte[] ReadFile(string filePath)
        {
            byte[] buffer;
            FileStream fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
            try
            {
                int length = (int)fileStream.Length;  // get file length
                buffer = new byte[length];            // create buffer
                int count;                            // actual number of bytes read
                int sum = 0;                          // total number of bytes read

                // read until Read method returns 0 (end of the stream has been reached)
                while ((count = fileStream.Read(buffer, sum, length - sum)) > 0)
                    sum += count;  // sum is a buffer offset for next reading
            }
            finally
            {
                fileStream.Close();
            }
            return buffer;
        }

        public void CopyStream(Stream input, Stream output)
        {
            byte[] buffer = new byte[16 * 1024];
            int read;
            while ((read = input.Read(buffer, 0, buffer.Length)) > 0)
            {
                output.Write(buffer, 0, read);
            }
        }
        public string RandomHexNumber(int digits)
        {
            Random random = new Random();
            byte[] buffer = new byte[digits / 2];
            random.NextBytes(buffer);
            string result = String.Concat(buffer.Select(x => x.ToString("X2")).ToArray());
            if (digits % 2 == 0)
                return result;
            return result + random.Next(16).ToString("X");
        }
        //public Bitmap ResizeImage(System.Net.Mime.MediaTypeNames.Image image, int width, int height)
        //{
        //    var destRect = new Rectangle(0, 0, width, height);
        //    var destImage = new Bitmap(width, height);

        //    destImage.SetResolution(image.HorizontalResolution, image.VerticalResolution);

        //    using (var graphics = Graphics.FromImage(destImage))
        //    {
        //        graphics.CompositingMode = CompositingMode.SourceCopy;
        //        graphics.CompositingQuality = CompositingQuality.HighQuality;
        //        graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
        //        graphics.SmoothingMode = SmoothingMode.HighQuality;
        //        graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

        //        using (var wrapMode = new ImageAttributes())
        //        {
        //            wrapMode.SetWrapMode(WrapMode.TileFlipXY);
        //            graphics.DrawImage(image, destRect, 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, wrapMode);
        //        }
        //    }
        //    return destImage;
        //}
        public bool CheckIsContainString(string sourcestring, string delimiter, string searchstring)
        {
            string[] temp = sourcestring.Split(delimiter);
            bool result = false;
            foreach (var item in temp)
            {
                if (item.Trim() == searchstring.Trim())
                {
                    result = true;
                    break;
                }

            }
            return result;
        }
        public T CopyClass<T>(T entity) where T : class
        {
            T result = (T)Activator.CreateInstance(typeof(T));
            foreach (var item in result.GetType().GetRuntimeProperties())
            {
                item.SetValue(result, item.GetValue(entity));
            }
            return result;
        }
        public List<T> CopyClass<T>(List<T> entity) where T : class
        {
            List<T> result = new List<T>();
            foreach (var item in entity)
            {
                result.Add(CopyClass<T>(item));
            }
            return result;
        }
        public void CopyPropertiesTo<T, TU>(T source, TU dest)
        {
            foreach (var property in source.GetType().GetRuntimeProperties())
            {
                if (!string.IsNullOrEmpty(property.Name))
                {
                    if (property.Name.Trim() != "getInstance")
                    {
                        PropertyInfo propertyS = dest.GetType().GetRuntimeProperty(property.Name);
                        if (propertyS != null)
                        {
                            if (propertyS.CanWrite)
                            {
                                var value = property.GetValue(source, null);
                                propertyS.SetValue(dest, property.GetValue(source, null), null);
                            }
                        }
                    }
                }
            }
        }
        public TU CopyPropertiesTo<T, TU>(T source)
        {
            TU dest = Activator.CreateInstance<TU>();
            foreach (var property in source.GetType().GetRuntimeProperties())
            {
                if (!string.IsNullOrEmpty(property.Name))
                {
                    if (property.Name.Trim() != "getInstance")
                    {
                        PropertyInfo propertyS = dest.GetType().GetRuntimeProperty(property.Name);
                        if (propertyS != null)
                        {
                            if (propertyS.CanWrite)
                            {
                                var value = property.GetValue(source, null);
                                propertyS.SetValue(dest, property.GetValue(source, null), null);
                            }
                        }
                    }
                }
            }
            return dest;
        }
        public List<TU> CopyPropertiesTo<T, TU>(List<T> sumber)
        {
            List<TU> listDest = new List<TU>();
            foreach (var source in sumber)
            {
                TU dest = Activator.CreateInstance<TU>();
                foreach (var property in source.GetType().GetRuntimeProperties())
                {
                    if (!string.IsNullOrEmpty(property.Name))
                    {
                        if (property.Name.Trim() != "getInstance")
                        {
                            PropertyInfo propertyS = dest.GetType().GetRuntimeProperty(property.Name);
                            if (propertyS != null)
                            {
                                if (propertyS.CanWrite)
                                {
                                    var value = property.GetValue(source, null);
                                    propertyS.SetValue(dest, property.GetValue(source, null), null);
                                }
                            }
                        }
                    }
                }
                listDest.Add(dest);
            }
           
           
            return listDest;
        }
        public byte[] CompressedData(string Message, object data)
        {
            string json = string.Empty;
            byte[] result = null;
            APIReturn apiReturn = new APIReturn();
            apiReturn.Message = Message;
            apiReturn.Data1 = data;
            using (var output = new StringWriter())
            {
                JSON.SerializeDynamic(apiReturn, output);
                json = output.ToString();
            }

            byte[] inputBytes = Encoding.UTF8.GetBytes(json);
            using (var outputStream = new MemoryStream())
            {
                using (var gZipStream = new GZipStream(outputStream, CompressionMode.Compress))
                    gZipStream.Write(inputBytes, 0, inputBytes.Length);

                var outputBytes = outputStream.ToArray();
                result = outputBytes;


            }
            return result;
        }
        public byte[] CompressedData(object data)
        {
            string json = string.Empty;
            byte[] result = null;
            using (var output = new StringWriter())
            {
                JSON.SerializeDynamic(data, output);
                json = output.ToString();
            }

            byte[] inputBytes = Encoding.UTF8.GetBytes(json);
            using (var outputStream = new MemoryStream())
            {
                using (var gZipStream = new GZipStream(outputStream, CompressionMode.Compress))
                    gZipStream.Write(inputBytes, 0, inputBytes.Length);

                var outputBytes = outputStream.ToArray();
                result = outputBytes;

            }
            return result;
        }
        public string DeCompressedData(byte[] data)
        {
            string result = string.Empty;
            if (data != null)
            {
                using (var inputStream = new MemoryStream(data))
                using (var gZipStream = new GZipStream(inputStream, CompressionMode.Decompress))
                using (var outputStream = new MemoryStream())
                {
                    gZipStream.CopyTo(outputStream);
                    var outputBytes = outputStream.ToArray();

                    string decompressed = Encoding.UTF8.GetString(outputBytes);
                    result = decompressed;
                }
            }
            return result;
        }
        public string DeCompressedData(string database64string)
        {
            string result = string.Empty;
            if (database64string != null)
            {
                using (var inputStream = new MemoryStream(Convert.FromBase64String(database64string)))
                using (var gZipStream = new GZipStream(inputStream, CompressionMode.Decompress))
                using (var outputStream = new MemoryStream())
                {
                    gZipStream.CopyTo(outputStream);
                    var outputBytes = outputStream.ToArray();

                    string decompressed = Encoding.UTF8.GetString(outputBytes);
                    result = decompressed;
                }
            }
            return result;
        }
        public ChunkData ChunkDataUpload(ChunkData chunk)
        {
            MemoryCacher cacher = new MemoryCacher();
            ChunkData result = new ChunkData();
            string savedchunk = string.Empty;
            object checkchunk = null;
            //check apakah ada data chunk         
            checkchunk = cacher.GetValue(chunk.ChunkKey);
            result.ChunkCurrent = chunk.ChunkCurrent;
            result.ChunkKey = chunk.ChunkKey;
            result.ChunkMaxCount = chunk.ChunkMaxCount;
            result.FileName = chunk.FileName;
            if (checkchunk != null)
            {
                //resultchunk =jika ada, data chunk lama + data terbaru
                savedchunk = (string)checkchunk;
                result.DataChunk = savedchunk + chunk.DataChunk;
            }
            else
            {
                //resultchunk =jika tidak ada, data terbaru
                result.DataChunk = chunk.DataChunk;
            }
            //menghapus data chace
            cacher.Delete(chunk.ChunkKey);
            if (chunk.ChunkMaxCount == chunk.ChunkCurrent)
            {
                result.CompleteChunk = true;
            }
            else
            {
                result.CompleteChunk = false;
                //membuat chache data chace
                cacher.Add(result.ChunkKey, result.DataChunk, 5);
            }
            return result;
        }

        private string DecryptStringFromBytes(byte[] cipherText, byte[] key, byte[] iv)
        {
            // Check arguments.  
            if (cipherText == null || cipherText.Length <= 0)
            {
                throw new ArgumentNullException("cipherText");
            }
            if (key == null || key.Length <= 0)
            {
                throw new ArgumentNullException("key");
            }
            if (iv == null || iv.Length <= 0)
            {
                throw new ArgumentNullException("key");
            }

            // Declare the string used to hold  
            // the decrypted text.  
            string plaintext = null;

            // Create an RijndaelManaged object  
            // with the specified key and IV.  
            using (var rijAlg = new RijndaelManaged())
            {
                //Settings  
                rijAlg.Mode = CipherMode.CBC;
                rijAlg.Padding = PaddingMode.PKCS7;
                rijAlg.FeedbackSize = 128;

                rijAlg.Key = key;
                rijAlg.IV = iv;
                //rijAlg.Padding = PaddingMode.Zeros;
                // Create a decrytor to perform the stream transform.  
                var decryptor = rijAlg.CreateDecryptor(rijAlg.Key, rijAlg.IV);

                try
                {
                    // Create the streams used for decryption.  
                    using (var msDecrypt = new MemoryStream(cipherText))
                    {
                        using (var csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                        {

                            using (var srDecrypt = new StreamReader(csDecrypt))
                            {
                                // Read the decrypted bytes from the decrypting stream  
                                // and place them in a string.  
                                plaintext = srDecrypt.ReadToEnd();

                            }

                        }
                    }
                }
                catch 
                {
                    plaintext = "keyError";
                }
            }

            return plaintext;
        }
        public string DecryptStringAES(string cipherText, string cipherKey)
        {
            if (!string.IsNullOrEmpty(cipherText) && !string.IsNullOrEmpty(cipherKey))
            {
                var keybytes = Encoding.UTF8.GetBytes(cipherKey);
                var iv = Encoding.UTF8.GetBytes(cipherKey);
                var encrypted = Convert.FromBase64String(cipherText);
                var decriptedFromJavascript = DecryptStringFromBytes(encrypted, keybytes, iv);
                return string.Format(decriptedFromJavascript);
            }
            else
            {
                return  RandomHexNumber(16);
            }
        }
    }
}
