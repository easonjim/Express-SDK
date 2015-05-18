/*
 * Author: Kishore Reddy
 * Url: http://commonlibrarynet.codeplex.com/
 * Title: CommonLibrary.NET
 * Copyright: � 2009 Kishore Reddy
 * License: LGPL License
 * LicenseUrl: http://commonlibrarynet.codeplex.com/license
 * Description: A C# based .NET 3.5 Open-Source collection of reusable components.
 * Usage: Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */
using System;
using System.Net.Mail;

namespace ComLib.EmailLib
{
    /// <summary>
    /// 注意：使用的时候必须先初始化 EmailService(EmailServiceSettings settings) 构造函数设置配置
    /// 如需要配置成扩展 增加一个构造函数即可
    /// </summary>
    public class EmailService : IEmailService
    {
        private EmailServiceSettings _config;

        /// <summary>
        /// Initializes a new instance of the <see cref="EmailService"/> class.
        /// </summary>
        //public EmailService()
        //{
        //}

        /// <summary>
        /// Initializes a new instance of the <see cref="EmailService"/> class.
        /// </summary>
        /// <param name="config">The config.</param>
        public EmailService(EmailServiceSettings settings)
        {
            Init(settings);
        }


        /// <summary>
        /// Initialize the configuration.
        /// </summary>
        /// <param name="config"></param>
        public void Init(EmailServiceSettings config)
        {
            _config = config;
        }


        #region IEmailService Members
        /// <summary>
        /// The email service configuration object.
        /// </summary>
        public EmailServiceSettings Settings
        {
            get { return _config; }
            set { _config = value; }
        }


        /// <summary>
        /// Sends the specified message.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="credentialsUser">The credentials user.</param>
        /// <param name="credentialsPassword">The credentials password.</param>
        /// <returns></returns>
        public bool Send(NotificationMessage message, string credentialsUser, string credentialsPassword)
        {
            MailMessage mailMessage = new MailMessage(_config.From, message.To, message.Subject, message.Body);
            mailMessage.IsBodyHtml = message.IsHtml;
            return InternalSend(mailMessage, true, credentialsUser, credentialsPassword);
        }


        /// <summary>
        /// Sends the mail message.
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public bool Send(NotificationMessage message)
        {
            MailMessage mailMessage = new MailMessage(_config.From, message.To, message.Subject, message.Body);
            mailMessage.IsBodyHtml = message.IsHtml;
            return InternalSend(mailMessage, Settings.IsAuthenticationRequired, Settings.AuthenticationUserName, Settings.AuthenticationPassword);
        }


        /// <summary>
        /// Mail the message using the native MailMessage class.
        /// </summary>
        /// <param name="message">The mail message</param>
        /// <param name="useCredentials">Whether or not to use credentials for security when sending emails.</param>
        /// <param name="credentialsUser">User name when using credentials.</param>
        /// <param name="credentialsPassword">Password when using credentials.</param>
        /// <returns></returns>
        public bool Send(MailMessage message, bool useCredentials, string credentialsUser, string credentialsPassword)
        {
            return InternalSend(message, useCredentials, credentialsUser, credentialsPassword);
        }


        /// <summary>
        /// Mail the message using the native MailMessage class.
        /// </summary>
        /// <param name="from">Who the email is from.</param>
        /// <param name="to">Who the email is being sent to.</param>
        /// <param name="subject">Subject of email.</param>
        /// <param name="body">Email body.</param>
        /// <param name="useCredentials">Whether or not to use credentials for security when sending emails.</param>
        /// <param name="credentialsUser">User name when using credentials.</param>
        /// <param name="credentialsPassword">Password when using credentials.</param>
        /// <returns></returns>
        public bool Send(string from, string to, string subject, string body,
            bool useCredentials, string credentialsUser, string credentialsPassword)
        {
            MailMessage message = new MailMessage(from, to, subject, body);
            message.IsBodyHtml = true;
            return InternalSend(message, useCredentials, credentialsUser, credentialsPassword);
        }


        /// <summary>
        /// Mail the message using the native MailMessage class and the credentials from the current configuration.
        /// </summary>
        /// <param name="message">The mail message</param>
        /// <returns></returns>
        public bool Send(MailMessage mailMessage)
        {
            return InternalSend(mailMessage, Settings.IsAuthenticationRequired, Settings.AuthenticationUserName, Settings.AuthenticationPassword);
        }
        #endregion



        /// <summary>
        /// Internals the send.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="useCredentials">if set to <c>true</c> [use credentials].</param>
        /// <param name="credentialsUser">The credentials user.</param>
        /// <param name="credentialsPassword">The credentials password.</param>
        /// <returns></returns>
        private bool InternalSend(MailMessage mailMessage, bool useCredentials, string credentialsUser, string credentialsPassword)
        {
            bool sent = true;
            try
            {
                string host = _config.SmptServer;
                int port = _config.Port;
                System.Net.Mail.SmtpClient client = null;

                if (_config.UsePort)
                {
                    client = new SmtpClient(host, port == 0 ? 25 : port);
                }
                else
                {
                    client = new SmtpClient(host);
                }
                if (useCredentials)
                {
                    client.UseDefaultCredentials = false;
                    client.Credentials = new System.Net.NetworkCredential(credentialsUser, credentialsPassword);
                }
                client.Send(mailMessage);
            }
            catch (Exception ex)
            {
                //Logger.Error("Unable to send email.", ex);
                sent = false;
            }
            return sent;
        }

    }
}
