using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.Text.RegularExpressions;
using System.Net;
using Newtonsoft.Json;
using tudouApi.Model;

namespace tudouApi
{
	//
    public class tudouHelper
    {
        private string _appkey = "7ee7a34f14a4c74b"; //20110527 seasun owner tudou appkey
        public string Appkey
        {
            get
            {
                if (ConfigurationManager.AppSettings["tudouAppKey"] != null)
                {
                    _appkey = ConfigurationManager.AppSettings["tudouAppKey"];
                }
                return _appkey;
            }
        }
        private string apiUrl = "http://api.tudou.com/v3/gw?method=item.info.get&appKey={0}&format=json&itemCodes={1}";

        public string Url { get; set; }
        public tudouHelper() { }
        public tudouHelper(string url)
        {
            this.Url = url;
        }

        /// <summary>
        /// 获取视频的的itemcode
        /// </summary>
        /// <param name="url">视频URL</param>
        /// <returns></returns>
        public static string GetItemCode(string url)
        {//URL格式：http://www.tudou.com/programs/view/XnQ1-NrAFk0/
            string itemCode = null;
            Regex regV = new Regex(@"\/view\/([\w-]+)/?", RegexOptions.IgnoreCase);
            if (regV.IsMatch(url))//普通视频格式，直接从URL分析
            {
                itemCode = regV.Match(url).Result("$1");
            }
            else
            {//URL格式：http://www.tudou.com/playlist/p/a66633i76946800.html
                Regex regP = new Regex(@"\/p\/[a-z]\d+i(\d+).*\.html", RegexOptions.IgnoreCase);
                if (regP.IsMatch(url))//土單/劇集视频，直接從URL分析
                {
                    string iid = regP.Match(url).Result("$1");//列表中的某個視頻的iid
                    WebClient wclient = new WebClient();
                    try
                    {//downLoad土豆視頻的網頁原代碼，分析裡一段JSON格式
                        string tudouHtml = Encoding.GetEncoding("GBK").GetString(wclient.DownloadData(url));
                        Regex regPCode = new Regex("(?is)iid:" + iid + ".*?icode:\"([\\w-]+)\".*?,cid:", RegexOptions.IgnoreCase);//非貪婪模式且(?is)表示.字符可以包括換行符
                        if (regPCode.IsMatch(tudouHtml))//從網頁原代碼分析，主要是拿視頻的itemcode
                        {
                            itemCode = regPCode.Match(tudouHtml).Result("$1");
                            return itemCode;//因為用了WebClient,異步，所以要在這裡再返回一次
                        }
                    }
                    catch (Exception e)
                    {
                    }
                }
                else
                {//URL格式：http://www.tudou.com/playlist/p/a66633.html
                    Regex regPfirst = new Regex(@"\/p\/[a-z]\d+\.html", RegexOptions.IgnoreCase);
                    if (regPfirst.IsMatch(url))//土單/劇集視頻，直接從URL分析
                    {
                        WebClient wclient = new WebClient();
                        try
                        {//downLoad土豆視頻的網頁原代碼，分析裡一段JSON格式
                            string tudouHtml = Encoding.GetEncoding("GBK").GetString(wclient.DownloadData(url));
                            Regex regPCode = new Regex("(?is)icode:\"([\\w-]+)\".*?,cid:", RegexOptions.IgnoreCase);//非貪婪模式且(?is)表示.字符可以包括換行符
                            if (regPCode.IsMatch(tudouHtml))//從網頁原代碼分析，主要是拿視頻的itemcode
                            {
                                itemCode = regPCode.Match(tudouHtml).Result("$1");
                                return itemCode;//因為用了WebClient,異步，所以要在這裡再返回一次
                            }
                        }
                        catch (Exception e)
                        {
                        }
                    }
                }

            }
            return itemCode;
        }
        /// <summary>
        /// 獲取土豆JSON格式的視頻信息
        /// </summary>
        /// <param name="url">視頻URL</param>
        /// <returns></returns>
        public string GetVideoInfo(string url)
        {
            if (!string.IsNullOrEmpty(url) && url.StartsWith("http://"))
            {
                string itemcode = GetItemCode(url);
                if (itemcode != null)
                {
                    WebClient wc = new WebClient();
                    wc.Encoding = Encoding.UTF8;
                    string info = wc.DownloadString(string.Format(apiUrl, this.Appkey, itemcode));
                    return info;
                }
            }
            return null;

        }

        /// <summary>
        /// 獲取土豆的視頻信息實體
        /// </summary>
        /// <returns></returns>
        public VideoInfo GetVideoInfo()
        {
            VideoInfo video = new VideoInfo();
            string info = GetVideoInfo(this.Url);
            if (info != null)
            {
                try
                {
                    List<VideoInfo> videoList = JsonConvert.DeserializeObject<WholeVideoInfo>(info).multiResult.results;
                    if (videoList.Count > 0)
                    {
                        video = videoList[0];
                    }
                }
                catch (Exception e) { }
            }
            return video;
        }
    }
}
