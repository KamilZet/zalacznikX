using Microsoft.Exchange.WebServices.Data;
using System;
using System.IO;
using System.ServiceProcess;
using System.Threading;
using InputWindow;
using System.Xaml;

namespace zalacznikX
{
    public partial class Sluchaj : ServiceBase
    {
        
        private static ExchangeService service = null;

        private static string logDir = null;

        public Sluchaj()
        {
            InitializeComponent();
        }

        internal void DebugStart()
        {
            string[] a = new string[0];
            this.OnStart(a);
        }

        protected override void OnStart(string[] args)
        {
            initLog();
            Log("Serwis uruchomiony");

            PocoUserData ud = new PocoUserData(){ EmailAddress = AccessSettings.MailAddress
                                                , Password = AccessSettings.MailPswd };

            if (string.IsNullOrEmpty(AccessSettings.MailAddress))
            {
                Log("Dane dostępowe do konta są niedostępne");
                throw new InvalidDataException("nie ma adresu");
            }
                
            service = Service.ConnectToService(ud , new TraceListener());

            SetStreamingNotifications(service,ud);
            
        }

        protected override void OnStop()
        {
            Log("Serwis zatrzymany");
        }

        static void SetStreamingNotifications(ExchangeService service,IUserData ud)
        {
            // Subscribe to streaming notifications on the Inbox folder, and listen 
            // for "NewMail", "Created", and "Deleted" events. 
            try
            {
                StreamingSubscription streamingsubscription = service.SubscribeToStreamingNotifications(
                    new FolderId[] { WellKnownFolderName.Inbox },
                    EventType.NewMail,
                    EventType.Created,
                    EventType.Deleted);

                StreamingSubscriptionConnection connection = new StreamingSubscriptionConnection(service, 1);


                connection.AddSubscription(streamingsubscription);
                // Delegate event handlers. 
                connection.OnNotificationEvent +=
                    new StreamingSubscriptionConnection.NotificationEventDelegate(OnEvent);
                connection.OnSubscriptionError +=
                    new StreamingSubscriptionConnection.SubscriptionErrorDelegate(OnError);
                connection.OnDisconnect +=
                    new StreamingSubscriptionConnection.SubscriptionErrorDelegate(OnDisconnect);
                connection.Open();

                Log(string.Format("Zasubskrybowano konto: {0}", ud.EmailAddress));

            }
            catch (Exception e)
            {
                Log("Błąd w trakcie próby podłączenia subskrypcji." + e.InnerException.ToString());
                
            }
        }
        static private void OnDisconnect(object sender, SubscriptionErrorEventArgs args)
        {
            ((StreamingSubscriptionConnection)sender).Open();

            #region user interaction on disconnecting (OFF)
            //ConsoleKeyInfo cki;
            //Console.WriteLine("The connection to the subscription is disconnected.");
            //Console.WriteLine("Do you want to reconnect to the subscription? Y/N");
            //while (true)
            //{
            //    cki = Console.ReadKey(true);
            //    {
            //        if (cki.Key == ConsoleKey.Y)
            //        {
            //            connection.Open();
            //            Console.WriteLine("Connection open.");
            //            Console.WriteLine("\r\n");
            //            break;
            //        }
            //        else if (cki.Key == ConsoleKey.N)
            //        {
            //            Signal.Set();
            //            bool isOpen = connection.IsOpen;

            //            if (isOpen == true)
            //            {
            //            // Close the connection
            //            connection.Close();
            //            }
            //            else
            //            {
            //                break;
            //            }
            //        }
            //    }
            //}
            #endregion

        }

        static void OnEvent(object sender, NotificationEventArgs args)
        {
            foreach (NotificationEvent notification in args.Events)
            {
                if (notification.EventType == EventType.NewMail && notification is ItemEvent)
                {
                    Log("Zarejestrowałem nową wiadomość");
                    TryGetAttachment(service, ((ItemEvent)notification).ItemId);
                }
            }
        }
        static void OnError(object sender, SubscriptionErrorEventArgs args)
        {
            // Handle error conditions. 
            Exception e = args.Exception;
            Log("Błąd. " + e.Message );
        }

        static void TryGetAttachment(ExchangeService es, ItemId iid)
        {
            EmailMessage message = EmailMessage.Bind(es, iid, new PropertySet(ItemSchema.Attachments));
            foreach (Attachment attachment in message.Attachments)
            {
                if (attachment is FileAttachment)
                {
                    FileAttachment fileAttachment = attachment as FileAttachment;

                    var filePath = logDir + fileAttachment.Name;
                    // Load the attachment into a file.
                    // This call results in a GetAttachment call to EWS.
                    fileAttachment.Load(filePath);
                    Log("Zapisałem załącznik: " + fileAttachment.Name + " w lokalizacji: " + filePath);
                    
                }
            }
        }

        private static void Log(string message)
        {
            FileStream fs = new FileStream(logDir + "\\zalacznikXLog.txt",
                                FileMode.OpenOrCreate, FileAccess.Write);
            StreamWriter m_streamWriter = new StreamWriter(fs);
            m_streamWriter.BaseStream.Seek(0, SeekOrigin.End);
            m_streamWriter.WriteLine("zalacznikX: " + message + " o godzinie " +
               DateTime.Now.ToShortDateString() + " " +
               DateTime.Now.ToShortTimeString() + "\n");
            m_streamWriter.Flush();
            m_streamWriter.Close();
        }
        private static void initLog()
        {
            logDir = Properties.Settings.Default.LogDir;
            if (!System.IO.Directory.Exists(logDir))
                System.IO.Directory.CreateDirectory(logDir);
        }
    }
}
