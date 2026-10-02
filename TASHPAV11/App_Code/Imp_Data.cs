using static System.Runtime.InteropServices.JavaScript.JSType;

namespace TASHPAV11.App_Code
{
    public class Imp_Data
    {

        public static string ConString { get; } =
            $"Provider=Microsoft.ACE.OLEDB.12.0;" +
            $"Data Source={Path.Combine(
                Directory.GetCurrentDirectory(),
                "App_Data",
                "tashpav11-CND0401YWR.accdb")};" +
            $"Persist Security Info=True;";

       
    }
}
      
