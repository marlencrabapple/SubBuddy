/*
Copyright (C) 2012 Ian Bradley

This file is part of SubBuddy.

SubBuddy is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.

SubBuddy is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU General Public License for more details.

You should have received a copy of the GNU General Public License along with SubBuddy. If not, see http://www.gnu.org/licenses/.
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
using System.Data;
//using System.Xml.Linq;

namespace SubBuddy
{
    public class Buddy
    {
        SyndicationFeed newVids;
        public string currentextention;
        String[] ids;
        String[] names;
        String[] titles;
        String username;
        String password;
        String whichApp;
        Main window;
        int localMode=0;
        List<string> currentlyDownloading = new List<string>();
        downloadObject taargus;

        public void watch(String username1, String password1, String context, Main ui, int fromSelf)
        {
            if (fromSelf != 1)
            {
                writeToLog("Started Watching.");
                ui.EnableButton(false);
                ui.set_button1_text("Watching...");
                setVars(username1, password1, context, ui);
            
                if (password1 != "")
                {
                    //login();
                    // One day I'll get to implementing this...
                }
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
            //Console.WriteLine("Waited: " + Convert.ToInt32(Settings.Default.Delay * 1000) * 60);
            ui.newThread();
            //watch(username, password, "", ui,1);
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
            whichApp = context;
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

        public void login()
        {
            // not yet implemented
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
                            XmlReader reader = XmlReader.Create("http://gdata.youtube.com/feeds/api/users/" + dengus[i] + "/uploads?max-results=" + Settings.Default.DownloadQueue);
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
                    XmlReader reader = XmlReader.Create("http://gdata.youtube.com/feeds/api/users/" + dengus[i] + "/uploads?max-results=" + Settings.Default.DownloadQueue);
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

            while (true)
            {
                try
                {
                    XmlReader reader = XmlReader.Create("http://gdata.youtube.com/feeds/api/users/" + username + "/newsubscriptionvideos?max-results=" + Settings.Default.DownloadQueue); ;
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

            titles = new String[newVidsList.Count];
            newVidsList.CopyTo(titles);
            int i = 0;
            ids = new String[newVids.Items.Count()];
            names = new String[newVids.Items.Count()];
            
            foreach (var item in newVids.Items)
            {
                ids[i] = item.Id.ToString();

                foreach (var author in item.Authors)
                {
                    names[i] = author.Name;
                }

                ids[i] = ids[i].Remove(0, (ids[i].LastIndexOf("/") + 1));
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

            //trimDownloadedLog(databasepath);

            foreach (var video in ids)
            {
                window.set_statusbar_text("Waiting to download video " + (i + 1) + "/" + ids.Length + ": " + titles[i]);

                // check if id is in file before downloading
                String[] dungus = File.ReadAllLines(databasepath);
                String[] dongus = File.ReadAllLines(blacklistpath);
                String[] dengus = File.ReadAllLines(synonymspath);

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
                            String degrengos = "turkey";

                            if (Settings.Default.Async)
                            {
                                if ((currentlyDownloading.Contains(video)) || (currentlyDownloading.Count >= Settings.Default.MaxDownloads))
                                {
                                    degrengos = "turkey";
                                }
                                else
                                {
                                    degrengos = getDownloadLink("http://www.youtube.com/watch?v=" + video);
                                }
                            }
                            else
                            {
                                degrengos = getDownloadLink("http://www.youtube.com/watch?v=" + video);
                            }

                            if (degrengos != "turkey") // Checks if there were no problems with the video page (needs NSFW handler)
                            {
                                Uri downloadlink = new Uri(degrengos);
                                String videopath = Settings.Default.Path + "/" + names[i] + " - " + titles[i] + " - " + video + currentextention;

                                // Account synonyms
                                foreach (String turkey in dengus)
                                {
                                    if (turkey.Contains(names[i]))
                                    {
                                        videopath = Settings.Default.Path + "/" + turkey + "/" + names[i] + " - " + titles[i] + " - " + video + currentextention;
                                    }
                                }

                                bool dispalyAsyncDownloadStatus = false;

                                if (Settings.Default.Async == true)
                                {
                                    dispalyAsyncDownloadStatus = true;
                                    //window.set_statusbar_text("Downloading " + currentlyDownloading.Count + " video(s).");
                                }
                                else
                                {
                                    window.set_statusbar_text("Downloading video " + (i + 1) + "/" + ids.Length + ": " + titles[i]);
                                }

                                // Hopefully this works
                                try
                                {
                                    if (attempts <= 5)
                                    {
                                        if (Settings.Default.Async == true)
                                        {
                                            if (currentlyDownloading.Count < Settings.Default.MaxDownloads)
                                            {
                                                //wc.DownloadFileAsync(downloadlink, videopath);
                                                //downloadObject taargus;
                                                taargus.downloadlink = downloadlink;
                                                taargus.id = video;
                                                taargus.downloadpath = videopath;
                                                //downloadAsync(taargus);

                                                Thread t = new Thread(downloadAsync);
                                                t.Start();
                                            }
                                        }
                                        else
                                        {
                                            WebClient wc = new WebClient();
                                            wc.DownloadFile(downloadlink, videopath);

                                            // for thumbnail downloading
                                            taargus.downloadlink = downloadlink;
                                            taargus.id = video;
                                            taargus.downloadpath = videopath;

                                            downloadThumbnails(taargus);
                                            wc.Dispose();
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
                                writeToLog("Couldn't download \"" + names[i] + " - " + titles[i] + " - " + video + currentextention + "\" after 5 attempts.");
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

        public struct downloadObject
        {
            public string id;
            public string downloadpath;
            public Uri downloadlink;

            public downloadObject(string setId, string setPath, Uri setLink)
            {
                id = setId;
                downloadlink = setLink;
                downloadpath = setPath;
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
                    wc.DownloadFile(paynus.downloadlink, paynus.downloadpath);
                    downloadThumbnails(paynus);
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
                    File.AppendAllText(databasepath, paynus.id + "\n");
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

        public string getDownloadLink(String sourceurl)
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

                break;
            }

            // Fixes bad video crashes
            if (source.Contains("This video is currently being processed"))
            {
                return "turkey";
            }

            if (source.Contains("This video is unavailable"))
            {
                return "turkey";
            }

            if (source.Contains("This video may be inappropriate for some users."))
            {
                return "turkey";
            }

            //int start = source.IndexOf("movie_player");
            //source = source.Substring(start);
            //start = source.IndexOf("flashvars");
            //source = source.Substring(start);
           
            int start = source.IndexOf("url_encoded_fmt_stream_map");
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
                    fdsa[i] = dungus;
                    i++;
                }
            }

            // Fixes crashes that started in july/august 2012
            if (fdsa[0].StartsWith(" \"url_encoded_fmt_stream_map\":"))
            {
                fdsa[0] = fdsa[0].Replace(" \"url_encoded_fmt_stream_map\": \"", "");
            }

            String[] itag = new String[fdsa.Length];
            String[] quality = new String[fdsa.Length];
            String[] fallback_host = new String[fdsa.Length];
            String[] type = new String[fdsa.Length];
            String[] url = new String[fdsa.Length];
            String[] sig = new String[fdsa.Length];

            i = 0;

            foreach (var dungus in fdsa)
            {
                itag[i] = dungus.Substring(dungus.IndexOf("itag=") + "itag=".Length);
                if (itag[i].Contains("\\u0026"))
                {
                    itag[i] = itag[i].Substring(0,itag[i].IndexOf("\\u0026"));
                }

                quality[i] = dungus.Substring(dungus.IndexOf("quality=") + "quality=".Length);
                if (quality[i].Contains("\\u0026"))
                {
                    quality[i] = quality[i].Substring(0, quality[i].IndexOf("\\u0026"));
                }

                quality[i] = dungus.Substring(dungus.IndexOf("quality=") + "quality=".Length);
                if (quality[i].Contains("\\u0026"))
                {
                    quality[i] = quality[i].Substring(0, quality[i].IndexOf("\\u0026"));
                }

                // This could probably prevent a lot of crashes...
                fallback_host[i] = dungus.Substring(dungus.IndexOf("fallback_host=") + "fallback_host=".Length);
                if (fallback_host[i].Contains("\\u0026"))
                {
                    fallback_host[i] = fallback_host[i].Substring(0, fallback_host[i].IndexOf("\\u0026"));
                }

                type[i] = dungus.Substring(dungus.IndexOf("type=") + "type=".Length);
                if (type[i].Contains("\\u0026"))
                {
                    type[i] = type[i].Substring(0, type[i].IndexOf("\\u0026"));
                }

                url[i] = dungus.Substring(dungus.IndexOf("url=") + "url=".Length);
                if (url[i].Contains("\\u0026"))
                {
                    url[i] = url[i].Substring(0, url[i].IndexOf("\\u0026"));
                }

                sig[i] = dungus.Substring(dungus.IndexOf("sig=") + "sig=".Length);
                if (sig[i].Contains("\\u0026"))
                {
                    sig[i] = sig[i].Substring(0, sig[i].IndexOf("\\u0026"));
                }

                url[i] = Uri.UnescapeDataString(url[i]);
                sig[i] = Uri.UnescapeDataString(sig[i]);
                url[i] += "&signature=" + sig[i];

                i++;
            }

            foreach (var qualitylevel in url)
            {
                if (qualitylevel.Contains("itag=84"))
                {
                    currentextention = ".mp4";
                    return qualitylevel;
                }
                else if (qualitylevel.Contains("itag=85"))
                {
                    currentextention = ".mp4";
                    return qualitylevel;
                }
                else if (qualitylevel.Contains("itag=82"))
                {
                    currentextention = ".mp4";
                    return qualitylevel;
                }
                else if (qualitylevel.Contains("itag=83"))
                {
                    currentextention = ".mp4";
                    return qualitylevel;
                }
                else if (qualitylevel.Contains("itag=38"))
                {
                    currentextention = ".mp4";
                    return qualitylevel;
                }
                else if (qualitylevel.Contains("itag=37"))
                {
                    currentextention = ".mp4";
                    return qualitylevel;
                }
                else if (qualitylevel.Contains("itag=22"))
                {
                    currentextention = ".mp4";
                    return qualitylevel;
                }
                else if (qualitylevel.Contains("itag=35"))
                {
                    currentextention = ".flv";
                    return qualitylevel;
                }
                else if (qualitylevel.Contains("itag=34"))
                {
                    currentextention = ".flv";
                    return qualitylevel;
                }
                else if (qualitylevel.Contains("itag=18"))
                {
                    currentextention = ".mp4";
                    return qualitylevel;
                }
                else if (qualitylevel.Contains("itag=6"))
                {
                    currentextention = ".flv";
                    return qualitylevel;
                }
                else if (qualitylevel.Contains("itag=6"))
                {
                    currentextention = ".flv";
                    return qualitylevel;
                }
            }
            Debug.WriteLine("Nothing?");
            return url[0];
        }

        public void SubSync(String user, Main ui)
        {
            ui.set_statusbar_text("Syncing subscriptions");

            int amount;
            int pages;
            int i=0;
            WebClient wc = new WebClient();
            String source = wc.DownloadString("http://gdata.youtube.com/feeds/api/users/" + user + "/subscriptions");
            String localSubs = File.ReadAllText(Settings.Default.Path + "localsubs");
            
            String[] subsList;

            source = System.Web.HttpUtility.HtmlDecode(source);
            source = source.Substring(source.IndexOf("totalResults>")+13,5);
            source = source.Substring(0,source.IndexOf("<"));

            amount = Convert.ToInt32(source);
            pages = (amount / 25) + 1;
            subsList = new String[amount];

            //MessageBox.Show(amount + " / " +pages);

            for (int page = 1; page < amount; page+=25)
            {
                if (page > 976)
                {
                    break;
                }

                XmlReader reader = XmlReader.Create("http://gdata.youtube.com/feeds/api/users/" + user + "/subscriptions?start-index="+page);
                SyndicationFeed subs = SyndicationFeed.Load(reader);

                foreach (var sub in subs.Items)
                {
                    String subText;
                    
                    subText = sub.Links[2].Uri.ToString();

                    if (subText.LastIndexOf("/") > subText.IndexOf("UC"))
                    {
                        int dringus = subText.IndexOf("UC");
                        int drungul = subText.LastIndexOf("/");
                        subText = subText.Substring(dringus, drungul-dringus);
                    }
                    else
                    {
                        subText = subText.Substring(subText.IndexOf("UC"));
                    }

                    if (!localSubs.Contains(subText))
                    {
                        subsList[i] = subText; // I'm keeping this in memory for debugging purposes.
                        System.IO.File.AppendAllText(Settings.Default.Path + "localsubs", subsList[i]+"\n");
                    }

                    i++;
                }
                reader.Close();

                if (page > 976)
                {
                    break;
                }
            }

            MessageBox.Show("Sync Completed!");
            ui.set_statusbar_text("Idle");
            wc.Dispose();
        }

        public void writeToLog(String message)
        {
            File.AppendAllText(Settings.Default.Path + "/errorlog", "[" + System.DateTime.Now.ToShortDateString() + " " + System.DateTime.Now.ToShortTimeString() + "]\n" + message + "\n\n");
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
