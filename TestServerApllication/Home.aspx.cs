using HtmlAgilityPack;
using Quartz;
using Quartz.Impl;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace TestServerApllication
{
    public partial class Home : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                try
                {

                    ISchedulerFactory schedFact = new StdSchedulerFactory();
                    // get a scheduler
                    IScheduler sched = schedFact.GetScheduler();
                    sched.Start();

                    // define the job and tie it to our HelloJob class
                    IJobDetail job = JobBuilder.Create<SimpleJob>()
                        .WithIdentity("myJob", "group1")
                        .Build();

                    // Trigger the job to run now, and then every 40 seconds
                    ITrigger trigger = TriggerBuilder.Create()
                      .WithIdentity("myTrigger", "group1")
                      .StartNow()
                      .WithSimpleSchedule(x => x
                          .WithIntervalInSeconds(10)
                          .RepeatForever())
                      .Build();

                    sched.ScheduleJob(job, trigger);
                }
                catch (Exception) { }
            }
        }

    }
    public class SimpleJob : IJob
    {

        public List<string> Headlines = new List<string>();
        void IJob.Execute(IJobExecutionContext context)
        {
            //throw new NotImplementedException();
            WebClient client = new WebClient();
            String downloadedString = client.DownloadString("http://economictimes.indiatimes.com/headlines.cms");

            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(downloadedString);
            foreach (HtmlNode node in doc.DocumentNode.SelectNodes("//ul[@class='headlineData']//li"))
            {
                if (!Headlines.Contains(node.ChildNodes[0].InnerHtml))
                    insertNews(node, client);
            }
            foreach (HtmlNode node in doc.DocumentNode.SelectNodes("//ul[@class='data']"))
            {

                HtmlDocument docTemp = new HtmlDocument();
                docTemp.LoadHtml(node.InnerHtml);
                foreach (HtmlNode innerNode in docTemp.DocumentNode.SelectNodes("//li"))
                {
                    if (!Headlines.Contains(node.ChildNodes[0].InnerHtml))
                        insertNews(innerNode, client);
                }
            }
        }

        public void insertNews(HtmlNode node, WebClient client)
        {
            try
            {
                News.NewsStruct news = new News.NewsStruct();
                news.Headline = node.ChildNodes[0].InnerHtml;
                Headlines.Add(news.Headline);
                news.FullNewsLink = "http://economictimes.indiatimes.com" + node.ChildNodes[0].Attributes["href"].Value;
                //DateTime abcd = DateTime.Parse("dfdfdsf");
                string newsContentString = client.DownloadString(news.FullNewsLink);
                HtmlDocument innerDoc = new HtmlDocument();
                innerDoc.LoadHtml(newsContentString);
                news.NewsContent = innerDoc.DocumentNode.SelectNodes("//div[@class='Normal']")[0].InnerHtml;
                string DateUpdate = innerDoc.DocumentNode.SelectNodes("//div[@class='byline']")[0].InnerHtml;
                DateUpdate = DateUpdate.Substring(DateUpdate.IndexOf(':') + 2, DateUpdate.LastIndexOf('M') - (DateUpdate.IndexOf(':') + 1));
                news.DateTimeUpdate = DateTime.ParseExact(DateUpdate, "MMM dd, yyyy, hh.mm tt", null);
                using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString))
                {
                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = connection;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "NEWS_ADD";

                    cmd.Parameters.Add(new SqlParameter("@Headline", news.Headline));
                    cmd.Parameters.Add(new SqlParameter("@DateTimeUpdate", news.DateTimeUpdate));
                    cmd.Parameters.Add(new SqlParameter("@FullNewsLink", news.FullNewsLink));
                    cmd.Parameters.Add(new SqlParameter("@NewsContent", news.NewsContent));

                    cmd.Connection.Open();
                    SqlDataReader reader = cmd.ExecuteReader();

                    cmd.Connection.Close();

                }
            }
            catch (Exception)
            {
            }
        }
    }
}