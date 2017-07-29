namespace Model.JobLanes
{
    public class App
    {               
        private static ConfigurationVariables _configurations;
        public static ConfigurationVariables Configurations { get { return _configurations ?? (_configurations = new ConfigurationVariables()); } }               
    }
}
