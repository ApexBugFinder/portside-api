using Microsoft.Extensions.Configuration;

namespace Portfolio.WebApp.Helpers {
  public static class GetConnConstants {

    private static IConfiguration Configuration {get; set;}
    public static string PortfolioDB = "Server=198.211.29.93,1433; Database = PortfolioDB;User Id = sa;Password = 'Apple&Pie79';MultipleActiveResultSets=true;Persist Security Info=True;";

    public static string Authority = "https://identity.portside.sbs";

  }
}