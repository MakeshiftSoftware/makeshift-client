using System;
using System.IO;
using System.Net;
using System.Threading;
using System.Text;
using UnityEngine;
using SimpleJSON;

public class RequestState
{
    public int BufferSize = 1024;
    public byte[] BufferRead;
    public StringBuilder ResponseContent;
    public HttpWebRequest Request;
    public HttpWebResponse Response;
    public Stream ResponseStream;
    public Stream RequestStream;
    public Action<HttpResponse> Callback;
    public JSONObject PostData;

    public RequestState()
    {
        BufferRead = new byte[BufferSize];
        ResponseContent = new StringBuilder();
    }
}

public class HttpRequest
{
    private const int timeoutMSec = 10000;

    public void PostAsync(string url, JSONObject data, Action<HttpResponse> callback = null)
    {
        try
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "POST";
            request.ContentType = "application/json";

            RequestState state = new RequestState();
            state.Request = request;
            state.Callback = callback;
            state.PostData = data;
            request.BeginGetRequestStream(new AsyncCallback(OnStreamAvailable), state);
        }
        catch (Exception e)
        {
            Debug.Log(e.Message);
        }
    }

    public void OnStreamAvailable(IAsyncResult result)
    {
        try
        {
            RequestState state = (RequestState)result.AsyncState;
            HttpWebRequest request = state.Request;
            Stream stream = request.EndGetRequestStream(result);
            state.RequestStream = stream;

            // Async write to stream
            byte[] bytes = Encoding.UTF8.GetBytes(state.PostData.ToString());
            stream.BeginWrite(bytes, 0, bytes.Length, new AsyncCallback(WriteCallback), state);
        }
        catch (Exception e)
        {
            Debug.Log(e.Message);
        }
    }

    private void WriteCallback(IAsyncResult result)
    {
        try
        {
            RequestState state = (RequestState)result.AsyncState;
            HttpWebRequest request = state.Request;
            state.RequestStream.EndWrite(result);
            state.RequestStream.Close();

            request.BeginGetResponse(new AsyncCallback(ResponseCallback), state);

            // Register timeout
            ThreadPool.RegisterWaitForSingleObject(result.AsyncWaitHandle,
                new WaitOrTimerCallback(TimeOutCallback), request, timeoutMSec, true);
        }
        catch (Exception e)
        {
            Debug.Log(e.Message);
        }
    }

    public void GetAsync(string url, Action<HttpResponse> callback = null)
    {
        try
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "GET";

            RequestState state = new RequestState();
            state.Request = request;
            state.Callback = callback;

            IAsyncResult result = request.BeginGetResponse(new AsyncCallback(ResponseCallback), state);

            // Register timeout
            ThreadPool.RegisterWaitForSingleObject(result.AsyncWaitHandle,
                new WaitOrTimerCallback(TimeOutCallback), request, timeoutMSec, true);
        }
        catch (Exception e)
        {
            Debug.Log(e.Message);
        }
    }
    private void ResponseCallback(IAsyncResult result)
    {
        try
        {
            RequestState state = (RequestState)result.AsyncState;
            HttpWebRequest request = state.Request;
            state.Response = (HttpWebResponse)request.EndGetResponse(result);
            Stream responseStream = state.Response.GetResponseStream();
            state.ResponseStream = responseStream;

            // Begin async reading of the contents
            IAsyncResult readResult = responseStream.BeginRead(state.BufferRead,
                    0, state.BufferSize, new AsyncCallback(ReadCallback), state);
        }
        catch (Exception e)
        {
            RequestState state = (RequestState)result.AsyncState;

            if (state.Response != null)
            {
                state.Response.Close();
            }
            Debug.Log(e.Message);
        }
    }

    private void ReadCallback(IAsyncResult result)
    {
        try
        {
            RequestState state = (RequestState)result.AsyncState;
            int bytesRead = state.ResponseStream.EndRead(result);

            if (bytesRead > 0)
            {
                state.ResponseContent.Append(Encoding.ASCII.GetString(state.BufferRead, 0, bytesRead));
                // Read more bytes from stream
                state.ResponseStream.BeginRead(state.BufferRead, 0, state.BufferSize,
                    new AsyncCallback(ReadCallback), state);
            }
            else
            {
                // Finished reading response bytes
                if (state.ResponseContent.Length > 0)
                {
                    if (state.Callback != null)
                    {
                        int status = (int)state.Response.StatusCode;
                        HttpResponse response = new HttpResponse(status, state.ResponseContent.ToString());
                        state.Callback(response);
                    }

                    state.ResponseStream.Close();
                    state.Response.Close();
                }
            }
        }
        catch (Exception e)
        {
            RequestState state = (RequestState)result.AsyncState;

            if (state.Response != null)
            {
                state.Response.Close();
            }
        }
    }

    private void TimeOutCallback(object state, bool timedOut)
    {
        if (timedOut)
        {
            HttpWebRequest request = state as HttpWebRequest;

            if (request != null)
            {
                request.Abort();
            }
        }
    }
}