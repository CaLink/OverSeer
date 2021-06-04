using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;

namespace DesktopPart.Model
{
    public class HttpMessage
    {
        public const string serverIP = "http://localhost:49999/";


        public static async Task<List<T>> MethodGet<T>(string route) where T : class
        {
            HttpClient client = new HttpClient();
            DataContractJsonSerializer dataContract = new DataContractJsonSerializer(typeof(List<T>));

            try
            {
                HttpResponseMessage responseMessage = await client.GetAsync(serverIP + route);

                using (var streamResult = await responseMessage.Content.ReadAsStreamAsync())
                {

                    return (List<T>)dataContract.ReadObject(streamResult);
                }
            }
            catch (Exception e)
            {
                System.Windows.MessageBox.Show(e.Message);
                return null;
            }
            
        }

        public static async Task<T> MethodGetBut<T>(string route) where T : class
        {
            HttpClient client = new HttpClient();
            DataContractJsonSerializer dataContract = new DataContractJsonSerializer(typeof(T));

            try
            {
                HttpResponseMessage responseMessage = await client.GetAsync(serverIP + route);

                using (var streamResult = await responseMessage.Content.ReadAsStreamAsync())
                {

                    return (T)dataContract.ReadObject(streamResult);
                }
            }
            catch (Exception e)
            {
                System.Windows.MessageBox.Show(e.Message);
                return null;
            }

        }

        public static async Task<T> MethodPut<T>(string route, T content) where T : class
        {
            HttpClient client = new HttpClient();
            DataContractJsonSerializer dataContract = new DataContractJsonSerializer(typeof(T));

            byte[] bytes = ContentMaker(dataContract, content);

            StringContent sc = new StringContent(Encoding.UTF8.GetString(bytes), Encoding.UTF8, "application/json");

            try
            {
                HttpResponseMessage responseMessage = client.PutAsync(serverIP + route, sc).Result;
                using (var streamResult = await responseMessage.Content.ReadAsStreamAsync())
                {

                    return (T)dataContract.ReadObject(streamResult);
                }
                
            }
            catch (Exception e)
            {
                System.Windows.MessageBox.Show(e.Message);
                return null;
            }

        }


        public static async Task<T> MethodPost<T>(string route, T content) where T : class
        {
            HttpClient client = new HttpClient();
            DataContractJsonSerializer dataContract = new DataContractJsonSerializer(typeof(T));

            byte[] bytes = ContentMaker<T>(dataContract, content);

            StringContent sc = new StringContent(Encoding.UTF8.GetString(bytes), Encoding.UTF8, "application/json");

            try
            {
                HttpResponseMessage responseMessage = client.PostAsync(serverIP + route, sc).Result;

                using (var streamResult = await responseMessage.Content.ReadAsStreamAsync())
                {

                    return (T)dataContract.ReadObject(streamResult);
                }
            }
            catch (Exception e)
            {
                System.Windows.MessageBox.Show(e.Message);
                return null;
            }

        }

        public static async Task<T> MethodDell<T>(string route, int id) where T : class
        {
            HttpClient client = new HttpClient();
            DataContractJsonSerializer dataContract = new DataContractJsonSerializer(typeof(int));

            byte[] bytes = BitConverter.GetBytes(id);

            StringContent sc = new StringContent(Encoding.UTF8.GetString(bytes), Encoding.UTF8, "application/json");

            try
            {
                HttpResponseMessage responseMessage = client.DeleteAsync(serverIP + route + "/" + id ).Result;
                using (var streamResult = await responseMessage.Content.ReadAsStreamAsync())
                {

                    return (T)dataContract.ReadObject(streamResult);
                }
                
            }
            catch (Exception e)
            {
                System.Windows.MessageBox.Show(e.Message);
                return null;
            }

        }


        public static byte[] ContentMaker<T>(DataContractJsonSerializer dataContract, T content) where T : class
        {
            byte[] temp = null;

            using (MemoryStream ms = new MemoryStream())
            using (StreamContent sc = new StreamContent(ms))
            {
                dataContract.WriteObject(ms, content);
                ms.Seek(0, SeekOrigin.Begin);
                temp = new byte[ms.Length];
                ms.Read(temp, 0, (int)ms.Length);
            }

            return temp;
        }

    }
}
