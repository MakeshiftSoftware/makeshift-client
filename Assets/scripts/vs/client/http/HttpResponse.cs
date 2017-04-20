using System;
using System.Net;

public class HttpResponse
{
    public int status;
    public string body;

    public HttpResponse(int status, string body)
    {
        this.status = status;
        this.body = body;
    }
    public int Status()
    {
        return this.status;
    }

    public string Body()
    {
        return this.body;
    }

    public bool Success()
    {
        return this.status >= 200 && this.status < 400;
    }
}

