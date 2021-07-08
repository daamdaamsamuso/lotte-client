using LotteCinemaService.Database.Configuration.Properties;

namespace LotteCinemaService.Database.Configuration
{
    public class Config
    {
        public static string ticket_db_addr
        {
            get { return Settings.Default.ticket_db_addr; }
        }

        public static string ticket_db_name
        {
            get { return Settings.Default.ticket_db_name; }
        }

        public static string ticket_db_id
        {
            get { return Settings.Default.ticket_db_id; }
        }

        public static string ticket_db_pw
        {
            get { return Settings.Default.ticket_db_pw; }
        }

        public static string web_db_addr
        {
            get { return Settings.Default.web_db_addr; }
        }

        public static string web_db_name
        {
            get { return Settings.Default.web_db_name; }
        }

        public static string web_db_id
        {
            get { return Settings.Default.web_db_id; }
        }

        public static string web_db_pw
        {
            get { return Settings.Default.web_db_pw; }
        }
    }
}