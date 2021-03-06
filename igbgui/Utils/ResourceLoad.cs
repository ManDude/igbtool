using System.IO;
using System.Reflection;

namespace igbgui
{
    public static class ResourceLoad
    {
        public static string[] GetAllFileNames() => Assembly.GetExecutingAssembly().GetManifestResourceNames();

        public static string LoadTextFile(string name)
        {
            var exe = Assembly.GetExecutingAssembly();
            var fullname = string.Format("{0}.{1}", exe.GetName().Name, name.Replace("/", "."));
            using StreamReader r = new(exe.GetManifestResourceStream(fullname));
            return r.ReadToEnd();
        }
    }
}
