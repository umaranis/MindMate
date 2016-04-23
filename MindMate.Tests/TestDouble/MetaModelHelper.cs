using System.Diagnostics;
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
    }
}
