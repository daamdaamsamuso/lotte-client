using System;
using System.Diagnostics;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using System.Data;
using Newtonsoft.Json.Linq;

namespace lotte_Client.Helpers
{
    public static class WebApiHelper
    {
        public static T GetResultJson<T>(string query, Uri server)
        {
            return GetResult<T>(query, server, "application/json");
        }
        public static string GetResultJson(string query, Uri server)
        {
            return GetResult(query, server, "application/json");
        }

        public static DataSet GetResultDataSet(string query, Uri server)
        {
            string result = string.Empty;
            var resultDataset = new DataSet();
            try
            {
                using (var client = CreateHttpClient(server))
                {
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    var requestUri = string.Format("{0}/{1}", server.OriginalString, query);
                    var response = client.GetAsync(requestUri).Result;
                    response.EnsureSuccessStatusCode();
                    result = JValue.Parse(response.Content.ReadAsStringAsync().Result).ToString();
                    resultDataset = (JsonConvert.DeserializeObject(result, typeof(DataSet)) as DataSet);
                }
                return resultDataset;
            }
            catch (JsonException ex)
            {
                Debug.WriteLine("[JsonException]\n" + ex.Message);
            }
            catch (HttpRequestException ex)
            {
                Debug.WriteLine("[HttpRequestException]\n" + ex.Message);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("[Exception]\n" + ex.Message);
            }

            return resultDataset;
        }

        public static T GetResultXml<T>(string query, Uri server)
        {
            return GetResult<T>(query, server, "application/xml");
        }

     

        public static T GetResult<T>(string query, Uri server, string mediaType)
        {
            T result = (T)Activator.CreateInstance(typeof(T));

            try
            {
                using (var client = CreateHttpClient(server))
                {
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(mediaType));

                    var requestUri = string.Format("{0}/{1}", server.OriginalString, query);

                    var response = client.GetAsync(requestUri).Result;
                    response.EnsureSuccessStatusCode();

                    result = response.Content.ReadAsAsync<T>().Result;
                }
            }
            catch (JsonException ex)
            {
                Debug.WriteLine("[JsonException]\n" + ex.Message);
            }
            catch (HttpRequestException ex)
            {
                Debug.WriteLine("[HttpRequestException]\n" + ex.Message);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("[Exception]\n" + ex.Message);
            }

            return result;
        }

        public static string GetResult(string query, Uri server, string mediaType)
        {
            string result = string.Empty;
            try
            {
                using (var client = CreateHttpClient(server))
                {
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(mediaType));

                    var requestUri = string.Format("{0}/{1}", server.OriginalString, query);

                    var response = client.GetAsync(requestUri).Result;
                    response.EnsureSuccessStatusCode();

                    result = response.Content.ReadAsStringAsync().Result.Trim('"');
                }
                return result;
            }
            catch (JsonException ex)
            {
                Debug.WriteLine("[JsonException]\n" + ex.Message);
            }
            catch (HttpRequestException ex)
            {
                Debug.WriteLine("[HttpRequestException]\n" + ex.Message);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("[Exception]\n" + ex.Message);
            }

            return result;
        }

        private static HttpClient CreateHttpClient(Uri server)
        {
            HttpClient client = new HttpClient();
            client.BaseAddress = server;
            return client;
        }
    }
}