using System.Windows.Forms;

namespace MindMate.View.NoteEditing
{
    public partial class HtmlSourceDialog : Form
    {
        public HtmlSourceDialog()
        {
            InitializeComponent();
            Debugging.FormDebugHooks.Instance.ApplyHook(this);
        }

        public string HtmlSource
        {
            get => txtSource.Text;
            set => txtSource.Text = value;
        }
    }    
}
