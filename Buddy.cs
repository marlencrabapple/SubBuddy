/*
Copyright (C) 2012 Ian Bradley

This file is part of SubBuddy.

SubBuddy is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.

SubBuddy is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU General Public License for more details.

You should have received a copy of the GNU General Public License along with SubBuddy. If not, see https://www.gnu.org/licenses/.
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.IO;
using System.Xml;
using System.ServiceModel.Syndication;
using System.Net;
using System.Threading;
using System.Windows.Forms;
using Google.GData.YouTube;
using Google.GData.Client;
using Google.YouTube;

namespace SubBuddy
{
    public class Buddy
    {
        SyndicationFeed newVids;
        public string extension;
        String[] ids;
        String[] names;
        String[] titles;
        String username;
        String password;
        String whichapp;
        Main window;
        int localMode=0;
        List<string> currentlyDownloading = new List<string>();
        downloadObject taargus;
        String devkey = "";
        YouTubeRequest request;

        public struct downloadObject
        {
            public string id;
            public string downloadpath;
            public Uri videourl;
            public Uri audiourl;
            int downloadcompleted;

            public downloadObject(string setId, string setPath, Uri setVideoLink, Uri setAudioLink, int setCompleted)
            {
                id = setId;
                videourl = setVideoLink;
                audiourl = setAudioLink;
                downloadpath = setPath;
                downloadcompleted = setCompleted;
            }
        }

        public void watch(String username1, String password1, String context, Main ui, int fromSelf)
        {
            if (fromSelf != 1)
            {
                writeToLog("Started Watching.");
                ui.EnableButton(false);
                ui.set_button1_text("Watching...");
                setVars(username1, password1, context, ui);
                login(username, password);
            }

            readAndParse();
            downloadVids();
            ui.set_statusbar_text("Waiting " + Settings.Default.Delay.ToString() + " minute(s)...");
            ui.set_list_text(new List<string>());

            if ((Settings.Default.Async) && (currentlyDownloading.Count > 0))
            {
                window.set_statusbar_text("Downloading " + currentlyDownloading.Count + " video(s).");
            }

            Thread.Sleep(Convert.ToInt32(Settings.Default.Delay * 1000) * 60);
            ui.newThread();
        }

        public void setQueueDownloaded(String username1, String password1, String context, Main ui)
        {
            String databasepath = Settings.Default.Path + "downloaded";
            String[] dungus = File.ReadAllLines(databasepath);
            setVars(username1, password1, context, ui);
            getQueue();

            foreach (var video in ids)
            {
                if (!dungus.Contains<string>(video))
                {
                    System.IO.File.AppendAllText(databasepath, video + "\n");
                }
            }
        }

        private void setVars(String username1, String password1, String context, Main ui)
        {
            username = username1;
            password = password1;
            whichapp = context;
            window = ui;

            if (Settings.Default.SubscriptionType==1)
            {
                localMode = 1;
            }

            if (Settings.Default.SubscriptionType==2)
            {
                localMode = 2;
            }
        }

        public void login(string user, string pass)
        {
            devkey = Properties.Resources.devkey.Length == 0 ? File.ReadAllText("devkey") : System.Text.Encoding.Default.GetString(Properties.Resources.devkey);
            YouTubeRequestSettings settings = new YouTubeRequestSettings("SubBuddy", devkey, user, pass);
            request = new YouTubeRequest(settings);
        }

        public void readAndParse()
        {
            // Local Subs
            if (localMode==1 || localMode==2)
            {
                String[] dengus = File.ReadAllLines(Settings.Default.Path + "localsubs");
                List<string> newVidsList = new List<string>();

                for (int i = 0; i < dengus.Length; i++)
                {
                    while (true)
                    {
                        try
                        {
                            XmlReader reader = XmlReader.Create("https://gdata.youtube.com/feeds/api/users/" + dengus[i] + "/uploads?max-results=" + Settings.Default.DownloadQueue);
                            newVids = SyndicationFeed.Load(reader);
                            reader.Close();
                        }
                        catch (WebException e)
                        {
                            writeToLog(e.ToString());
                            continue;
                        }
                        break;
                    }

                    foreach (var item in newVids.Items)
                    {
                        newVidsList.Add(item.Title.Text);
                    }
                }

                ids = new String[newVidsList.Count];
                names = new String[newVidsList.Count];
                titles = new String[newVidsList.Count];

                newVidsList.CopyTo(titles);

                int i2 = 0;

                for (int i = 0; i < dengus.Length; i++)
                {
                    XmlReader reader = XmlReader.Create("https://gdata.youtube.com/feeds/api/users/" + dengus[i] + "/uploads?max-results=" + Settings.Default.DownloadQueue);
                    newVids = SyndicationFeed.Load(reader);
                    reader.Close();

                    foreach (var item in newVids.Items)
                    {
                        ids[i2] = item.Id.ToString();
                        ids[i2] = ids[i2].Remove(0, (ids[i2].LastIndexOf("/") + 1));

                        foreach (var author in item.Authors)
                        {
                            if (author.Name.StartsWith("UC"))
                            {
                                names[i2] = author.Name.Substring(2);
                            }
                            else
                            {
                                names[i2] = author.Name;
                            } 
                        }

                        i2++;
                    }
                }

                window.set_list_text(newVidsList);
            }

            // Normal Subs
            if (localMode == 0 || localMode == 2) 
            {
                // getQueue() doesn't support localsubs yet
                window.set_list_text(getQueue());
            }
        }

        public List<string> getQueue()
        {
            List<string> newVidsList = new List<string>();

            if (username == "")
            {
                newVidsList.Add("Error");
            }

            Feed<Video> videofeed;

            while (true)
            {
                try
                {
                    Uri requesturi = new Uri("https://gdata.youtube.com/feeds/api/users/default/newsubscriptionvideos?max-results=" + Settings.Default.DownloadQueue);
                    videofeed = request.Get<Video>(requesturi);

                    // test the feed
                    foreach (var item in videofeed.Entries)
                    {
                        // do nothing
                    }
                }
                catch (Exception e)
                {
                    writeToLog(e.ToString());
                    continue;
                }
                break;
            }

            foreach (var item in videofeed.Entries)
            {
                newVidsList.Add(item.Title);
            }

            titles = new String[newVidsList.Count];
            newVidsList.CopyTo(titles);
            int i = 0;
            ids = new String[videofeed.Entries.Count()];
            names = new String[videofeed.Entries.Count()];
            
            foreach (var item in videofeed.Entries)
            {
                ids[i] = item.VideoId;
                names[i] = item.Author;
                i++;
            }

            return newVidsList;
        }

        public void downloadVids()
        {
            int i = 0;
            
            String databasepath = Settings.Default.Path + "/downloaded";
            String blacklistpath = Settings.Default.Path + "/blacklist";
            String synonymspath = Settings.Default.Path + "/synonyms";

            foreach (var video in ids)
            {
                window.set_statusbar_text("Waiting to download video " + (i + 1) + "/" + ids.Length + ": " + titles[i]);

                // check if id is in file before downloading
                String[] dungus;
                String[] dongus;
                String[] dengus;

                while (true)
                {
                    try
                    {
                        dungus = File.ReadAllLines(databasepath);
                        dongus = File.ReadAllLines(blacklistpath);
                        dengus = File.ReadAllLines(synonymspath);
                        break;
                    }
                    catch (IOException e)
                    {
                        writeToLog(e.ToString());
                        continue;
                    }
                }

                if ((!dungus.Contains<string>(video))&&(!currentlyDownloading.Contains(video)))
                {
                    foreach (char c in System.IO.Path.GetInvalidFileNameChars())
                    {
                        names[i] = names[i].Replace(c, '_');
                        titles[i] = titles[i].Replace(c, '_');
                    }

                    if (!dongus.Contains<string>(names[i]))
                    {
                        bool pruppets = true;

                        while (pruppets == true)
                        {
                            int attempts = 0;
                            String[] degrengos = { "","" };

                            if (Settings.Default.Async)
                            {
                                if ((currentlyDownloading.Contains(video)) || (currentlyDownloading.Count >= Settings.Default.MaxDownloads))
                                {
                                    degrengos[0] = "turkey";
                                }
                                else
                                {
                                    degrengos = getDownloadLink("https://www.youtube.com/watch?v=" + video);
                                }
                            }
                            else
                            {
                                degrengos = getDownloadLink("https://www.youtube.com/watch?v=" + video);
                            }

                            if (degrengos[0] != "turkey") // Checks if there were no problems with the video page
                            {
                                Uri videourl = new Uri(degrengos[0]);
                                Uri audiourl = degrengos[1] != "" ? new Uri(degrengos[1]) : null;
                                
                                String videopath = Settings.Default.Path + "/" + names[i] + " - " + titles[i] + " - " + video + extension;

                                // Account synonyms
                                foreach (String turkey in dengus)
                                {
                                    if (turkey.Contains(names[i]))
                                    {
                                        videopath = Settings.Default.Path + "/" + turkey + "/" + names[i] + " - " + titles[i] + " - " + video + extension;
                                    }
                                }

                                bool displayAsyncDownloadStatus = false;

                                if (Settings.Default.Async == true)
                                {
                                    displayAsyncDownloadStatus = true;
                                }
                                else
                                {
                                    window.set_statusbar_text("Downloading video " + (i + 1) + "/" + ids.Length + ": " + titles[i]);
                                }

                                try
                                {
                                    if (attempts <= 2)
                                    {
                                        if (Settings.Default.Async == true)
                                        {
                                            if (currentlyDownloading.Count < Settings.Default.MaxDownloads)
                                            {
                                                taargus.videourl = videourl;
                                                taargus.audiourl = audiourl;
                                                taargus.id = video;
                                                taargus.downloadpath = videopath;

                                                Thread t = new Thread(downloadAsync);
                                                t.Start();
                                            }
                                        }
                                        else
                                        {
                                            while (true)
                                            {
                                                WebClient wc = new WebClient();

                                                wc.DownloadFile(videourl, videopath);
                                                Int64 fsize = Convert.ToInt64(wc.ResponseHeaders["Content-Length"]);

                                                // check file size
                                                FileInfo f = new FileInfo(videopath);
                                                Int64 dlfsize = f.Length;

                                                if (dlfsize < fsize)
                                                {
                                                    writeToLog("Filesize mismatch: YT=" + fsize.ToString() + ", Local=" + dlfsize.ToString());
                                                    continue;
                                                }

                                                if (degrengos[1] != null)
                                                {
                                                    wc.DownloadFile(audiourl, videopath.Replace("mp4", "m4a"));
                                                    Int64 afsize = Convert.ToInt64(wc.ResponseHeaders["Content-Length"]);

                                                    FileInfo af = new FileInfo(videopath.Replace("mp4", "m4a"));
                                                    Int64 adlfsize = af.Length;

                                                    if (adlfsize < afsize)
                                                    {
                                                        writeToLog("Filesize mismatch: YT=" + afsize.ToString() + ", Local=" + adlfsize.ToString());
                                                        continue;
                                                    }
                                                    else
                                                    {
                                                        string cleanaudiopath = '"' + af.FullName + '"';
                                                        string cleanvideopath = '"' + f.FullName + '"';
                                                        System.Diagnostics.Process process = new System.Diagnostics.Process();                                                  
                                                        System.Diagnostics.ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo();
                                                        startInfo.UseShellExecute = false;
                                                        startInfo.CreateNoWindow = true;
                                                        startInfo.FileName = "cmd.exe";
                                                        startInfo.Arguments = "/c mp4box.exe -add " + cleanaudiopath + " " + cleanvideopath;
                                                        process.StartInfo = startInfo;
                                                        process.Start();

                                                        Thread.Sleep(1000);
                                                        File.Delete(af.FullName);

                                                        break;
                                                    } 
                                                }
                                                else
                                                {
                                                    break;
                                                }
                                            }

                                            // for thumbnail downloading
                                            taargus.videourl = videourl;
                                            taargus.audiourl = videourl;
                                            taargus.id = video;
                                            taargus.downloadpath = videopath;

                                            downloadThumbnails(taargus);
 
                                        }
                                    }
                                    pruppets = false;
                                }
                                catch (System.Net.WebException e)
                                {
                                    writeToLog(e.ToString());
                                    pruppets = true;
                                    attempts++;
                                    continue;
                                }
                            }
                            else
                            {
                                Thread.Sleep(1000);
                                continue;
                            }

                            if (attempts <= 5)
                            {
                                // After download write downloaded id to file
                                if (Settings.Default.Async == false)
                                {
                                    Debug.WriteLine(Settings.Default.Async);
                                    File.AppendAllText(databasepath, video + "\n");
                                }
                            }
                            else
                            {
                                writeToLog("Couldn't download \"" + names[i] + " - " + titles[i] + " - " + video + extension + "\" after 5 attempts.");
                            }
                        }
                    }
                    else
                    {
                        window.set_statusbar_text(names[i] +" is on your blacklist");
                    }
                }
                else
                {
                    window.set_statusbar_text("Already downloaded video " + (i + 1) + "/" + ids.Length + ": " + titles[i]);
                    Thread.Sleep(30);
                }
                i++;
            }
        }

        public void downloadAsync()
        {
            bool success = true;
            String databasepath = Settings.Default.Path + "/downloaded";
            downloadObject paynus = taargus;

            if ((currentlyDownloading.Contains(paynus.id) == false) && (currentlyDownloading.Count() < Settings.Default.MaxDownloads))
            {
                currentlyDownloading.Add(paynus.id);
                window.set_statusbar_text("Downloading " + currentlyDownloading.Count + " video(s).");
                WebClient wc = new WebClient();
               
                try
                {
                    // get content-length from headers
                    downloadThumbnails(paynus);
                    wc.DownloadFile(paynus.videourl, paynus.downloadpath);
                    Int64 vfsize = Convert.ToInt64(wc.ResponseHeaders["Content-Length"]);

                    // check file size
                    FileInfo f = new FileInfo(paynus.downloadpath);
                    Int64 vdlfsize = f.Length;

                    if (vdlfsize < vfsize)
                    {
                        writeToLog("Filesize mismatch: YT=" + vfsize.ToString() + ", Local=" + vdlfsize.ToString());
                        currentlyDownloading.Remove(paynus.id);
                        success = false;
                    }

                    if (paynus.audiourl != null)
                    {
                        wc.DownloadFile(paynus.audiourl, paynus.downloadpath.Replace("mp4", "m4a"));
                        Int64 afsize = Convert.ToInt64(wc.ResponseHeaders["Content-Length"]);

                        FileInfo af = new FileInfo(paynus.downloadpath.Replace("mp4","m4a"));
                        Int64 adlfsize = af.Length;

                        if (adlfsize < afsize)
                        {
                            writeToLog("Filesize mismatch: YT=" + afsize.ToString() + ", Local=" + adlfsize.ToString());
                            currentlyDownloading.Remove(paynus.id);
                            success = false;
                        }
                        else
                        {
                            string cleanaudiopath = '"' + af.FullName + '"';
                            string cleanvideopath = '"' + f.FullName + '"';
                            System.Diagnostics.Process process = new System.Diagnostics.Process();
                            System.Diagnostics.ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo();
                            startInfo.UseShellExecute = false;
                            startInfo.CreateNoWindow = true;
                            startInfo.FileName = "cmd.exe";
                            startInfo.Arguments = "/c mp4box.exe -add " + cleanaudiopath + " " + cleanvideopath;
                            process.StartInfo = startInfo;
                            process.Start();

                            Thread.Sleep(1000);
                            File.Delete(af.FullName);
                        }
                    }
                }
                catch(WebException e)
                {
                    writeToLog(e.ToString());
                    currentlyDownloading.Remove(paynus.id);
                    success = false;
                }
                
                wc.Dispose();

                if (success)
                {
                    while (true)
                    {
                        try
                        {
                            File.AppendAllText(databasepath, paynus.id + "\n");
                            break;
                        }
                        catch (IOException e)
                        {
                            continue;
                        }
                    }

                    currentlyDownloading.Remove(paynus.id);
                }

                if (currentlyDownloading.Count == 0)
                {
                    window.set_statusbar_text("Waiting " + Settings.Default.Delay.ToString() + " minute(s)...");
                }
                else
                {
                    window.set_statusbar_text("Downloading " + currentlyDownloading.Count + " video(s).");
                }
            }
        }

        public void downloadThumbnails(downloadObject paynus)
        {
            WebClient wc = new WebClient();

            while (true)
            {
                try
                {
                    if (Settings.Default.Thumbnails == 0)
                    {
                        wc.DownloadFile("https://img.youtube.com/vi/" + paynus.id + "/0.jpg", paynus.downloadpath + "_thumb-1.jpg");
                        wc.DownloadFile("https://img.youtube.com/vi/" + paynus.id + "/1.jpg", paynus.downloadpath + "_thumb-2.jpg");
                        wc.DownloadFile("https://img.youtube.com/vi/" + paynus.id + "/2.jpg", paynus.downloadpath + "_thumb-3.jpg");
                        wc.DownloadFile("https://img.youtube.com/vi/" + paynus.id + "/3.jpg", paynus.downloadpath + "_thumb-4.jpg");
                    }
                    else if (Settings.Default.Thumbnails == 1)
                    {
                        wc.DownloadFile("https://img.youtube.com/vi/" + paynus.id + "/1.jpg", paynus.downloadpath + "_thumb-1.jpg");
                    }
                    else if (Settings.Default.Thumbnails == 2)
                    {
                        wc.DownloadFile("https://img.youtube.com/vi/" + paynus.id + "/0.jpg", paynus.downloadpath + "_thumb-1.jpg");
                    }
                }
                catch (WebException e)
                {
                    writeToLog(e.ToString());
                    continue;
                }

                break;
            }

            wc.Dispose();
        }

        public string[] getDownloadLink(String sourceurl)
        {
            String source;

            while (true)
            {
                try
                {
                    WebClient wc = new WebClient();
                    source = wc.DownloadString(sourceurl);
                    wc.Dispose();
                    source = System.Web.HttpUtility.HtmlDecode(source);
                }
                catch (System.Net.WebException e)
                {
                    writeToLog(e.ToString());
                    continue;
                }

                if (source.Contains("\"adaptive_fmts\": \"") || (source.Contains("\"url_encoded_fmt_stream_map\"")))
                {
                    break;
                }

                // Fixes bad video crashes
                if (source.Contains("This video is currently being processed"))
                {
                    String[] error = { "turkey" };
                    return error;
                }

                if (source.Contains("This video is unavailable"))
                {
                    if (source.Contains("<div id=\\\"player-unavailable\\\" class=\\\"    hid  \\\">"))
                    {
                        String[] error = { "turkey" };
                        return error;
                    }
                }

                if (source.Contains("This video may be inappropriate for some users."))
                {
                    String[] error = { "turkey" };
                    return error;
                }

                System.Threading.Thread.Sleep(1000);
            }

            int start = (source.IndexOf("\"adaptive_fmts\": ") < source.IndexOf("\"url_encoded_fmt_stream_map\": ") && (source.IndexOf("\"adaptive_fmts\": ") != -1)) ? source.IndexOf("\"adaptive_fmts\": ") : source.IndexOf("\"url_encoded_fmt_stream_map\": ");

            source = source.Substring(start);

            String[] asdf;
            String[] fdsa;
            int i = 0;

            asdf = source.Split(',');

            foreach (var dungus in asdf)
            {
                if (dungus.Contains("url=http") && dungus.Contains("itag="))
                {
                    i++;
                }
            }

            fdsa = new String[i];
            i = 0;

            foreach (var dungus in asdf)
            {
                if (dungus.Contains("url=http") && dungus.Contains("itag="))
                {
                    fdsa[i] = dungus.Replace("\"adaptive_fmts\": \"", "");
                    fdsa[i] = fdsa[i].Replace(" \"url_encoded_fmt_stream_map\": \"", "");
                    fdsa[i] = fdsa[i].Replace(" \"adaptive_fmts\": \"", "");
                    fdsa[i] = fdsa[i].Replace("\"url_encoded_fmt_stream_map\": \"", "");
                    i++;
                }
            }

            String[] fallback_host = new String[fdsa.Length];
            String[] url = new String[fdsa.Length];
            String[] sig = new String[fdsa.Length];

            i = 0;

            foreach (var dungus in fdsa)
            {
                // This could probably prevent a lot of crashes...
                fallback_host[i] = dungus.Substring(dungus.IndexOf("fallback_host=") + "fallback_host=".Length);
                if (fallback_host[i].Contains("\\u0026"))
                {
                    fallback_host[i] = fallback_host[i].Substring(0, fallback_host[i].IndexOf("\\u0026"));
                }

                url[i] = dungus.Substring(dungus.IndexOf("url=") + "url=".Length);
                if (url[i].Contains("\\u0026"))
                {
                    url[i] = url[i].Substring(0, url[i].IndexOf("\\u0026"));
                }

                if (!url[i].Contains("signature"))
                {
                    sig[i] = dungus.Substring(dungus.IndexOf("sig=") + "sig=".Length);
                    if (sig[i].Contains("\\u0026"))
                    {
                        sig[i] = sig[i].Substring(0, sig[i].IndexOf("\\u0026"));
                    }

                    sig[i] = "&signature=" + Uri.UnescapeDataString(sig[i]);
                }
                else
                {
                    sig[i] = "";
                }

                url[i] = Uri.UnescapeDataString(url[i]);
                url[i] += sig[i];

                i++;
            }

            string[] vitags = { "itag=38", "itag=37", "itag=137", "itag=22", "itag=136", "itag=135", "itag=34", "itag=18", "itag=134", "itag=133", "itag=160", "itag=17" };
            string[] aitags = { "itag=141", "itag=140", "itag=139" };
            string[] urls = { "", "" };

            foreach (var itag in vitags)
            {
                foreach (var urlvar in url)
                {
                    if (urlvar.Contains(itag))
                    {
                        extension = ".mp4";
                        urls[0] = urlvar;

                        if ((itag == "itag=137") || (itag == "itag=136") || (itag == "itag=135") || (itag == "itag=134") || (itag == "itag=133"))
                        {
                            foreach (var audio in aitags)
                            {
                                foreach (var urlvar2 in url)
                                {
                                    if (urlvar2.Contains(audio))
                                    {
                                        urls[1] = urlvar2;
                                        extension = ".mp4";
                                        return urls;
                                    }
                                }
                            }
                        }
                        else
                        {
                            return urls;
                        }
                        break;
                    }
                }
            }

            urls[0] = url[0];
            return urls;
        }

        public void SubSync(String user, String pass, Main ui)
        {
            String[] localsubs = File.Exists(Settings.Default.Path + "localsubs") ? File.ReadAllLines(Settings.Default.Path + "localsubs") : new String[1];
            String newsubs = "";
            
            ui.set_statusbar_text("Syncing subscriptions");
            
            if(request == null)
            {
                login(user, pass);
            }

            Feed<Subscription> videofeed;
            Uri requesturi = new Uri("https://gdata.youtube.com/feeds/api/users/default/subscriptions?max-results=50");
            videofeed = request.Get<Subscription>(requesturi);

            int pages = (videofeed.TotalResults / 50) + 1;
            int page = 0;

            for (int i = 0; i < pages; i++)
            {
                if(i > 0)
                {
                    requesturi = new Uri("https://gdata.youtube.com/feeds/api/users/default/subscriptions?max-results=50&start-index=" + page);
                    videofeed = request.Get<Subscription>(requesturi);
                }

                foreach (var item in videofeed.Entries)
                {
                    if (!localsubs.Contains<String>(item.UserName))
                    {
                        newsubs += item.UserName + "\n";
                    }
                }

                if (page == 951)
                {
                    break;
                }

                page += 50;
            }

            foreach (var sub in localsubs)
            {
                newsubs += sub + "\n";
            }

            File.WriteAllText(Settings.Default.Path + "localsubs", newsubs);

            MessageBox.Show("Sync Completed!");
            ui.set_statusbar_text("Idle");
        }

        public void writeToLog(String message)
        {
            while (true)
            {
                try
                {
                    File.AppendAllText(Settings.Default.Path + "/errorlog", "[" + System.DateTime.Now.ToShortDateString() + " " + System.DateTime.Now.ToShortTimeString() + "]\n" + message + "\n\n");
                    break;
                }
                catch (IOException e)
                {
                    Thread.Sleep(1000);
                    continue;
                }
            }
            Thread.Sleep(1000);
        }

        public void trimDownloadedLog(String databasepath)
        {
            List<String> newDownloaded = new List<String>();
            int i = 0;

            foreach(String id in File.ReadAllLines(databasepath))
            {
                foreach (String downloadedId in ids)
                {
                    if(id.Equals(downloadedId))
                    {
                        newDownloaded.Add(id);
                    }
                    else if(i>49){
                        newDownloaded.Add(id);
                    }

                    i++;

                    if (i >= 65)
                    {
                        break;
                    }
                }

                if (i >= 65)
                {
                    break;
                }
            }

            File.WriteAllLines(databasepath,newDownloaded.ToArray());
        }
    }
}
