using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Skay.WebBot;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.IO;
using Ivony.Html.Parser;
using Ivony.Html;

namespace Kylin.GetHttpIp
{
    public class kylinIp
    {
        ///爬虫获取网站的高匿代理IP
        ///目前使用的网站有:
        ///http://www.xdaili.cn/freeproxy
        ///http://www.xicidaili.com/nn/
        ///http://www.goubanjia.com/free/gngn/index.shtml
        ///

        ///第一个网站可以抓包：http://www.xdaili.cn/ipagent//freeip/getFreeIps?page=1&rows=10
        ///其他几个都要进页面
        ///
        HttpUtility http;
         

        public kylinIp()
        {
            http = new HttpUtility();
        }
        

        public string GetIPBy_All()
        {
            int foreach_i = 0;
            ///讯代理
            string Url = "http://www.xdaili.cn/ipagent//freeip/getFreeIps?page=1&rows=10";
            try {
                string Area_Html = http.GetHtmlText(Url);
                JObject Area_Json = (JObject)JsonConvert.DeserializeObject(Area_Html);
                for (int j = 0; j < Area_Json["RESULT"]["rows"].Count()-5; j++)
                {
                    if (Area_Json["RESULT"]["rows"][0]["anony"].ToString() == "高匿")
                    {
                        string IP = Area_Json["RESULT"]["rows"][0]["ip"].ToString();
                        string Duankou = Area_Json["RESULT"]["rows"][0]["port"].ToString();
                        try
                        {
                            WebProxy proxyObject = new WebProxy(IP, int.Parse(Duankou));//str为IP地址 port为端口号 代理类
                            HttpWebRequest Req = (HttpWebRequest)WebRequest.Create("http://www.whatismyip.com.tw/"); // 61.183.192.5
                            Req.UserAgent = "Mozilla/4.0 (compatible; MSIE 8.0; Windows NT 6.1; Trident/4.0; QQWubi 133; SLCC2; .NET CLR 2.0.50727; .NET CLR 3.5.30729; .NET CLR 3.0.30729; Media Center PC 6.0; CIBA; InfoPath.2)";
                            Req.Proxy = proxyObject; //设置代理 
                            Req.Method = "GET";
                            HttpWebResponse Resp = (HttpWebResponse)Req.GetResponse();
                            string StringSub = "";
                            string OkStr = "";
                            Encoding code = Encoding.GetEncoding("utf-8");
                            using (StreamReader sr = new StreamReader(Resp.GetResponseStream(), code))
                            {
                                string str1 = sr.ReadToEnd();//获取得到的网址html返回数据，这里就可以使用某些解析html的dll直接使用了,比如htmlpaser 
                                if (str1.IndexOf(IP) > 0)
                                {
                                    return IP + ":" + Duankou;
                                }
                            }
                        }
                        catch { }

                    }
                }
            }
            catch { }

            ///西刺免费代理IP
            Url = "http://www.xicidaili.com/nn/";
            try
            {
                string Area_Html = http.GetHtmlText(Url);
                var documenthtml = new JumonyParser().Parse(Area_Html);

                var lists = documenthtml.FindFirst("#ip_list").Find("tr").ToList();

                bool first_ip = true;
                foreach_i = 0;
                foreach (var list in lists)
                {
                    foreach_i++;
                    if (foreach_i > 5)
                    {
                        break;
                    }
                    if (first_ip)
                    {
                        first_ip = false;
                        continue;
                    }
                    var IP_i = list.Find("td").ToList();
                    string IP = IP_i[1].InnerText().ToString();
                    string Duankou = IP_i[2].InnerText().ToString();
                    try
                    {
                        WebProxy proxyObject = new WebProxy(IP, int.Parse(Duankou));//str为IP地址 port为端口号 代理类
                        HttpWebRequest Req = (HttpWebRequest)WebRequest.Create("http://www.whatismyip.com.tw/"); // 61.183.192.5
                        Req.UserAgent = "Mozilla/4.0 (compatible; MSIE 8.0; Windows NT 6.1; Trident/4.0; QQWubi 133; SLCC2; .NET CLR 2.0.50727; .NET CLR 3.5.30729; .NET CLR 3.0.30729; Media Center PC 6.0; CIBA; InfoPath.2)";
                        Req.Proxy = proxyObject; //设置代理 
                        Req.Method = "GET";
                        HttpWebResponse Resp = (HttpWebResponse)Req.GetResponse();
                        string StringSub = "";
                        string OkStr = "";
                        Encoding code = Encoding.GetEncoding("utf-8");
                        using (StreamReader sr = new StreamReader(Resp.GetResponseStream(), code))
                        {
                            string str1 = sr.ReadToEnd();//获取得到的网址html返回数据，这里就可以使用某些解析html的dll直接使用了,比如htmlpaser 
                            if (str1.IndexOf(IP) > 0)
                            {
                                return IP + ":" + Duankou;
                            }
                        }
                    }
                    catch { }
                }
            }
            catch { }

            ///全网代理IP
            Url = "http://www.goubanjia.com/free/anoy/%E9%AB%98%E5%8C%BF/index.shtml";
            try
            {
                string Area_Html = http.GetHtmlText(Url);
                var documenthtml = new JumonyParser().Parse(Area_Html);

                var lists = documenthtml.FindFirst("#list").Find("tr").ToList();

                bool first_ip = true;
                foreach_i = 0;
                foreach (var list in lists)
                {
                    foreach_i++;
                    if (foreach_i > 5)
                    {
                        break;
                    }
                    if (first_ip)
                    {
                        first_ip = false;
                        continue;
                    }
                    var IP_all = list.FindFirst(".ip");
                    var ip_text = IP_all.Find("p");
                    ip_text.Remove();
                    string IP = IP_all.InnerText().Split(':')[0];
                    string Duankou = IP_all.InnerText().Split(':')[1];
                    try
                    {
                        WebProxy proxyObject = new WebProxy(IP, int.Parse(Duankou));//str为IP地址 port为端口号 代理类
                        HttpWebRequest Req = (HttpWebRequest)WebRequest.Create("http://www.whatismyip.com.tw/"); // 61.183.192.5
                        Req.UserAgent = "Mozilla/4.0 (compatible; MSIE 8.0; Windows NT 6.1; Trident/4.0; QQWubi 133; SLCC2; .NET CLR 2.0.50727; .NET CLR 3.5.30729; .NET CLR 3.0.30729; Media Center PC 6.0; CIBA; InfoPath.2)";
                        Req.Proxy = proxyObject; //设置代理 
                        Req.Method = "GET";
                        HttpWebResponse Resp = (HttpWebResponse)Req.GetResponse();
                        string StringSub = "";
                        string OkStr = "";
                        Encoding code = Encoding.GetEncoding("utf-8");
                        using (StreamReader sr = new StreamReader(Resp.GetResponseStream(), code))
                        {
                            string str1 = sr.ReadToEnd();//获取得到的网址html返回数据，这里就可以使用某些解析html的dll直接使用了,比如htmlpaser 
                            if (str1.IndexOf(IP) > 0)
                            {
                                return IP + ":" + Duankou;
                            }
                        }
                    }
                    catch { }
                }
            }
            catch { }


            ///无忧代理IP
            Url = "http://www.data5u.com/free/anoy/%E9%AB%98%E5%8C%BF/index.html";
            try
            {
                string Area_Html = http.GetHtmlText(Url);
                var documenthtml = new JumonyParser().Parse(Area_Html);

                var lists = documenthtml.FindLast(".wlist").Find("ul li ul").ToList();

                bool first_ip = true;
                foreach_i = 0;
                foreach (var list in lists)
                {
                    foreach_i++;
                    if (foreach_i > 5)
                    {
                        break;
                    }
                    if (first_ip)
                    {
                        first_ip = false;
                        continue;
                    }

                    string IP = list.FindFirst("li").InnerText().ToString();
                    string Duankou = list.FindFirst(".port").InnerText().ToString(); ;
                    try
                    {
                        WebProxy proxyObject = new WebProxy(IP, int.Parse(Duankou));//str为IP地址 port为端口号 代理类
                        HttpWebRequest Req = (HttpWebRequest)WebRequest.Create("http://www.whatismyip.com.tw/"); // 61.183.192.5
                        Req.UserAgent = "Mozilla/4.0 (compatible; MSIE 8.0; Windows NT 6.1; Trident/4.0; QQWubi 133; SLCC2; .NET CLR 2.0.50727; .NET CLR 3.5.30729; .NET CLR 3.0.30729; Media Center PC 6.0; CIBA; InfoPath.2)";
                        Req.Proxy = proxyObject; //设置代理 
                        Req.Method = "GET";
                        HttpWebResponse Resp = (HttpWebResponse)Req.GetResponse();
                        string StringSub = "";
                        string OkStr = "";
                        Encoding code = Encoding.GetEncoding("utf-8");
                        using (StreamReader sr = new StreamReader(Resp.GetResponseStream(), code))
                        {
                            string str1 = sr.ReadToEnd();//获取得到的网址html返回数据，这里就可以使用某些解析html的dll直接使用了,比如htmlpaser 
                            if (str1.IndexOf(IP) > 0)
                            {
                                return IP + ":" + Duankou;
                            }
                        }
                    }
                    catch { }
                }
            }
            catch { }

            
            
            return "当前暂无可用";
        }
    }
}
