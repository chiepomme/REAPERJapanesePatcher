using System.Text;

namespace REAPERJapanesePatcher
{
    public static class PathStringExtension
    {
        public static string Quote(this string path)
        {
            var sb = new StringBuilder();
            sb.Append("\"");
            sb.Append(path.Trim('"'));
            sb.Append("\"");
            return sb.ToString();
        }
    }
}
