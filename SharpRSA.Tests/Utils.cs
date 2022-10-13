namespace SharpRSA.Tests
{
    public class Utils
    {
        //Function to return the string representation of the raw bytes in an array.
        public static string RawByteString( byte[] bytes )
        {
            return BitConverter.ToString( bytes ).Replace( "-", " " );
        }
    }
}
