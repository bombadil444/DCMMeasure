using System.Text;


// Used for debug purposes for the dev console
public class RichTextBoxWriter : TextWriter
{
    private RichTextBox _richTextBox;

    public RichTextBoxWriter(RichTextBox richTextBox)
    {
        _richTextBox = richTextBox;
    }

    public override void Write(char value)
    {
        _richTextBox.AppendText(value.ToString());
    }

    public override Encoding Encoding
    {
        get { return Encoding.UTF8; }
    }
}
