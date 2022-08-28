using MindMate.Controller;

namespace MindMate.View
{
    public interface IEditorTabs
    {
        MapCtrl CurrentMapCtrl { get; }

        void UpdateAppTitle();
        
        int TabCount { get; }
    }
}