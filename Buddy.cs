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
        string currentextention;
        String[] ids;
        //String[] subscriptions;
        //String[] localsubscriptions;
        String[] names;
        String[] itag;
        String[] quality;
        String[] fallback_host;
        String[] type;
        String[] url;
        String[] sig;
        String[] titles;
        //String[] cookies;
        String username;
        //String user;
        String password;
        String whichApp;
        //String homePath;
        Main window;
        //int downloadamount;
        int localMode=0;

        private void threadHelper(object sender, System.EventArgs e)
        {
            watch(username, password, "", window);
        }

        public void watch(String username1, String password1, String context, Main ui)
        {
            ui.EnableButton(false);
            ui.set_button1_text("Watching...");
            setVars(username1, password1, context, ui);
            //checkDirs(); // apparently this isn't necessary anymore.

            if (password1 != "")
            {
                //login();
                // One day I'll get to implementing this...
            }

            readAndParse();
            downloadVids();
            ui.set_statusbar_text("Idle");
            //ui.EnableButton(true);
            //ui.set_button1_text("Watch");
            int subdelay = Convert.ToInt32(CompatSettings.Default.Delay * 1000) * 60;
            window.set_statusbar_text("Waiting " + CompatSettings.Default.Delay.ToString() + " minute(s)...");
            List<string> nothing = new List<string>();
            window.set_list_text(nothing);
            //System.Windows.Forms.Timer aTimer = new System.Windows.Forms.Timer();
            //aTimer.Interval = subdelay;
            //aTimer.Tick +=new EventHandler(threadHelper);
            //aTimer.Start();
            Thread.Sleep(subdelay); // Now we only use one thread
            watch(username, password, "", window);
        }

        public void setQueueDownloaded(String username1, String password1, String context, Main ui)
        {
            String databasepath = CompatSettings.Default.Path + "downloaded";
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

            if (CompatSettings.Default.SubscriptionType==1)
            {
                localMode = 1;
            }

            if (CompatSettings.Default.SubscriptionType==2)
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
                String[] dengus = File.ReadAllLines(CompatSettings.Default.Path + "localsubs");
                List<string> newVidsList = new List<string>();

                for (int i = 0; i < dengus.Length; i++)
                {
                    XmlReader reader = XmlReader.Create("http://gdata.youtube.com/feeds/api/users/" + dengus[i] + "/uploads?max-results=" + CompatSettings.Default.DownloadQueue);
                    newVids = SyndicationFeed.Load(reader);

                    foreach (var item in newVids.Items)
                    {
                        newVidsList.Add(item.Title.Text);
                    }

                    reader.Close();
                }

                ids = new String[newVidsList.Count];
                names = new String[newVidsList.Count];
                titles = new String[newVidsList.Count];

                newVidsList.CopyTo(titles);

                int i2 = 0;

                for (int i = 0; i < dengus.Length; i++)
                {
                    XmlReader reader = XmlReader.Create("http://gdata.youtube.com/feeds/api/users/" + dengus[i] + "/uploads?max-results=" + CompatSettings.Default.DownloadQueue);
                    newVids = SyndicationFeed.Load(reader);

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

                    reader.Close();
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

        public void getAndSetFeed(){
            XmlReader reader;
            reader = XmlReader.Create("http://gdata.youtube.com/feeds/api/users/" + username + "/newsubscriptionvideos?max-results=" + CompatSettings.Default.DownloadQueue);
            newVids = SyndicationFeed.Load(reader);
            reader.Close();
        }

        public List<string> getQueue()
        {
            List<string> newVidsList = new List<string>();

            if (username == "")
            {
                newVidsList.Add("Error");
            }

            // ghetto exception handling
            while (true)
            {
                try
                {
                    getAndSetFeed();
                }
                catch
                {
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
            WebClient wc = new WebClient();
            int i = 0;

            foreach (var video in ids)
            {
                window.set_statusbar_text("Waiting to download video " + (i + 1) + "/" + ids.Length + ": " + titles[i]);

                // check if id is in file before downloading
                String databasepath = CompatSettings.Default.Path + "/downloaded";
                String blacklistpath = CompatSettings.Default.Path + "/blacklist";
                String synonymspath = CompatSettings.Default.Path + "/synonyms";
                String[] dungus = File.ReadAllLines(databasepath);
                String[] dongus = File.ReadAllLines(blacklistpath);
                String[] dengus = File.ReadAllLines(synonymspath);

                Debug.Write(dungus.ToString() + "\n");
                Debug.Write(ids[i].ToString() + "\n");
                Debug.Write(i.ToString() + "\n");
                Debug.Write(video.ToString() + "\n");

                if (!dungus.Contains<string>(video))
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
                            String degrengos = GetDownloadLink("http://www.youtube.com/watch?v=" + video, "");

                            if (degrengos != "turkey") // Checks if there were no problems with the video page (needs NSFW handler)
                            {
                                Uri downloadlink = new Uri(degrengos);

                                String videopath = CompatSettings.Default.Path + "/" + names[i] + " - " + titles[i] + " - " + video + currentextention;

                                // Account synonyms
                                foreach (String turkey in dengus)
                                {
                                    if (turkey.Contains(names[i]))
                                    {
                                        videopath = CompatSettings.Default.Path + "/" + turkey + "/" + names[i] + " - " + titles[i] + " - " + video + currentextention;
                                    }
                                }

                                window.set_statusbar_text("Downloading video " + (i + 1) + "/" + ids.Length + ": " + titles[i]);
                                
                                // Hopefully this works
                                try
                                {
                                    wc.DownloadFile(downloadlink, videopath);
                                    pruppets = false;
                                }
                                catch (System.Net.WebException e)
                                {
                                    Debug.Write(e);
                                    pruppets = true;
                                    continue;
                                }
                            }

                            // After download write downloaded id to file
                            System.IO.File.AppendAllText(databasepath, video + "\n");
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
                    Thread.Sleep(50);
                }
                i++;
            }
            List<string> nothing = new List<string>();
            window.set_list_text(nothing);
        }

        public string GetDownloadLink(String sourceurl, String format)
        {
            WebClient wc = new WebClient();
            String source;

            while (true)
            {
                try
                {
                    source = wc.DownloadString(sourceurl);
                }
                catch
                {
                    continue;
                }

                break;
            }

            source = System.Web.HttpUtility.HtmlDecode(source);

            // Fixes bad video crashes (There will be more for NSFW and Private vids once SubBuddy detects one)
            if (source.Contains("This video is currently being processed"))
            {
                return "turkey";
            }

            if (source.Contains("This video is unavailable"))
            {
                return "turkey";
            }

            int start = source.IndexOf("movie_player");
            source = source.Substring(start);
            start = source.IndexOf("flashvars");
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

            // fixes crashes that started in july/august 2012
            if (fdsa[0].StartsWith(" \"url_encoded_fmt_stream_map\":"))
            {
                fdsa[0] = fdsa[0].Replace(" \"url_encoded_fmt_stream_map\": \"", "");
            }
            else
            {
                start = fdsa[0].IndexOf("url=");
                fdsa[0] = fdsa[0].Substring(start);
            }

            itag = new String[fdsa.Length];
            quality = new String[fdsa.Length];
            fallback_host = new String[fdsa.Length];
            type = new String[fdsa.Length];
            url = new String[fdsa.Length];
            sig = new String[fdsa.Length];

            i = 0;

            foreach (var dungus in fdsa)
            {
                if (dungus.StartsWith("url"))
                {
                    itag[i] = dungus.Substring(dungus.LastIndexOf("itag"));
                    itag[i] = itag[i].Replace("itag=", "");
                    itag[i] = itag[i].Replace("\"", "");
                    quality[i] = dungus.Substring(dungus.LastIndexOf("quality"));
                    quality[i] = quality[i].Substring(quality[i].IndexOf("=") + 1, quality[i].IndexOf("\\"));
                    quality[i] = quality[i].Replace("\\u0026fa", "");
                    fallback_host[i] = dungus.Substring(dungus.LastIndexOf("fallback_host"));
                    fallback_host[i] = fallback_host[i].Substring(fallback_host[i].IndexOf("=") + 1);
                    fallback_host[i] = fallback_host[i].Substring(0, fallback_host[i].IndexOf("\\"));
                    type[i] = dungus.Substring(dungus.LastIndexOf("type"));
                    type[i] = type[i].Substring(type[i].IndexOf("video"), type[i].IndexOf("\\") - 5);
                    url[i] = dungus.Substring(dungus.IndexOf("url"), dungus.IndexOf("\\"));
                    url[i] = url[i].Replace("url=", "");
                    url[i] = Uri.UnescapeDataString(url[i]);
                    sig[i] = dungus.Substring(dungus.LastIndexOf("sig="));
                    sig[i] = sig[i].Substring(0, sig[i].LastIndexOf("\\"));
                    sig[i] = sig[i].Replace("sig=", "signature=");
                    sig[i] = Uri.UnescapeDataString(sig[i]);
                    url[i] += "&" + sig[i];
                    i++;
                }
                else if (dungus.StartsWith("itag"))
                {
                    itag[i] = dungus.Substring(dungus.IndexOf("itag"));
                    itag[i] = itag[i].Substring(0, itag[i].IndexOf("\\"));
                    itag[i] = itag[i].Replace("itag=", "");
                    itag[i] = itag[i].Replace("\"", "");
                    quality[i] = dungus.Substring(dungus.LastIndexOf("quality"));
                    quality[i] = quality[i].Substring(quality[i].IndexOf("=") + 1);
                    quality[i] = quality[i].Replace("\\u0026fa", "");
                    fallback_host[i] = dungus.Substring(dungus.LastIndexOf("fallback_host"));
                    fallback_host[i] = fallback_host[i].Substring(fallback_host[i].IndexOf("=") + 1);
                    fallback_host[i] = fallback_host[i].Substring(0, fallback_host[i].IndexOf("\\"));
                    type[i] = dungus.Substring(dungus.LastIndexOf("type"));
                    type[i] = type[i].Substring(type[i].IndexOf("video"), type[i].IndexOf("\\") - 5);
                    url[i] = dungus.Substring(dungus.IndexOf("url"), dungus.IndexOf("type"));
                    url[i] = url[i].Substring(0, url[i].IndexOf("\\"));
                    url[i] = url[i].Replace("url=", "");
                    url[i] = Uri.UnescapeDataString(url[i]);
                    sig[i] = dungus.Substring(dungus.LastIndexOf("sig="));
                    sig[i] = sig[i].Substring(0, sig[i].LastIndexOf("\\"));
                    sig[i] = sig[i].Replace("sig=", "signature=");
                    sig[i] = Uri.UnescapeDataString(sig[i]);
                    url[i] += "&" + sig[i];
                    i++;
                }
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
            //Debug.WriteLine("Nothing?");
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
            String localSubs = File.ReadAllText(CompatSettings.Default.Path + "localsubs");
            
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
                XmlReader reader = XmlReader.Create("http://gdata.youtube.com/feeds/api/users/" + user + "/subscriptions?start-index="+page);
                SyndicationFeed subs = SyndicationFeed.Load(reader);

                foreach (var sub in subs.Items)
                {
                    String subText;
                    
                    subText = sub.Title.Text;
                    subText = subText.Substring(subText.IndexOf(":") + 2);
                    
                    if (!localSubs.Contains(subText))
                    {
                        subsList[i] = subText; // I'm keeping this in memory for debugging purposes.
                        System.IO.File.AppendAllText(CompatSettings.Default.Path + "localsubs", subsList[i]+"\n");
                    }

                    i++;
                }
                reader.Close();
            }

            MessageBox.Show("Sync Completed!");
            ui.set_statusbar_text("Idle");
            wc.Dispose();
        }
    }
}
