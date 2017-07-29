namespace Model.JobLanes
{
    public class ConfigurationVariables
    {

        public ConfigurationVariable<string> SiteLogoPath = new ConfigurationVariable<string>("SiteLogoPath");
        public ConfigurationVariable<string> FromEmail = new ConfigurationVariable<string>("FromEmail");
        public ConfigurationVariable<string> FromEmailName = new ConfigurationVariable<string>("FromEmailName");
        public ConfigurationVariable<string> EmailTemplatePath = new ConfigurationVariable<string>("EmailTemplatePath");

        public ConfigurationVariable<string> RegistrationEmailName = new ConfigurationVariable<string>("RegistrationEmailName");
        public ConfigurationVariable<string> RegistrationEmailSubject = new ConfigurationVariable<string>("RegistrationEmailSubject");

        public ConfigurationVariable<int> ItemPerPage = new ConfigurationVariable<int>("ItemPerPage");
        public ConfigurationVariable<string> AdminEmail = new ConfigurationVariable<string>("AdminEmail");

    }
}
