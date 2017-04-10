﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using RedHttpServer;

namespace TestServer
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var http = new RedHttpServer.RedHttpServer();
            http.Get("/:test/:test1/:test2", (req, res) =>
            {
                res.SendString("URL: " + req.Params["test"] + req.Params["test1"] + req.Params["test2"]);
            });

            http.Get("/", (req, res) =>
            {
                res.Redirect("/test");
            });
            http.WebSocket("/wstest", async (req, wsd) =>
            {
                await wsd.SendText("Welcome to the echo test server");
                wsd.OnTextReceived += (sender, eventArgs) =>
                {
                    wsd.SendText("why do you say " + eventArgs.Text);
                };
            });
            http.Start();
            Console.ReadKey();
        }
    }
}
