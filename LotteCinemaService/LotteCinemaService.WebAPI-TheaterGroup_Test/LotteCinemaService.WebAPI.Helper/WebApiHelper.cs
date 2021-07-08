using System;
using System.Diagnostics;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json;

namespace LotteCinemaService.WebAPI.Helper
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

        public static T GetResultXml<T>(string query, Uri server)
        {
            return GetResult<T>(query, server, "application/xml");
        }

        public static bool PutJson<T>(string query, Uri server, T value)
        {
            bool result = false;

            try
            {
                using (var client = CreateHttpClient(server))
                {
                    var requestUri = string.Format("{0}/{1}", server.OriginalString, query);
                    var response = client.PutAsJsonAsync<T>(requestUri, value).Result;
                    result = response.IsSuccessStatusCode;
                }
            }
            catch { }

            return result;
        }

        public static bool PutXml<T>(string query, Uri server, T value)
        {
            bool result = false;

            try
            {
                using (var client = CreateHttpClient(server))
                {
                    var requestUri = string.Format("{0}/{1}", server.OriginalString, query);
                    var response = client.PutAsXmlAsync<T>(requestUri, value).Result;
                    result = response.IsSuccessStatusCode;
                }
            }
            catch { }

            return result;
        }

        public static bool PostJson<T>(string query, Uri server, T value)
        {
            bool result = false;

            try
            {
                using (var client = CreateHttpClient(server))
                {
                    var requestUri = string.Format("{0}/{1}", server.OriginalString, query);
                    var response = client.PostAsJsonAsync<T>(requestUri, value).Result;
                    result = response.IsSuccessStatusCode;
                }
            }
            catch { }

            return result;
        }

        public static bool PostXml<T>(string query, Uri server, T value)
        {
            bool result = false;

            try
            {
                using (var client = CreateHttpClient(server))
                {
                    var requestUri = string.Format("{0}/{1}", server.OriginalString, query);
                    var response = client.PutAsXmlAsync<T>(requestUri, value).Result;
                    result = response.IsSuccessStatusCode;
                }
            }
            catch { }

            return result;
        }

        public static bool Delete(string query, Uri server)
        {
            bool result = false;

            try
            {
                using (var client = CreateHttpClient(server))
                {
                    var requestUri = string.Format("{0}/{1}", server.OriginalString, query);
                    var response = client.DeleteAsync(requestUri).Result;
                    result = response.IsSuccessStatusCode;
                }
            }
            catch { }

            return result;
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

        public static T PostResult<T>(string query, Uri server,string mediaType, T value)
        {
            T result = (T)Activator.CreateInstance(typeof(T));

            try
            {
                using (var client = CreateHttpClient(server))
                {
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(mediaType));
                    var requestUri = string.Format("{0}/{1}", server.OriginalString, query);
                    var response = client.PostAsJsonAsync<T>(requestUri, value).Result;
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
      
        private static HttpClient CreateHttpClient(Uri server)
        {
            HttpClient client = new HttpClient();
            client.BaseAddress = server;
            return client;
        }
    }
}