using MindMate.Serialization;
using System.Diagnostics;
using System.IO;
using System.Reflection;

namespace MindMate.Tests.TestDouble
{
    public static class MetaModelHelper
    {
        /// <summary>
        /// Creates MetaModel (without going through the deserialization)
        /// </summary>
        public static MetaModel.MetaModel Create()
        {
            var metaModel = new MetaModel.MetaModel();

            var field = typeof(MetaModel.MetaModel).GetField("instance",
                            BindingFlags.Static |
                            BindingFlags.NonPublic);

            Debug.Assert(field != null, "Cannot field private static field 'instance' through reflection.");
            field.SetValue(null, metaModel);

            return metaModel;
        }

        public static MetaModel.MetaModel CreateWithTestSettingsFile()
        {
            var field = typeof(MetaModel.MetaModel).GetField("instance",
                            BindingFlags.Static |
                            BindingFlags.NonPublic);

            Debug.Assert(field != null, "Cannot field private static field 'instance' through reflection.");
            field.SetValue(null, null);
            File.Copy(@"Resources\Settings.Yaml", Dir.UserSettingsDirectory + "Settings.Yaml", true);
            MindMate.MetaModel.MetaModel.Initialize();
            return MetaModel.MetaModel.Instance;
        }

        public static MetaModel.MetaModel CreateWithDefaultSettingsFile()
        {
            var field = typeof(MetaModel.MetaModel).GetField("instance",
                            BindingFlags.Static |
                            BindingFlags.NonPublic);

            Debug.Assert(field != null, "Cannot field private static field 'instance' through reflection.");
            field.SetValue(null, null);
            File.Copy(@"Resources\DefaultSettings.Yaml", Dir.UserSettingsDirectory + "Settings.Yaml", true);
            MindMate.MetaModel.MetaModel.Initialize();
            return MetaModel.MetaModel.Instance;
        }
    }
}
