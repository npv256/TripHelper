﻿using System;
using System.Collections;
using System.IO;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Web;

using RestSharp;
using RestSharp.Extensions;


namespace WEB
{

    public class Client : RestClient
    {
        // Constructor
        public Client(string baseUrl) : base(baseUrl)
        {
        }

        public static RestRequest GetUploadRequest(string filename)
        {
            var request = new RestRequest("/path/data/", Method.POST);
            request.AddFile("file", filename);

            return request;
        }

        public static IRestResponse Upload(RestClient client, string filename)
        {
            var request = GetUploadRequest(filename);

            return client.Execute(request);
        }

        public static RestRequest GetDownloadRequest(string downloadName)
        {
            var request = new RestRequest("/path/data/" + downloadName, Method.GET);

            return request;
        }

        public static IRestResponse Download(RestClient client, string downloadName, string downloadLocation)
        {
            var request = GetDownloadRequest(downloadName);
            client.DownloadData(request).SaveAs(downloadLocation);

            return client.Execute(request);
        }

        public static RestRequest GetMoveRequest(string sourceFile, string destinationFolder)
        {
            var request = new RestRequest("/path/oper/move/", Method.POST);
            request.AddParameter("src", sourceFile);
            request.AddParameter("dst", destinationFolder);

            return request;
        }

        public static IRestResponse Move(RestClient client, string sourceFile, string destinationFolder)
        {
            var request = GetMoveRequest(sourceFile, destinationFolder);

            return client.Execute(request);
        }

        public static RestRequest GetRemoveRequest(string FileToBeDeleted)
        {
            var request = new RestRequest("/path/oper/remove/", Method.POST);
            request.AddParameter("path", FileToBeDeleted);

            return request;
        }

        public static IRestResponse Remove(RestClient client, string FileToBeDeleted)
        {
            var request = GetRemoveRequest(FileToBeDeleted);

            return client.Execute(request);
        }
    }
}
