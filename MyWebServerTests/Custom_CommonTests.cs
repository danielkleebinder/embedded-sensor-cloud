using System;
using NUnit.Framework;
using System.IO;
using BIF.SWE1.Interfaces;
using System.Text;
using MyWebServer.Plugins;

namespace MyWebServer.Tests
{
    [TestFixture]
    public class Custom_CommonTests
    {
        [Test]
        public void mime_type_conversion()
        {
            Assert.AreEqual(HTTP.MimeTypeFromExtension(".html"), "text/html");
            Assert.AreEqual(HTTP.MimeTypeFromExtension("html"), "text/html");
            Assert.AreEqual(HTTP.MimeTypeFromExtension(".css"), "text/css");
            Assert.AreEqual(HTTP.MimeTypeFromExtension("css"), "text/css");
            Assert.AreEqual(HTTP.MimeTypeFromExtension(".js"), "application/javascript");
            Assert.AreEqual(HTTP.MimeTypeFromExtension("js"), "application/javascript");
            Assert.AreEqual(HTTP.MimeTypeFromExtension(".pdf"), "application/pdf");
            Assert.AreEqual(HTTP.MimeTypeFromExtension("pdf"), "application/pdf");
            Assert.AreEqual(HTTP.MimeTypeFromExtension(".jpeg"), "image/jpeg");
            Assert.AreEqual(HTTP.MimeTypeFromExtension("jpg"), "image/jpeg");
        }

        [Test]
        public void content_type_encoding()
        {
            string contentType = HTTP.MimeTypeFromExtension("html");
            string encoding = HTTP.CONTENT_ENCODING_UTF8;
            Assert.AreEqual(HTTP.ContentTypeEncoding(contentType, encoding), "text/html; charset=utf-8");
        }

        [Test]
        public void check_for_valid_app_context()
        {
            Assert.IsNotNull(AppContext.Current);
            Assert.IsNotNull(AppContext.Current.WorkingDirectory);
            Assert.IsNotNull(AppContext.Current.PluginDirectory);
        }


        #region StaticFilePlugin
        [Test]
        public void static_file_plugin_can_handle()
        {
            // Get plugin
            IPlugin plugin = new StaticFilePlugin();
            Assert.LessOrEqual(plugin.CanHandle(null), 0.0);

            // Should be less than 1.0 because other plugins may be more important
            Single valid = plugin.CanHandle(new Request(ValidRequestStream("/")));
            Assert.That(valid, Is.GreaterThan(0.0).And.LessThan(1.0));
        }
        #endregion


        #region Helper
        /// <summary>
        /// Creates a valid request stream. This method was created by Arthur Zaczek and copied
        /// over to prevent the overhead of an additional library.
        /// </summary>
        /// <param name="url">URL.</param>
        /// <param name="method">HTTP Method.</param>
        /// <param name="host">Host.</param>
        /// <param name="header">Headers.</param>
        /// <param name="body">Body content.</param>
        /// <returns>Valid request stream.</returns>
        public static Stream ValidRequestStream(string url, string method = "GET", string host = "localhost", string[][] header = null, string body = null)
        {
            byte[] bodyBytes = null;
            if (body != null)
            {
                bodyBytes = Encoding.UTF8.GetBytes(body);
            }

            var ms = new MemoryStream();
            var sw = new StreamWriter(ms, Encoding.ASCII);

            sw.WriteLine("{0} {1} HTTP/1.1", method, url);
            sw.WriteLine("Host: {0}", host);
            sw.WriteLine("Connection: keep-alive");
            sw.WriteLine("Accept: text/html,application/xhtml+xml");
            sw.WriteLine("User-Agent: Unit-Test-Agent/1.0 (The OS)");
            sw.WriteLine("Accept-Encoding: gzip,deflate,sdch");
            sw.WriteLine("Accept-Language: de-AT,de;q=0.8,en-US;q=0.6,en;q=0.4");
            if (bodyBytes != null)
            {
                sw.WriteLine(string.Format("Content-Length: {0}", bodyBytes.Length));
                sw.WriteLine("Content-Type: application/x-www-form-urlencoded");
            }
            if (header != null)
            {
                foreach (var h in header)
                {
                    sw.WriteLine(string.Format("{0}: {1}", h[0], h[1]));
                }
            }
            sw.WriteLine();

            if (bodyBytes != null)
            {
                sw.Flush();
                ms.Write(bodyBytes, 0, bodyBytes.Length);
            }

            sw.Flush();

            ms.Seek(0, SeekOrigin.Begin);
            return ms;
        }
        #endregion
    }
}