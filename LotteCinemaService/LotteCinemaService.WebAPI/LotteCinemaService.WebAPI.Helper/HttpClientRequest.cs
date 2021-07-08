using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace LotteCinemaService.WebAPI.Helper
{
    public class HttpClientRequest
    {
        public T Get<T>(string serverUrl, string query)
        {
            return GetAsyn<T>(serverUrl, query).Result;
        }

        public DataSet GetAsynToDataSet(string serverUrl, string query)
        {
            var resultDataset = new DataSet();
            Task.Factory.StartNew(async () =>
            {
                try
                {
                    using (var client = CreateHttpClient(serverUrl))
                    {
                        var requestUri = GetRequestUri(serverUrl, query);
                        var response = await client.GetAsync(requestUri);

                        response.EnsureSuccessStatusCode();
                        var resultString = await response.Content.ReadAsStringAsync();

                        var bytes = Encoding.Default.GetBytes(resultString);

                        var str = Encoding.Default.GetString(bytes);

                        var result = resultString.Replace(@"\", "");
                        result = result.Remove(0, 1);
                        result = result.Remove(result.Length - 1);
                        resultDataset = (JsonConvert.DeserializeObject(result, typeof(DataSet)) as DataSet);
                    }
                }
                catch (Exception ex)
                {
                    WriteErrorMessage(ex);
                }
            });

            return resultDataset;
        }

        public Task<T> GetAsyn<T>(string serverUrl, string query)
        {
            var tcs = new TaskCompletionSource<T>();

            Task.Factory.StartNew(async () =>
            {
                T result = default(T);

                try
                {
                    using (var client = CreateHttpClient(serverUrl))
                    {
                        var requestUri = GetRequestUri(serverUrl, query);
                        var response = await client.GetAsync(requestUri);

                        response.EnsureSuccessStatusCode();
                        result = await response.Content.ReadAsAsync<T>();
                    }
                }
                catch (Exception ex)
                {
                    WriteErrorMessage(ex);
                }

                tcs.SetResult(result);
            });

            return tcs.Task;
        }

        public HttpResponseMessage Put<T>(string serverUrl, string query, T value)
        {
            return PutAsync<T>(serverUrl, query, value).Result;
        }

        public Task<HttpResponseMessage> PutAsync<T>(string serverUrl, string query, T value)
        {
            var tcs = new TaskCompletionSource<HttpResponseMessage>();

            Task.Factory.StartNew(async () =>
            {
                HttpResponseMessage result = null;

                try
                {
                    using (var client = CreateHttpClient(serverUrl))
                    {
                        var requestUri = GetRequestUri(serverUrl, query);
                        result = await client.PutAsJsonAsync<T>(requestUri, value);
                    }
                }
                catch (Exception ex)
                {
                    WriteErrorMessage(ex);
                }

                tcs.SetResult(result);
            });

            return tcs.Task;
        }

        public HttpResponseMessage Post<T>(string serverUrl, string query, T value)
        {
            return PostAsync<T>(serverUrl, query, value).Result;
        }

        public Task<HttpResponseMessage> PostAsync<T>(string serverUrl, string query, T value)
        {
            var tcs = new TaskCompletionSource<HttpResponseMessage>();

            Task.Factory.StartNew(async () =>
            {
                HttpResponseMessage result = null;

                try
                {
                    using (var client = CreateHttpClient(serverUrl))
                    {
                        var requestUri = GetRequestUri(serverUrl, query);
                        result = await client.PostAsJsonAsync<T>(requestUri, value);
                    }
                }
                catch (Exception ex)
                {
                    WriteErrorMessage(ex);
                }

                tcs.SetResult(result);
            });

            return tcs.Task;
        }

        public HttpResponseMessage Delete(string serverUrl, string query)
        {
            return DeleteAsync(serverUrl, query).Result;
        }

        public Task<HttpResponseMessage> DeleteAsync(string serverUrl, string query)
        {
            var tcs = new TaskCompletionSource<HttpResponseMessage>();

            Task.Factory.StartNew(async () =>
            {
                HttpResponseMessage result = null;

                try
                {
                    using (var client = CreateHttpClient(serverUrl))
                    {
                        var requestUri = GetRequestUri(serverUrl, query);
                        result = await client.DeleteAsync(requestUri);
                    }
                }
                catch (Exception ex)
                {
                    WriteErrorMessage(ex);
                }

                tcs.SetResult(result);
            });

            return tcs.Task;
        }

        private HttpClient CreateHttpClient(string serverUri)
        {
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(serverUri);
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            return client;
        }

        private string GetRequestUri(string serverUri, string query)
        {
            return string.Format("{0}/{1}", serverUri, query);
        }

        private void WriteErrorMessage(Exception ex)
        {
#if DEBUG
            if (ex is HttpRequestException)
            {
                System.Diagnostics.Debug.WriteLine("[HttpRequestException]\n" + ex.Message);
            }
            else
            {
                System.Diagnostics.Debug.WriteLine("[Exception]\n" + ex.Message);
            }
#endif
        }
    }
}